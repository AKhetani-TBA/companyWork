using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.IO;
using Microsoft.Win32;
using System.Reflection;
using System.Diagnostics;
using System.Xml;
using System.Security.Cryptography;
using System.Configuration;
using System.Net;
namespace ExcelAddIn5
{
    class Utilities : IDisposable
    {
        public String ErrorMessage;
        private static volatile Utilities instance = null;
        private static object syncRoot = new Object();
        public Boolean IsShare = false;
        Microsoft.Office.Tools.Excel.Worksheet xlWorkSheet;
        public String InstanceType;
        public String Password;
        public string UserRole = string.Empty;
        public String ErrorMessageVersion;
        public bool IsConnectionExists = true;
        public Login objLoginTmp;
        public static byte[] KEY_64 = { 42, 16, 93, 156, 78, 4, 218, 32 };
        public static byte[] IV_64 = { 55, 103, 246, 79, 36, 99, 167, 3 };
        public Int32 Invalidpwdcnt = 1;
        Int32 CalPlacing = 0;
        public Dictionary<string, string> ImageToAddInList;
        public string UserID;

        //Create xml for cusip
        public static Utilities Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new Utilities();
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
        public Utilities()
        {
            InstanceType = System.Configuration.ConfigurationManager.AppSettings["InstanceType"].ToString();
            ErrorMessage = String.Empty;
            ImageToAddInList = new Dictionary<string, string>();
        }

