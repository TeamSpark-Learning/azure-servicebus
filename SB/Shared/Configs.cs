using System.Diagnostics;

namespace Shared
{
    public static class Configs
    {
        public static string SbFailoverConnectionString => "";
        
        public static string SbPrimaryConnectionString => "";
        public static string SbSecondaryConnectionString => "";
        
        public static int PID => Process.GetCurrentProcess().Id;
    }
}