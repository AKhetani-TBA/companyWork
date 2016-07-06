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
namespace ExcelAddIn5
{
    [RunInstaller(true)]
    public partial class BeastExcel : System.Configuration.Install.Installer
    {
        public BeastExcel()
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
            catch (Exception ex)
            {
                throw new InstallException("Installation process is been cancelled.", ex);
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
                throw new InstallException("Uninstallation process is been cancelled.");
            }
            Globals.ThisAddIn.Dispose();
        }
        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            //  base.Uninstall(savedState);
            //RegistrationServices regSrv = new RegistrationServices();
            //regSrv.UnregisterAssembly(base.GetType().Assembly);

            RegistryKey BaseKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Office\\Excel\\Addins\\" + DataUtil.Instance.ProductName, true);
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
            //System.Diagnostics.Process.Start("Excel");
        }
    }
}
