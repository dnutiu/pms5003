using System;
using System.Device.Gpio;
using System.IO.Ports;
using System.Threading;
using Microsoft.Extensions.Logging;
using PMS5003.Exceptions;

namespace PMS5003
{
    /// <summary>
    /// PMS5003 digital universal particle concentration sensor.
    /// Datasheet: https://www.aqmd.gov/docs/default-source/aq-spec/resources-page/plantower-pms5003-manual_v2-3.pdf
    /// </summary>
    public class Pms5003
    {
        public static Logger<Pms5003> Logger;
        private readonly GpioController _gpioController;
        private readonly SerialPort _serialPort;
        private readonly short _pinSet;
        private readonly short _pinReset;
        private bool _isSleeping;

        /// <summary>
        /// Initializes the <see cref="Pms5003"/>.
        /// </summary>
        /// <param name="portSerialName">The name of the serial port.</param>
        /// <param name="pinSet">The number of the SET pin.</param>
        /// <param name="pinReset">The number if the RESET pin.</param>
        public Pms5003(string portSerialName, short pinSet, short pinReset)
        {
            _serialPort = new SerialPort(portSerialName, Pms5003Constants.DefaultBaudRate);
            _pinReset = pinReset;
            _pinSet = pinSet;

            _serialPort.Open();
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                _gpioController = new GpioController();
                if (pinSet >= 0)
                {
                    _gpioController.OpenPin(_pinSet, PinMode.Output);
                    _gpioController.Write(_pinSet, PinValue.High);
                }

                if (pinReset >= 0)
                {
                    _gpioController.OpenPin(_pinReset, PinMode.Output);
                    _gpioController.Write(_pinReset, PinValue.High);
                }
            }
            else
            {
                Logger?.LogWarning("Not running under Unix platform, skipping GPIO configuration.");
            }
        }

        /// <summary>
        /// Initializes the <see cref="Pms5003"/> using /dev/ttyAMA0 serial port name.
        /// </summary>
        /// <param name="pinSet"></param>
        /// <param name="pinReset"></param>
        public Pms5003(short pinSet, short pinReset) : this("/dev/ttyAMA0", pinSet, pinReset)
        {
        }

        /// <summary>
        /// Initializes the <see cref="Pms5003"/> using /dev/ttyAMA0 serial port name
        /// and no GPIO pins for SET and RESET.
        /// </summary>
        public Pms5003() : this("/dev/ttyAMA0", -1, -1)
        {
        }

        /// <summary>
        /// Reads the data from the sensor.
        /// </summary>
        /// <returns cref="Pms5003Data">The data.</returns>
        /// <exception cref="ReadFailedException">Thrown when the read fails.</exception>
        public Pms5003Data ReadData()
        {
            var currentTry = 0;
            const int maxRetries = 5;

            while (currentTry < maxRetries)
            {
                try
                {
                    var buffer = new byte[32];
                    _serialPort.Read(buffer, 0, 32);
                    return Pms5003Data.FromBytes(buffer);
                }
                catch (Exception e)
                {
                    Logger?.LogWarning(e.ToString());
                    Thread.Sleep(1000);
                }
                finally
                {
                    currentTry += 1;
                }
            }

            throw new ReadFailedException();
        }

        /// <summary>
        /// Resets the PMS5003 Sensor.
        /// </summary>
        public void Reset()
        {
            if (_gpioController == null || _pinReset < 0) return;
            _gpioController.Write(_pinReset, PinValue.Low);
            Thread.Sleep(200);
            _gpioController.Write(_pinReset, PinValue.High);
        }

        /// <summary>
        /// Enables Sleep Mode for the PMS5003 Sensor.
        /// </summary>
        public void Sleep()
        {
            if (_gpioController == null || _pinSet < 0) return;
            _isSleeping = true;
            _gpioController.Write(_pinSet, PinValue.Low);
        }

        /// <summary>
        /// Disables Sleep Mode for the PMS5003 Sensor.
        /// </summary>
        public void WakeUp()
        {
            if (_gpioController == null || _pinSet < 0) return;
            _isSleeping = false;
            _gpioController.Write(_pinSet, PinValue.High);
        }

        /// <summary>
        /// Checks if Sleep Mode is enabled.
        /// </summary>
        /// <returns>True if sensor is in Sleep Mode, false otherwise.</returns>
        public bool IsSleeping()
        {
            return _isSleeping;
        }

        ~Pms5003()
        {
            _serialPort.Close();
            _gpioController?.Dispose();
        }
    }
}