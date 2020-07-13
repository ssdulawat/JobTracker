using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class cErrorLog
    {
        private static string m_sLogFilePath = string.Empty;
       
        public static string LogFilePath
        {
            get { return m_sLogFilePath; }
            set { m_sLogFilePath = value; }
        }

        public static void WriteLog(string sClassName, string sFunctionName, string sMsg)
        {
            string sData = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(m_sLogFilePath))
                {
                    if (!Directory.Exists(m_sLogFilePath)) Directory.CreateDirectory(m_sLogFilePath);
                    string sFileName = "ErrorLog.txt";  //DateTime.Now.ToString("yyyy_MM_dd") + 
                    string sFilePath = Path.Combine(m_sLogFilePath, sFileName);
                    sData = sData + string.Format("{0}|{1}|{2}|\t{3}", DateTime.Now.ToString("hh:mm:ss tttt"), sClassName, sFunctionName, sMsg) + Environment.NewLine;
                    File.AppendAllText(sFilePath, sData);
                }
            }
            catch (Exception)
            {

            }
        }     
    }
}