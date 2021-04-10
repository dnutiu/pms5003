using System;

namespace PMS5003.Exceptions
{
    /// <summary>
    /// BufferUnderflowExceptions is thrown when the PMS5003 data buffer is incomplete.
    /// </summary>
    [Serializable]
    public class BufferUnderflowException : Exception
    {
        public BufferUnderflowException() : base("The PMS5003 data buffer is underrun, not enough bytes read!")
        {
            
        }
    }
}