using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Xml;
using System.Drawing;
using Microsoft.Office.Tools.Excel;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System.ComponentModel;
namespace ExcelAddIn5
{
    class CalculatorDesign : IDisposable
    {
        #region Declare Varible And Crerate Class Instance
        private static volatile CalculatorDesign instance = null;
        private static object syncRoot = new Object();
        private String _dirpath;
        String ControlType;
        String Title;
        String ControlID;
        String CellValue;
        String Imagepath = string.Empty;
        public System.Windows.Forms.Button Btn;
        ComboBox cb;
        Range Cell;
        Range currentSelectedCell;
        Int32 Rows = 0;
        Int32 Columns = 0;
        Int32 CalTitleCnt = 0;
        Int32 StartNewRow = 0;
        Int32 CountCols = 0;
        Int32 MaxCol = 0;
        Int32 CCount = 0;
        System.Data.DataTable dt = new System.Data.DataTable();
        BackgroundWorker SendDataBK;
        Int32 CalPlacing = 0;
        public static CalculatorDesign Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new CalculatorDesign();
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
        public String DirPath
        {
            get
            {
                return _dirpath;
            }
            set
            {
                _dirpath = value;
            }
        }
        public CalculatorDesign()
        {
            DirPath = string.Empty;
            SendDataBK = new BackgroundWorker();
            SendDataBK.DoWork += new DoWorkEventHandler(SendDataBK_DoWork);
            SendDataBK.RunWorkerCompleted += new RunWorkerCompletedEventHandler(SendDataBK_RunWorkerCompleted);
        }
        void SendDataBK_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SendDataBK.WorkerSupportsCancellation = true;
            SendDataBK.CancelAsync();
        }
        void SendDataBK_DoWork(object sender, DoWorkEventArgs e)
        {
            dynamic getArguement = e.Argument;
            String eleID = getArguement[0].ToString();
            String eleVal = getArguement[1].ToString();
            //to be implement
            SignalRConnectionManager.Instance.SendDataToImage(eleID.Substring(0, eleID.LastIndexOf('_')), AuthenticationManager.Instance.userID, AuthenticationManager.Instance.customerID, Utilities.Instance.InstanceType, eleID, eleVal, 1);
        }
        #endregion
        #region Change Events and Range Fuction
        private void GetCellchange(Excel.Range Target)
        {
            try
            {
                string eleID = Convert.ToString(Target.Cells.Name.Name);
                string eleVal = Convert.ToString(Target.Cells.Value2);

                string calcName = eleID.Substring(0, eleID.LastIndexOf('_'));
                int indxs = Utilities.Instance.GetWorksheetByCalcName(calcName).Controls.IndexOf(eleID);
                Microsoft.Office.Tools.Excel.NamedRange NameRange = (NamedRange)Utilities.Instance.GetWorksheetByCalcName(calcName).Controls[indxs];
                if (Target.Value2 != null)
                {
                    NameRange.Change -= new DocEvents_ChangeEventHandler(GetCellchange);
                    if (Convert.ToString(Target.Cells.Name.Name) == Convert.ToString(currentSelectedCell.Cells.Name.Name))
                    {
                        if (Convert.ToString(Target.Value2) != Convert.ToString(currentSelectedCell.ID))
                        {
                            if (Target.Interior.Color == 14215660.0)
                            {
                                Target.Value2 = CellValue;
                                CellValue = null;
                            }
                            else
                            {
                                object[] objTemp = { eleID, eleVal };
                                SendDataBK.RunWorkerAsync(objTemp);
                            }
                        }
                        Target.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Gray);
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                string ss = ex.Message;
#endif
                LogUtility.Error("CalculatorDesgin.cs", "GetCellchange()", Target.get_Address(), ex);
            }
        }
        void NRngs_SelectionChange(Range Target)
        {
            try
            {
                string eleID = Convert.ToString(Target.Cells.Name.Name);
                string eleVal = Convert.ToString(Target.Cells.Value2);
                string calcName = eleID.Substring(0, eleID.LastIndexOf('_'));
                int indx = Utilities.Instance.GetWorksheetByCalcName(calcName).Controls.IndexOf(eleID);
                Microsoft.Office.Tools.Excel.NamedRange NameRange = (NamedRange)Utilities.Instance.GetWorksheetByCalcName(calcName).Controls[indx];
                if (currentSelectedCell != null && currentSelectedCell.Name != null)
                {
                    var varCalcName = currentSelectedCell.Name;
                    String CalcName = varCalcName.Name;
                    NameRange.Change -= new DocEvents_ChangeEventHandler(GetCellchange);
                    string crntCellID = Convert.ToString(currentSelectedCell.Value2);
                    currentSelectedCell.Value2 = currentSelectedCell.ID;
                    currentSelectedCell.ID = crntCellID;
                }
                if (Target.Value2 != null && Target.Name != null)
                {
                    CellValue = Target.ID;
                    String TargetValue = Target.Value2.ToString();
                    Target.Value2 = Target.ID;
                    Target.ID = TargetValue;
                    var varCalcName = Target.Name;
                    String CalcName = varCalcName.Name;
                    NameRange.Change += new DocEvents_ChangeEventHandler(GetCellchange);
                }
                else if (Target.Name != null)
                {
                    if (calcName.ToString() == "vcm_calc_kcg_bondpoint_DepthOfBook")
                    {
                        NameRange.Change += new DocEvents_ChangeEventHandler(GetCellchange);
                    }
                }
                currentSelectedCell = Target;
            }
            catch (Exception ex)
            {
#if DEBUG
                string ss = ex.Message;
#endif
                LogUtility.Error("CalculatorDesgin.cs", "NRngs_SelectionChange()", Target.get_Address(), ex);
            }
        }
        void Cell_Deselected(Range Target)
        {
            try
            {
                if (Target.ID == null)
                {
                    if (currentSelectedCell != null && currentSelectedCell.Name != null)
                    {
                        string crntCellID = currentSelectedCell.Value2;
                        currentSelectedCell.Value2 = currentSelectedCell.ID;
                        currentSelectedCell.ID = crntCellID;
                        currentSelectedCell = null;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("CalculatorDesgin.cs", "Cell_Deselected()", "CellChange Deleget Call", ex);

            }
        }
        void cb_LostFocus(object sender, EventArgs e)
        {
            try
            {
                ComboBox cbname = (ComboBox)sender;
                string calcName = cbname.Name.Substring(0, cbname.Name.LastIndexOf('_'));
                if (cbname.SelectedValue.ToString() != "-1")
                {
                    object[] objTemp = { cbname.Name, cbname.SelectedValue.ToString() };
                    SendDataBK.RunWorkerAsync(objTemp);
                }
            }
            catch (Exception ee)
            {

#if DEBUG
                string ss = ee.Message;
#endif
                LogUtility.Error("CalculatorDesgin.cs", "cb_LostFocus()", "ComboBox Lost Focus Event", ee);
            }

        }
        void cb_GotFocus(object sender, EventArgs e)
        {
            try
            {
                ComboBox cbname = (ComboBox)sender;
            }
            catch (Exception ex)
            {
                LogUtility.Error("CalculatorDesgin.cs", "cb_GotFocus()", "ComboBox Get On Focus Event", ex);

            }
        }
        void Btn_Click(object sender, EventArgs e)
        {
            try
            {
                Btn = (System.Windows.Forms.Button)sender;
                if (Btn.Text == "Close")
                {
                    SignalRConnectionManager.Instance.StopCalculatorUpdate(Btn.Name, AuthenticationManager.Instance.userID, AuthenticationManager.Instance.customerID, Utilities.Instance.InstanceType);
                    if (UpdateManager.Instance.CalcPlacingonSheet.ContainsKey(Btn.Name))
                    {
                        string[] splitcalcposition = UpdateManager.Instance.CalcPlacingonSheet[Btn.Name].Split('^');

                        foreach (var strtemp in UpdateManager.Instance.CalcRowColSheet.Select(x => UpdateManager.Instance.CalcRowColSheet[Globals.ThisAddIn.Application.ActiveWorkbook.Name + "^" + Utilities.Instance.GetWorksheetByCalcName(Btn.Name).Name]).ToList())
                        {
                            List<string> strpostionlist = strtemp;
                            foreach (var strpostion in strpostionlist)
                            {
                                try
                                {
                                    string[] splitstrpostion = strpostion.Split('^');
                                    Int32 StartRow = Convert.ToInt32(splitstrpostion[0]);
                                    Int32 StartColumn = Convert.ToInt32(splitstrpostion[1]);
                                    Int32 TotalRow = Convert.ToInt32(splitstrpostion[2]) + 2;
                                    Int32 TotalColumn = Convert.ToInt32(splitstrpostion[3]) - 1;

                                    if (StartRow == Convert.ToInt32(splitcalcposition[0]) && StartColumn == Convert.ToInt32(splitcalcposition[1]))
                                    {
                                        Utilities.Instance.DeleteStatusImage(Btn.Name);
                                        Excel.Range rng = (Excel.Range)Utilities.Instance.GetWorksheetByCalcName(Btn.Name).get_Range(Utilities.Instance.GetWorksheetByCalcName(Btn.Name).Cells[StartRow, StartColumn], Utilities.Instance.GetWorksheetByCalcName(Btn.Name).Cells[Convert.ToInt32(StartRow + TotalRow - 2), Convert.ToInt32(StartColumn + TotalColumn + 1)]);
                                        Utilities.Instance.GetWorksheetByCalcName(Btn.Name).Controls.Remove(Btn);
                                        foreach (Range cell in rng.Cells)
                                        {
                                            try
                                            {
                                                if (cell.Name != null)
                                                {
                                                    Utilities.Instance.GetWorksheetByCalcName(Btn.Name).Controls.Remove(cell.Name.Name);
                                                    Globals.ThisAddIn.Application.ActiveWorkbook.Names.Item(cell.Name.Name, Type.Missing, Type.Missing).Delete();
                                                }
                                            }
                                            catch { }
                                        }
                                        rng.Clear();
                                        if (strpostionlist.Count == 1)
                                            UpdateManager.Instance.CalcRowColSheet.Remove(Globals.ThisAddIn.Application.ActiveWorkbook.Name + "^" + Utilities.Instance.GetWorksheetByCalcName(Btn.Name).Name);
                                        else
                                            UpdateManager.Instance.CalcRowColSheet[Globals.ThisAddIn.Application.ActiveWorkbook.Name + "^" + Utilities.Instance.GetWorksheetByCalcName(Btn.Name).Name].Remove(strpostion);
                                        UpdateManager.Instance.CalcPlacingonSheet.Remove(Btn.Name);
                                        UpdateManager.Instance.CalcSheetMap.Remove(Btn.Name);

                                        break;
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
                else
                {
                    // to be implement
                    SignalRConnectionManager.Instance.SendDataToImage(Btn.Name.Substring(0, Btn.Name.LastIndexOf('_')), AuthenticationManager.Instance.userID, AuthenticationManager.Instance.customerID, Utilities.Instance.InstanceType, Btn.Name, Btn.Tag.ToString(), 1);
                    string tag = Convert.ToString(Btn.Tag);
                    if (tag.Equals("1")) { Btn.Tag = "0"; } else { Btn.Tag = "1"; }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("CalculatorDesgin.cs", "Btn_Click()", "All Button click Event", ex);
            }
        }
        #endregion
        #region Create & Render Calculator
        public void createCalculator(string calcID, string calcTitle, Int32 StartRow, Int32 StartColumn)
        {
            Rows = Columns = CalTitleCnt = StartNewRow = CountCols = MaxCol = CCount = 0;
            string calcXmlFileName = GetXmlFileName(calcID);
            CreateDesgin(calcXmlFileName, StartRow, StartColumn, calcID, calcTitle);
            SignalRConnectionManager.Instance.SendImageRequest(calcID, 1);
        }

        private string GetFileName(System.Data.DataTable dt, string CalcID)
        {
            try
            {
                string XMLFile;
                string XMLFileName = string.Empty;
                XmlDocument xdoc = new XmlDocument();
                XMLFileName = Convert.ToDateTime(dt.Rows[0]["Modified_Date"]).ToString("dd_MM_yyyy~HH-mm");
                XMLFile = "<?xml version='1.0' encoding='utf-8'?>" + Convert.ToString(dt.Rows[0]["XML"]);
                xdoc.LoadXml(XMLFile);
                xdoc.Save(DataUtil.Instance.DirectoryPath + "\\Products\\VCM\\" + CalcID + "\\" + XMLFileName + ".xml");
                return XMLFileName;
            }
            catch (Exception ex)
            {
                string msg = "Calculator Name: " + CalcID;
                LogUtility.Error("Utilities.cs", "GetFileName();", msg, ex);
                return "";
            }
        }

        public string GetXmlFileName(String CalcID)
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                string XMLFileName = string.Empty;
                DataSet ds = new DataSet();

                if (!Directory.Exists(DataUtil.Instance.DirectoryPath + "\\Products\\VCM\\" + CalcID)) //xml file is not exists
                {
                    try
                    {
                        Directory.CreateDirectory(DataUtil.Instance.DirectoryPath + "\\Products\\VCM\\" + CalcID);
                        //ds = AuthenticationManager.Instance.objservice.Excel_GetXml(CalcID, "");
                        if (ds != null)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                XMLFileName = GetFileName(ds.Tables[0], CalcID);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string msg = "getting xml file from database, calc name :" + CalcID;
                        LogUtility.Error("Utilities.cs", "GetXmlFileName();", msg, ex);
                    }
                }
                else
                {
                    try
                    {
                        Directory.Delete(DataUtil.Instance.DirectoryPath + "\\Products\\VCM\\" + CalcID, true);
                        Directory.CreateDirectory(DataUtil.Instance.DirectoryPath + "\\Products\\VCM\\" + CalcID);
                        ds = AuthenticationManager.Instance.objservice.Excel_GetXml(CalcID, "");
                        if (ds != null)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                XMLFileName = GetFileName(ds.Tables[0], CalcID);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string msg = "getting xml file name when calc file store in case of old version calc name :" + CalcID;
                        LogUtility.Error("Utilities.cs", "GetXmlFileName();", msg, ex);
                    }
                }

                return Directory.GetFiles(DataUtil.Instance.DirectoryPath + "\\Products\\VCM\\" + CalcID + "\\", XMLFileName + ".xml").FirstOrDefault();
            }
            catch (Exception ex)
            {
                string msg = "Calculator Name: " + CalcID;
                LogUtility.Error("Utilities.cs", "GetXmlFileName();", msg, ex);
                return null;
            }
        }
        private void CreateDesgin(String XmlFileName, Int32 StartRow, Int32 StartColumn, string calcID, String CalcTitle)
        {
            try
            {
                System.Xml.XmlTextReader reader = null;
                reader = new XmlTextReader(XmlFileName);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(reader);
                foreach (XmlNode xnode in xmldoc.SelectNodes("Calculator"))
                {
                    string strValue = xnode.Attributes[0].Value;
                    string title = xmldoc.ChildNodes[1].ChildNodes[0].InnerText;
                    MaxCol = Convert.ToInt32(xmldoc.ChildNodes[1].ChildNodes[2].InnerText);
                    Int32 MaxRow = Convert.ToInt32(xmldoc.ChildNodes[1].ChildNodes[3].InnerText);
                    CalPlacing = Utilities.Instance.CoverArea(calcID, StartRow, StartColumn, Convert.ToInt32(MaxRow - 2), Convert.ToInt32(MaxCol + 1));
                    if (CalPlacing == 1)
                    {
                        if (Globals.Ribbons.Ribbon1.CBCalculatorList.SelectedItemIndex != -1)
                        {
                            Globals.Ribbons.Ribbon1.CBCalculatorList.SelectedItemIndex = 0;
                        }
                        Excel.Range rng = (Excel.Range)Utilities.Instance.GetWorksheetByCalcName(calcID).get_Range(Utilities.Instance.GetWorksheetByCalcName(calcID).Cells[StartRow, StartColumn], Utilities.Instance.GetWorksheetByCalcName(calcID).Cells[Convert.ToInt32(MaxRow + StartRow - 2), Convert.ToInt32(MaxCol + StartColumn + 1)]);
                        rng.Borders[XlBordersIndex.xlEdgeRight].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                        rng.Borders[XlBordersIndex.xlEdgeLeft].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                        rng.Borders[XlBordersIndex.xlEdgeTop].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                        rng.Borders[XlBordersIndex.xlEdgeBottom].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                        UpdateManager.Instance.CalcPlacingonSheet.Add(calcID, StartRow + "^" + StartColumn);

                        CCount = CountCols;
                        CountCols += MaxCol;
                        foreach (XmlNode xnodes in xnode.SelectNodes("Field"))
                        {
                            Int32 cnt = xnodes.Attributes.Count;
                            foreach (XmlNode item in xnodes)
                            {
                                Title = "";
                                ControlID = item.Attributes[0].Value;
                                for (int i = 0; i < item.ChildNodes.Count; i++)
                                {
                                    String Name = item.ChildNodes[i].Name;
                                    if (Name == "Row") { Rows = Convert.ToInt32(item.ChildNodes[i].InnerText); }
                                    if (Name == "Column") { Columns = Convert.ToInt32(item.ChildNodes[i].InnerText); }
                                    if (Name == "Type") { ControlType = item.ChildNodes[i].InnerText.ToString(); }
                                    if (Name == "Title") { Title = item.ChildNodes[i].InnerText.ToString(); }
                                }
                                if (StartNewRow == 0)
                                {

                                    #region First Calculator Image Placing
                                    Int32 StrRow = StartRow + Rows - 3;
                                    Int32 StrCol = Columns + StartColumn - 2;
                                    Cell = (Range)Utilities.Instance.GetWorksheetByCalcName(calcID).Cells[StrRow, CCount + StrCol];
                                    if (CalTitleCnt == 0)
                                    {
                                        Range rngTitle = (Range)Utilities.Instance.GetWorksheetByCalcName(calcID).Cells[StrRow - 1, StartColumn];
                                        rngTitle.ID = calcID;
                                        rngTitle.Value2 = CalcTitle.Replace("_", " ");
                                        rngTitle.Font.Bold = true;
                                        rngTitle.Font.Size = 14;

                                        Range oRange = (Range)Utilities.Instance.GetWorksheetByCalcName(calcID).Cells[StrRow - 1, StartColumn + CountCols - 1];
                                        Imagepath = DataUtil.Instance.DirectoryPath + "\\Images\\red.png";
                                        oRange.Name = "Status_" + calcID;
                                        Shape ShapiMg = Utilities.Instance.GetWorksheetByCalcName(calcID).Shapes.AddPicture(Imagepath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, (float)oRange.Left, (float)oRange.Top, 10, 10);
                                        ShapiMg.Name = "Image_" + calcID;



                                        Range StatTitleusrng = (Range)Utilities.Instance.GetWorksheetByCalcName(calcID).Cells[StrRow - 1, StartColumn + CountCols - 2];
                                        StatTitleusrng.Value = "Status:";

                                        Range RngStart = (Range)Utilities.Instance.GetWorksheetByCalcName(calcID).Cells[StrRow - 1, StartColumn + CountCols];

                                        Btn = new System.Windows.Forms.Button();

                                        Btn.Name = calcID;
                                        Btn.Text = "Close";
                                        RngStart.Name = "Close_" + calcID;
                                        Btn.Click += new EventHandler(Btn_Click);
                                        Utilities.Instance.GetWorksheetByCalcName(calcID).Controls.AddControl(Btn, (double)RngStart.Left, (double)RngStart.Top, (double)RngStart.Width, (double)RngStart.Height, "Btn_" + calcID);
                                        CalTitleCnt++;
                                    }
                                    #endregion
                                    Cell.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Gray);
                                    if (ControlType == "Label")
                                    {
                                        #region Label
                                        Cell.Name = ControlType + Rows + Columns;
                                        Cell.Value = Title;
                                        Cell.Font.Bold = true;
                                        #endregion
                                    }
                                    else if (ControlType.ToLower() == "textbox")
                                    {
                                        #region TextBox
                                        Cell.Name = ControlID;
                                        Cell.Locked = false;
                                        Microsoft.Office.Tools.Excel.NamedRange NameRange = Utilities.Instance.GetWorksheetByCalcName(calcID).Controls.AddNamedRange(Cell, ControlID);
                                        NameRange.Deselected += new DocEvents_SelectionChangeEventHandler(Cell_Deselected);
                                        NameRange.SelectionChange += new DocEvents_SelectionChangeEventHandler(NRngs_SelectionChange);

                                        #endregion
                                    }
                                    else if (ControlType.ToLower() == "dropdown")
                                    {
                                        #region DriopDownList
                                        dt = new System.Data.DataTable();
                                        dt.Columns.Add("EleID");
                                        dt.Columns.Add("EleName");
                                        cb = new ComboBox();
                                        cb.Name = ControlID;
                                        cb.DisplayMember = dt.Columns["EleName"].ToString();
                                        cb.DropDown += new EventHandler(cb_GotFocus);
                                        cb.DropDownClosed += new EventHandler(cb_LostFocus);
                                        cb.DropDownStyle = ComboBoxStyle.DropDownList;
                                        cb.ValueMemberChanged += new EventHandler(cb_ValueMemberChanged);
                                        Cell.Name = ControlID;
                                        Utilities.Instance.GetWorksheetByCalcName(calcID).Controls.AddControl(cb, Cell, ControlID);

                                        #endregion
                                    }
                                    else if (ControlType == "Button")
                                    {
                                        #region Button

                                        Btn = new System.Windows.Forms.Button();
                                        Btn.Name = ControlID;
                                        Btn.Tag = 1;
                                        Btn.Text = Title;
                                        Btn.Click += new EventHandler(Btn_Click);
                                        Utilities.Instance.GetWorksheetByCalcName(calcID).Controls.AddControl(Btn, Cell, ControlID);
                                        Cell.Name = ControlID;
                                        #endregion

                                    }
                                    else if (ControlType == "Date")
                                    {

                                    }
                                }
                            }
                            CalTitleCnt = 0;
                        }
                    }
                    else
                    {
                        UpdateManager.Instance.CalcPlacingonSheet.Remove(calcID);
                    }
                }
            }
            catch (Exception ex)
            {
                String Msg = "XmlFileName: " + XmlFileName + "; Rowcnt: " + StartRow + "; Calculator Name: " + calcID + "; CalCulator Title: " + CalcTitle + ";";
                LogUtility.Error("CalculatorDesgin.cs", "CreateDesgin()", Msg, ex);
            }
        }
        void cb_ValueMemberChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
        #endregion
        public void Dispose()
        {
            instance = null;
        }
    }
}
