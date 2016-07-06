using System;
using System.Collections.Generic;
using Excel = Microsoft.Office.Tools.Excel;//Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Office.Interop.Excel;
//*********************************BackGroundWorker*********************************
using System.ComponentModel;
//*********************************BackGroundWorker*********************************


namespace ExcelAddIn5
{
    class UpdateManager : IDisposable
    {
        private static volatile UpdateManager instance = null;
        private static object syncRoot = new Object();
        private Dictionary<string, Excel.Worksheet> _dirBeastImg;
        private Dictionary<string, string> _dirCalcSheetMap;
        private Dictionary<string, string> _dirCalcPlaceing;

        private Dictionary<string, List<string>> _dirCalcRowCol;
        private Dictionary<string, string> _dirsubmit_order_excel;
        public String CacluatorName;
        DateTime dtTemp;

        //public delegate void mymethod(System.Data.DataTable dtGrid, string HTMLClientID);
        //public static System.Data.DataTable dt;
        //public static mymethod inv;

        public static UpdateManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new UpdateManager();
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
        public Dictionary<string, Excel.Worksheet> CalcWorksheetRepo
        {
            get
            {
                return _dirBeastImg;
            }
        }

        public Dictionary<string, string> CalcSheetMap
        {
            get
            {
                return _dirCalcSheetMap;
            }
        }
        public Dictionary<string, string> CalcPlacingonSheet
        {
            get
            {
                return _dirCalcPlaceing;
            }
        }

        public Dictionary<string, string> SubmitOrder
        {
            get
            {
                return _dirsubmit_order_excel;
            }
        }

        public Dictionary<string, List<string>> CalcRowColSheet
        {
            get
            {
                return _dirCalcRowCol;
            }
        }

        #region constructor

        public UpdateManager()
        {
            _dirBeastImg = new Dictionary<string, Excel.Worksheet>();
            _dirCalcSheetMap = new Dictionary<string, string>();
            _dirCalcPlaceing = new Dictionary<string, string>();
            _dirCalcRowCol = new Dictionary<string, List<string>>();
            _dirsubmit_order_excel = new Dictionary<string, string>();

        }

        #endregion

        #region Update Functions

        #region Swpation Grid
        public void handleIncomingMessageFromBeastSignal(string name, string message)
        {
            // debugger;
            try
            {
                string[] ary = message.Split('#');

                int row = (Convert.ToInt32(ary[0])) + 1;
                int col = (Convert.ToInt32(ary[1])) + 2;

                string value = ary[2];

                if (name == "premdata")
                {
                    //updatePrimGridDataIDVise(message, "signalr");
                    row = row + 15;
                    updateSwapVolPremStrikeEle(row, col, value, "");
                }
                else if (name == "strikedata")
                {
                    row = row + 30;
                    updateSwapVolPremStrikeEle(row, col, value, "");

                }
                else if (name == "voldata")
                {
                    //row = row;
                    updateSwapVolPremStrikeEle(row, col, value, "");
                }
                else if (name == "otherdata")
                {
                    //updateOtherData(message);
                }
            }
            catch (Exception err)
            {
                string msg = "name: " + name + ";message: " + message;
                LogUtility.Error("UpdateManager.cs", "handleIncomingMessageFromBeastSignal();", msg, err);

            }
        }

        public void updateSwapVolPremStrikeEle(int row, int col, string eleValue, string updateType)
        {
            try
            {
                CalcWorksheetRepo["SwaptionGrid"].Cells[row, col].Value2 = eleValue;
            }
            catch (Exception ee)
            {
                string msg = "row: " + row + "; col:" + col + ";eleValue: " + eleValue + ";updateType: " + updateType;
                LogUtility.Error("UpdateManager.cs", "updateSwapVolPremStrikeEle();", msg, ee);

            }
        }
        #endregion
        #region Generic Grid
        public void ArrayDataFromServer(System.Data.DataTable dtGrid, string HTMLClientID)
        {
            try
            {
                Utilities.Instance.UpdateCustomAddInsData(dtGrid, HTMLClientID);
            }
            catch
            {
            }
            finally
            {
            }
        }

        /* public void processServerMessageGenericGrid(string updateType, string updateEleType, string GridID, string Rows, string Columns, string EleValue, string HTMLClientID)
         {
             try
             {
                 if (GridID == "5" || GridID == "8" || GridID == "14" || GridID == "15") //  Updat Type f : IsprocessID, d = DOP 
                 {
                     LogUtility.Info("UpdateManger.cs", "processServerMessageGenericGrid(f);", "Getting batch flag # " + EleValue + " Of " + HTMLClientID + "_" + GridID);
                     Microsoft.Office.Interop.Excel.Worksheet wk = Globals.ThisAddIn.Application.Worksheets["Common"];
                     wk.get_Range(HTMLClientID + "_" + GridID).Value2 = EleValue;
                 }
                 else if (updateType == "c")
                 {
                     UpdateData(Rows, Columns, EleValue, HTMLClientID);
                 }
             }
             catch (Exception ee)
             {
                 string msg = "updateType: " + updateType + ";updateEleType: " + updateEleType + ";GridID: " + GridID + ";Row: " + Rows + ";Col: " + Columns + ";EleVal: " + EleValue + "; HTMLClientID: " + HTMLClientID;
                 LogUtility.Error("UpdateManager.cs", "processServerMessageGenericGrid();", msg, ee);
             }
         }*/

