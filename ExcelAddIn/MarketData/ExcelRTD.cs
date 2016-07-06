using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using Microsoft.Win32;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;

namespace Beast_MarketData_Addin
{
    [RunInstaller(true)]
    public partial class ExcelRTD : System.Configuration.Install.Installer
    {
        public ExcelRTD()
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
                    result = MessageBox.Show("A copy of Excel is still running. Please save your work,  close Excel, and then click \"Retry\" button to continue with the installation.", "", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    if (result == DialogResult.Cancel)
                    {
                        throw new InstallException("Installation process is been cancelled.");
                    }
                }
            }
            catch
            {
                throw new InstallException("Installation process is been cancelled.");
            }
        }
        private bool isExcelRunning()
        {
            try
            {
                System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcessesByName("Excel");
                foreach (System.Diagnostics.Process p in process)
                {
                    if (!string.IsNullOrEmpty(p.ProcessName))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {   
                throw;
            }
        }

        protected override void OnBeforeUninstall(IDictionary savedState)
        {
            base.OnBeforeUninstall(savedState);
            try
            {
                DialogResult result = DialogResult.None;
                while (isExcelRunning())
                {
                    result = MessageBox.Show("Excel is still running. Please save your work and close Excel, and then click \"Retry\" button to continue with uninstallation.", "", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    if (result == DialogResult.Cancel)
                    {
                        throw new InstallException("Uninstallation process is been cancelled.");
                    }
                }
            }
            catch
            {
                throw;
            }

            Globals.ThisAddIn.Dispose();
        }

        public override void Rollback(IDictionary savedState)
        {
            try
            {
                base.Rollback(savedState);
            }
            catch
            {
            }
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

        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
          //  System.Diagnostics.Process.Start("Excel");
        }
    }
}
