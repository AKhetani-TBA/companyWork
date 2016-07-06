using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Office.Interop.Excel;
using System.Xml.Linq;
using Microsoft.Office.Core;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Office.Tools.Excel;
using BeastCommunicationInterface;
using BeastCommunicationInterface.Excel;
using System.Threading;

namespace BeastShareAreaAddin
{
    public class BeastShareUtility
    {

        #region Variable Declaration
        public Microsoft.Office.Core.COMAddIn BeastAddin;
        public dynamic utils;

        private static volatile BeastShareUtility instance = null;
        private static object syncRoot = new Object();

        Microsoft.Office.Core.CommandBar cb = null;

        public Boolean IsShare = false;
        public bool IsConnectionExists = true;

        public String DeplymentProjectVersion = string.Empty;
        public String DirPath = string.Empty;
        public String UserID;
        string BeastExcelDecPath = string.Empty;

        public Range ExcelShareRange;
        Microsoft.Office.Tools.Excel.Worksheet WkHiddinSheet;
        private Thread _processPendingUpdate;
        public bool ShareImageStatus = false;

        Microsoft.Office.Interop.Excel.Workbook objbook = Globals.ThisAddIn.Application.ActiveWorkbook;
        Microsoft.Office.Interop.Excel.Worksheet objsheet = Globals.ThisAddIn.Application.ActiveSheet;

        public Int32 ExcelShareingID = 0;
        public Dictionary<string, string> DirExcelSheetRep;

        private Queue<UpdateExcelImage> _pendingExcelUpdate = new Queue<UpdateExcelImage>();
        public static BeastShareUtility Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new BeastShareUtility();

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

        public bool IsConnected
        {
            get;
            set;
        }
        #endregion

