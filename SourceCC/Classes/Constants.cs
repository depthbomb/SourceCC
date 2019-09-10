using System;
using System.IO;
using System.Diagnostics;

namespace SourceCC.Classes
{
    class Constants
    {
        public static readonly string DataPath        = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SourceCC");
        public static readonly string ConfigPath      = Path.Combine(DataPath, "SourceCC.ini");
        public static readonly bool Is64BitOs         = Environment.Is64BitOperatingSystem;
        public static readonly string TF2DefaultPath  = Is64BitOs ? @"C:\Program Files (x86)\Steam\SteamApps\common\Team Fortress 2\tf" : @"C:\Program Files\Steam\SteamApps\common\Team Fortress 2\tf";
        public static readonly string L4D2DefaultPath = Is64BitOs ? @"C:\Program Files (x86)\Steam\SteamApps\common\Left 4 Dead 2\left4dead2" : @"C:\Program Files\Steam\SteamApps\common\Left 4 Dead 2\left4dead2";
    }
}
