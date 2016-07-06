using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;

namespace ExcelWorkbook1
{
    [RunInstaller(true)]
    public partial class KCGWorkBook : System.Configuration.Install.Installer
    {
        public KCGWorkBook()
        {
            InitializeComponent();
        }
        protected override void OnBeforeInstall(IDictionary savedState)
        {

            try
            {

                base.OnBeforeInstall(savedState);
                DialogResult result = DialogResult.None;
                while (isExcelRunning())
                {
                    result = MessageBox.Show("Excel is still running. Please save your work and close Excel, and then click \"Retry\" button to continue with installation.", "", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.Cancel)
                    {
                        throw new InstallException("Installation process is been canceled.");
                        break;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new InstallException("Installation process is been canceled.");
            }
        }
        private bool isExcelRunning()
        {

            System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcessesByName("Excel");
            bool flag = false;
            foreach (System.Diagnostics.Process p in process)
            {
                if (!string.IsNullOrEmpty(p.ProcessName))
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }
        public override void Uninstall(System.Collections.IDictionary savedState)
        {

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            String DeplymentProjectVersion = fileVersionInfo.ProductName;
            RegistryKey BaseKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Office\\Excel\\Addins\\" + DeplymentProjectVersion, true);
            if (BaseKey != null)
            {
                try
                {
                    RegistryKey UserName = BaseKey.OpenSubKey("UserCredential");
                    if (UserName != null)
                    {
                        BaseKey.DeleteSubKeyTree("UserCredential");
                    }
                }
                catch { }
                BaseKey.Dispose();
            }
        }
    }
}
