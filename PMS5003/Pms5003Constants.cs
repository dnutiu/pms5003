namespace PMS5003
{
    /// <summary>
    /// The Pms5003Constants defines constants required by the PMS5003 communication protocol.
    /// </summary>
    public class Pms5003Constants
    {
        public static readonly int DefaultBaudRate = 9600;
        public static readonly int StartByte1 = 0x42;
        public static readonly int StartByte2 = 0x4d;
        public static readonly int CommandReadInPassive = 0xe2;
        public static readonly int CommandChangeMode = 0xe1;
        public static readonly int CommandSleep = 0xe4;
    }
}