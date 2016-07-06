using System;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Security.Permissions;
using System.Net;
using Ionic.Zip;
//using VCM.Common.Log;
using System.Text;
using TBA.Utilities;

namespace Administration
{
    public partial class ExcelPackage : System.Web.UI.Page
    {
        private SessionInfo _session;
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SessionServerConnectionString"].ToString());

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

        protected void Page_Load(object sender, EventArgs e)
        {

            string Apppath = Request.ApplicationPath;
            string HostPath = Request.Url.Host.ToString();
            if (CurrentSession.User == null)
            {
                Session.Clear();
                Session.Abandon();
                Response.Redirect("SessionTimeOut.htm", false);
                return;
            }

            lblExcelPackageMsg.Text = "";
            if (!IsPostBack)
            {
                GetExcelPackage();
                GetExcelPackageVersion();
                GetExcelPackageVersionMapping();
                excelProductMasterId = 0;
                ExcelProductVersionMasterId = 0;
                ExcelProductVersionMappingId = 0;
                GetExcelPackageVersionForPackage();




            }
            lblExcelPackageMessage.Text = "";
            lblExcelPackageMsg.Text = "";
            lblmsgMapping.Text = "";


        }



        private void GetExcelPackageVersionMapping()
        {
            DataTable dt = BLL.Domain.GetExcelLatestVersion(null);

            if (dt.Rows.Count > 0)
            {
                lblNoRecordsMappings.Visible = false;
                rptExcelProductVersionMap.DataSource = GenerateMappings(dt);


            }
            else
            {
                lblNoRecordsMappings.Visible = true;
                rptExcelProductVersionMap.DataSource = null;
            }
            rptExcelProductVersionMap.DataBind();
        }

        private void GetExcelPackageVersion()
        {
            DataTable dt = BLL.Domain.GetExcelProductVersionMaster(null);
            if (dt.Rows.Count > 0)
            {

                dt.Columns.Add("MasterwithVersion", typeof(System.String));
                foreach (DataRow dr in dt.Rows)
                {


                    dr["VersionNumber"] = GetVersion(Convert.ToString(dr["VersionNumber"]));

                    dr["MasterwithVersion"] = dr["ExcelProductMasterName"].ToString() + " - " + dr["VersionNumber"].ToString();



                }
                lblNoRecordsVersion.Visible = false;
                rptExcelVersions.DataSource = dt;

                DataView view = dt.AsDataView();
                view.RowFilter = "isMain = true";

                if (view.Count > 0)
                {
                    ddlMainExcelPlugin.DataSource = view;
                    ddlMainExcelPlugin.DataTextField = "MasterwithVersion";
                    ddlMainExcelPlugin.DataValueField = "ExcelProductVersionMasterId";
                    ddlMainExcelPlugin.DataBind();
                }
                ddlMainExcelPlugin.Items.Insert(0, new ListItem("--Select--", "0", true));
                chkAddinVersions.Items.Clear();
                DataView viewAdding = dt.AsDataView();
                viewAdding.RowFilter = "isMain = false or Isnull(isMain,false) = false";
                if (viewAdding.Count > 0)
                {
                    chkAddinVersions.DataSource = viewAdding;
                    chkAddinVersions.DataTextField = "MasterwithVersion";
                    chkAddinVersions.DataValueField = "ExcelProductVersionMasterId";
                    chkAddinVersions.DataBind();
                }

            }
            else
            {
                rptExcelVersions.DataSource = null;
                lblNoRecordsVersion.Visible = true;
            }
            rptExcelVersions.DataBind();
        }

        private void GetExcelPackage()
        {
            DataTable dt = BLL.Domain.GetExcelProductMaster(null);
            if (dt != null && dt.Rows.Count > 0)
            {
                lblNoRecordsForBase.Visible = false;
                rptExcelProductMaster.DataSource = dt;
                DataView parentView = dt.AsDataView();
                parentView.RowFilter = "Isnull(ParentId, 0) = 0";
                if (parentView.Count > 0)
                {

                    ddlExcelPackage.DataSource = parentView;
                    ddlExcelPackage.DataTextField = "ExcelProductMasterName";
                    ddlExcelPackage.DataValueField = "ExcelProductMasterId";
                    ddlExcelPackage.DataBind();


                    ddlParentExcelPackage.DataSource = parentView;
                    ddlParentExcelPackage.DataTextField = "ExcelProductMasterName";
                    ddlParentExcelPackage.DataValueField = "ExcelProductMasterId";
                    ddlParentExcelPackage.DataBind();

                }
                else
                {
                    rptExcelProductMaster.DataSource = null;
                    lblNoRecordsForBase.Visible = true;
                }
                ddlExcelPackage.Items.Insert(0, new ListItem("--Select--", "0", true));
                ddlParentExcelPackage.Items.Insert(0, new ListItem("--Select--", "0", true));

            }
            else
            {
                rptExcelProductMaster.DataSource = null;
            }
            rptExcelProductMaster.DataBind();
        }

