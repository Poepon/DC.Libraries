namespace CX.Web.Themes
{
    public static class DeviceSupport
    {
        /// <summary>
        /// Set True distinguish PC or Mobile,Set False not distinguish
        /// </summary>
        public static bool DistinguishDevice { get; set; }

        /// <summary>
        /// Force Mode
        /// </summary>
        public static DeviceType ForceMode { get; set; } = DeviceType.Auto;
    }

    public enum DeviceType
    {
        Auto = 0,
        PC = 1,
        Mobile = 2
    }
}