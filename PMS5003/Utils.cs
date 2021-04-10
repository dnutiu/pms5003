namespace PMS5003
{
    public class Utils
    {
        /// <summary>
        /// Combines two bytes and returns the results ((High RSHIFT 8) | Low)
        /// </summary>
        /// <param name="high">The high byte.</param>
        /// <param name="low">The low byte.</param>
        /// <returns></returns>
        public static uint CombineBytes(byte high, byte low)
        {
            return (uint)(low | (high << 8));;
        }
    }
}