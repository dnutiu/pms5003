using System.Text;
using PMS5003.Exceptions;

namespace PMS5003
{
    /// <summary>
    /// Pms5003Measurement is an abstraction over the PMS5003 data.
    /// </summary>
    public class Pms5003Data
    {
        private readonly uint[] _data;
        public uint Pm1Standard => _data[0];
        public uint Pm2Dot5Standard => _data[1];
        public uint Pm10Standard => _data[2];
        public uint Pm1Atmospheric => _data[3];
        public uint Pm2Dot5Atmospheric => _data[4];
        public uint Pm10Atmospheric => _data[5];
        public uint ParticlesDiameterBeyond0Dot3 => _data[6];
        public uint ParticlesDiameterBeyond0Dot5 => _data[7];
        public uint ParticlesDiameterBeyond1Dot0 => _data[8];
        public uint ParticlesDiameterBeyond2Dot5 => _data[9];
        public uint ParticlesDiameterBeyond5Dot0 => _data[10];
        public uint ParticlesDiameterBeyond10Dot0 => _data[11];
        private uint Reserved => _data[12];
        private uint Checksum => _data[13];

        private Pms5003Data()
        {
            _data = new uint[14];
        }

        /// <summary>
        /// Instantiates a new instance from the given byte buffer.
        /// </summary>
        /// <param name="buffer">The data buffer</param>
        /// <returns>A new instance.</returns>
        public static Pms5003Data FromBytes(byte[] buffer)
        {
            var pms5003Measurement = new Pms5003Data();
            
            if (buffer[0] != Pms5003Constants.StartByte1 || buffer[1] != Pms5003Constants.StartByte2)
            {
                throw new InvalidStartByteException(buffer[0], buffer[1]);
            }

            var frameLength = Utils.CombineBytes(buffer[2], buffer[3]);
            if (frameLength > 0)
            {
                var currentDataPoint = 0;
                for (var i = 4; i < frameLength + 4; i += 2)
                {
                    pms5003Measurement._data[currentDataPoint] = Utils.CombineBytes(buffer[i], buffer[i + 1]);
                    currentDataPoint += 1;
                }
            }

            var checkSum = 0;
            for (var i = 0; i < frameLength + 2; i++)
            {
                checkSum += buffer[i];
            }

            if (pms5003Measurement.Checksum != checkSum)
            {
                throw new ChecksumMismatchException();
            }

            return pms5003Measurement;
        }

        /// <summary>
        /// Returns a string representation of the Pms5003Data.
        /// </summary>
        /// <returns>String with all the fields and values.</returns>
        public override string ToString()
        {
            var buffer = new StringBuilder();
            buffer.AppendLine("Pms5003Data[");
            buffer.AppendLine($"Pm1Standard={Pm1Standard},");
            buffer.AppendLine($"Pm2Dot5Standard={Pm2Dot5Standard},");
            buffer.AppendLine($"Pm10Standard={Pm10Standard},");
            buffer.AppendLine($"Pm1Atmospheric={Pm1Atmospheric},");
            buffer.AppendLine($"Pm2Dot5Atmospheric={Pm2Dot5Atmospheric},");
            buffer.AppendLine($"Pm10Atmospheric={Pm10Atmospheric},");
            buffer.AppendLine($"ParticlesDiameterBeyond0Dot3={ParticlesDiameterBeyond0Dot3},");
            buffer.AppendLine($"ParticlesDiameterBeyond0Dot5={ParticlesDiameterBeyond0Dot5},");
            buffer.AppendLine($"ParticlesDiameterBeyond1Dot0={ParticlesDiameterBeyond1Dot0},");
            buffer.AppendLine($"ParticlesDiameterBeyond2Dot5={ParticlesDiameterBeyond2Dot5},");
            buffer.AppendLine($"ParticlesDiameterBeyond5Dot0={ParticlesDiameterBeyond5Dot0},");
            buffer.AppendLine($"ParticlesDiameterBeyond10Dot0={ParticlesDiameterBeyond10Dot0},");
            buffer.AppendLine($"Reserved={Reserved},");
            buffer.AppendLine($"Checksum={Checksum}");
            buffer.AppendLine("]");
            return buffer.ToString();
        }
    }
}