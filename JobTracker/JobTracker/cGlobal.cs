using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker
{
    public static class cGlobal
    {
        public static string sProgramDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), cProgramInfo.sProductName);
        public static string sLogPath = Path.Combine(sProgramDataPath, "Log");
        public static string sApplicationPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string sSettingFilePath = Path.Combine(sApplicationPath, "ConnectionStringSetting");
    }
}