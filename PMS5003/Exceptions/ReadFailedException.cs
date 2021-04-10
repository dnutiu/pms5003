using System;

namespace PMS5003.Exceptions
{
    /// <summary>
    /// Thrown the the read has failed.
    /// </summary>
    public class ReadFailedException : Exception
    {
        public ReadFailedException() : base("PMS5003 read failed, max retries exceeded")
        {
        }
    }
}