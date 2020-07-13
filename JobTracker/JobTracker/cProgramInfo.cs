using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker
{
    static public class cProgramInfo
    {
        public static string sProductTitle = string.Empty;
        public static string sProductName = string.Empty;
        public static string sProductDescription = string.Empty;
        public static string sProductCompanyName = string.Empty;
        public static string sProductCopyright = string.Empty;
        public static string sAssemblyGuid = string.Empty;
        static cProgramInfo()
        {
            sProductTitle = GetProductTitle();
            sProductName = GetProductName();
            sProductDescription = GetProductDescription();
            sProductCompanyName = GetProductCompanyName();
            sProductCopyright = GetProductCopyright();
            sAssemblyGuid = GetAssemblyGuid();
        }
        static string GetProductTitle()
        {
            return ((AssemblyTitleAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0]).Title;
        }
        static string GetProductName()
        {
            return ((AssemblyProductAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false)[0]).Product;
        }
        static string GetProductDescription()
        {
            return ((AssemblyDescriptionAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0]).Description;
        }
        static string GetProductCopyright()
        {
            return ((AssemblyCopyrightAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0]).Copyright;
        }
        static string GetProductCompanyName()
        {
            return ((AssemblyCompanyAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false)[0]).Company;
        }
        static string GetAssemblyGuid()
        {
            return ((System.Runtime.InteropServices.GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(System.Runtime.InteropServices.GuidAttribute), false)[0]).Value;
        }
    }
}
