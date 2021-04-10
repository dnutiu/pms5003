using System;

namespace PMS5003.Exceptions
{
    /// <summary>
    /// Exception that is thrown by the PMS5003 sensor when the read contains an invalid start sequence.
    /// </summary>
    [Serializable]
    public class InvalidStartByteException : Exception
    {
        public InvalidStartByteException(short firstByte, short secondByte) : base(
            $"Invalid start characters, expected 0x42 0x4d got [{firstByte:X}, {secondByte:X}]")
        {
        }
    }
}