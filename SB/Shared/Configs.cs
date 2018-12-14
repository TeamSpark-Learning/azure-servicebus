using System.Diagnostics;

namespace Shared
{
    public static class Configs
    {
        public static string SbConnectionString => "";
        
        public static int PID => Process.GetCurrentProcess().Id;
    }
}