        protected void btnSubmitExcelPackage_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtExcelPackageName.Text) && !string.IsNullOrEmpty(txtAppGuid.Text))
            {
                bool? isMain = null;
                if (chkIsBasePlugin.Checked)
                    isMain = true;
                else
                    isMain = false;

                int? ParentId = null;
                if (trParentPackage.Visible == true)
                {
                    if (ddlParentExcelPackage.SelectedIndex > 0)
                        ParentId = Convert.ToInt32(ddlParentExcelPackage.SelectedValue);
                }


                BLL.Domain.Submit_ExcelPackage(excelProductMasterId, txtExcelPackageName.Text.Trim(), txtAppGuid.Text.Trim(), true, isMain, Convert.ToInt32(CurrentSession.User.UserID), ParentId);
                GetExcelPackage();
                txtExcelPackageName.Text = "";
                txtAppGuid.Text = "";

                chkIsBasePlugin.Checked = false;
                excelProductMasterId = 0;
                btnSubmitExcelPackage.Text = "Save";
                lblExcelPackageMessage.Text = "New Excel Addin has been added successfully.";
                trParentPackage.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "SelectTab", "SelectExcelTab();", true);
            }
        }

        protected void btnSubmitExcelVersion_Click(object sender, EventArgs e)
        {
            int excelMstVersionId = 0;
            if (trchildpackage.Visible)
                excelMstVersionId = Convert.ToInt32(ddlChildPackages.SelectedValue);
            else
                excelMstVersionId = Convert.ToInt32(ddlExcelPackage.SelectedValue);

            string version = txtExcelVersionNumber.Text.Trim() + txtExcelVersionNumber2.Text.Trim() + txtExcelVersionNumber3.Text.Trim() + txtExcelVersionNumber4.Text.Trim();

            string pathProductionFirst = @"\\" + ConfigurationManager.AppSettings["Aws_Web01_Ip"].ToString() + "\\ExcelVersion\\UpdateDownLoad\\" + ddlExcelPackage.SelectedItem.Text.Trim() + "\\" + version + "\\" + "Beast_" + ddlExcelPackage.SelectedItem.Text.Trim() + ".exe";
            string pathProductionSecond = @"\\" + ConfigurationManager.AppSettings["Aws_Web02_Ip"].ToString() + "\\ExcelVersion\\UpdateDownLoad\\" + ddlExcelPackage.SelectedItem.Text.Trim() + "\\" + version + "\\" + "Beast_" + ddlExcelPackage.SelectedItem.Text.Trim() + ".exe";

            string path = Server.MapPath("~/ExcelVersion/UpdateDownLoad/" + ddlExcelPackage.SelectedItem.Text.Trim() + "/" + version + "/" + "Beast_" + ddlExcelPackage.SelectedItem.Text.Trim() + ".exe");
            string SavedPath = ConfigurationManager.AppSettings["HostURL"].ToString() + "ExcelVersion/UpdateDownLoad" + "/" + ddlExcelPackage.SelectedItem.Text.Trim() + "/" + version + "/" + "Beast_" + ddlExcelPackage.SelectedItem.Text.Trim() + ".exe";
            if (ExcelProductVersionMasterId == 0)
            {
                if (flSetupMSI.HasFile)
                {
                    if (trchildpackage.Visible)
                    {
                        path = Server.MapPath("~/ExcelVersion/UpdateDownLoad/" + ddlExcelPackage.SelectedItem.Text.Trim() + "/" + ddlChildPackages.SelectedItem.Text + "/" + version + "/" + "Beast_" + ddlExcelPackage.SelectedItem.Text.Trim() + "_" + ddlChildPackages.SelectedItem.Text.Trim() + ".exe");
                        pathProductionFirst = @"\\" + ConfigurationManager.AppSettings["Aws_Web01_Ip"].ToString() + "\\ExcelVersion\\UpdateDownLoad\\" + ddlExcelPackage.SelectedItem.Text.Trim() + "\\" + ddlChildPackages.SelectedItem.Text + "\\" + version + "\\" + "Beast_" + ddlExcelPackage.SelectedItem.Text.Trim() + "_" + ddlChildPackages.SelectedItem.Text.Trim() + ".exe";
                        pathProductionSecond = @"\\" + ConfigurationManager.AppSettings["Aws_Web02_Ip"].ToString() + "\\ExcelVersion\\UpdateDownLoad\\" + ddlExcelPackage.SelectedItem.Text.Trim() + "\\" + ddlChildPackages.SelectedItem.Text + "\\" + version + "\\" + "Beast_" + ddlExcelPackage.SelectedItem.Text.Trim() + "_" + ddlChildPackages.SelectedItem.Text.Trim() + ".exe";
                        SavedPath = ConfigurationManager.AppSettings["HostURL"].ToString() + "ExcelVersion/UpdateDownLoad" + "/" + ddlExcelPackage.SelectedItem.Text.Trim() + "/" + ddlChildPackages.SelectedItem.Text.Trim() + "/" + version + "/" + "Beast_" + ddlExcelPackage.SelectedItem.Text.Trim() + "_" + ddlChildPackages.SelectedItem.Text.Trim() + ".exe";
                        DataTable dtversion = BLL.Domain.GetExcelProductionVersionMasterByExcelProducMastertId(Convert.ToInt32(ddlExcelPackage.SelectedValue));
                        if (dtversion.Rows.Count == 0)
                            BLL.Domain.SubmitExcelProductVersionMasterForParent(Convert.ToInt32(ddlExcelPackage.SelectedValue), txtExcelVersionNumber.Text.Trim() + txtExcelVersionNumber2.Text.Trim() + txtExcelVersionNumber3.Text.Trim() + txtExcelVersionNumber4.Text.Trim(), false, Convert.ToInt32(CurrentSession.User.UserID), txtFeatureDetails.Text.Trim(), txtResolvedIssueDetails.Text.Trim());
                        ddlChildPackages.SelectedIndex = 0;
                    }

                    byte[] bt = ReadFully(flSetupMSI.PostedFile.InputStream);
                    if (ConfigurationManager.AppSettings["SystemRunningOn"].ToLower() == "amazon")
                    {

                        using (NetworkShareAccesser.Access(ConfigurationManager.AppSettings["Aws_Web01_Ip"].ToString(), ConfigurationManager.AppSettings["NetworkUserName"].ToString(), ConfigurationManager.AppSettings["NetworkPassword"].ToString()))
                        {
                            SaveBytesToFile(pathProductionFirst, bt);
                        }

                        using (NetworkShareAccesser.Access(ConfigurationManager.AppSettings["Aws_Web02_Ip"].ToString(), ConfigurationManager.AppSettings["NetworkUserName"].ToString(), ConfigurationManager.AppSettings["NetworkPassword"].ToString()))
                        {
                            SaveBytesToFile(pathProductionSecond, bt);
                        }

                    }
                    else
                        SaveBytesToFile(path, bt);

                    int returnExcelMstVersionId = BLL.Domain.Submit_ExcelProductVersionMaster(0, excelMstVersionId, version, null, Convert.ToInt32(CurrentSession.User.UserID), txtFeatureDetails.Text.Trim(), txtResolvedIssueDetails.Text.Trim(), SavedPath);
                    lblExcelPackageMsg.Text = "New Excel Package Version has been added successfully..";

                }
            }
            else
            {
                if (trchildpackage.Visible)
                {
                    path = Server.MapPath("~/ExcelVersion/UpdateDownLoad/" + ddlExcelPackage.SelectedItem.Text.Trim() + "/" + ddlChildPackages.SelectedItem.Text + "/" + version + "/" + "Beast_" + ddlExcelPackage.SelectedItem.Text.Trim() + "_" + ddlChildPackages.SelectedItem.Text.Trim() + ".exe");
                    SavedPath = ConfigurationManager.AppSettings["HostURL"].ToString() + "ExcelVersion/UpdateDownLoad" + "/" + ddlExcelPackage.SelectedItem.Text.Trim() + "/" + ddlChildPackages.SelectedItem.Text.Trim() + "/" + version + "/" + "Beast_" + ddlExcelPackage.SelectedItem.Text.Trim() + "_" + ddlChildPackages.SelectedItem.Text.Trim() + ".exe";
                    pathProductionFirst = @"\\" + ConfigurationManager.AppSettings["Aws_Web01_Ip"].ToString() + "\\ExcelVersion\\UpdateDownLoad\\" + ddlExcelPackage.SelectedItem.Text.Trim() + "\\" + ddlChildPackages.SelectedItem.Text + "\\" + version + "\\" + "Beast_" + ddlExcelPackage.SelectedItem.Text.Trim() + "_" + ddlChildPackages.SelectedItem.Text.Trim() + ".exe";
                    pathProductionSecond = @"\\" + ConfigurationManager.AppSettings["Aws_Web02_Ip"].ToString() + "\\ExcelVersion\\UpdateDownLoad\\" + ddlExcelPackage.SelectedItem.Text.Trim() + "\\" + ddlChildPackages.SelectedItem.Text + "\\" + version + "\\" + "Beast_" + ddlExcelPackage.SelectedItem.Text.Trim() + "_" + ddlChildPackages.SelectedItem.Text.Trim() + ".exe";
                }
                if (flSetupMSI.HasFile)
                {
                    byte[] bt = ReadFully(flSetupMSI.PostedFile.InputStream);
                    if (ConfigurationManager.AppSettings["SystemRunningOn"].ToLower() == "amazon")
                    {
                        using (NetworkShareAccesser.Access(ConfigurationManager.AppSettings["Aws_Web01_Ip"].ToString(), ConfigurationManager.AppSettings["NetworkUserName"].ToString(), ConfigurationManager.AppSettings["NetworkPassword"].ToString()))
                        {
                            SaveBytesToFile(pathProductionFirst, bt);
                        }

                        using (NetworkShareAccesser.Access(ConfigurationManager.AppSettings["Aws_Web02_Ip"].ToString(), ConfigurationManager.AppSettings["NetworkUserName"].ToString(), ConfigurationManager.AppSettings["NetworkPassword"].ToString()))
                        {
                            SaveBytesToFile(pathProductionSecond, bt);
                        }

                    }
                    else
                        SaveBytesToFile(path, bt);

                }
                BLL.Domain.Submit_ExcelProductVersionMaster(ExcelProductVersionMasterId, excelMstVersionId, version, null, Convert.ToInt32(CurrentSession.User.UserID), txtFeatureDetails.Text.Trim(), txtResolvedIssueDetails.Text.Trim(), SavedPath);
                lblExcelPackageMsg.Text = "Excel Package Version has been updated successfully..";
            }
            lblmsgddl.Text = "*";
            lblmsgExcelVersion.Text = "*";
            lblflMessage.Text = "*";
            ddlExcelPackage.SelectedValue = "0";
            txtExcelVersionNumber.Text = "";
            txtExcelVersionNumber2.Text = "";
            txtExcelVersionNumber3.Text = "";
            txtExcelVersionNumber4.Text = "";
            ExcelProductVersionMasterId = 0;
            txtFeatureDetails.Text = "";
            txtResolvedIssueDetails.Text = "";
            btnSubmitExcelVersion.Text = "Save";
            trchildpackage.Visible = false;
            if (ddlChildPackages.Items.Count > 0)
                ddlChildPackages.SelectedIndex = 0;
            imgDownloadExcelSetup.Visible = false;
            lblSetupFileName.Visible = false;
            lblMsgV2.Text = "";
            lblMsgV3.Text = "";
            lblMsgV4.Text = "";
            GetExcelPackageVersion();
            GetExcelPackageVersionMapping();
        }



        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[input.Length];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public int excelProductMasterId
        {
            get { return (int)ViewState["excelProductMasterId"]; }
            set { ViewState["excelProductMasterId"] = value; }
        }

        public int ExcelProductVersionMasterId
        {
            get { return (int)ViewState["ExcelProductVersionMasterId"]; }
            set { ViewState["ExcelProductVersionMasterId"] = value; }
        }

        protected void imgEditExcelMaster_Click(object sender, EventArgs e)
        {
            excelProductMasterId = 0;
            ImageButton button = (sender as ImageButton);
            int? _excelProductMasterId = Convert.ToInt32(button.CommandArgument);
            DataTable dt = BLL.Domain.GetExcelProductMaster(_excelProductMasterId);
            if (dt.Rows.Count > 0)
            {
                txtExcelPackageName.Text = dt.Rows[0]["ExcelProductMasterName"].ToString();
                txtAppGuid.Text = dt.Rows[0]["AppGUID"].ToString();

                if (dt.Rows[0]["isMain"] == DBNull.Value || !Convert.ToBoolean(dt.Rows[0]["isMain"]))
                {
                    chkIsBasePlugin.Checked = false;
                    trParentPackage.Visible = true;
                    if (dt.Rows[0]["ParentId"] != DBNull.Value)
                        ddlParentExcelPackage.SelectedValue = Convert.ToString(dt.Rows[0]["ParentId"]);
                }
                else
                {
                    chkIsBasePlugin.Checked = true;
                    trParentPackage.Visible = false;
                }


                excelProductMasterId = Convert.ToInt32(dt.Rows[0]["ExcelProductMasterId"]);
                btnSubmitExcelPackage.Text = "Update";
                ScriptManager.RegisterStartupScript(this, GetType(), "SelectTab", "SelectExcelTab();", true);
            }
        }



        protected void btnEditExcelVersion_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdnEditExcelVersion.Value))
            {
                ExcelProductVersionMasterId = 0;
                int? excelProductVersionId = Convert.ToInt32(hdnEditExcelVersion.Value);
                ExcelProductVersionMasterId = Convert.ToInt32(hdnEditExcelVersion.Value);
                DataTable dt = BLL.Domain.GetExcelProductVersionMaster(excelProductVersionId);
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0]["VersionNumber"]).Length == 7)
                    {
                        txtExcelVersionNumber.Text = Convert.ToString(dt.Rows[0]["VersionNumber"]).Substring(0, 1);
                        txtExcelVersionNumber2.Text = Convert.ToString(dt.Rows[0]["VersionNumber"]).Substring(1, 2);
                        txtExcelVersionNumber3.Text = Convert.ToString(dt.Rows[0]["VersionNumber"]).Substring(3, 2);
                        txtExcelVersionNumber4.Text = Convert.ToString(dt.Rows[0]["VersionNumber"]).Substring(5, 2);
                    }
                    else
                    {
                        txtExcelVersionNumber.Text = Convert.ToString(dt.Rows[0]["VersionNumber"]).Substring(0, 2);
                        txtExcelVersionNumber2.Text = Convert.ToString(dt.Rows[0]["VersionNumber"]).Substring(2, 2);
                        txtExcelVersionNumber3.Text = Convert.ToString(dt.Rows[0]["VersionNumber"]).Substring(4, 2);
                        txtExcelVersionNumber4.Text = Convert.ToString(dt.Rows[0]["VersionNumber"]).Substring(6, 2);
                    }
                    if (dt.Rows[0]["ParentId"] != DBNull.Value)
                    {
                        ddlChildPackages.Items.Clear();
                        DataTable dtChild = BLL.Domain.GetExcelProductMaster(null);
                        if (dtChild.Rows.Count > 0)
                        {
                            DataView viewChilds = dtChild.AsDataView();
                            string row = "Isnull(ParentId, 0) =" + Convert.ToString(dt.Rows[0]["ParentId"]);
                            viewChilds.RowFilter = row;
                            if (viewChilds.Count > 0)
                            {
                                ddlChildPackages.DataSource = viewChilds;
                                ddlChildPackages.DataTextField = "ExcelProductMasterName";
                                ddlChildPackages.DataValueField = "ExcelProductMasterId";
                                ddlChildPackages.DataBind();
                                ddlChildPackages.Items.Insert(0, new ListItem("--Select--", "0", true));
                            }
                        }

                        trchildpackage.Visible = true;
                        ddlExcelPackage.SelectedValue = Convert.ToString(dt.Rows[0]["ParentId"]);
                        ddlChildPackages.SelectedValue = dt.Rows[0]["ExcelProductMasterId"].ToString();

                    }
                    else
                    {
                        ddlExcelPackage.SelectedValue = Convert.ToString(dt.Rows[0]["ExcelProductMasterId"]);
                        trchildpackage.Visible = false;
                    }
                    txtFeatureDetails.Text = Convert.ToString(dt.Rows[0]["FeatureDetails"]);
                    txtResolvedIssueDetails.Text = Convert.ToString(dt.Rows[0]["ResolvedIssueDetails"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["IsSetUpAvailable"])))
                    {
                        lblSetupFileName.Visible = true;
                        string setupname = Convert.ToString(dt.Rows[0]["ExcelProductMasterName"]);
                        if (Convert.ToBoolean(dt.Rows[0]["isMain"]) == false)
                        {
                            if (setupname.Contains('-'))
                            {
                                string[] setupsplit = setupname.Split('-');
                                setupname = "Beast_" + setupsplit[0].Trim() + "_" + setupsplit[1].Trim() + ".exe";
                            }
                            else
                                setupname = "Beast_" + setupname + ".exe";
                        }
                        else
                            setupname = Convert.ToString(dt.Rows[0]["ExcelProductMasterName"]) + ".exe";
                        lblSetupFileName.Text = setupname;
                        imgDownloadExcelSetup.Visible = true;
                        hdnExcelVersionDownload.Value = Convert.ToString(excelProductVersionId);
                    }
                    else
                    {
                        imgDownloadExcelSetup.Visible = false;
                        hdnExcelVersionDownload.Value = "";
                        lblSetupFileName.Text = "";
                        lblSetupFileName.Visible = false;
                    }
                    btnSubmitExcelVersion.Text = "Update";
                }
                else
                {
                    imgDownloadExcelSetup.Visible = false;
                    hdnExcelVersionDownload.Value = "";
                    lblSetupFileName.Text = "";
                    lblSetupFileName.Visible = false;
                }
                hdnEditExcelVersion.Value = "";
            }
        }

        public int ExcelProductVersionMappingId
        {
            get { return (int)ViewState["ExcelProductVersionMappingId"]; }
            set { ViewState["ExcelProductVersionMappingId"] = value; }
        }

        protected void imgEditExcelMasterMap_Click(object sender, EventArgs e)
        {
            ExcelProductVersionMappingId = 0;
            ImageButton button = (sender as ImageButton);
            ExcelProductVersionMappingId = Convert.ToInt32(button.CommandArgument);
            int? excelProductMapid = Convert.ToInt32(button.CommandArgument);
            DataTable dt = BLL.Domain.GetExcelProductVersionMappings(excelProductMapid);
            if (dt.Rows.Count > 0)
            {
                ddlMainExcelPlugin.SelectedValue = Convert.ToString(dt.Rows[0]["ExcelProductVersionMasterId"]);

            }

        }

        protected string GetVersion(string str)
        {
            string updated = string.Empty;
            if (str.Length == 7)
                updated = str.Insert(5, ".").Insert(3, ".").Insert(1, ".");
            else if (str.Length == 8)
                updated = str.Insert(6, ".").Insert(4, ".").Insert(2, ".");
            return updated;
        }

        protected DataTable GenerateMappings(DataTable dt)
        {
            DataTable dtMap = new DataTable();
            dtMap.Columns.Add("ExcelProductVersionMapId", typeof(int));
            dtMap.Columns.Add("MainExcelProductMasterName", typeof(string));
            dtMap.Columns.Add("MainVersionNumber", typeof(string));
            dtMap.Columns.Add("CompatibleExcelProductMasterName", typeof(string));
            dtMap.Columns.Add("CompatibleVersionNumber", typeof(string));
            dtMap.Columns.Add("MainExcelProductMasterId", typeof(int));
            dtMap.Columns.Add("CompatibleExcelProductMasterId", typeof(int));
            dtMap.Columns.Add("UniID", typeof(string));


            if (dt.Rows.Count > 0)
            {
                string masterid = string.Empty;
                string deleteExcelMapIds = string.Empty;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dtMap.NewRow();

                    dr["ExcelProductVersionMapId"] = Convert.ToInt32(dt.Rows[i]["ExcelProductVersionMapId"]);
                    if (masterid == Convert.ToString(dt.Rows[i]["UniID"]))
                    {
                        dr["MainExcelProductMasterName"] = "";
                        dr["MainVersionNumber"] = "0";

                        deleteExcelMapIds += Convert.ToString(dt.Rows[i]["ExcelProductVersionMapId"]) + ",";
                    }
                    else
                    {

                        dr["MainExcelProductMasterName"] = Convert.ToString(dt.Rows[i]["MainExcelProductMasterName"]);
                        dr["MainVersionNumber"] = GetVersion(Convert.ToString(dt.Rows[i]["MainVersionNumber"]));
                        if (i == 0)
                            deleteExcelMapIds += Convert.ToString(dt.Rows[i]["ExcelProductVersionMapId"]) + ",";
                        else
                            deleteExcelMapIds = string.Empty;
                    }


                    DataTable dtChild = BLL.Domain.GetExcelProductVersionMaster(Convert.ToInt32(dt.Rows[i]["CompatibleExcelProductMasterId"]));
                    if (dtChild.Rows.Count > 0)
                    {
                        dr["CompatibleExcelProductMasterName"] = Convert.ToString(dtChild.Rows[0]["ExcelProductMasterName"]);

                    }
                    else
                    {
                        dr["CompatibleExcelProductMasterName"] = Convert.ToString(dt.Rows[i]["CompatibleExcelProductMasterName"]);
                    }


                    dr["CompatibleVersionNumber"] = GetVersion(Convert.ToString(dt.Rows[i]["CompatibleVersionNumber"]));
                    dr["MainExcelProductMasterId"] = Convert.ToInt32(dt.Rows[i]["MainExcelProductMasterId"]);
                    dr["CompatibleExcelProductMasterId"] = Convert.ToInt32(dt.Rows[i]["CompatibleExcelProductMasterId"]);
                    dr["UniID"] = Convert.ToString(dt.Rows[i]["UniID"]);
                    masterid = Convert.ToString(dt.Rows[i]["UniID"]);
                    dtMap.Rows.Add(dr);
                }
            }
            return dtMap;
        }

        protected string writeTdwithColspan(string PageDataId)
        {
            return "<td rowspan='2'> " + PageDataId + "</td>";

        }

        protected string writeTdimgwithColspan(string ExcelProductVersionMapGuId)
        {
            return "<img src='images/getDetails.png' alt='' onclick=\"getMapDetails('" + ExcelProductVersionMapGuId + "');\"/>";
        }
        protected string writeTd()
        {
            return "<td></td>";
        }

        protected string writeTdimgDelete(string ExcelMasterId)
        {
            return "<img src='images/delete-icon.png' alt='' onclick='deleteMapDetails(\"" + ExcelMasterId.Trim() + "\" );'  />";
        }

        public string EditMapGuid
        {
            get { return Convert.ToString(ViewState["EditMapGuid"]); }
            set { ViewState["EditMapGuid"] = value; }
        }

        protected void btnEditMapping_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdnExcelProductVersionMapId.Value))
            {
                btnSaveExcelMappings.Text = "Update";
                DataTable dt = BLL.Domain.GetExcelProductVersionMappingByGuid(hdnExcelProductVersionMapId.Value);
                if (dt.Rows.Count > 0)
                {
                    EditMapGuid = Convert.ToString(dt.Rows[0]["UniID"]);
                    ddlMainExcelPlugin.SelectedValue = Convert.ToString(dt.Rows[0]["ExcelProductVersionMasterId"]);
                    chkAddinVersions.ClearSelection();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        foreach (ListItem item in chkAddinVersions.Items)
                        {
                            if (item.Value == dt.Rows[i]["ExcelProductCompatibleVersionMasterId"].ToString())
                            {
                                item.Selected = true;
                                break;
                            }
                        }

                    }
                }
            }
        }

        protected string writeAutoURLwithImage(string id)
        {
            return "<img src='images/download.png' alt='' onclick='downLoadSetup(" + id + ");'  />";
        }
        protected string writeAutoURLwithoutImage(string id)
        {
            return "&nbsp;";
        }

        protected string writeAutoURLEditwithImage(string id)
        {
            return "<td><img src='images/download.png' alt='' onclick='gerVersionDetails(e, " + id + ");'  /></td>";
        }

        protected void btnDownLoadExcelVersion_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdnExcelVersionDownload.Value))
            {
                try
                {

                    int? excelversionMasterId = Convert.ToInt32(hdnExcelVersionDownload.Value);
                    DataTable dt = BLL.Domain.GetExcelProductVersionMaster(excelversionMasterId);
                    if (dt.Rows.Count > 0)
                    {
                        string Fullpath = Convert.ToString(dt.Rows[0]["SetupPath"]);
                        if (!string.IsNullOrEmpty(Fullpath))
                        {
                            Stream stream = null;
                            try
                            {
                                string setupname = Convert.ToString(dt.Rows[0]["ExcelProductMasterName"]);
                                if (Convert.ToBoolean(dt.Rows[0]["isMain"]) == false)
                                {
                                    if (setupname.Contains('-'))
                                    {
                                        string[] setupsplit = setupname.Split('-');
                                        setupname = "Beast_" + setupsplit[0].Trim() + "_" + setupsplit[1].Trim() + ".exe";
                                    }
                                    else
                                        setupname = "Beast_" + setupname + ".exe";
                                }
                                else
                                    setupname = Convert.ToString(dt.Rows[0]["ExcelProductMasterName"]) + ".exe";


                                int bytesToRead = 10000;
                                byte[] buffer = new Byte[bytesToRead];
                                HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(Fullpath);

                                HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();
                                if (fileReq.ContentLength > 0)
                                    fileResp.ContentLength = fileReq.ContentLength;
                                stream = fileResp.GetResponseStream();
                                var resp = HttpContext.Current.Response;
                                resp.ContentType = "application/x-msdownload";
                                resp.AddHeader("Content-Disposition", "attachment; filename=\"" + setupname + "\"");
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
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        protected void btnSaveExcelMappings_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(EditMapGuid))
            {
                lblmsgMapping.Text = "";
                DataTable dtmap = BLL.Domain.GetExcelProductVersionMappingByGuid(EditMapGuid);
                if (dtmap.Rows.Count > 0)
                {

                    for (int i = 0; i < dtmap.Rows.Count; i++)
                    {
                        BLL.Domain.Submit_ExcelProductVersionMappings(Convert.ToInt32(dtmap.Rows[i]["ExcelProductVersionMapId"]), Convert.ToInt32(dtmap.Rows[i]["ExcelProductVersionMasterId"]), Convert.ToInt32(dtmap.Rows[i]["ExcelProductCompatibleVersionMasterId"]), Convert.ToInt32(CurrentSession.User.UserID), 1, EditMapGuid);
                    }
                }
                Guid guid = Guid.NewGuid();
                for (int i = 0; i < chkAddinVersions.Items.Count; i++)
                {
                    if (chkAddinVersions.Items[i].Selected == true)
                    {

                        BLL.Domain.Submit_ExcelProductVersionMappings(0, Convert.ToInt32(ddlMainExcelPlugin.SelectedValue), Convert.ToInt32(chkAddinVersions.Items[i].Value), Convert.ToInt32(CurrentSession.User.UserID), 0, guid.ToString());
                    }
                }
            }
            else
            {
                Guid guid = Guid.NewGuid();
                for (int i = 0; i < chkAddinVersions.Items.Count; i++)
                {
                    if (chkAddinVersions.Items[i].Selected == true)
                    {
                        BLL.Domain.Submit_ExcelProductVersionMappings(0, Convert.ToInt32(ddlMainExcelPlugin.SelectedValue), Convert.ToInt32(chkAddinVersions.Items[i].Value), Convert.ToInt32(CurrentSession.User.UserID), 0, guid.ToString());
                    }
                }
            }

            ddlMainExcelPlugin.SelectedIndex = 0;
            btnSaveExcelMappings.Text = "Save";
            chkAddinVersions.ClearSelection();
            lblmsgMapping.Text = "Mapping has been saved successfully..";
            EditMapGuid = "";
            GetExcelPackageVersionMapping();

        }

        public static void SaveBytesToFile(string filename, byte[] bytesToWrite)
        {
            if (filename != null && filename.Length > 0 && bytesToWrite != null)
            {
                if (!Directory.Exists(Path.GetDirectoryName(filename)))
                    Directory.CreateDirectory(Path.GetDirectoryName(filename));

                if (File.Exists(filename))
                    File.Delete(filename);

                FileStream file = File.Create(filename);
                file.Write(bytesToWrite, 0, bytesToWrite.Length);
                file.Close();
            }
        }

        protected void imgDownloadExcelSetup_Click(object sender, ImageClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(hdnExcelVersionDownload.Value))
            {
                try
                {
                    int? excelversionMasterId = Convert.ToInt32(hdnExcelVersionDownload.Value);
                    DataTable dt = BLL.Domain.GetExcelProductVersionMaster(excelversionMasterId);
                    if (dt.Rows.Count > 0)
                    {
                        string Fullpath = Convert.ToString(dt.Rows[0]["SetupPath"]);
                        if (!string.IsNullOrEmpty(Fullpath))
                        {
                            Stream stream = null;
                            try
                            {
                                string setupname = Convert.ToString(dt.Rows[0]["ExcelProductMasterName"]);
                                if (Convert.ToBoolean(dt.Rows[0]["isMain"]) == false)
                                {
                                    if (setupname.Contains('-'))
                                    {
                                        string[] setupsplit = setupname.Split('-');
                                        setupname = "Beast_" + setupsplit[0].Trim() + "_" + setupsplit[1].Trim() + ".exe";
                                    }
                                    else
                                        setupname = "Beast_" + setupname + ".exe";
                                }
                                else
                                    setupname = Convert.ToString(dt.Rows[0]["ExcelProductMasterName"]) + ".exe";

                                int bytesToRead = 10000;
                                byte[] buffer = new Byte[bytesToRead];
                                HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(Fullpath);

                                HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();
                                if (fileReq.ContentLength > 0)
                                    fileResp.ContentLength = fileReq.ContentLength;
                                stream = fileResp.GetResponseStream();
                                var resp = HttpContext.Current.Response;
                                resp.ContentType = "application/x-msdownload";
                                resp.AddHeader("Content-Disposition", "attachment; filename=\"" + setupname + "\"");
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
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        protected void ddlExcelPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlExcelPackage.SelectedIndex > 0)
            {
                DataTable dt = BLL.Domain.GetExcelProductMaster(null);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataView viewChilds = dt.AsDataView();
                    string row = "Isnull(ParentId, 0) =" + ddlExcelPackage.SelectedValue;
                    viewChilds.RowFilter = row;

                    //DataView viewMain = dt.AsDataView();
                    //string rowfilter = "isMain = true and ExcelProducMastertId=" + ddlExcelPackage.SelectedValue;
                    //viewMain.RowFilter = rowfilter;
                    //if (viewMain.Count > 0)
                    //    trExe.Visible = true;
                    //else
                    //    trExe.Visible = false;

                    if (viewChilds.Count > 0)
                    {
                        ddlChildPackages.DataSource = viewChilds;
                        ddlChildPackages.DataTextField = "ExcelProductMasterName";
                        ddlChildPackages.DataValueField = "ExcelProductMasterId";
                        ddlChildPackages.DataBind();
                        ddlChildPackages.Items.Insert(0, new ListItem("--Select--", "0", true));
                        trchildpackage.Visible = true;

                    }
                    else
                    {
                        trchildpackage.Visible = false;
                    }
                }
                else
                {
                    trchildpackage.Visible = false;



                }
            }
            else
            {
                trchildpackage.Visible = false;

            }
        }

        protected void chkIsBasePlugin_Clicked(Object sender, EventArgs e)
        {
            if (chkIsBasePlugin.Checked)
                trParentPackage.Visible = false;
            else
                trParentPackage.Visible = true;

            ScriptManager.RegisterStartupScript(this, GetType(), "SelectTab", "SelectExcelTab();", true);
        }

        protected void btnExcelVersionClear_Click(object sender, EventArgs e)
        {
            ddlExcelPackage.SelectedIndex = 0;
            lblmsgddl.Text = "*";
            if (ddlChildPackages.Items.Count > 0)
                ddlChildPackages.SelectedIndex = 0;
            lblmsgddlchild.Text = "*";
            txtExcelVersionNumber.Text = "";
            txtExcelVersionNumber2.Text = "";
            txtExcelVersionNumber3.Text = "";
            txtExcelVersionNumber4.Text = "";
            lblSetupFileName.Visible = false;
            imgDownloadExcelSetup.Visible = false;
            lblmsgExcelVersion.Text = "*";
            flSetupMSI.Attributes.Clear();
            lblflMessage.Text = "*";
            txtFeatureDetails.Text = "";
            txtResolvedIssueDetails.Text = "";
            lblMsgV2.Text = "";
            lblMsgV3.Text = "";
            lblMsgV4.Text = "";

            //trExe.Visible = false;
            //imgDownloadExcelEXE.Visible = false;
            //lblEXEName.Text = "";
            //lblEXEName.Visible = false;
            //lblfl2Message.Text = "*";

            btnSubmitExcelVersion.Text = "Save";

        }

        protected void btnExcelMappingClear_Click(object sender, EventArgs e)
        {
            ddlMainExcelPlugin.SelectedIndex = 0;
            chkAddinVersions.ClearSelection();
            btnSaveExcelMappings.Text = "Save";

        }

        protected void btnExcelPackageClear_Click(object sender, EventArgs e)
        {
            txtExcelPackageName.Text = "";
            txtAppGuid.Text = "";
            chkIsBasePlugin.Checked = false;
            ddlParentExcelPackage.SelectedIndex = 0;
            ScriptManager.RegisterStartupScript(this, GetType(), "SelectTab", "SelectExcelTab();", true);
        }

        protected void btnDeleteVersion_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdnDeleteVersionId.Value))
            {
                BLL.Domain.SubmitExcelProductionVersionMasterIsDelete(Convert.ToInt32(hdnDeleteVersionId.Value), Convert.ToInt32(CurrentSession.User.UserID), true);
                hdnDeleteVersionId.Value = "";
                GetExcelPackageVersion();
                GetExcelPackageVersionMapping();
            }

        }


        protected void imgDeleteExcelVersion_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton button = (sender as ImageButton);
            int ExcelProductVersionMasterId = Convert.ToInt32(button.CommandArgument);
            BLL.Domain.SubmitExcelProductionVersionMasterIsDelete(ExcelProductVersionMasterId, Convert.ToInt32(CurrentSession.User.UserID), true);
            GetExcelPackageVersion();

        }

        protected void btnDeleteMapping_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdnDeleteExcelMap.Value))
            {
                DataTable dt = BLL.Domain.GetExcelProductVersionMappingByGuid(hdnDeleteExcelMap.Value);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        BLL.Domain.SubmitExcelProductVersionMappingsIsDelete(Convert.ToInt32(dt.Rows[i]["ExcelProductVersionMapId"]), Convert.ToInt32(CurrentSession.User.UserID), true);
                    }
                }
                GetExcelPackageVersionMapping();
            }
            hdnDeleteExcelMap.Value = "";
        }

        protected void imgDeleteExcelMaster_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton button = (sender as ImageButton);
            int ExcelProductMasterId = Convert.ToInt32(button.CommandArgument);
            BLL.Domain.SubmitExcelProductMasterIsDelete(ExcelProductMasterId, Convert.ToInt32(CurrentSession.User.UserID), true);
            GetExcelPackage();

        }

        private void GetExcelPackageVersionForPackage()
        {
            DataTable dt = BLL.Domain.GetExcelProductMaster(null);
            if (dt.Rows.Count > 0)
            {

                DataView view = dt.AsDataView();
                view.RowFilter = "Isnull(isMain,false) = true and Isnull(ParentId,0) = 0";
                if (view.Count > 0)
                {
                    ddlExcelBasePlugins.DataTextField = "ExcelProductMasterName";
                    ddlExcelBasePlugins.DataValueField = "ExcelProductMasterId";
                    ddlExcelBasePlugins.DataSource = view;
                    ddlExcelBasePlugins.DataBind();
                }

            }
            ddlExcelBasePlugins.Items.Insert(0, new ListItem("--Select--", "0", true));

        }


        protected void ddlExcelBasePlugins_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlExcelBasePlugins.SelectedIndex > 0)
            {
                ddlExcelVersions.Items.Clear();
                DataTable dt = BLL.Domain.GetExcelProductionVersionMasterByExcelProducMastertId(Convert.ToInt32(ddlExcelBasePlugins.SelectedValue));
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["VersionNumber"] = GetVersion(Convert.ToString(dr["VersionNumber"]));
                    }
                    ddlExcelVersions.DataSource = dt;
                    ddlExcelVersions.DataTextField = "VersionNumber";
                    ddlExcelVersions.DataValueField = "ExcelProductVersionMasterId";
                    ddlExcelVersions.DataBind();
                }
                ddlExcelVersions.Items.Insert(0, new ListItem("--Select--", "0", true));
            }
            else
            {
                ddlExcelVersions.Items.Clear();
                ddlExcelVersions.Items.Insert(0, new ListItem("--Select--", "0", true));
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "SelectTabPack", "SelectExcelPackageTab();", true);
        }

        protected void ddlExcelVersions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlExcelVersions.SelectedIndex > 0)
            {
                trAddins.Visible = true;
                LoadAddIns();
                trflPackage.Visible = true;
            }
            else
            {
                trAddins.Visible = false;
                trflPackage.Visible = false;
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "SelectTabPack", "SelectExcelPackageTab();", true);
        }

        protected void btnGeneratePack_Click(object sender, EventArgs e)
        {

            /// sample hdnBuild.Value = "1#gfhgh#--Select-->AddIns=5#08050000>AddIns=6#02080600#ChildAddIns=2#4#2>AddIns=7#011200#ChildAddIns=3#7#3";
            try
            {
                if (!string.IsNullOrEmpty(hdnBuild.Value))
                {
                    string VersionInfo = string.Empty;
                    Guid guid = Guid.NewGuid();

                    ZipFile zipFile = new ZipFile();
                    string[] split = hdnBuild.Value.Split('>');
                    string[] basepl = null;
                    string rootname = string.Empty;
                    for (int i = 0; i < split.Length; i++)
                    {
                        if (i == 0)
                        {
                            basepl = split[i].Split('#');
                            VersionInfo += basepl[1].ToString() + "-" + basepl[2].ToString() + " ";
                            rootname = "TheBeastAppsExcel_V" + basepl[2].ToString();
                            string zippath = Server.MapPath("~/ExcelVersion/ExcelVersionDownload/" + guid.ToString() + "/setup.exe");


                            if (flPackage.HasFile)
                            {
                                byte[] bt = ReadFully(flPackage.PostedFile.InputStream);
                                SaveBytesToFile(zippath, bt);
                            }
                            LogUtility.Info("ExcelPackage.aspx.cs", "btnGeneratePack_Click()", "Saved Bytes at specific location");
                            string[] filezip = Directory.GetFiles(Server.MapPath("~/ExcelVersion/ExcelVersionDownload/" + guid.ToString()));
                            zipFile.AddFiles(filezip, rootname);
                            LogUtility.Info("ExcelPackage.aspx.cs", "btnGeneratePack_Click()", "Zip File Added");

                        }

                        if (i > 0)
                        {
                            string Addins = split[i].Substring(split[i].IndexOf('=') + 1);
                            string[] AddinSplit = Addins.Split('#');
                            if (AddinSplit[1] != "0")
                            {
                                if (Addins.Contains("ChildAddIns"))
                                {
                                    string[] ChildAddIns = Addins.Split('=');
                                    string[] ch2 = ChildAddIns[1].Split('#');
                                    VersionInfo += "[" + AddinSplit[0].ToString() + "-" + ch2[3].ToString() + "-" + GetVersion(AddinSplit[1].ToString()) + "]";
                                }
                                else
                                {
                                    VersionInfo += "[" + AddinSplit[0].ToString() + "-" + GetVersion(AddinSplit[1].ToString()) + "]";
                                }
                            }
                        }

                    }
                    if (basepl.Length > 0)
                    {
                        string zipSavepath = Server.MapPath("~/ExcelVersion/ExcelVersionZip/" + guid.ToString());
                        LogUtility.Info("ExcelPackage.aspx.cs", "btnGeneratePack_Click()", "start creating directory");
                        if (!Directory.Exists(zipSavepath))
                            Directory.CreateDirectory(zipSavepath);
                        LogUtility.Info("ExcelPackage.aspx.cs", "btnGeneratePack_Click()", "start saving zip file");
                        zipFile.Save(zipSavepath + "/" + rootname + ".zip");
                        LogUtility.Info("ExcelPackage.aspx.cs", "btnGeneratePack_Click()", "start saving full path");

                        if (ConfigurationManager.AppSettings["SystemRunningOn"].ToLower() == "amazon")
                        {
                            bool dirExist = false;
                            string pathProductionFirst = @"\\" + ConfigurationManager.AppSettings["Aws_Web01_Ip"].ToString() + "\\ExcelVersion\\ExcelVersionZip\\" + guid.ToString();
                            string pathProductionSecond = @"\\" + ConfigurationManager.AppSettings["Aws_Web02_Ip"].ToString() + "\\ExcelVersion\\ExcelVersionZip\\" + guid.ToString();

                            using (NetworkShareAccesser.Access(ConfigurationManager.AppSettings["Aws_Web01_Ip"].ToString(), ConfigurationManager.AppSettings["NetworkUserName"].ToString(), ConfigurationManager.AppSettings["NetworkPassword"].ToString()))
                            {
                                if (Directory.Exists(pathProductionFirst))
                                {
                                    using (NetworkShareAccesser.Access(ConfigurationManager.AppSettings["Aws_Web02_Ip"].ToString(), ConfigurationManager.AppSettings["NetworkUserName"].ToString(), ConfigurationManager.AppSettings["NetworkPassword"].ToString()))
                                    {
                                        if (!Directory.Exists(pathProductionSecond))
                                            Directory.CreateDirectory(pathProductionSecond);
                                        File.Copy(pathProductionFirst + "\\" + rootname + ".zip", pathProductionSecond + "\\" + rootname + ".zip");
                                    }
                                    dirExist = true;
                                }

                            }
                            if (!dirExist)
                            {
                                using (NetworkShareAccesser.Access(ConfigurationManager.AppSettings["Aws_Web02_Ip"].ToString(), ConfigurationManager.AppSettings["NetworkUserName"].ToString(), ConfigurationManager.AppSettings["NetworkPassword"].ToString()))
                                {
                                    using (NetworkShareAccesser.Access(ConfigurationManager.AppSettings["Aws_Web01_Ip"].ToString(), ConfigurationManager.AppSettings["NetworkUserName"].ToString(), ConfigurationManager.AppSettings["NetworkPassword"].ToString()))
                                    {
                                        if (!Directory.Exists(pathProductionFirst))
                                            Directory.CreateDirectory(pathProductionFirst);
                                        File.Copy(pathProductionSecond + "\\" + rootname + ".zip", pathProductionFirst + "\\" + rootname + ".zip");
                                    }
                                }
                            }

                        }

                        string FullPath = ConfigurationManager.AppSettings["ExcelDownLoadURL"].ToString() + guid.ToString() + "/" + rootname + ".zip";
                        BLL.Domain.SubmitExcelPackageVersionInfo(0, FullPath, VersionInfo, Convert.ToInt32(CurrentSession.User.UserID), false);
                        lblpackagemsg.Text = "Package has been genearated successfully.";
                        LogUtility.Info("ExcelPackage.aspx.cs", "btnGeneratePack_Click()", "Saved full path to db");
                    }

                    System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo(Server.MapPath("~/ExcelVersion/ExcelVersionDownload/" + guid.ToString()));
                    LogUtility.Info("ExcelPackage.aspx.cs", "btnGeneratePack_Click()", "Start Delete Files");
                    foreach (FileInfo file in downloadedMessageInfo.GetFiles())
                    {

                        file.Delete();
                    }
                    LogUtility.Info("ExcelPackage.aspx.cs", "btnGeneratePack_Click()", "Start Delete Inner Directory");
                    foreach (DirectoryInfo dir in downloadedMessageInfo.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                    LogUtility.Info("ExcelPackage.aspx.cs", "btnGeneratePack_Click()", "Start Delete Main Directory");
                    if (Directory.Exists(Server.MapPath("~/ExcelVersion/ExcelVersionDownload/" + guid.ToString())))
                    {
                        Directory.Delete(Server.MapPath("~/ExcelVersion/ExcelVersionDownload/" + guid.ToString()));
                    }


                }
                hdnBuild.Value = "";

                ScriptManager.RegisterStartupScript(this, GetType(), "SelectTabPack", "SelectExcelPackageTab();", true);
            }
            catch (Exception ex)
            {
                LogUtility.Error("ExcelPackage.aspx.cs", "btnGeneratePack_Click", ex.Message, ex);
            }
        }

        private void LoadAddIns()
        {

            DataTable dt = BLL.Domain.GetExcelProductMaster(null);
            DataView viewAddins = dt.AsDataView();
            viewAddins.RowFilter = "(isMain = false or Isnull(isMain,false) = false) and Isnull(ParentId,0) = 0";
            trAddins.InnerHtml = "";
            trAddins.InnerHtml += "<td style='text-align: right; width: 18%;'>Select Addins :  </td>";
            trAddins.InnerHtml += "<td>";
            trAddins.InnerHtml += "<table id='tblAddins' border='0' >";
            DataTable dtComp = BLL.Domain.GetExcelMappingByExcelProductMasterId(Convert.ToInt32(ddlExcelVersions.SelectedValue));
            foreach (DataRowView drview in viewAddins)
            {

                DataView dtCompView = dtComp.AsDataView();
                DataRow dr = drview.Row;
                DataTable datatAddIns = BLL.Domain.GetExcelProductionVersionMasterProduct(null);
                DataView dvAddIns = datatAddIns.AsDataView();
                dvAddIns.RowFilter = "ParentId = " + dr["ExcelProductMasterId"].ToString();
                DataView dvParent = datatAddIns.AsDataView();
                dvParent.RowFilter = "(isMain = false or Isnull(isMain,false) = false) and Isnull(ParentId,0) = 0 and ExcelProductMasterId = " + Convert.ToInt32(dr["ExcelProductMasterId"]);



                trAddins.InnerHtml += "<tr>";
                trAddins.InnerHtml += "<td style='width:30px;'>" + dr["ExcelProductMasterName"].ToString() + "</td>";
                trAddins.InnerHtml += "<td style='width:150px;'>";
                trAddins.InnerHtml += "<select id='" + dr["ExcelProductMasterId"].ToString() + "' name='" + dr["ExcelProductMasterName"].ToString() + "'>";
                trAddins.InnerHtml += "<option value=\"0\" selected=\"selected\">-- Select --</option>";
                int VersionNumber = 0;
                if (dvAddIns.Count > 0)
                {
                    foreach (DataRowView drAddin in dvAddIns)
                    {
                        DataRow dv = drAddin.Row;
                        dtCompView.RowFilter = "VersionNumber = " + Convert.ToInt32(dv["VersionNumber"]);
                        if (Convert.ToInt32(dv["VersionNumber"]) != VersionNumber && dtCompView.Count > 0)
                        {
                            trAddins.InnerHtml += "<option value=\"" + dv["VersionNumber"].ToString() + "\">" + GetVersion(dv["VersionNumber"].ToString()) + "</option>";
                        }
                        VersionNumber = Convert.ToInt32(dv["VersionNumber"]);
                    }
                }
                else
                {
                    if (dvParent.Count > 0)
                    {
                        foreach (DataRowView drParent in dvParent)
                        {
                            DataRow dvP = drParent.Row;
                            dtCompView.RowFilter = "VersionNumber = " + Convert.ToInt32(dvP["VersionNumber"]);
                            if (Convert.ToInt32(dvP["VersionNumber"]) != VersionNumber && dtCompView.Count > 0)
                            {
                                trAddins.InnerHtml += "<option value=\"" + dvP["VersionNumber"].ToString() + "\">" + GetVersion(dvP["VersionNumber"].ToString()) + "</option>";
                            }
                            VersionNumber = Convert.ToInt32(dvP["VersionNumber"]);

                        }
                    }

                }


                trAddins.InnerHtml += "</select>";

                DataView childPluginsView = dt.AsDataView();
                childPluginsView.RowFilter = "Isnull(ParentId,0) = " + dr["ExcelProductMasterId"].ToString();
                if (childPluginsView.Count > 0)
                {
                    trAddins.InnerHtml += "<td>";
                    foreach (DataRowView drchild in childPluginsView)
                    {
                        DataRow drchildrow = drchild.Row;
                        trAddins.InnerHtml += "<input type=\"radio\" name='radio" + drchildrow["ParentId"].ToString() + "' value='" + drchildrow["ParentId"].ToString() + "#" + drchildrow["ExcelProductMasterId"].ToString() + "#" + dr["ExcelProductMasterId"].ToString() + "#" + drchildrow["ExcelProductMasterName"].ToString() + "' >" + "  " + drchildrow["ExcelProductMasterName"].ToString() + "   ";


                    }
                    trAddins.InnerHtml += "</td>";
                }
                else
                {
                    trAddins.InnerHtml += "<td></td>";
                }
                trAddins.InnerHtml += "</td>";
                trAddins.InnerHtml += "</tr>";

            }

            trAddins.InnerHtml += "</table>";
            trAddins.InnerHtml += "</td><td>&nbsp;</td>";
        }

    }


}