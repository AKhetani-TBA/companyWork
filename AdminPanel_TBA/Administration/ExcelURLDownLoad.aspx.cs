using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using VCM.Common;
using System.Data;
using System.Web.UI.HtmlControls;
//using VCM.Common.Log;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Services;
using System.Web.Script.Services;
using System.Configuration;
using System.IO.Compression;
using Ionic.Zip;
using System.Net;
using TBA.Utilities;


namespace Administration
{
    public partial class ExcelURLDownLoad : System.Web.UI.Page
    {
        private SessionInfo _session;

        public SessionInfo CurrentSession
        {
            get
            {
                if (_session == null)
                {
                    _session = new SessionInfo(HttpContext.Current.Session);
                }
                return _session;
            }
            set
            {
                _session = value;
            }
        }
        DataTable tblSendAutourlInfo = null;
        string page_strFromEmail = "";
        string page_strCompanyAddress = "";
        string page_strAdditionalCCemail = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentSession.User == null)
            {
                Session.Clear();
                Session.Abandon();
                Response.Redirect("SessionTimeOut.htm", false);
                return;
            }
            tblSendAutourlInfo = new DataTable();
            DataColumn dcTmp = new DataColumn("UserId");
            tblSendAutourlInfo.Columns.Add(dcTmp);
            dcTmp = new DataColumn("UserName");
            tblSendAutourlInfo.Columns.Add(dcTmp);
            dcTmp = new DataColumn("UserEmailId");
            tblSendAutourlInfo.Columns.Add(dcTmp);
            dcTmp = new DataColumn("CustomerId");
            tblSendAutourlInfo.Columns.Add(dcTmp);
            page_strFromEmail = System.Configuration.ConfigurationManager.AppSettings["FromEmail"].ToString();
            page_strCompanyAddress = UtilityHandler.strVCM_RrMailAddress_Html.ToString();
            page_strAdditionalCCemail = System.Configuration.ConfigurationManager.AppSettings["Openf2Admin"].ToString();
            if (!IsPostBack)
            {
                GetCustomerList();
                //GetExcelPackageVersion();
                //trChkPlugins.Visible = false;
                //LoadAddIns();
                GetExcelPackageVersionInfo();
                GetAutoURLHistory();
            }
        }

        //private void GetExcelPackageVersion()
        //{
        //    DataTable dt = BLL.Domain.GetExcelProductMaster(null);
        //    if (dt.Rows.Count > 0)
        //    {

        //        DataView view = dt.AsDataView();
        //        view.RowFilter = "Isnull(isMain,false) = true and Isnull(ParentId,0) = 0";
        //        if (view.Count > 0)
        //        {
        //            ddlExcelBasePlugins.DataTextField = "ExcelProductMasterName";
        //            ddlExcelBasePlugins.DataValueField = "ExcelProducMastertId";
        //            ddlExcelBasePlugins.DataSource = view;
        //            ddlExcelBasePlugins.DataBind();
        //        }

        //    }
        //    ddlExcelBasePlugins.Items.Insert(0, new ListItem("--Select--", "0", true));

        //}

        private void GetExcelPackageVersionInfo()
        {
            DataTable dt = BLL.Domain.GetExcelPackageVersionInfo(null);
            if (dt.Rows.Count > 0)
            {
                rptExcelVersionInfo.DataSource = dt;
                lblNoRecordsExcelVersionInfo.Visible = false;
            }
            else
            {
                rptExcelVersionInfo.DataSource = null;
                lblNoRecordsExcelVersionInfo.Visible = true;

            }
            rptExcelVersionInfo.DataBind();
        }


        //protected void ddlExcelVersions_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    LoadAddIns();

        //}


        //private void LoadAddIns()
        //{

        //    DataTable dt = BLL.Domain.GetExcelProductMaster(null);
        //    DataView viewAddins = dt.AsDataView();
        //    viewAddins.RowFilter = "(isMain = false or Isnull(isMain,false) = false) and Isnull(ParentId,0) = 0";
        //    trAddins.InnerHtml = "";
        //    trAddins.InnerHtml += "<td style='text-align: right; width: 18%;'>Select Addins :  </td>";
        //    trAddins.InnerHtml += "<td>";
        //    trAddins.InnerHtml += "<table id='tblAddins'>";
        //    DataTable dtComp = BLL.Domain.GetExcelMappingByExcelProductMasterId(Convert.ToInt32(ddlExcelVersions.SelectedValue));
        //    foreach (DataRowView drview in viewAddins)
        //    {

        //        DataView dtCompView = dtComp.AsDataView();
        //        DataRow dr = drview.Row;//Convert.ToInt32(dr["ExcelProducMastertId"]
        //        DataTable datatAddIns = BLL.Domain.GetExcelProductionVersionMasterProduct(null);
        //        DataView dvAddIns = datatAddIns.AsDataView();
        //        dvAddIns.RowFilter = "ParentId = " + dr["ExcelProducMastertId"].ToString();
        //        DataView dvParent = datatAddIns.AsDataView();
        //        dvParent.RowFilter = "(isMain = false or Isnull(isMain,false) = false) and Isnull(ParentId,0) = 0 and ExcelProducMastertId = " + Convert.ToInt32(dr["ExcelProducMastertId"]);

        //        // if (dvAddIns.Count > 0)
        //        {

        //            trAddins.InnerHtml += "<tr>";
        //            trAddins.InnerHtml += "<td style='width:30px;'>" + dr["ExcelProductMasterName"].ToString() + "</td>";
        //            trAddins.InnerHtml += "<td style='width:50px;'>";
        //            trAddins.InnerHtml += "<select id='" + dr["ExcelProducMastertId"].ToString() + "'>";
        //            trAddins.InnerHtml += "<option value=\"0\" selected=\"selected\">-- Select --</option>";
        //            int VersionNumber = 0;
        //            if (dvAddIns.Count > 0)
        //            {
        //                foreach (DataRowView drAddin in dvAddIns)
        //                {
        //                    DataRow dv = drAddin.Row;
        //                    dtCompView.RowFilter = "VersionNumber = " + Convert.ToInt32(dv["VersionNumber"]);
        //                    if (Convert.ToInt32(dv["VersionNumber"]) != VersionNumber && dtCompView.Count > 0)
        //                    {
        //                        trAddins.InnerHtml += "<option value=\"" + dv["VersionNumber"].ToString() + "\">" + GetVersion(dv["VersionNumber"].ToString()) + "</option>";
        //                    }
        //                    VersionNumber = Convert.ToInt32(dv["VersionNumber"]);
        //                }
        //            }
        //            else
        //            {
        //                if (dvParent.Count > 0)
        //                {
        //                    foreach (DataRowView drParent in dvParent)
        //                    {
        //                        DataRow dvP = drParent.Row;
        //                        dtCompView.RowFilter = "VersionNumber = " + Convert.ToInt32(dvP["VersionNumber"]);
        //                        if (Convert.ToInt32(dvP["VersionNumber"]) != VersionNumber && dtCompView.Count > 0)
        //                        {
        //                            trAddins.InnerHtml += "<option value=\"" + dvP["VersionNumber"].ToString() + "\">" + GetVersion(dvP["VersionNumber"].ToString()) + "</option>";
        //                        }
        //                        VersionNumber = Convert.ToInt32(dvP["VersionNumber"]);

        //                    }
        //                }

        //            }


        //            trAddins.InnerHtml += "</select>";

        //            DataView childPluginsView = dt.AsDataView();
        //            childPluginsView.RowFilter = "Isnull(ParentId,0) = " + dr["ExcelProducMastertId"].ToString();
        //            if (childPluginsView.Count > 0)
        //            {
        //                trAddins.InnerHtml += "<td>";
        //                foreach (DataRowView drchild in childPluginsView)
        //                {
        //                    DataRow drchildrow = drchild.Row;
        //                    trAddins.InnerHtml += "<input type=\"radio\" name='radio" + drchildrow["ParentId"].ToString() + "' value='" + drchildrow["ParentId"].ToString() + "#" + drchildrow["ExcelProducMastertId"].ToString() + "#" + dr["ExcelProducMastertId"].ToString() + "' >" + "  " + drchildrow["ExcelProductMasterName"].ToString() + "   ";


        //                }
        //                trAddins.InnerHtml += "</td>";
        //            }
        //            else
        //            {
        //                trAddins.InnerHtml += "<td></td>";
        //            }
        //            trAddins.InnerHtml += "</td>";
        //            trAddins.InnerHtml += "</tr>";
        //        }
        //    }

        //    trAddins.InnerHtml += "</table>";
        //    trAddins.InnerHtml += "</td>";
        //}

        public void GetCustomerList()
        {
            try
            {
                DataTable dTable = BLL.Domain.FillUsersList(Convert.ToString(CurrentSession.User.UserID));
                //   DataTable dTable = BLL.Domain.Get_AppStore_User_List(null);
                if (dTable != null && dTable.Rows.Count > 0)

                    rptrUsers.DataSource = dTable;
                else
                    rptrUsers.DataSource = null;

                rptrUsers.DataBind();
            }
            catch (Exception ex)
            {
                //VcmLogManager.Log.writeLog(Convert.ToString(CurrentSession.User.FirstName), "", "", "AutoURL", "Get_Customer_List", ex.StackTrace.ToString() + "<br/><br/>" + ex.Message, UtilityHandler.Get_IPAddress(Request.UserHostAddress));
                LogUtility.Error("AutoURL", "Get_Customer_List", ex.Message, ex);
            }
        }

        public void GetAutoURLHistory()
        {
            DataTable dt = BLL.Domain.GetExcelAutoUrl(null);
            dt.Columns.Add("SentPackageURL", typeof(System.String));
            foreach (DataRow dr in dt.Rows)
            {
                dr["SentPackageURL"] = ConfigurationManager.AppSettings["ExcelAutoDownloadURL"].ToString() + "?RefNo=" + Convert.ToString(dr["ExcelGuid"]) + "&pkg=" + Convert.ToString(dr["PackageId"]);
            }

            if (dt.Rows.Count > 0)
            {
                rptAutoURLHistory.DataSource = dt;
                // lblNoRecordsAutoURLHistory.Visible = false;
            }
            else
            {
                rptAutoURLHistory.DataSource = null;
                // lblNoRecordsAutoURLHistory.Visible = true;
            }
            rptAutoURLHistory.DataBind();
        }

        protected void lstBasePlugins_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //protected void ddlExcelBasePlugins_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    if (ddlExcelBasePlugins.SelectedIndex > 0)
        //    {
        //        ddlExcelVersions.Items.Clear();
        //        DataTable dt = BLL.Domain.GetExcelProductionVersionMasterByExcelProducMastertId(Convert.ToInt32(ddlExcelBasePlugins.SelectedValue));
        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                dr["VersionNumber"] = GetVersion(Convert.ToString(dr["VersionNumber"]));
        //            }
        //            ddlExcelVersions.DataSource = dt;
        //            ddlExcelVersions.DataTextField = "VersionNumber";
        //            ddlExcelVersions.DataValueField = "ExcelProductVersionMasterId";
        //            ddlExcelVersions.DataBind();
        //        }
        //        ddlExcelVersions.Items.Insert(0, new ListItem("--Select--", "0", true));
        //    }
        //    else
        //    {
        //        ddlExcelVersions.Items.Clear();
        //        ddlExcelVersions.Items.Insert(0, new ListItem("--Select--", "0", true));
        //    }


        //}

        protected string GetVersion(string str)
        {
            string updated = string.Empty;
            if (str.Length == 7)
                updated = str.Insert(5, ".").Insert(3, ".").Insert(1, ".");
            else if (str.Length == 8)
                updated = str.Insert(6, ".").Insert(4, ".").Insert(2, ".");
            return updated;
        }

        //protected void btnGeneratePack_Click(object sender, EventArgs e)
        //{

        //    /// sample hdnBuild.Value = "1#gfhgh#--Select-->AddIns=5#08050000>AddIns=6#02080600#ChildAddIns=2#4#2>AddIns=7#011200#ChildAddIns=3#7#3";

        //    if (!string.IsNullOrEmpty(hdnBuild.Value))
        //    {
        //        string VersionInfo = string.Empty;
        //        Guid guid = Guid.NewGuid();

        //        ZipFile zipFile = new ZipFile();
        //        string[] split = hdnBuild.Value.Split('>');
        //        string[] basepl = null;
        //        string rootname = string.Empty;
        //        for (int i = 0; i < split.Length; i++)
        //        {
        //            if (i == 0)
        //            {
        //                basepl = split[i].Split('#');
        //                VersionInfo += basepl[1].ToString() + "-" + basepl[2].ToString() + " ";
        //                rootname = "TheBeastAppsExcel_V" + basepl[2].ToString();
        //                DataTable dt = BLL.Domain.GetExcelProductionVersionMasterProduct(Convert.ToInt32(basepl[0]));
        //                if (dt.Rows.Count > 0)
        //                {
        //                    //TheBeastAppsExcel_V8.05.00

        //                    string Mainpath = Server.MapPath("~/ExcelVersionDownload/" + guid.ToString() + "/" + basepl[1].ToString() + "/" + basepl[2].ToString() + "/" + basepl[1] + ".msi");
        //                    byte[] setupData = (byte[])dt.Rows[0]["Data"];
        //                    if (setupData.Length > 0)
        //                    {
        //                        SaveBytesToFile(Mainpath, setupData);
        //                        string filepath = Server.MapPath("~/ExcelVersionDownload/" + guid.ToString() + "/" + basepl[1].ToString() + "/" + basepl[2].ToString());
        //                        string[] files = Directory.GetFiles(filepath);
        //                        //zipFile.AddFiles(files, basepl[1].ToString() + "/" + basepl[2].ToString());                                

        //                        zipFile.AddFiles(files, rootname);
        //                        string[] fileFrameworkpath = Directory.GetFiles(Server.MapPath("~/ExcelVersionDownload/DotNetFX40Client"));
        //                        zipFile.AddFiles(fileFrameworkpath, rootname + "/DotNetFX40Client");
        //                        string[] filesVsto = Directory.GetFiles(Server.MapPath("~/ExcelVersionDownload/VSTOR40"));
        //                        zipFile.AddFiles(filesVsto, rootname + "/VSTOR40");

        //                    }
        //                    byte[] setupDataExe = (byte[])dt.Rows[0]["DataExe"];
        //                    string Mainpathexe = Server.MapPath("~/ExcelVersionDownload/" + guid.ToString() + "/" + basepl[1].ToString() + "/" + basepl[2].ToString() + "/SetUp/setup.exe");
        //                    if (setupDataExe.Length > 0)
        //                    {
        //                        SaveBytesToFile(Mainpathexe, setupDataExe);
        //                        string filepath = Server.MapPath("~/ExcelVersionDownload/" + guid.ToString() + "/" + basepl[1].ToString() + "/" + basepl[2].ToString() + "/SetUp");
        //                        string[] files = Directory.GetFiles(filepath);
        //                        zipFile.AddFiles(files, rootname);

        //                    }

        //                }
        //            }

        //            if (i > 0)
        //            {
        //                string Addins = split[i].Substring(split[i].IndexOf('=') + 1);
        //                string[] AddinSplit = Addins.Split('#');
        //                if (AddinSplit[0] != "0" && AddinSplit[1] != "0")
        //                {

        //                    DataTable dtpl = BLL.Domain.GetExcelProductionVersionMasterByExcelProducMastertId(Convert.ToInt32(AddinSplit[0]));
        //                    if (dtpl.Rows.Count > 0)
        //                    {
        //                        bool isContainChild = false;
        //                        bool noChildRecords = false;
        //                        string childname = "";
        //                        string drpath = Server.MapPath("~/ExcelVersionDownload/" + guid.ToString() + "/" + basepl[1].ToString() + "/" + basepl[2].ToString() + "/" + Convert.ToString(dtpl.Rows[0]["ExcelProductMasterName"]) + "/" + AddinSplit[1] + "/" + Convert.ToString(dtpl.Rows[0]["ExcelProductMasterName"]) + ".msi");
        //                        byte[] setupData = null;
        //                        if (dtpl.Rows[0]["Data"] != DBNull.Value)
        //                            setupData = (byte[])dtpl.Rows[0]["Data"];
        //                        else
        //                        {
        //                            setupData = new byte[0];
        //                            noChildRecords = true;
        //                        }
        //                        if (setupData.Length > 0)
        //                        {
        //                            SaveBytesToFile(drpath, setupData);
        //                            string filepath = Server.MapPath("~/ExcelVersionDownload/" + guid.ToString() + "/" + basepl[1].ToString() + "/" + basepl[2].ToString() + "/" + Convert.ToString(dtpl.Rows[0]["ExcelProductMasterName"]) + "/" + AddinSplit[1]);
        //                            string[] files = Directory.GetFiles(filepath);
        //                            //zipFile.AddFiles(files, basepl[1].ToString() + "/" + basepl[2].ToString() + "/" + Convert.ToString(dtpl.Rows[0]["ExcelProductMasterName"]) + "/" + AddinSplit[1]);
        //                            zipFile.AddFiles(files, rootname);
        //                        }


        //                        if (Addins.Contains("ChildAddIns"))
        //                        {
        //                            string ChildAddIns = Addins.Substring(Addins.IndexOf("ChildAddIns=") + 1);
        //                            string[] ChildAddInSplit = ChildAddIns.Split('#');

        //                            DataTable dtchild = BLL.Domain.GetExcelProductionVersionMasterByExcelProducMastertId(Convert.ToInt32(ChildAddInSplit[1]));
        //                            DataView dvchild = dtchild.AsDataView();
        //                            dvchild.RowFilter = "VersionNumber = " + AddinSplit[1].ToString();
        //                            if (dvchild.Count > 0)
        //                            {

        //                                foreach (DataRowView child in dvchild)
        //                                {
        //                                    DataRow dvdr = child.Row;
        //                                    isContainChild = true;
        //                                    childname = dvdr["ExcelProductMasterName"] + "-" + GetVersion(Convert.ToString(dvdr["VersionNumber"]));
        //                                    string childmsiname = string.Empty;
        //                                    if (dvdr["ExcelProductMasterName"].ToString().Contains("-"))
        //                                    {
        //                                        string[] SplitChild = dvdr["ExcelProductMasterName"].ToString().Split('-');
        //                                        childmsiname = "Beast_" + SplitChild[1] + "_AddIn";
        //                                    }
        //                                    else
        //                                    {
        //                                        childmsiname = dvdr["ExcelProductMasterName"].ToString();
        //                                    }
        //                                    //string childpath = Server.MapPath("~/ExcelVersionDownload/" + guid.ToString() + "/" + basepl[1].ToString() + "/" + basepl[2].ToString() + "/" + Convert.ToString(dtpl.Rows[0]["ExcelProductMasterName"]) + "/" + AddinSplit[1] + "/" + dvdr["ExcelProductMasterName"] + "/" + dvdr["VersionNumber"] + "/" + dvdr["ExcelProductMasterName"] + ".msi");
        //                                    string childpath = Server.MapPath("~/ExcelVersionDownload/" + guid.ToString() + "/" + basepl[1].ToString() + "/" + basepl[2].ToString() + "/" + Convert.ToString(dtpl.Rows[0]["ExcelProductMasterName"]) + "/" + childmsiname + ".msi");
        //                                    byte[] childSetupdata = (byte[])dvdr["Data"];
        //                                    if (childSetupdata.Length > 0)
        //                                    {
        //                                        SaveBytesToFile(childpath, childSetupdata);
        //                                        //string filepath = Server.MapPath("~/ExcelVersionDownload/" + guid.ToString() + "/" + basepl[1].ToString() + "/" + basepl[2].ToString() + "/" + Convert.ToString(dtpl.Rows[0]["ExcelProductMasterName"]) + "/" + AddinSplit[1] + "/" + dvdr["ExcelProductMasterName"] + "/" + dvdr["VersionNumber"]);
        //                                        string filepath = Server.MapPath("~/ExcelVersionDownload/" + guid.ToString() + "/" + basepl[1].ToString() + "/" + basepl[2].ToString() + "/" + Convert.ToString(dtpl.Rows[0]["ExcelProductMasterName"]));
        //                                        string[] files = Directory.GetFiles(filepath);
        //                                        //zipFile.AddFiles(files, basepl[1].ToString() + "/" + basepl[2].ToString() + "/" + Convert.ToString(dtpl.Rows[0]["ExcelProductMasterName"]) + "/" + AddinSplit[1] + "/" + dvdr["ExcelProductMasterName"] + "/" + dvdr["VersionNumber"]);
        //                                        //zipFile.AddFiles(files, basepl[1].ToString() + "/" + basepl[2].ToString() + "/" + Convert.ToString(dtpl.Rows[0]["ExcelProductMasterName"]));
        //                                        zipFile.AddFiles(files, rootname);
        //                                    }


        //                                }
        //                            }
        //                            else
        //                                noChildRecords = true;
        //                        }
        //                        if (isContainChild)
        //                        {
        //                            if (!string.IsNullOrEmpty(childname))
        //                                VersionInfo += "[ " + childname + " ]";
        //                            else
        //                                VersionInfo += "[ " + Convert.ToString(dtpl.Rows[0]["ExcelProductMasterName"]) + " ]";
        //                        }
        //                        else if (noChildRecords)
        //                        {

        //                        }
        //                        else
        //                        {
        //                            VersionInfo += "[" + Convert.ToString(dtpl.Rows[0]["ExcelProductMasterName"]) + "-" + GetVersion(Convert.ToString(dtpl.Rows[0]["VersionNumber"])) + "]";
        //                        }
        //                    }

        //                }


        //            }

        //        }
        //        if (basepl.Length > 0)
        //        {
        //            string zippath = Server.MapPath("~/ExcelVersionZip/" + guid.ToString());
        //            if (!Directory.Exists(zippath))
        //                Directory.CreateDirectory(zippath);
        //            zipFile.Save(zippath + "/" + basepl[1].ToString() + ".zip");
        //            string FullPath = ConfigurationManager.AppSettings["ExcelDownLoadURL"].ToString() + guid.ToString() + "/" + basepl[1].ToString() + ".zip";
        //            BLL.Domain.SubmitExcelPackageVersionInfo(0, FullPath, VersionInfo, Convert.ToInt32(CurrentSession.User.UserID), false);
        //            lblpackagemsg.Text = "Package has been genearated successfully.";

        //        }
        //        hdnBuild.Value = "";
        //        ddlExcelVersions.SelectedIndex = 0;
        //        ddlExcelBasePlugins.SelectedIndex = 0;
        //        VersionInfo = "";
        //        System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo(Server.MapPath("~/ExcelVersionDownload/" + guid.ToString()));
        //        foreach (FileInfo file in downloadedMessageInfo.GetFiles())
        //        {
        //            file.Delete();
        //        }
        //        foreach (DirectoryInfo dir in downloadedMessageInfo.GetDirectories())
        //        {
        //            dir.Delete(true);
        //        }

        //        if (Directory.Exists(Server.MapPath("~/ExcelVersionDownload/" + guid.ToString())))
        //        {
        //            Directory.Delete(Server.MapPath("~/ExcelVersionDownload/" + guid.ToString()));
        //        }
        //        GetExcelPackageVersionInfo();

        //    }
        //}



        public static void SaveBytesToFile(string filename, byte[] bytesToWrite)
        {
            if (filename != null && filename.Length > 0 && bytesToWrite != null)
            {
                if (!Directory.Exists(Path.GetDirectoryName(filename)))
                    Directory.CreateDirectory(Path.GetDirectoryName(filename));

                FileStream file = File.Create(filename);
                file.Write(bytesToWrite, 0, bytesToWrite.Length);
                file.Close();
            }
        }

        protected void btnPackageDownLoad_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdnPackageDownload.Value))
            {
                Stream stream = null;
                int bytesToRead = 10000;
                byte[] buffer = new Byte[bytesToRead];
                DataTable dt = BLL.Domain.GetExcelPackageVersionInfo(Convert.ToInt32(hdnPackageDownload.Value));
                if (dt.Rows.Count > 0)
                {
                    string FullURL = dt.Rows[0]["PackageName"].ToString();
                    string DummyURL = FullURL.Substring(0, FullURL.ToString().LastIndexOf('/')) + "/";

                    try
                    {
                        HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(FullURL);
                        string fileName = FullURL.Substring(FullURL.ToString().LastIndexOf('/') + 1);
                        HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();
                        if (fileReq.ContentLength > 0)
                            fileResp.ContentLength = fileReq.ContentLength;
                        stream = fileResp.GetResponseStream();
                        var resp = HttpContext.Current.Response;
                        resp.ContentType = "application/zip";
                        resp.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                        resp.AddHeader("Content-Length", fileResp.ContentLength.ToString());
                        int length;
                        do
                        {
                            if (resp.IsClientConnected)
                            {
                                length = stream.Read(buffer, 0, bytesToRead);
                                resp.OutputStream.Write(buffer, 0, length);
                                resp.Flush();
                                buffer = new Byte[bytesToRead];
                            }
                            else
                            {

                                length = -1;
                            }
                        } while (length > 0);
                    }
                    finally
                    {
                        if (stream != null)
                        {
                            stream.Close();
                        }
                    }
                }



            }
            hdnPackageDownload.Value = "";
        }

        private void DownLoadFileByWebRequest(string urlAddress, string filePath)
        {
            try
            {
                System.Net.HttpWebRequest request = null;
                System.Net.HttpWebResponse response = null;
                request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(urlAddress);
                request.Timeout = 30000;  //8000 Not work
                response = (System.Net.HttpWebResponse)request.GetResponse();
                Stream s = response.GetResponseStream();
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                FileStream os = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
                byte[] buff = new byte[102400];
                int c = 0;
                while ((c = s.Read(buff, 0, 10400)) > 0)
                {
                    os.Write(buff, 0, c);
                    os.Flush();
                }
                os.Close();
                s.Close();

            }
            catch
            {
                return;
            }
            finally
            {
            }

        }

        protected void btnSetExpiry_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdnSetExpiryId.Value) && ddlExpiryGuid.SelectedIndex > 0)
            {

                DataTable dt = BLL.Domain.GetExcelAutoUrl(Convert.ToInt32(hdnSetExpiryId.Value));
                if (dt.Rows.Count > 0)
                {
                    DateTime dtFrom = DateTime.UtcNow;
                    DateTime dtTo = DateTime.UtcNow.AddMinutes(Convert.ToDouble(ddlExpiryGuid.SelectedValue));

                    BLL.Domain.SubmitExcelAutoUrl(Convert.ToInt32(hdnSetExpiryId.Value), Convert.ToInt32(dt.Rows[0]["Userid"]), dtFrom, Convert.ToString(dt.Rows[0]["ExcelGuid"]), dtTo, Convert.ToInt32(CurrentSession.User.UserID), false, Convert.ToInt32(dt.Rows[0]["PackageId"]), Convert.ToInt32(dt.Rows[0]["ClickCount"]));
                    hdnSetExpiryId.Value = "";
                    GetAutoURLHistory();
                }

            }
        }

        protected void btnDeleteExcelPackage_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdnDeletePackage.Value))
            {
                BLL.Domain.SubmitExcelPackageVersionInfoIsDelete(Convert.ToInt32(hdnDeletePackage.Value), Convert.ToInt32(CurrentSession.User.UserID), true);
                hdnDeletePackage.Value = "";
                GetExcelPackageVersionInfo();
            }
        }


        protected void btnDeleteAutoURL_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdnDeleteAutoURL.Value))
            {
                BLL.Domain.SubmitExcelAutoUrlIsDelete(Convert.ToInt32(hdnDeleteAutoURL.Value), Convert.ToInt32(CurrentSession.User.UserID), true);
                hdnDeleteAutoURL.Value = "";
                GetAutoURLHistory();

            }
        }

        protected void btnSendMail_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdnSelectedPackage.Value))
            {
                DataTable dt = BLL.Domain.GetExcelAutoUrl(Convert.ToInt32(hdnSelectedPackage.Value));
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Expirydate"] != DBNull.Value)
                    {
                        DateTime currentDate = DateTime.UtcNow;
                        DateTime expdate = Convert.ToDateTime(dt.Rows[0]["ExpiryDate"]);
                        if (expdate < currentDate)
                        {
                            lblMessage.Style.Add("display", "block");
                            lblMessage.Text = "Selected package expiry date is less than current date. Select another package or extend expiry of selected package.";
                            return;

                        }

                    }
                }
                SessionInfo CurrentSession = new SessionInfo(HttpContext.Current.Session);

                lblMessage.Style.Add("display", "none");
                lblMessage.Text = "";

                string UserID = string.Empty;
                string strfailuer = string.Empty;

                DataRow _dr;

                string[] getDetails = hdnMailId.Value.ToString().Split(new string[] { "!," }, StringSplitOptions.None);
                for (int i = 0; i < getDetails.Length; i++)
                {

                    string[] getUserDetails = getDetails[i].Split(new char[] { ',' });
                    int var = getUserDetails[3].IndexOf("!");

                    if (var > 0)
                        getUserDetails[3] = getUserDetails[3].Remove(var);
                    _dr = tblSendAutourlInfo.NewRow();
                    _dr["UserId"] = getUserDetails[0];
                    _dr["UserName"] = getUserDetails[1];
                    _dr["UserEmailId"] = getUserDetails[2];
                    _dr["CustomerId"] = getUserDetails[3];
                    tblSendAutourlInfo.Rows.Add(_dr);
                }

                hdnMailId.Value = "";
                for (int i = 0; i < rptrUsers.Items.Count; i++)
                {
                    HtmlInputCheckBox chkDisplayTitle = (HtmlInputCheckBox)rptrUsers.Items[i].FindControl("CheckAll");
                    if (chkDisplayTitle.Checked)
                    {
                        (((HtmlInputCheckBox)rptrUsers.Items[i].FindControl("CheckAll"))).Checked = false;
                    }
                }

                //rptExcelVersionInfo //cbSelect
                for (int i = 0; i < rptExcelVersionInfo.Items.Count; i++)
                {
                    CheckBox chkDisplayTitle = (CheckBox)rptExcelVersionInfo.Items[i].FindControl("cbSelect");
                    if (chkDisplayTitle.Checked)
                    {
                        (((CheckBox)rptExcelVersionInfo.Items[i].FindControl("cbSelect"))).Checked = false;
                    }
                }



                lblMessage.Style.Add("display", "block");

                if (tblSendAutourlInfo.Rows.Count > 0)
                {
                    SendAutoUrlMail(tblSendAutourlInfo);
                    if (string.IsNullOrEmpty(strfailuer))
                        lblMessage.Text = "Mail has been sent.";
                    else
                        lblMessage.Text = "Mail has been sent to selected users except <br/> " + strfailuer;

                }
                else
                {
                    lblMessage.Text = "No user selected to send Auto url";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        private void SendAutoUrlMail(DataTable dtUserDtl)
        {
            try
            {
                if (!string.IsNullOrEmpty(hdnSelectedPackage.Value))
                {
                    string strMinuteInterval = Convert.ToString(ddlSendExpiry.SelectedValue);
                    DateTime dtFrom = DateTime.UtcNow;
                    DateTime dtTo = DateTime.UtcNow.AddMinutes(int.Parse(strMinuteInterval));

                    string MsgBodyTemplet, MsgBody;

                    string AutoURL = "";

                    string FromEmail = page_strFromEmail;

                    string strBcc = System.Configuration.ConfigurationManager.AppSettings["BCCEMAIL"].ToString();

                    string strMailSubject = "";
                    MsgBodyTemplet = "";

                    strMailSubject = "Download The Beast Apps Excel Package - AutoURL";
                    MsgBodyTemplet = "<div style=\"color:navy;font:normal 12px verdana\">" +

                                   "<br/>Dear [USERNAME],<br/><br/> You may download  Excel Package by clicking on the following URL. You may copy and paste this URL in your internet browser as well." +
                                    "<p><a href=[AUTOURL]>[AUTOURL]</a></p>" +
                                    "<br/><br/> This URL is valid for is as follows:<br/> " +
                                    "<table><tr><td> URL Valid For: </td> <td>&nbsp; " + dtFrom.ToString("dd-MMM-yyyy hh:mm:ss tt") + " (GMT)" + " To " + dtTo.ToString("dd-MMM-yyyy hh:mm:ss tt") + " (GMT)" + "</td></tr></table></p> " + "</td></tr></table>" +
                                    "<p><b>NOTE:</b><b><i>&nbsp;Please treat this URL confidential.</i></b></p>" +
                                    "<p> If you do not wish to receive these URLs, please let us know.</p>" +
                                    "<p>Please contact us if you have any questions.</p><br/>" +
                                      "Sincerely, <br/>" + CurrentSession.User.FirstName.ToString() + "<br/>" + page_strCompanyAddress + "</div></p>";
                    AutoURL = System.Configuration.ConfigurationManager.AppSettings["ExcelAutoDownloadURL"].ToString();

                    for (int i = 0; i < dtUserDtl.Rows.Count; i++)
                    {

                        string strUserEmail = Convert.ToString(dtUserDtl.Rows[i]["UserEmailId"]).Trim();
                        Guid guid = Guid.NewGuid();
                        string strFullUrl = AutoURL + "?RefNo=" + guid.ToString() + "&pkg=" + hdnSelectedPackage.Value.ToString();
                        MsgBody = MsgBodyTemplet;
                        MsgBody = MsgBody.Replace("[AUTOURL]", strFullUrl);
                        MsgBody = MsgBody.Replace("[USERNAME]", Convert.ToString(dtUserDtl.Rows[i]["UserName"]));
                        DateTime startDate = DateTime.UtcNow;
                        DateTime endDate = startDate.AddMinutes(Convert.ToDouble(strMinuteInterval));
                        BLL.Domain.SubmitExcelAutoUrl(0, Convert.ToInt32(dtUserDtl.Rows[i]["UserId"]), startDate, guid.ToString(), endDate, Convert.ToInt32(CurrentSession.User.UserID), false, Convert.ToInt32(hdnSelectedPackage.Value), 0);


                        string dtURLSts = BLL.Domain.Submit_Download_AutoURL(Convert.ToInt32(dtUserDtl.Rows[i]["UserId"]),
                            startDate,
                            endDate,
                            guid.ToString(),
                            MsgBody);

                        SendMail(FromEmail
                              , strUserEmail
                              , Convert.ToString(CurrentSession.User.PrimaryEmailID)
                              , strBcc
                              , strMailSubject
                              , MsgBody
                              , true);

                        LogUtility.Info("ExcelURLDownLoad", "SendMail", "AutoUrl sent to " + strUserEmail + ". Url=" + strFullUrl);
                    }
                    hdnSelectedPackage.Value = "";
                    GetAutoURLHistory();
                }
            }
            catch (Exception ex)
            {
                //VcmLogManager.Log.writeLog(Convert.ToString(CurrentSession.User.FirstName), "", "", "AutoURL", "SendAutoUrlMail()", ex.StackTrace.ToString() + "<br/><br/>" + ex.Message, UtilityHandler.Get_IPAddress(Request.UserHostAddress));
                LogUtility.Error("ExcelURLDownLoad", "SendAutoUrlMail", ex.Message, ex);
            }

        }

        private void SendMail(string FromId, string strTo, string strCC, string strBCC, string strSubject, string strBodyMsg, bool bFlag)
        {
            try
            {
                VCM_Mail _vcmMail = new VCM_Mail();

                _vcmMail.From = string.IsNullOrEmpty(FromId.Trim()) ? System.Configuration.ConfigurationManager.AppSettings["FromEmail"].ToString() : FromId.Trim();
                _vcmMail.To = strTo;

                if (!string.IsNullOrEmpty(strCC))
                {

                    if (string.IsNullOrEmpty(page_strAdditionalCCemail.Trim()))
                        _vcmMail.CC = strCC;
                    else
                        _vcmMail.CC = strCC + "," + page_strAdditionalCCemail;
                }
                if (bFlag)
                {
                    _vcmMail.BCC = strBCC;
                }
                // _vcmMail.SendAsync = true;
                _vcmMail.Subject = strSubject;
                _vcmMail.Body = strBodyMsg;
                _vcmMail.IsBodyHtml = true;
                _vcmMail.SendMail(0);
                _vcmMail = null;
                LogUtility.Info("AutoURL", "SendMail", "AutoUrl sent to " + strTo + ".");
            }
            catch (Exception ex)
            {
                //VcmLogManager.Log.writeLog(Convert.ToString(CurrentSession.User.FirstName), "", "", "AutoURL", "SendMail()", ex.StackTrace.ToString() + "<br/><br/>" + ex.Message, UtilityHandler.Get_IPAddress(Request.UserHostAddress));
                LogUtility.Error("AutoURL", "SendMail", ex.Message, ex);
            }
        }

        protected void rptExcelVersionInfo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBox cb = (CheckBox)e.Item.FindControl("cbSelect");
                HiddenField hdn = (HiddenField)e.Item.FindControl("hdnPackageId");

                if (cb != null)
                {
                    cb.Attributes.Add("onclick", "javascript:onCheckPack(this, '" + hdn.Value.ToString() + "');");
                }
            }
        }

        protected string writeImagewithCount(string str)
        {
            return "<img src='images/right-icon.png' alt=''  />&nbsp;&nbsp;" + str;
        }

        protected string writeImageForExpiry(string id, string expdate)
        {
            if (!string.IsNullOrEmpty(expdate))
            {
                if (Convert.ToDateTime(expdate) < DateTime.UtcNow)
                    return "<img src='images/plus-arrow-icon.png' alt='' onclick='setExipry(" + id + ")' />";
                else
                    return "";


            }
            else
                return "";
        }

        protected void imgDeleteExcelPackage_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton button = (sender as ImageButton);
            int ExcelPackageId = Convert.ToInt32(button.CommandArgument);
            BLL.Domain.SubmitExcelPackageVersionInfoIsDelete(ExcelPackageId, Convert.ToInt32(CurrentSession.User.UserID), true);
            GetExcelPackageVersionInfo();


        }

        protected void imgDeleteExcelAutoURL_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton button = (sender as ImageButton);
            int ExcelAutoURLId = Convert.ToInt32(button.CommandArgument);
            BLL.Domain.SubmitExcelAutoUrlIsDelete(ExcelAutoURLId, Convert.ToInt32(CurrentSession.User.UserID), true);
            GetAutoURLHistory();
        }


    }
}