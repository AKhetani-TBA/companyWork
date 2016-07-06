using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ExcelAddIn5
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Item_Get(bool clickFromButton)
        {
            var items = chkListAddins.Items;
            RegistryKey BaseKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Office\\Excel\\Addins\\", true);
            BaseKey.GetSubKeyNames().ToList().ForEach(sAddInName =>
            {
                if (!sAddInName.Equals("TheBeastAppsAddin"))
                {
                    try
                    {
                        if (BaseKey != null)
                        {
                            RegistryKey SubKeyValue = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Office\\Excel\\Addins\\" + sAddInName.ToString(), true);
                            if (SubKeyValue != null)
                            {
                                string sFriendlyName = SubKeyValue.GetValue("FriendlyName").ToString();

                                if (!string.IsNullOrEmpty(sAddInName) && !sAddInName.Equals("TheBeastAppsAddin") && !clickFromButton)
                                {
                                    items.Add(sFriendlyName);
                                    if (SubKeyValue.GetValue("LoadBehavior").Equals(3))
                                    {
                                        chkListAddins.SetItemChecked(chkListAddins.Items.IndexOf(sFriendlyName), true);
                                    }
                                }
                                if (clickFromButton)
                                {
                                    if (chkListAddins.GetItemChecked(chkListAddins.Items.IndexOf(sFriendlyName)).Equals(true))
                                    {
                                        SubKeyValue.SetValue("LoadBehavior", 3);

                                        object addinRef = sAddInName;
                                        Microsoft.Office.Core.COMAddIn G_Addi = Globals.ThisAddIn.Application.COMAddIns.Item(ref addinRef);
                                        if (G_Addi.Connect == false)
                                        {
                                            G_Addi.Connect = true;
                                            if (DataUtil.Instance.bIsUserLoggedIn)
                                                ConnectionManager.Instance.LoadCustomAddIn(sAddInName);
                                        }
                                    }
                                    if (chkListAddins.GetItemChecked(chkListAddins.Items.IndexOf(sFriendlyName)).Equals(false) && SubKeyValue.GetValue("LoadBehavior").Equals(3))
                                    {
                                        SubKeyValue.SetValue("LoadBehavior", 2);

                                        object addinRef = sAddInName;
                                        Microsoft.Office.Core.COMAddIn G_Addi = Globals.ThisAddIn.Application.COMAddIns.Item(ref addinRef);
                                        if (G_Addi.Connect == true)
                                        {
                                            if (DataUtil.Instance.bIsUserLoggedIn)
                                                ConnectionManager.Instance.UnloadCustomAddIn(sAddInName);
                                            G_Addi.Connect = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogUtility.Error("ListAddins.cs", "Item_Get", ex.Message, ex);
                    }
                }
            });
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            //if (DataUtil.Instance.bIsUserLoggedIn)
            //    chkListAddins.Enabled = false;

            Item_Get(false);
            cmbLogLevel.SelectedIndex = DataUtil.Instance.LogLevel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DataUtil.Instance.LogLevel = cmbLogLevel.SelectedIndex;
            Item_Get(true);
            this.Close();
        }

        private void Settings_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
    }
}