        /*private static void UpdateData(String Row, String Column, String EleValue, String CalcName)
        {
            try
            {
                //  if (UpdateManager.Instance.BondGridRep.ContainsKey(CalcName) && Column != "0")
                // {
                //    CalcPlace objCalcPlace = UpdateManager.Instance.BondGridRep[CalcName];

                //    Microsoft.Office.Interop.Excel.Workbook tempWorkbook = Globals.ThisAddIn.Application.Workbooks[objCalcPlace.Workbookname];
                //    Microsoft.Office.Interop.Excel.Worksheet tempsheet = tempWorkbook.Worksheets[objCalcPlace.Sheetname];
                //    int Rows = objCalcPlace.StartRow + Convert.ToInt32(Row);
                //    int Columns = objCalcPlace.StartColumn + Convert.ToInt32(Column);
                //    Range endCell = tempsheet.Cells[Rows, Columns];
                //    endCell.Value2 = EleValue;

                //    //if (CalcName == "vcm_calc_kcg_bonds_submit_order_excel" && Convert.ToInt32(Column) == 6)
                //    //{
                //    //    if (UpdateManager.instance.SubmitOrder.ContainsKey(Rows.ToString()))
                //    //        UpdateManager.instance.SubmitOrder[Rows.ToString()] = EleValue;
                //    //    else if (!UpdateManager.instance.SubmitOrder.ContainsKey(Rows.ToString()))
                //    //        UpdateManager.instance.SubmitOrder.Add(Rows.ToString(), EleValue);

                //    //    // endCell.Name = "G_" + Column + "_" + Row;
                //    //}
                //}

               // Utilities.Instance.CustomAddinMethod(CalcName, EleValue, Row, Column);
                // }
            }
            catch (Exception ex)
            {
                LogUtility.Error("UpdateManager.cs", "UpdateData();", "Error Message:- Rows: " + Row + "; Columns: " + Column + "; EleValue: " + EleValue + ";CalcName: " + CalcName, ex);
            }
        }*/

        #endregion
        #region Generic Process Update

