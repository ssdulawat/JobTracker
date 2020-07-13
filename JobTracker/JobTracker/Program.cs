using Common;
using JobTracker.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobTracker
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            InitializeValues();
            Application.Run(new FrmJTLogin());
        }

        private static void InitializeValues()
        {
            try
            {
                cErrorLog.LogFilePath = cGlobal.sLogPath;
            }
            catch (Exception ex)
            {
                cErrorLog.WriteLog("Program", "InitializeValues", ex.Message);
            }
        }

        public static void LoadDefaultSettings()
        {
            try
            {
                if (!System.IO.File.Exists(cGlobal.sApplicationPath + @"\VESoftwareSetting.xml"))
                {
                    if (System.IO.File.Exists(cGlobal.sSettingFilePath + @"\VESoftwareSetting_Default.xml"))
                    {
                        System.IO.File.Copy(cGlobal.sSettingFilePath + @"\VESoftwareSetting_Default.xml", cGlobal.sApplicationPath + @"\VESoftwareSetting.xml");
                        System.IO.File.Delete(cGlobal.sApplicationPath + @"\VESoftwareSetting_Default.xml");
                    }
                    else
                        MessageBox.Show("Unable to load default settings. Please contact support.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void Showform()
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.IsMdiContainer == true)
                    frm.Close();
            }
        }
    }
}