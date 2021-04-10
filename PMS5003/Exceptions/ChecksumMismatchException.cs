using System;

namespace PMS5003.Exceptions
{
    /// <summary>
    /// ChecksumMismatchException when the frame contents of PMS5003 doesn't match the checksum.
    /// </summary>
    public class ChecksumMismatchException : Exception
    {
        public ChecksumMismatchException() : base("Checksum mismatch.")
        {
        }
    }
}