        public void processServerMessageGeneric(string updtTyp, string updtEleTyp, string eleID, string eleVal, string elePrntID)
        {
            try
            {
                if (updtEleTyp == "m")
                {
                    if (updtTyp == "imgStatus")
                    {
                        ShowConnectionStatus(Convert.ToBoolean(eleVal), elePrntID);
                    }
                    else
                    {
                        if (elePrntID == "AuthInvalid")
                        {
                            AuthenticationManager.Instance.UserToken = string.Empty;
                            AuthenticationManager.Instance.GetAuthenticationToken();
                            SignalRConnectionManager.Instance.CallBackMethod(eleID);
                        }
                        else if (elePrntID != "ConnectionIDSignalR")
                        {
                            MessageBox.Show(eleVal, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        }
                    }
                }
                else
                {

                    if (updtEleTyp == "i")
                    {
                        string[] eleValAry = eleVal.Split('#');
                        setElementValueGeneric(elePrntID + "_" + eleID, eleValAry[0], elePrntID, eleID, updtTyp, updtEleTyp, eleValAry[1]);
                    }
                    else if (updtEleTyp == "p")
                    {
                        string[] propArySpl = eleVal.Split('#');
                        setEleProps(elePrntID + "_" + eleID, propArySpl[0], elePrntID);
                        setElementValueGeneric(elePrntID + "_" + eleID, propArySpl[1], elePrntID, eleID, updtTyp, updtEleTyp, propArySpl[2]);
                    }
                    else if (updtEleTyp == "tt")
                    {
                    }
                    else
                    {
                        populateDropDownGenericList(elePrntID + "_" + eleID, eleID, eleVal, updtTyp, updtEleTyp);
                    }
                }
            }
            catch (Exception err)
            {
                string Msg = "updtTyp: " + updtTyp + "; updtEleTyp: " + updtEleTyp + " ; eleID: " + eleID + "; eleVal: " + eleVal + ";elePrntID: " + elePrntID;
                LogUtility.Error("CalculatorDesgin.cs", "processServerMessageGeneric()", Msg, err);

            }
        }

        void bwTemp_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                BackgroundWorker BeTemp = (BackgroundWorker)sender;
                BeTemp.CancelAsync();
            }
            catch { }
        }
        private void setElementValueGeneric(string Element, string ElementVal, string EleParent, string eleID, string updtTyp, string updtEleTyp, string EletDispStr)
        {
            try
            {
                int indx = Utilities.Instance.GetWorksheetByCalcName(EleParent).Controls.IndexOf(Element);
                object objEle = Utilities.Instance.GetWorksheetByCalcName(EleParent).Controls[indx];
                if (objEle.GetType() == typeof(ComboBox))
                {
                    try
                    {
                        ComboBox cmbTmp = (ComboBox)Utilities.Instance.GetWorksheetByCalcName(EleParent.ToString()).Controls[Utilities.Instance.GetWorksheetByCalcName(EleParent.ToString()).Controls.IndexOf(Element)];
                        cmbTmp.Invoke(new EventHandler(delegate
                         {
                             if (ElementVal != "" && cmbTmp.Items.Count > 0)
                             {
                                 cmbTmp.SelectedValue = ElementVal;
                             }
                         }));
                    }
                    catch (Exception ex)
                    {
                        string Msg = "Element: " + Element + "; ElementVal: " + ElementVal + " ; EleParent: " + EleParent + "; eleID: " + eleID + ";updtTyp: " + updtTyp + ";updtEleTyp: " + updtEleTyp + ";EletDispStr: " + EletDispStr;
                        LogUtility.Error("updateManger.cs", "setElementValueGeneric();", Msg, ex);
                    }
                }
                else
                {
                    try
                    {
                        if (Utilities.Instance.GetWorksheetByCalcName(EleParent).get_Range(Element) != null)
                        {
                            Range rng = Utilities.Instance.GetWorksheetByCalcName(EleParent).get_Range(Element);
                            if (EletDispStr != null)
                            {
                                if (DateTime.TryParse(ElementVal, out dtTemp))
                                {
                                    rng.Cells.Value2 = EletDispStr;
                                    string[] strDate = ElementVal.Split(' ');
                                    rng.Cells.ID = strDate[0];// Convert.ToDateTime(ElementVal).ToString("MM/dd/yyyy");
                                }
                                else
                                {
                                    rng.Cells.Value2 = EletDispStr;
                                    rng.Cells.ID = ElementVal;
                                }

                            }
                            rng.NumberFormat = "@";
                        }
                    }
                    catch (Exception ex)
                    {

                        string Msg = "Element: " + Element + "; ElementVal: " + ElementVal + " ; EleParent: " + EleParent + "; eleID: " + eleID + ";updtTyp: " + updtTyp + ";updtEleTyp: " + updtEleTyp + ";EletDispStr: " + EletDispStr;
                        LogUtility.Error("updateManger.cs", "setElementValueGeneric()", Msg, ex);
                    }
                }
            }
            catch (Exception err)
            {

#if DEBUG
                string ss = err.Message;
#endif
            }
        }
        private void populateDropDownGenericList(string tblID, string ddID, string dataObj, string updtTyp, string updtEleTyp)
        {
            try
            {
                System.Data.DataTable dtLcl = Utilities.Instance.GetDataTableForDD(dataObj);
                string Calcname = tblID.Substring(0, tblID.LastIndexOf('_')).ToString();

                if (UpdateManager.Instance.CalcSheetMap.ContainsKey(Calcname))
                {
                    int indx = Utilities.Instance.GetWorksheetByCalcName(Calcname).Controls.IndexOf(tblID);
                    ComboBox cbLcl = (ComboBox)Utilities.Instance.GetWorksheetByCalcName(Calcname).Controls[indx];
                    cbLcl.Invoke(new EventHandler(delegate
                    {
                        try
                        {
                            cbLcl.Name = tblID;
                            cbLcl.DataSource = dtLcl;
                            cbLcl.ValueMember = dtLcl.Columns["EleID"].ToString();
                            cbLcl.DisplayMember = dtLcl.Columns["EleName"].ToString();
                        }
                        catch (Exception ex)
                        {
                            String msg = "tblID: " + tblID + "; ddID: " + ddID + ";dataObj: " + dataObj + "; updtTyp: " + updtTyp + ";updtEleTyp: " + updtEleTyp;
                            LogUtility.Error("CalculatorDesgin.cs", "populateDropDownGenericList()", msg, ex);
                        }
                    }));
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                string ss = ex.Message;
#endif
            }
        }
        private void ShowConnectionStatus(Boolean IsConnected, String CalcName)
        {
            try
            {
                if (Utilities.Instance.ImageToAddInList.ContainsKey(CalcName))
                {
                    Utilities.Instance.UpdateCustomAddInsImageStatus(IsConnected, CalcName);
                }
                else
                {
                    String Imagepath = string.Empty;
                    if (IsConnected == true)
                    {
                        Imagepath = DataUtil.Instance.DirectoryPath + "\\Images\\green.png";
                        int indxstart = Utilities.Instance.GetWorksheetByCalcName(CalcName).Controls.IndexOf("Btn_" + CalcName);
                        System.Windows.Forms.Button btnStop = (System.Windows.Forms.Button)Utilities.Instance.GetWorksheetByCalcName(CalcName).Controls[indxstart];
                        btnStop.Invoke(new EventHandler(delegate { btnStop.Enabled = true; }));

                    }
                    else
                    {
                        Imagepath = DataUtil.Instance.DirectoryPath + "\\Images\\red.png";
                    }
                    Utilities.Instance.DeleteStatusImage(CalcName);
                    Range oRange = Utilities.Instance.GetWorksheetByCalcName(CalcName).get_Range("Status_" + CalcName);
                    Shape btnImageSatus = Utilities.Instance.GetWorksheetByCalcName(CalcName).Shapes.AddPicture(Imagepath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, (float)oRange.Left, (float)oRange.Top, 10, 10);
                    btnImageSatus.Name = "Image_" + CalcName;
                }
            }
            catch (Exception ex)
            {
                string msgs = "IsConnected: " + IsConnected + ";CalcName: " + CalcName;
                LogUtility.Error("UpdateManager.cs", "ShowConnectionStatus()", msgs, ex);
            }
        }
        private void setEleProps(string Element, string strProps, string EleParent)
        {
            try
            {
                string[] aryProps = strProps.Split('|');
                if ((aryProps[0] == "V=0")) //Visable elements
                {
                    int indx = Utilities.Instance.GetWorksheetByCalcName(EleParent).Controls.IndexOf(Element);
                    object objEle = Utilities.Instance.GetWorksheetByCalcName(EleParent).Controls[indx];
                    if (objEle.GetType() == typeof(ComboBox))
                    {
                        try
                        {
                            ComboBox cbLcl = (ComboBox)Utilities.Instance.GetWorksheetByCalcName(EleParent.ToString()).Controls[indx];
                            cbLcl.Invoke(new EventHandler(delegate { cbLcl.Visible = false; }));
                        }
                        catch (Exception ex)
                        {
                            string msg = "Element: " + Element + ";strProps: " + strProps + "EleParent:" + EleParent;
                            LogUtility.Error("Updatemanager.cs", "setEleProps(); ComboBox # V=0 #", msg, ex);
                        }
                    }
                    else if (objEle.GetType() == typeof(System.Windows.Forms.Button))
                    {
                        try
                        {
                            System.Windows.Forms.Button btn = (System.Windows.Forms.Button)Utilities.Instance.GetWorksheetByCalcName(EleParent.ToString()).Controls[indx];
                            btn.Invoke(new EventHandler(delegate { btn.Visible = false; }));
                        }
                        catch (Exception ex)
                        {
                            string msg = "Element: " + Element + ";strProps: " + strProps + "EleParent:" + EleParent;
                            LogUtility.Error("Updatemanager.cs", "setEleProps(); Button # V=0 #", msg, ex);
                        }
                    }
                }
                if ((aryProps[2] == "E=0")) //enable elements
                {
                    int indx = Utilities.Instance.GetWorksheetByCalcName(EleParent).Controls.IndexOf(Element);
                    object objEle = Utilities.Instance.GetWorksheetByCalcName(EleParent).Controls[indx];
                    if (objEle.GetType() == typeof(ComboBox)) //Combo box
                    {
                        try
                        {
                            ComboBox cbLcl = (ComboBox)Utilities.Instance.GetWorksheetByCalcName(EleParent.ToString()).Controls[indx];
                            cbLcl.Invoke(new EventHandler(delegate { cbLcl.Enabled = false; }));
                        }
                        catch (Exception ex)
                        {
                            string msg = "Element: " + Element + ";strProps: " + strProps + "EleParent:" + EleParent;
                            LogUtility.Error("Updatemanager.cs", "setEleProps(); ComboBox # E=0 #", msg, ex);
                        }
                    }
                    else if (objEle.GetType() == typeof(System.Windows.Forms.Button)) //Button
                    {
                        try
                        {
                            System.Windows.Forms.Button btn = (System.Windows.Forms.Button)Utilities.Instance.GetWorksheetByCalcName(EleParent.ToString()).Controls[indx];
                            btn.Invoke(new EventHandler(delegate { btn.Enabled = false; }));
                        }
                        catch (Exception ex)
                        {
                            string msg = "Element: " + Element + ";strProps: " + strProps + "EleParent:" + EleParent;
                            LogUtility.Error("Updatemanager.cs", "setEleProps(); ComboBox # E=0 #", msg, ex);
                        }
                    }
                    else
                    {
                        try
                        {
                            Range rng = Utilities.Instance.GetWorksheetByCalcName(EleParent).get_Range(Element); //TextBox
                            rng.Locked = false;
                            rng.Interior.Color = Color.FromArgb(236, 233, 216);
                        }
                        catch (Exception ex)
                        {
                            string msg = "Element: " + Element + ";strProps: " + strProps + "EleParent:" + EleParent;
                            LogUtility.Error("Updatemanager.cs", "setEleProps(); Range # E=0 #", msg, ex);
                        }
                    }
                }
                else if ((aryProps[2] == "E=1")) //disable elements
                {
                    int indx = Utilities.Instance.GetWorksheetByCalcName(EleParent).Controls.IndexOf(Element);
                    object objEle = Utilities.Instance.GetWorksheetByCalcName(EleParent).Controls[indx];
                    if (objEle.GetType() == typeof(ComboBox))
                    {
                        try
                        {
                            ComboBox cbLcl = (ComboBox)Utilities.Instance.GetWorksheetByCalcName(EleParent.ToString()).Controls[indx];
                            cbLcl.Invoke(new EventHandler(delegate { cbLcl.Enabled = true; }));
                        }
                        catch (Exception ex)
                        {
                            string msg = "Element: " + Element + ";strProps: " + strProps + "EleParent:" + EleParent;
                            LogUtility.Error("Updatemanager.cs", "setEleProps(); ComboBox # E=1 #", msg, ex);
                        }
                    }
                    else if (objEle.GetType() == typeof(System.Windows.Forms.Button))
                    {
                        try
                        {
                            System.Windows.Forms.Button btn = (System.Windows.Forms.Button)Utilities.Instance.GetWorksheetByCalcName(EleParent.ToString()).Controls[indx];
                            btn.Invoke(new EventHandler(delegate { btn.Enabled = true; }));
                        }
                        catch (Exception ex)
                        {
                            string msg = "Element: " + Element + ";strProps: " + strProps + "EleParent:" + EleParent;
                            LogUtility.Error("Updatemanager.cs", "setEleProps(); ComboBox # E=1 #", msg, ex);
                        }
                    }
                    else
                    {
                        try
                        {
                            Range rng = Utilities.Instance.GetWorksheetByCalcName(EleParent).get_Range(Element);
                            rng.Interior.Color = Color.FromArgb(255, 255, 255);
                            rng.Locked = false;
                        }
                        catch (Exception ex)
                        {
                            string msg = "Element: " + Element + ";strProps: " + strProps + "EleParent:" + EleParent;
                            LogUtility.Error("Updatemanager.cs", "setEleProps(); Range # E=1 #", msg, ex);
                        }
                    }
                }
                if ((aryProps[4].Trim().StartsWith("R")))
                {

                    int indx = Utilities.Instance.GetWorksheetByCalcName(EleParent).Controls.IndexOf(Element);
                    object objEle = Utilities.Instance.GetWorksheetByCalcName(EleParent).Controls[indx];
                    if (objEle.GetType() != typeof(ComboBox))
                    {
                        try
                        {
                            getColorFromClass(aryProps[4].Trim(), Element, EleParent);
                        }
                        catch (Exception ex)
                        {
                            string msg = "Element: " + Element + ";strProps: " + strProps + "EleParent:" + EleParent;
                            LogUtility.Error("Updatemanager.cs", "setEleProps(); Color # R #", msg, ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

#if DEBUG
                string ss = ex.Message;
#endif
            }

        }
        private String getColorFromClass(string strProps, string Element, String EleParent)
        {

            Range rng = Utilities.Instance.GetWorksheetByCalcName(EleParent).get_Range(Element);

            var clrName = "";
            var clrAry = strProps.Split('=');
            var clrNum = clrAry[1];
            var clrTyp = clrAry[0];

            switch (Convert.ToInt32(clrNum))
            {
                case -1:
                    if (clrTyp == "B")
                        //clrName = "RGB(255, 255, 255)";
                        rng.Interior.Color = Color.FromArgb(255, 255, 255);

                    else
                        //clrName = "RGB(47,47,47)"; //1B1B1B
                        rng.Font.Color = Color.FromArgb(47, 47, 47);
                    break;
                case 0:
                    rng.Font.Color = Color.FromArgb(51, 51, 51);
                    break;
                case 1:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 2:
                    rng.Font.Color = Color.FromArgb(0, 255, 255);
                    break;
                case 3:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 4:
                    rng.Font.Color = Color.FromArgb(0, 0, 255);
                    break;
                case 5:
                    rng.Font.Color = Color.FromArgb(255, 255, 0);
                    break;
                case 6:

                    rng.Font.Color = Color.FromArgb(255, 255, 255);
                    break;
                case 7:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 8:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 9:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 10:
                    rng.Font.Color = Color.FromArgb(0, 255, 255);
                    break;
                case 11:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 12:
                    rng.Font.Color = Color.FromArgb(0, 255, 0);
                    break;
                case 13:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 14:
                    rng.Font.Color = Color.FromArgb(141, 160, 216);
                    break;
                case 15:
                    rng.Font.Color = Color.FromArgb(141, 160, 2160);
                    break;
                case 16:
                    rng.Font.Color = Color.FromArgb(141, 160, 216);
                    break;
                case 17:
                    rng.Font.Color = Color.FromArgb(255, 255, 0);
                    break;
                case 18:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 19:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 20:
                    rng.Font.Color = Color.FromArgb(255, 255, 255);
                    break;
                case 21:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 22:
                    rng.Font.Color = Color.FromArgb(255, 255, 255);
                    break;
                case 23:
                    rng.Font.Color = Color.FromArgb(255, 255, 0);
                    break;
                case 24:
                    rng.Font.Color = Color.FromArgb(255, 0, 0);
                    break;
                case 25:
                    rng.Font.Color = Color.FromArgb(192, 192, 192);
                    break;
                case 26:
                    rng.Font.Color = Color.FromArgb(0, 0, 255);
                    break;
                case 27:
                    rng.Font.Color = Color.FromArgb(0, 255, 0);
                    break;
                case 28:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 29:
                    rng.Font.Color = Color.FromArgb(255, 255, 255);
                    break;
                case 30:
                    rng.Font.Color = Color.FromArgb(255, 255, 0);
                    break;
                case 31:
                    rng.Font.Color = Color.FromArgb(255, 0, 0);
                    break;
                case 32:
                    rng.Font.Color = Color.FromArgb(91, 163, 240);
                    break;
                case 33:
                    rng.Font.Color = Color.FromArgb(255, 128, 255);
                    break;
                case 34:
                    rng.Font.Color = Color.FromArgb(255, 0, 0);
                    break;
                case 35:
                    rng.Font.Color = Color.FromArgb(0, 160, 0);
                    break;
                case 36:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 37:
                    rng.Font.Color = Color.FromArgb(240, 240, 240);
                    break;
                case 38:
                    rng.Font.Color = Color.FromArgb(255, 255, 0);
                    break;
                case 39:
                    rng.Font.Color = Color.FromArgb(255, 0, 0);
                    break;
                case 40:
                    rng.Font.Color = Color.FromArgb(0, 255, 255);
                    break;

                case 41:
                    rng.Font.Color = Color.FromArgb(64, 64, 255);
                    break;

                case 42:
                    rng.Font.Color = Color.FromArgb(128, 128, 255);
                    break;
                case 43:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 44:
                    rng.Font.Color = Color.FromArgb(192, 192, 192);

                    break;
                case 45:
                    rng.Font.Color = Color.FromArgb(255, 0, 0);
                    break;
                case 46:
                    rng.Font.Color = Color.FromArgb(255, 0, 0);
                    break;
                case 47:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 48:
                    rng.Font.Color = Color.FromArgb(0, 255, 0);
                    break;
                case 49:
                    rng.Font.Color = Color.FromArgb(160, 255, 160);
                    break;
                case 50:
                    rng.Font.Color = Color.FromArgb(255, 255, 255);
                    break;
                case 51:
                    rng.Font.Color = Color.FromArgb(255, 255, 0);
                    break;
                case 52:
                    rng.Font.Color = Color.FromArgb(192, 0, 0);
                    break;
                case 53:
                    rng.Font.Color = Color.FromArgb(0, 128, 0);
                    break;

                case 54:
                    rng.Font.Color = Color.FromArgb(255, 128, 128);
                    break;
                case 55:
                    rng.Font.Color = Color.FromArgb(91, 163, 240);
                    break;
                case 56:
                    rng.Font.Color = Color.FromArgb(255, 0, 255);
                    break;
                case 57:
                    rng.Font.Color = Color.FromArgb(91, 163, 240);
                    break;
                case 58:
                    rng.Font.Color = Color.FromArgb(0, 128, 0);
                    break;
                case 59:
                    rng.Font.Color = Color.FromArgb(128, 128, 0);
                    break;
                case 60:
                    rng.Font.Color = Color.FromArgb(128, 0, 0);
                    break;
                case 61:
                    rng.Font.Color = Color.FromArgb(0, 255, 0);
                    break;
                case 62:
                    rng.Font.Color = Color.FromArgb(255, 255, 0);
                    break;
                case 63:
                    rng.Font.Color = Color.FromArgb(255, 0, 0);

                    break;
                case 64:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);

                    break;
                case 65:
                    rng.Font.Color = Color.FromArgb(64, 64, 64);

                    break;
                case 66:
                    rng.Font.Color = Color.FromArgb(128, 128, 128);

                    break;
                case 67:
                    rng.Font.Color = Color.FromArgb(192, 192, 192);
                    break;
                case 68:
                    rng.Font.Color = Color.FromArgb(255, 255, 255);
                    break;
                case 69:
                    rng.Font.Color = Color.FromArgb(160, 160, 160);
                    break;
                case 70:
                    rng.Font.Color = Color.FromArgb(96, 96, 96);
                    break;
                case 71:
                    rng.Font.Color = Color.FromArgb(224, 224, 255);
                    break;
                case 72:
                    rng.Font.Color = Color.FromArgb(255, 224, 255);
                    break;
                case 73:
                    rng.Font.Color = Color.FromArgb(255, 224, 192);
                    break;
                case 74:
                    rng.Font.Color = Color.FromArgb(255, 255, 192);
                    break;

                case 75:
                    rng.Font.Color = Color.FromArgb(224, 255, 192);
                    break;
                case 76:
                    rng.Font.Color = Color.FromArgb(224, 208, 255);
                    break;
                case 77:
                    rng.Font.Color = Color.FromArgb(255, 208, 208);
                    break;
                case 78:
                    rng.Font.Color = Color.FromArgb(208, 224, 255);
                    break;
                case 79:
                    rng.Font.Color = Color.FromArgb(32, 32, 64);
                    break;
                case 80:
                    rng.Font.Color = Color.FromArgb(64, 32, 64);
                    break;
                case 81:
                    rng.Font.Color = Color.FromArgb(64, 32, 0);
                    break;
                case 82:
                    rng.Font.Color = Color.FromArgb(64, 64, 0);
                    break;
                case 83:
                    rng.Font.Color = Color.FromArgb(32, 64, 0);
                    break;
                case 84:
                    rng.Font.Color = Color.FromArgb(32, 32, 64);
                    break;
                case 85:
                    rng.Font.Color = Color.FromArgb(64, 32, 32);
                    break;
                case 86:
                    rng.Font.Color = Color.FromArgb(32, 32, 64);
                    break;
                case 87:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 88:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 89:
                    rng.Font.Color = Color.FromArgb(255, 255, 255);
                    break;
                case 90:
                    rng.Font.Color = Color.FromArgb(0, 255, 0);
                    break;
                case 91:
                    rng.Font.Color = Color.FromArgb(255, 0, 0);
                    break;
                case 92:
                    rng.Font.Color = Color.FromArgb(255, 255, 0);
                    break;
                case 93:
                    rng.Font.Color = Color.FromArgb(0, 0, 255);
                    break;
                case 94:
                    rng.Font.Color = Color.FromArgb(255, 0, 0);
                    break;
                case 95:
                    rng.Font.Color = Color.FromArgb(128, 0, 0);
                    break;
                case 96:
                    rng.Font.Color = Color.FromArgb(255, 255, 255);
                    break;
                case 97:
                    rng.Font.Color = Color.FromArgb(255, 255, 255);
                    break;
                case 98:
                    rng.Font.Color = Color.FromArgb(0, 0, 255);
                    break;
                case 99:
                    rng.Font.Color = Color.FromArgb(255, 255, 255);
                    break;
                case 100:
                    rng.Font.Color = Color.FromArgb(0, 0, 127);
                    break;
                case 101:
                    rng.Font.Color = Color.FromArgb(255, 255, 255);
                    break;
                case 102:
                    rng.Font.Color = Color.FromArgb(255, 255, 192);
                    break;
                case 103:
                    rng.Font.Color = Color.FromArgb(192, 255, 192);
                    break;
                case 104:
                    rng.Font.Color = Color.FromArgb(255, 255, 192);
                    break;
                case 105:
                    rng.Font.Color = Color.FromArgb(255, 192, 192);
                    break;
                case 106:
                    rng.Font.Color = Color.FromArgb(255, 64, 64);
                    break;
                case 107:
                    rng.Font.Color = Color.FromArgb(255, 0, 0);
                    break;
                case 108:
                    rng.Font.Color = Color.FromArgb(240, 240, 240);
                    break;
                case 109:
                    rng.Font.Color = Color.FromArgb(255, 192, 192);
                    break;
                case 110:
                    Utilities.Instance.GetWorksheetByCalcName(EleParent).get_Range(Element).Font.Color = Color.FromArgb(192, 255, 192);
                    break;
                case 111:
                    Utilities.Instance.GetWorksheetByCalcName(EleParent).get_Range(Element).Font.Color = Color.FromArgb(192, 192, 255);
                    break;
                case 112:
                    Utilities.Instance.GetWorksheetByCalcName(EleParent).get_Range(Element).Font.Color = Color.FromArgb(255, 0, 0);
                    break;
                case 113:
                    Utilities.Instance.GetWorksheetByCalcName(EleParent).get_Range(Element).Font.Color = Color.FromArgb(255, 255, 0);
                    break;
                case 114:
                    Utilities.Instance.GetWorksheetByCalcName(EleParent).get_Range(Element).Font.Color = Color.FromArgb(0, 255, 0);
                    break;
                case 115:
                    Utilities.Instance.GetWorksheetByCalcName(EleParent).get_Range(Element).Font.Color = Color.FromArgb(0, 0, 255);
                    break;
                case 116:
                    Utilities.Instance.GetWorksheetByCalcName(EleParent).get_Range(Element).Font.Color = Color.FromArgb(0, 255, 255);
                    break;
                case 117:
                    Utilities.Instance.GetWorksheetByCalcName(EleParent).get_Range(Element).Font.Color = Color.FromArgb(255, 0, 255);
                    break;
                case 118:
                    Utilities.Instance.GetWorksheetByCalcName(EleParent).get_Range(Element).Font.Color = Color.FromArgb(255, 128, 128);
                    break;
                case 119:
                    Utilities.Instance.GetWorksheetByCalcName(EleParent).get_Range(Element).Font.Color = Color.FromArgb(128, 128, 255);
                    break;
                case 120:
                    Utilities.Instance.GetWorksheetByCalcName(EleParent).get_Range(Element).Font.Color = Color.FromArgb(128, 255, 128);
                    break;
                case 121:
                    Utilities.Instance.GetWorksheetByCalcName(EleParent).get_Range(Element).Font.Color = Color.FromArgb(80, 80, 80);
                    break;
                case 122:
                    Utilities.Instance.GetWorksheetByCalcName(EleParent).get_Range(Element).Font.Color = Color.FromArgb(255, 255, 255);
                    break;
                case 123:
                    Utilities.Instance.GetWorksheetByCalcName(EleParent).get_Range(Element).Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 124:
                    Utilities.Instance.GetWorksheetByCalcName(EleParent).get_Range(Element).Font.Color = Color.FromArgb(0, 0, 255);
                    break;
                case 125:
                    Utilities.Instance.GetWorksheetByCalcName(EleParent).get_Range(Element).Font.Color = Color.FromArgb(255, 0, 0);
                    break;
                case 126:
                    Utilities.Instance.GetWorksheetByCalcName(EleParent).get_Range(Element).Font.Color = Color.FromArgb(200, 200, 200);
                    break;
                case 127:
                    Utilities.Instance.GetWorksheetByCalcName(EleParent).get_Range(Element).Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 128:
                    rng.Font.Color = Color.FromArgb(255, 255, 255);
                    break;
                case 129:
                    rng.Font.Color = Color.FromArgb(255, 255, 0);
                    break;
                case 130:
                    rng.Font.Color = Color.FromArgb(255, 0, 0);
                    break;
                case 131:
                    rng.Font.Color = Color.FromArgb(0, 255, 255);
                    break;
                case 132:
                    rng.Font.Color = Color.FromArgb(200, 200, 200);
                    break;
                case 133:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 134:
                    rng.Font.Color = Color.FromArgb(0, 255, 255);
                    break;
                case 135:
                    rng.Font.Color = Color.FromArgb(255, 255, 0);
                    break;
                case 136:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 137:
                    rng.Font.Color = Color.FromArgb(128, 128, 128);
                    break;
                case 138:
                    rng.Font.Color = Color.FromArgb(192, 192, 192);
                    break;
                case 139:
                    rng.Font.Color = Color.FromArgb(255, 255, 255);
                    break;
                case 140:
                    rng.Font.Color = Color.FromArgb(255, 255, 0);
                    break;
                case 141:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 142:
                    rng.Font.Color = Color.FromArgb(0, 255, 255);
                    break;
                case 143:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 144:
                    rng.Font.Color = Color.FromArgb(255, 255, 255);
                    break;
                case 145:
                    rng.Font.Color = Color.FromArgb(255, 0, 0);
                    break;
                case 146:
                    rng.Font.Color = Color.FromArgb(255, 255, 255);
                    break;
                case 147:
                    rng.Font.Color = Color.FromArgb(0, 0, 255);
                    break;
                case 148:
                    rng.Font.Color = Color.FromArgb(255, 255, 255);
                    break;
                case 149:
                    rng.Font.Color = Color.FromArgb(255, 0, 0);
                    break;
                case 150:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 151:
                    rng.Font.Color = Color.FromArgb(0, 255, 255);
                    break;
                case 152:
                    rng.Font.Color = Color.FromArgb(255, 0, 0);
                    break;
                case 153:
                    rng.Font.Color = Color.FromArgb(255, 255, 255);
                    break;
                case 154:
                    rng.Font.Color = Color.FromArgb(0, 0, 255);
                    break;
                case 155:
                    rng.Font.Color = Color.FromArgb(255, 255, 255);
                    break;
                case 156:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 157:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 158:
                    rng.Font.Color = Color.FromArgb(0, 255, 255);
                    break;
                case 159:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 160:
                    rng.Font.Color = Color.FromArgb(255, 255, 0);
                    break;
                case 161:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 162:
                    rng.Font.Color = Color.FromArgb(0, 255, 255);
                    break;
                case 163:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 164:
                    rng.Font.Color = Color.FromArgb(0, 0, 255);
                    break;
                case 165:
                    rng.Font.Color = Color.FromArgb(255, 255, 0);
                    break;
                case 166:
                    rng.Font.Color = Color.FromArgb(255, 255, 255);
                    break;
                case 167:
                    rng.Font.Color = Color.FromArgb(0, 0, 255);
                    break;
                case 168:
                    rng.Font.Color = Color.FromArgb(255, 0, 0);
                    break;
                case 169:
                    rng.Font.Color = Color.FromArgb(255, 255, 255);
                    break;
                case 170:
                    rng.Font.Color = Color.FromArgb(255, 255, 255);
                    break;
                case 171:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 172:
                    rng.Font.Color = Color.FromArgb(128, 128, 128);
                    break;
                case 173:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 174:
                    rng.Font.Color = Color.FromArgb(64, 128, 64);
                    break;
                case 175:
                    rng.Font.Color = Color.FromArgb(255, 192, 0);
                    break;
                case 176:
                    rng.Font.Color = Color.FromArgb(255, 64, 64);
                    break;
                case 177:
                    rng.Font.Color = Color.FromArgb(0, 0, 0);
                    break;
                case 178:
                    rng.Font.Color = Color.FromArgb(255, 255, 128);
                    break;
                case 179:
                    rng.Font.Color = Color.FromArgb(128, 255, 128);
                    break;
                case 180:
                    rng.Font.Color = Color.FromArgb(224, 32, 255);
                    break;
                case 181:
                    rng.Font.Color = Color.FromArgb(255, 96, 96);
                    break;
                case 182:
                    rng.Font.Color = Color.FromArgb(240, 240, 240);
                    break;
                case 183:
                    rng.Font.Color = Color.FromArgb(255, 0, 0);
                    break;
                case 184:
                    rng.Font.Color = Color.FromArgb(0, 128, 0);
                    break;
                case 185:
                    rng.Font.Color = Color.FromArgb(65, 76, 202);
                    break;
                case 186:
                    rng.Font.Color = Color.FromArgb(255, 255, 255);
                    break;
                default:
                    rng.Font.Color = Color.FromArgb(255, 255, 255);
                    break;
            }
            return clrName;
        }
        #endregion
        #endregion
        public void Dispose()
        {
            instance = null;
        }

    }
}