        public System.Data.DataTable GetDataTableForDD(string dataObj)
        {

            System.Data.DataTable dtLcl = new System.Data.DataTable();

            dtLcl.Columns.Add("EleID");
            dtLcl.Columns.Add("EleName");

            try
            {
                if (dataObj.Split('#').Length > 1)
                {
                    bool isFirst = true;
                    string selectedVal;
                    string[] dataArray = dataObj.Split('#')[0].Split('|');
                    selectedVal = dataObj.Split('#')[1];
                    int dataLength = dataArray.Length;
                    string element = "";
                    int crntEleID = -1;
                    string crntEleStr = "";


                    for (int i = 0; i < dataLength; i++)
                    {
                        element = dataArray[i];
                        if (isFirst == true)
                        {
                            DataRow row1s = dtLcl.NewRow();
                            row1s["EleID"] = -1;
                            row1s["EleName"] = "Select Value";
                            dtLcl.Rows.Add(row1s);
                            isFirst = false;
                        }

                        var isHavingVal = false;

                        if (element.IndexOf("=") == -1)
                        {
                            crntEleID = crntEleID + 1;
                        }
                        else
                        {
                            int num2;
                            if (int.TryParse(element.Split('=')[1].Trim(), out num2))
                            {
                                crntEleID = Convert.ToInt32(element.Split('=')[1].Trim());
                                isHavingVal = true;
                            }
                        }

                        if (element.IndexOf("~") == -1)
                        {
                            if (isHavingVal == false)
                            {
                                crntEleStr = element;
                            }
                            else
                            {
                                crntEleStr = element.Split('=')[0];
                            }
                        }
                        else
                        {
                            crntEleStr = element.Split('~')[0];
                        }
                        DataRow row1 = dtLcl.NewRow();
                        row1["EleID"] = crntEleID;
                        row1["EleName"] = crntEleStr;

                        dtLcl.Rows.Add(row1);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("Utilities.cs", "getDataTableForDD();", "dataObj: " + dataObj, ex);
            }

            return dtLcl;
        }

        public string GetSheetNameByCalcName(string calcName)
        {
            string sheetName = "";
            try
            {
                sheetName = UpdateManager.Instance.CalcSheetMap[calcName].ToString();
            }
            catch (Exception ex)
            {
                LogUtility.Error("Utlities.cs", "getSheetNameByCalcName();", "Sheet name could not be fetched from calc name", ex);
            }
            return sheetName;
        }
        public Microsoft.Office.Tools.Excel.Worksheet GetWorksheetByName(string sheetName)
        {
            try
            {
                return UpdateManager.Instance.CalcWorksheetRepo[sheetName];
            }
            catch (Exception ex)
            {
                LogUtility.Error("Utlities.cs", "getWorksheetByName();", "Exception thrown while getting sheet from sheet name.", ex);
                return null;
            }
        }
        public Microsoft.Office.Tools.Excel.Worksheet GetWorksheetByCalcName(string calcName)
        {
            try
            {
                return GetWorksheetByName(GetSheetNameByCalcName(calcName));
            }
            catch (Exception ex)
            {
                LogUtility.Error("Utlities.cs", "GetWorksheetByCalcName();", "Exception thrown while getting sheet by calc name.", ex);
                return null;
            }
        }
        public void CalcRender(string CalName)
        {
            if (Globals.ThisAddIn.Application.ActiveCell.Value == null)
            {
                if (CalName == "")
                {
                    xlWorkSheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveSheet);
                    if (!UpdateManager.Instance.CalcSheetMap.ContainsKey(Globals.Ribbons.Ribbon1.CBCalculatorList.SelectedItem.Tag.ToString()))
                    {
                        UpdateManager.Instance.CalcSheetMap.Add(Globals.Ribbons.Ribbon1.CBCalculatorList.SelectedItem.Tag.ToString(), Globals.ThisAddIn.Application.ActiveWorkbook.Name + "^" + xlWorkSheet.Name);
                        CalculatorDesign.Instance.createCalculator(Globals.Ribbons.Ribbon1.CBCalculatorList.SelectedItem.Tag, Globals.Ribbons.Ribbon1.CBCalculatorList.SelectedItem.Label, Globals.ThisAddIn.Application.ActiveCell.Row, Globals.ThisAddIn.Application.ActiveCell.Column);
                    }
                    else
                    {
                        //  MessageBox.Show("Another instance of " + Globals.Ribbons.Ribbon1.CBCalculatorList.SelectedItem.Label.ToString() + " is already running.");
                        //MessageBox.Show("An instance of this app is already running in Excel.");
                        Messagecls.AlertMessage(19, "");
                    }
                }
            }
            else
            {
                Messagecls.AlertMessage(20, "");
                //  MessageBox.Show("The Excel cell where you want to place the app must be empty.");
            }
        }
        #region Write User credential inregestry and ebcyption and dfecryption
        public void WriteCredentialonRestry(string LoginID, string Passwords)
        {
            RegistryKey BaseKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Office\\Excel\\Addins\\" + DataUtil.Instance.ProductName, true);

            if (BaseKey != null)
            {
                RegistryKey UserName = BaseKey.CreateSubKey("UserCredential");
                if (UserName != null)
                {
                    RegistryKey ServerNameKey = UserName.CreateSubKey(DataUtil.Instance.ServerName);
                    if (ServerNameKey != null)
                    {
                        ServerNameKey.SetValue("UserName", LoginID);
                        ServerNameKey.SetValue("Password", Encryption(Passwords));
                        ServerNameKey.Close();
                    }
                    UserName.Close();
                }
                BaseKey.Close();
            }
        }
        public string Encryption(string value)
        {
            if (value != "")
            {
                DESCryptoServiceProvider CryptoProvidor = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, CryptoProvidor.CreateEncryptor(KEY_64, IV_64), CryptoStreamMode.Write);
                StreamWriter sw = new StreamWriter(cs);
                sw.Write(value);
                sw.Flush();
                cs.FlushFinalBlock();
                ms.Flush();
                return Convert.ToBase64String(ms.GetBuffer(), 0, Convert.ToInt32(ms.Length));
            }
            return string.Empty;
        }
        public string Decryption(string value)
        {
            if (value != "")
            {
                DESCryptoServiceProvider CryptoProvidor = new DESCryptoServiceProvider();
                Byte[] buf = Convert.FromBase64String(value);
                MemoryStream ms = new MemoryStream(buf);
                CryptoStream cs = new CryptoStream(ms, CryptoProvidor.CreateDecryptor(KEY_64, IV_64), CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            return string.Empty;
        }

        #endregion



        bool valueInRange(int value, int min, int max)
        { return (value >= min) && (value <= max); }
        public bool rectOverlap(System.Drawing.Rectangle A, System.Drawing.Rectangle B)
        {

            bool xOverlap = valueInRange(A.X, B.X, B.X + B.Height) ||
                            valueInRange(B.X, A.X, A.X + A.Height);

            bool yOverlap = valueInRange(A.Y, B.Y, B.Y + B.Width) ||
                            valueInRange(B.Y, A.Y, A.Y + A.Width);

            return (xOverlap && yOverlap);
        }
        public void DeleteStatusImage(string ImageName)
        {
            try
            {
                MessageFilter.Register();
                foreach (Microsoft.Office.Interop.Excel.Shape sh in Utilities.Instance.GetWorksheetByCalcName(ImageName).Shapes)
                {
                    if (sh.Name == "Image_" + ImageName)
                    {
                        sh.Delete();
                        break;
                    }
                }
                MessageFilter.Revoke();
            }
            catch (Exception ex)
            {
                LogUtility.Error("Utilites.cs", "DeleteStatusImage();", "Error while deleting status image", ex);
            }
        }
        public Int32 CoverArea(String calcID, Int32 StartingRow, Int32 StartingColumn, Int32 EndRow, Int32 EndColumn)
        {
            try
            {
                bool flag = false;
                if (AuthenticationManager.Instance.isUserAuthenticated)
                {
                    if (!UpdateManager.Instance.CalcWorksheetRepo.ContainsKey(Globals.ThisAddIn.Application.ActiveWorkbook.Name + "^" + Globals.ThisAddIn.Application.ActiveSheet.Name))
                    {
                        UpdateManager.Instance.CalcWorksheetRepo.Add(Globals.ThisAddIn.Application.ActiveWorkbook.Name + "^" + Globals.ThisAddIn.Application.ActiveSheet.Name, Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveSheet));
                    }

                    if (!UpdateManager.Instance.CalcSheetMap.ContainsKey(calcID))
                    {
                        UpdateManager.Instance.CalcSheetMap.Add(calcID, Globals.ThisAddIn.Application.ActiveWorkbook.Name + "^" + Globals.ThisAddIn.Application.Application.ActiveSheet.Name);
                    }

                    if (!UpdateManager.Instance.CalcRowColSheet.ContainsKey(Globals.ThisAddIn.Application.ActiveWorkbook.Name + "^" + Utilities.Instance.GetWorksheetByCalcName(calcID).Name))
                    {
                        List<string> LStr = new List<string>();
                        LStr.Add(Convert.ToInt32(StartingRow) + "^" + StartingColumn + "^" + Convert.ToInt32(EndRow) + "^" + EndColumn);
                        UpdateManager.Instance.CalcRowColSheet.Add(Globals.ThisAddIn.Application.ActiveWorkbook.Name + "^" + Utilities.Instance.GetWorksheetByCalcName(calcID).Name, LStr);
                        CalPlacing = 1;
                    }
                    else
                    {
                        foreach (var StrTemp in UpdateManager.Instance.CalcRowColSheet.Select(x => UpdateManager.Instance.CalcRowColSheet[Globals.ThisAddIn.Application.ActiveWorkbook.Name + "^" + Globals.ThisAddIn.Application.ActiveSheet.Name]).ToList())
                        {
                            List<String> StrPostionList = StrTemp;
                            foreach (var StrPostion in StrPostionList)
                            {
                                try
                                {
                                    String[] SplitStrPostion = StrPostion.Split('^');
                                    Int32 strExistsRow = Convert.ToInt32(SplitStrPostion[0]);
                                    Int32 strExistsCol = Convert.ToInt32(SplitStrPostion[1]);
                                    Int32 strEExistsRow = Convert.ToInt32(SplitStrPostion[2]);
                                    Int32 strEExistsCol = Convert.ToInt32(SplitStrPostion[3]);
                                    Int32 ExistsWidth = Convert.ToInt32(strEExistsCol);//Column
                                    Int32 ExistsHieght = Convert.ToInt32(strEExistsRow);//Row
                                    Int32 Width = Convert.ToInt32(EndColumn);
                                    Int32 Hieght = EndRow;
                                    System.Drawing.Rectangle rectangle1 = new System.Drawing.Rectangle(strExistsRow, strExistsCol, Math.Abs(ExistsWidth), ExistsHieght);
                                    System.Drawing.Rectangle rectangle2 = new System.Drawing.Rectangle(StartingRow, StartingColumn, Math.Abs(Width), Math.Abs(Hieght));
                                    if (!Utilities.Instance.rectOverlap(rectangle1, rectangle2))
                                    {
                                        flag = true;
                                        CalPlacing = 1;
                                    }
                                    else
                                    {
                                        CalPlacing = 0;
                                        break;
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                    if (CalPlacing == 1)
                    {
                        if (flag == true)
                        {
                            UpdateManager.Instance.CalcRowColSheet[Globals.ThisAddIn.Application.ActiveWorkbook.Name + "^" + Utilities.Instance.GetWorksheetByCalcName(calcID).Name].Add(StartingRow + "^" + StartingColumn + "^" + Convert.ToInt32(EndRow) + "^" + Convert.ToInt32(EndColumn));
                        }
                    }
                    else
                    {
                        UpdateManager.Instance.CalcSheetMap.Remove(calcID);
                        Messagecls.AlertMessage(21, "");
                        //  MessageBox.Show("An another app is already running at the selected cell in Excel. Please select some other empty cell instead.");
                    }
                }
                else
                {
                    Messagecls.AlertMessage(22, "");
                    // MessageBox.Show("Please login first before playing top of the book.");
                }
                return CalPlacing;
            }
            catch (Exception ex)
            {
                LogUtility.Error("Utilites.cs", "CoverArea();", "Storing protedted area of placed grid or calc on sheet. ", ex);
                return -1;
            }
        }

        public void UpdateCustomAddInsData(System.Data.DataTable objdob, string CalcName)
        {
            if (!DataUtil.Instance.bIsUserLoggedIn)
                return;

            if (objdob != null && CalcName != null)
            {
                string sAddIn = Utilities.instance.ImageToAddInList[CalcName];

                try
                {
                    object addinRef = sAddIn;
                    Microsoft.Office.Core.COMAddIn G_Addi = Globals.ThisAddIn.Application.COMAddIns.Item(ref addinRef);
                    if (G_Addi.Connect == true)
                    {
                        dynamic G_object = G_Addi.Object;
                        G_object.UpdateData(objdob, CalcName);
                    }
                }
                catch (Exception ex)
                {
                    LogUtility.Error("Utilites.cs", "UpdateCustomAddInsData();", "Error while getting real time data of Add-in", ex);
                }

                /*foreach (string subKeyName in DataUtil.Instance.AddInsList)
                {
                    try
                    {
                        object addinRef = subKeyName;
                        Microsoft.Office.Core.COMAddIn G_Addi = Globals.ThisAddIn.Application.COMAddIns.Item(ref addinRef);
                        if (G_Addi.Connect == true)
                        {
                            dynamic G_object = G_Addi.Object;
                            G_object.UpdateData(objdob, CalcName);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogUtility.Error("Utilites.cs", "UpdateCustomAddInsData();", "Error while getting real time data of Add-in", ex);
                        continue;
                    }
                }*/
            }
        }

        public void UpdateCustomAddInsImageStatus(Boolean IsConnected, String CalcName)
        {
            string sAddIn = Utilities.instance.ImageToAddInList[CalcName];

            try
            {
                object addinRef = sAddIn;
                Microsoft.Office.Core.COMAddIn G_Addi = Globals.ThisAddIn.Application.COMAddIns.Item(ref addinRef);
                if (G_Addi.Connect == true)
                {
                    LogUtility.Info("Utilites.cs", "UpdateCustomAddInsImageStatus();", "Updating Image Status: " + CalcName + " - " + IsConnected);

                    dynamic G_object = G_Addi.Object;
                    G_object.UpdateImageStatus(IsConnected, CalcName);
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("Utilites.cs", "UpdateCustomAddInsImageStatus();", "Error in updating image status", ex);
            }

            /*foreach (string subKeyName in DataUtil.Instance.AddInsList)
            {
                try
                {
                    object addinRef = subKeyName;
                    Microsoft.Office.Core.COMAddIn G_Addi = Globals.ThisAddIn.Application.COMAddIns.Item(ref addinRef);
                    if (G_Addi.Connect == true)
                    {
                        dynamic G_object = G_Addi.Object;
                        G_object.UpdateImageStatus(IsConnected, CalcName);
                        LogUtility.Info("Utilites.cs", "UpdateCustomAddInsImageStatus();", CalcName + " connection status updated to : " + IsConnected);
                    }
                }
                catch (Exception ex)
                {
                    LogUtility.Error("Utilites.cs", "UpdateCustomAddInsImageStatus();", "Error while getting real time data of Add-in", ex);
                    continue;
                }
            }*/
        }

        public void Dispose()
        {
            instance = null;
        }

        public bool IsInEditMode()
        {
            try
            {
                Globals.ThisAddIn.Application.Interactive = Globals.ThisAddIn.Application.Interactive;
                return false; // no exception, ecel is 
            }
            catch
            {
                return true; // in edit mode
            }
        }
        /// <summary>
        /// Create cookie with necessary information to be used for Beast Excel communication.
        /// </summary>
        /// <returns>Cookie with necessary information.</returns>
        internal Cookie GetCookies()
        {
            try
            {
                Cookie awsCookie;
                WebRequest webRequest = WebRequest.Create(DataUtil.Instance.SignalRHubKey + "/signalr/hubs") as HttpWebRequest;
                webRequest.UseDefaultCredentials = true;
                webRequest.PreAuthenticate = true;
                webRequest.Proxy = WebRequest.DefaultWebProxy;
                webRequest.CachePolicy = WebRequest.DefaultCachePolicy;
                webRequest.Proxy.Credentials = CredentialCache.DefaultCredentials;
                webRequest.Timeout = 5000;
                using (var response = webRequest.GetResponse() as HttpWebResponse)
                {
                    try
                    {
                        string[] cookieVal = response.Headers["Set-Cookie"].ToString().Split(';');
                        awsCookie = new Cookie(cookieVal[0].Split('=')[0], cookieVal[0].Split('=')[1], cookieVal[1].Split('=')[1], response.ResponseUri.Host);
                    }
                    catch (Exception ex)
                    {
                        if (DataUtil.Instance.ServerName.ToUpper() != "TEST")
                            LogUtility.Error("SingalRCoonectionManager.cs", "GetCookies();", "Inner Message of GetCookies fuction..", ex);

                        return null;
                    }
                }
                return awsCookie;
            }
            catch (Exception ex)
            {
                LogUtility.Error("SingalRCoonectionManager.cs", "GetCookies();", ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// Converting string to MD5 Format
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertToSMD5(string str)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Prov = new System.Security.Cryptography.MD5CryptoServiceProvider();
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] md5 = md5Prov.ComputeHash(encoding.GetBytes(str));
            string _result = "";
            for (int i = 0; i < md5.Length; i++)
            {
                _result += ("0" + String.Format("{0:X}", md5[i])).Substring(Convert.ToInt32(md5[i]) <= 15 ? 0 : 1, 2);
            }
            return _result;
        }
    }
}
