using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.Xml;
using Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Diagnostics;

namespace ExcelAddIn5
{
    class ShareCalculator
    {
        Microsoft.Office.Core.CommandBarButton btnMenuShare;
        Microsoft.Office.Core.CommandBar cb = null;
        private static volatile ShareCalculator instance = null;
        private static object syncRoot = new Object();

        public static ShareCalculator Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ShareCalculator();
                        }
                    }
                }

                return instance;
            }
            set
            {
                instance = value;
            }
        }

        public void ContextMenuButton()
        {
            /*try
            {
                cb = Globals.ThisAddIn.Application.CommandBars["Cell"];
                btnMenuShare = (CommandBarButton)cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Share Calculator", System.Reflection.Missing.Value, System.Reflection.Missing.Value);

                if (btnMenuShare == null)
                {
                    btnMenuShare = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                    btnMenuShare.Caption = "Share Calculator";
                    btnMenuShare.Tag = "Share Calculator";
                    btnMenuShare.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                    btnMenuShare.Click += new _CommandBarButtonEvents_ClickEventHandler(btnMenuShare_Click);
                    btnMenuShare.Visible = true;
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("Login.cs", "ContextMenuButton();", ex.Message, ex);
            }*/
        }

        private void btnMenuShare_Click(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                MessageFilter.Register();
                Microsoft.Office.Interop.Excel.Range SelectRange = (Range)Globals.ThisAddIn.Application.Selection;

                if (SelectRange.Cells.Name.Name != null)
                {
                    //  Utilities.Instance.IsShare = true;
                    UpdateManager.Instance.CacluatorName = SelectRange.Cells.Name.Name.Substring(0, SelectRange.Cells.Name.Name.LastIndexOf('_'));
                    Share ObjShare = new Share();
                    ObjShare.ShowDialog();
                }
                else
                {
                    Messagecls.AlertMessage(15, "");
                }
                MessageFilter.Revoke();
            }
            catch (Exception ex)
            {
                LogUtility.Error("Login.cs", "btnShare_Click();", ex.Message, ex);
            }
        }

        public void DeleteShareButton()
        {
            //btnMenuShare = (CommandBarButton)Globals.ThisAddIn.Application.CommandBars["Cell"].FindControl(MsoControlType.msoControlButton, Type.Missing, "Share Calculator", System.Reflection.Missing.Value, System.Reflection.Missing.Value);
            //btnMenuShare.Delete();
        }
    }
}