        #region declared class constructor
        public BeastShareUtility()
        {
            object addinRef = "TheBeastAppsAddin";
            BeastAddin = Globals.ThisAddIn.Application.COMAddIns.Item(ref addinRef);
            BeastExcelDecPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\BeastShare";
            utils = BeastAddin.Object;
            IsConnected = false;
            DirExcelSheetRep = new Dictionary<string, string>();
            _processPendingUpdate = new Thread(new ThreadStart(ProcessPendingUpdates));
            _processPendingUpdate.Name = "PendingUpdateProcessor";
            ShareCalculator.Instance._imageManager.OnCellChanged += new EventHandler<BeastCommunicationInterface.EventArgs.UpdateInfoArgs>(_imageManager_OnCellChanged);
        }
       /// <summary>
       /// True if Excel is in edit mode. Else false
       /// </summary>
       /// <returns></returns>
        private bool IsExcelInEditMode()
        {
            try
            {
                Globals.ThisAddIn.Application.Interactive = Globals.ThisAddIn.Application.Interactive;
                return false; 
            }
            catch
            {
                return true; // in edit mode
            }
        }
        /// <summary>
        /// Process all pendin updates from queue
        /// </summary>
        private void ProcessPendingUpdates()
        {
            lock (_pendingExcelUpdate)
            {
                while (_pendingExcelUpdate.Count > 0)
                {
                    dynamic updatePackage = null;
                    if (IsExcelInEditMode() == false)
                    {
                        updatePackage = _pendingExcelUpdate.Dequeue();
                        if (updatePackage != null)
                        {
                            UpdateSheet(updatePackage);
                        }
                    }
                    else
                        break;
                }
            }
        }
        /// <summary>
        /// Check if there are pending updates and process them if found
        /// </summary>
        public void CheckPendingUpdate()
        {
            try
            {
                if (_pendingExcelUpdate.Count > 0)
                {
                    if (_processPendingUpdate.IsAlive && _processPendingUpdate.ThreadState == ThreadState.Running)
                    {
                        _processPendingUpdate.Join();
                    }
                    else if (_processPendingUpdate.ThreadState != ThreadState.Unstarted)
                    {
                        _processPendingUpdate.Abort();
                        _processPendingUpdate = new Thread(new ThreadStart(ProcessPendingUpdates));
                        _processPendingUpdate.Name = "PendingUpdateProcessor";
                    }
                    _processPendingUpdate.Start();
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("BeastShareUtility.cs", "ChechPendingUpdate();",ex.Message);
            }
        }
        void _imageManager_OnCellChanged(object sender, BeastCommunicationInterface.EventArgs.UpdateInfoArgs e)
        {
            if (IsExcelInEditMode())
            {
                lock (_pendingExcelUpdate)
                {
                    _pendingExcelUpdate.Enqueue(e.UpdateInfo);
                }
            }
            else
            {
                UpdateSheet(e.UpdateInfo);
            }
        }

        private bool UpdateSheet(UpdateExcelImage updateInfo)
        {
            try
            {
                if (updateInfo == null)
                {
                    throw new ArgumentNullException("UpdateInfo is null");
                }
                Int32 StartRow = 0;
                Int32 Startcol = 0;
                if (updateInfo.UpdateSource == "Web")
                {
                    Int32 ShareID = Convert.ToInt32(ShareCalculator.Instance._imageManager.ExcelImage.sid);
                    objbook = Globals.ThisAddIn.Application.Workbooks[Convert.ToString(ShareCalculator.Instance._imageManager.ExcelImage.wbn)];
                    objsheet = Globals.ThisAddIn.Application.Worksheets[Convert.ToString(ShareCalculator.Instance._imageManager.ExcelImage.sn)];
                    Range rngd = objsheet.get_Range(Convert.ToString(ShareCalculator.Instance._imageManager.ExcelImage.rid));
                    StartRow = rngd.Row;
                    Startcol = rngd.Column;
                    string strBC, strTF, strTA;
                    foreach (var modifiedCell in updateInfo.Cells)
                    {
                        var cid = modifiedCell.CID;
                        int IndexOfRow = cid.ToString().IndexOf('R') + 1;
                        int IndexOfColumn = cid.ToString().IndexOf('C');
                        int Length = cid.ToString().Length;
                        string row = cid.ToString().Substring(IndexOfRow, IndexOfColumn - IndexOfRow);
                        string col = cid.ToString().Substring(IndexOfColumn + 1, (Length - IndexOfColumn) - 1);
                        int rowss = ShareCalculator.Instance.RowIndexes[Convert.ToInt32(row)];//Convert.ToInt32(row) + StartRow;
                        int colss = ShareCalculator.Instance.ColumnIndexes[Convert.ToInt32(col)];//Convert.ToInt32(col) + Startcol;
                        Range rng = (Range)objsheet.Cells[rowss, colss];
                        string StrBRTopC = string.Empty, StrBRBottomC = string.Empty, StrBRLeftC = string.Empty, StrBRRightC = string.Empty;
                        foreach (var _updateProps in modifiedCell.UpdatedProp)
                        {

                            string key = _updateProps.Key;
                            string values = _updateProps.Value;
                            if (key == "T")
                            {
                                ShareCalculator.Instance._checkIsExcelUpdate = false;
                                rng.Value2 = values.ToString();
                                ShareCalculator.Instance._checkIsExcelUpdate = true;
                            }
                            else if (!string.IsNullOrEmpty(values))
                            {
                                if (key == "FC")
                                    rng.Font.Color = System.Drawing.ColorTranslator.FromHtml(values.Replace("$", "#"));

                                else if (key == "BC")
                                {
                                    strBC = values.ToString();
                                    if (Convert.ToString(strBC).ToUpper() != "$FFFFFF")
                                        rng.Interior.Color = System.Drawing.ColorTranslator.FromHtml(strBC.Replace("$", "#"));
                                    else
                                        rng.Interior.ColorIndex = -4142;
                                }
                                else if (key == "FF")
                                    rng.Font.FontStyle = values;
                                else if (key == "FS")
                                    rng.Font.Size = values;
                                else if (key == "FS")
                                    rng.Font.Size = values;
                                else if (key == "CW")
                                    rng.ColumnWidth = (Convert.ToInt32(values)) * (8.43 / 64);
                                else if (key == "RW")
                                    rng.RowHeight = (Convert.ToInt32(values)) * (15.00 / 20);
                                else if (key == "TF")
                                {
                                    strTF = values;
                                    if (strTF.Split('_')[0] == "1")
                                    { rng.Font.Bold = true; }
                                    if (strTF.Split('_')[1] == "1")
                                    { rng.Font.Italic = true; }
                                    if (strTF.Split('_')[2] == "1")
                                    { rng.Font.Underline = 2; }
                                }
                                else if (key == "BRW")
                                {
                                    string[] strBRWAry = values.Split(';');
                                    string StrBRTopW = strBRWAry[0].Split('=')[1];
                                    string StrBRBottomW = strBRWAry[1].Split('=')[1];
                                    string StrBRLeftW = strBRWAry[2].Split('=')[1];
                                    string StrBRRightW = strBRWAry[3].Split('=')[1];

                                    if (StrBRTopW != "none")
                                    {
                                        rng.Borders[XlBordersIndex.xlEdgeTop].Color = System.Drawing.ColorTranslator.FromHtml(StrBRTopC);
                                        if (StrBRTopW == "solid")
                                            rng.Borders[XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                                        else if (StrBRTopW == "dashed")
                                            rng.Borders[XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDash;
                                        else if (StrBRTopW == "dotted")
                                            rng.Borders[XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDot;
                                        else if (StrBRTopW == "double")
                                            rng.Borders[XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDouble;
                                    }
                                    if (StrBRBottomW != "none")
                                    {
                                        rng.Borders[XlBordersIndex.xlEdgeBottom].Color = System.Drawing.ColorTranslator.FromHtml(StrBRBottomC);
                                        if (StrBRBottomW == "solid")
                                            rng.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                                        else if (StrBRBottomW == "dashed")
                                            rng.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDash;
                                        else if (StrBRBottomW == "dotted")
                                            rng.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDot;
                                        else if (StrBRBottomW == "double")
                                            rng.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDouble;

                                    }
                                    if (StrBRLeftW != "none")
                                    {
                                        rng.Borders[XlBordersIndex.xlEdgeLeft].Color = System.Drawing.ColorTranslator.FromHtml(StrBRLeftC);
                                        if (StrBRLeftW == "solid")
                                            rng.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                                        else if (StrBRLeftW == "dashed")
                                            rng.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDash;
                                        else if (StrBRLeftW == "dotted")
                                            rng.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDot;
                                        else if (StrBRLeftW == "double")
                                            rng.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDouble;

                                    }
                                    if (StrBRRightW != "none")
                                    {
                                        rng.Borders[XlBordersIndex.xlEdgeRight].Color = System.Drawing.ColorTranslator.FromHtml(StrBRRightC);
                                        if (StrBRRightW == "solid")
                                            rng.Borders[XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                                        else if (StrBRRightW == "dashed")
                                            rng.Borders[XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDash;
                                        else if (StrBRRightW == "dotted")
                                            rng.Borders[XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDot;
                                        else if (StrBRRightW == "double")
                                            rng.Borders[XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDouble;
                                    }
                                }
                                else if (key == "BRC")
                                {
                                    string[] strBRCAry = values.Split(';');
                                    StrBRTopC = (strBRCAry[0].Split('=')[1]).Replace("$", "#");
                                    StrBRBottomC = (strBRCAry[1].Split('=')[1]).Replace("$", "#");
                                    StrBRLeftC = (strBRCAry[2].Split('=')[1]).Replace("$", "#");
                                    StrBRRightC = (strBRCAry[3].Split('=')[1]).Replace("$", "#");

                                    if (Convert.ToString(StrBRTopC).ToUpper() == "#CCCCCC" && rng.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle == -4142)
                                        rng.Borders[XlBordersIndex.xlEdgeTop].LineStyle = -4142;

                                    if (Convert.ToString(StrBRBottomC).ToUpper() == "#CCCCCC" && rng.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle == -4142)
                                        rng.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = -4142;

                                    if (Convert.ToString(StrBRLeftC).ToUpper() == "#CCCCCC" && rng.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle == -4142)
                                        rng.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = -4142;

                                    if (Convert.ToString(StrBRRightC).ToUpper() == "#CCCCCC" && rng.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle == -4142)
                                        rng.Borders[XlBordersIndex.xlEdgeRight].LineStyle = -4142;

                                }
                                else if (key == "TA")
                                {
                                    strTA = values.ToString();
                                    if (strTA.Split('_')[0] == "left")
                                        rng.HorizontalAlignment = -4131;
                                    else if (strTA.Split('_')[0] == "right")
                                        rng.HorizontalAlignment = -4152;
                                    else if (strTA.Split('_')[0] == "center")
                                        rng.HorizontalAlignment = -4108;
                                    else if (strTA.Split('_')[0] == "justify")
                                        rng.HorizontalAlignment = -4130;
                                    if (strTA.Split('_')[1] == "top")
                                        rng.VerticalAlignment = -4160;
                                    else if (strTA.Split('_')[1] == "bottom")
                                        rng.VerticalAlignment = -4107;
                                    else if (strTA.Split('_')[1] == "middle")
                                        rng.VerticalAlignment = -4108;
                                    else if (strTA.Split('_')[1] == "central")
                                        rng.VerticalAlignment = -4108;
                                }
                                //else if (key == "RO")
                                //{
                                //    if (modifiedCell.UpdatedProp["RO"] == "0")
                                //        rng.Value2 = values;
                                //}

                            }
                        }

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                //utils.ErrorLog(ex.Message + Environment.NewLine + ex.StackTrace);
                return false;
            }
        }
        #endregion

        #region Excel Share get updates
        public void ExcelShareUpdates(System.Data.DataTable dtGrid, string CalcName)
        {
            try
            {
                if (CalcName == "vcm_calc_exceljsonshare")
                {
                    int TableRowCount = dtGrid.Rows.Count;
                    for (int i = 0; i < TableRowCount; i++)
                    {
                        if (dtGrid.Rows[i]["i"].ToString() == "2" && dtGrid.Rows[i]["d"].ToString() != string.Empty)
                            ShareCalculator.Instance._imageManager.UpdateImageAsync(dtGrid.Rows[i]["d"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                string strmsg = "Excel share image name: " + CalcName + "; Record updated count: " + dtGrid.Rows.Count;
                utils.ErrorLog("Utilites.cs", "ExcelShareUpdates();", strmsg, ex);
            }
        }
        #endregion

        #region Sending Imge request,Flag,Qucips to addin
        public void SendImageRequest()
        {
            utils = BeastAddin.Object;
            utils.LogInfo("BeastShareUtility.cs", "SendImageRequest();", "Send image request - Beast_BeastShare_AddIn");
            utils.SendImageRequest("vcm_calc_exceljsonshare", 6003, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        public void CloseImageRequest()
        {
            utils = BeastAddin.Object;
            utils.LogInfo("BeastShareUtility.cs", "CloseImageRequest();", "Close image request - Beast_BeastShare_AddIn");
            utils.CloseImageRequest("vcm_calc_exceljsonshare", 6003, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }
        #endregion

        #region Get installed registry path
        public void SetInstalledPathFromRegistry()
        {
            try
            {
                DirPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\BeastShare";
            }
            catch (Exception ex)
            {
                utils.ErrorLog("Utilities.cs", "SetInstalledPathFromRegistry();", ex.Message, ex);
            }
        }
        #endregion

        #region Delete Context menu
        public void DeleteContextMenu()
        {
            try
            {
                cb = Globals.ThisAddIn.Application.CommandBars["Cell"];
                cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Share Area", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Delete();
                cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Re-Share Area", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Delete();
                cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "End Share", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Delete();

            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility.cs", "DeleteContextMenu();", "delete all the custom right click menus", ex);
            }
        }
        #endregion

        public void ConnectCalc()
        {
            try
            {
                cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Share Area", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Enabled = true;
                cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Re-Share Area", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Enabled = false;
                cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "End Share", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Enabled = false;
            }
            catch (Exception ex)
            {
                utils.ErrorLog("BeastShareUtilty.cs", "ConnectCalc();", "Custom Connection method", ex);
            }
        }
        public void DisconnectCalc()
        {
            try
            {
                cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Share Area", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Enabled = false;
                cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Re-Share Area", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Enabled = false;
                cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "End Share Area", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Enabled = false;

                utils.LogInfo("CustomUtility.cs", "DisconnectCalc();", "Disconnecting all the calc after internet connection is disabled");

            }
            catch (Exception ex)
            {
                utils.ErrorLog("BeastShareUtilty.cs", "ConnectCalc();", "Custom Connection method", ex);
            }
        }

        public void BindEvent()
        {
            try
            {
                #region Bonds sheet bonding
                utils = BeastAddin.Object;
                #endregion

                //WkHiddinSheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.Worksheets["Common"]);
                //if (WkHiddinSheet != null)
                //{
                //    if (WkHiddinSheet.Cells[1, 1].Value != null)
                //        WkHiddinSheet.Cells[1, 1].Value = Convert.ToInt32(WkHiddinSheet.Cells[1, 1].Value) + 1;
                //    else
                //        WkHiddinSheet.Cells[1, 1].Value = 1;

                //    Int32 RowCount = Convert.ToInt32(WkHiddinSheet.Cells[1, 1].Value);
                //    Range ShareRange = (Range)WkHiddinSheet.Cells[RowCount + 1, 1];
                //    ShareRange.Name = "vcm_calc_beastshare";
                //    ShareRange.ID = "Beast_Share_AddIn";
                //    ShareRange.Value2 = false;
                //    NamedRange ShareRangeNr = WkHiddinSheet.Controls.AddNamedRange(ShareRange, "vcm_calc_beastshare");
                //    ShareRangeNr.Change += new DocEvents_ChangeEventHandler(ShareRangeNr_Change);

                //}

            }
            catch (Exception ex)
            {
                utils.ErrorLog("BeastShareUtility.cs", "BindEvent();", ex.Message, ex);
            }
        }

        void ShareRangeNr_Change(Range Target)
        {
            try
            {
                ShareImageStatus = Target.Value2;
                if (ShareImageStatus == true && (Target.ID == null || Target.ID == "0"))
                    Target.ID = "1";
                else
                    Target.ID = "0";

                cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Share Area", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Enabled = ShareImageStatus;
            }
            catch (Exception ex)
            {

                utils.ErrorLog("BeastShareUtility.cs", "ShareRangeNr_Change();", "Passing Target Value", ex);
            }
            //throw new NotImplementedException();
        }

        string ReturnSpecialValue(string Text)
        {
            Text = Text.Replace("&amp;", "&");
            Text = Text.Replace("&apos;", "'");
            Text = Text.Replace("&quot;", "\"");
            Text = Text.Replace("&lt;", "<");
            Text = Text.Replace("&gt;", ">");
            return Text;
        }

        #region Dispose class
        public void Dispose()
        {
            instance = null;
        }

        #endregion
    }
}
