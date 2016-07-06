using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Ionic.Zip;
using System.IO;
using System.Configuration;
using TBA.Utilities;
using System.Text;

namespace Administration
{
    public partial class ExcelAutoUpdateConfig : System.Web.UI.Page
    {
        private SessionInfo session;

        public SessionInfo CurrentSession
        {
            get
            {
                if (session == null)
                {
                    session = new SessionInfo(HttpContext.Current.Session);
                }
                return session;
            }
            set
            {
                session = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentSession.User == null)
            {
                Session.Clear();
                Session.Abandon();
                Response.Redirect("SessionTimeOut.htm", false);
                return;
            }
            if (!IsPostBack)
            {
                FillVersionGrid();
                FillObjectListDropdown();
                FillProductListDropdown();
                ddlExcelGroup_SelectedIndexChanged();
                ddlExcelVersions.Items.Insert(0, new ListItem("--Select--", "0", true));
                //ddExcelPriority.Items.Insert(0, new ListItem("--Select--", "0", true));
                ddExcelGroupName.Items.Insert(0, new ListItem("--Select--", "0", true));
                lblMessage.Text = "";
                FillVersionInfoGrid();

                FillExcelProductInfoGrid();
                FillExcelClickOnceInfoGrid();

                //FillExcelClickOnceInfoGrid();
                lblMsg.Text = "";
            }
        }

        private void FillObjectListDropdown()
        {

            DataTable dt = BLL.Domain.GetObjects();
            DataView dtView = dt.AsDataView();
            dtView.RowFilter = @"ObjectID='0bc4176d-5903-4674-8b32-de7b0808acd4' 
                                or ObjectID='ab404358-1c52-439d-865d-1b42b5c1115d' 
                                or ObjectID='04D6C014-ECB9-48DD-83E3-9193DEDA6D50' 
                                or ObjectID='8850735f-77a6-4861-abb1-d0c07f0f9caf'  
                                or ObjectID='BB18EC25-7451-4424-8112-F427080C84D7' 
                                or ObjectID='8483b7b4-f8a3-4a23-8ef4-828fe3f0fd8f'";
            if (dtView.Count > 0)
            {
                ddlExcelObjects.DataSource = dtView;
                ddlExcelObjects.DataTextField = "ObjectName";
                ddlExcelObjects.DataValueField = "ObjectID";
                ddlExcelObjects.DataBind();

            }
            ddlExcelObjects.Items.Insert(0, new ListItem("--Select--", "0", true));

        }

        private void FillProductListDropdown()
        {

            DataTable dt = BLL.Domain.GetProductNameAndId();
          
            ddlProduct.DataSource = dt;
            ddlProduct.DataTextField = "ProductName";
            ddlProduct.DataValueField = "ProductID";
            ddlProduct.DataBind();
            
            ddlProduct.Items.Insert(0, new ListItem("--Select--", "0", true));


        }

        public string ObjectID
        {
            get { return (string)ViewState["ObjectID"]; }
            set { ViewState["ObjectID"] = value; }

        }

        protected void ddlExcelGroup_SelectedIndexChanged()
        {
            DataTable dt = BLL.Domain.GetExcelGroupName();

            ddExcelGroupName.DataSource = dt;
            ddExcelGroupName.DataTextField = "NAME";
            ddExcelGroupName.DataValueField = "GROUPID";
            ddExcelGroupName.DataBind();
        }
       
        protected void ddlExcelObjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlExcelObjects.SelectedIndex > 0)
            {
                DataTable dt = BLL.Domain.GetObjectStoreVersions(ddlExcelObjects.SelectedValue.ToString(), 200);
                ddlExcelVersions.DataSource = dt;
                ddlExcelVersions.DataTextField = "Version";
                ddlExcelVersions.DataValueField = "Version";
                ddlExcelVersions.DataBind();
                ddlExcelVersions.Items.Insert(0, new ListItem("--Select--", "0", true));

            }

        }

        protected void btnSubmitExcelVersion_Click(object sender, EventArgs e)
        {
            if (ddlExcelObjects.SelectedIndex > 0 && ddlExcelVersions.SelectedIndex > 0)
            {
                if (string.IsNullOrEmpty(ObjectID))
                {
                    BLL.Domain.SubmitObjectVersionMappings(ddlExcelObjects.SelectedValue.ToString(), Convert.ToInt32(ddlExcelVersions.SelectedValue.ToString()), chkForceUpdate.Checked, "N", CurrentSession.User.UserID.ToString());
                    lblMessage.Text = "Record has been added successfully..";
                }
                else
                {
                    BLL.Domain.SubmitObjectVersionMappings(ddlExcelObjects.SelectedValue.ToString(), Convert.ToInt32(ddlExcelVersions.SelectedValue.ToString()), chkForceUpdate.Checked, "A", CurrentSession.User.UserID.ToString());
                    lblMessage.Text = "Record has been updated successfully..";
                }
                ObjectID = "";
                lblMessage.Visible = true;
                ddlExcelObjects.SelectedIndex = 0;
                ddlExcelVersions.SelectedIndex = 0;
                chkForceUpdate.Checked = false;
                btnSubmitExcelVersion.Text = "Save";
                FillVersionGrid();
            }
        }

        protected void FillVersionGrid()
        {
            DataTable dt = BLL.Domain.GetObjectVersionMappings("", 0);
            if (dt.Rows.Count > 0)
            {
                lblNoRecordsMappings.Visible = false;
                rptExcelVersions.DataSource = dt;
            }
            else
            {
                lblNoRecordsMappings.Visible = true;
                rptExcelVersions.DataSource = null;
            }
            rptExcelVersions.DataBind();
        }

        protected void btnEditExcelVersion_Click(object sender, EventArgs e)
        {
            ObjectID = hdnObjectId.Value;
            ddlExcelVersions.Items.Clear();
            DataTable dtVersion = BLL.Domain.GetObjectStoreVersions(hdnObjectId.Value.ToString(), 200);
            if (dtVersion.Rows.Count > 0)
            {
                ddlExcelVersions.DataSource = dtVersion;
                ddlExcelVersions.DataTextField = "Version";
                ddlExcelVersions.DataValueField = "Version";
                ddlExcelVersions.DataBind();
                ddlExcelVersions.Items.Insert(0, new ListItem("--Select--", "0", true));
            }

            DataTable dt = BLL.Domain.GetObjectVersionMappings(hdnObjectId.Value.ToString(), Convert.ToInt32(hdnVersion.Value));

            if (dt.Rows.Count > 0)
            {
                ddlExcelObjects.SelectedValue = dt.Rows[0]["ObjectID"].ToString();
                ddlExcelVersions.SelectedValue = dt.Rows[0]["ObjectVersion"].ToString();
                chkForceUpdate.Checked = Convert.ToBoolean(dt.Rows[0]["ForceUpdate"]);
                btnSubmitExcelVersion.Text = "Update";

            }
            hdnObjectId.Value = "";
            hdnVersion.Value = "";
        }

        protected void btnExcelVersionClear_Click(object sender, EventArgs e)
        {
            ObjectID = "";
            lblMessage.Visible = false;
            ddlExcelObjects.SelectedIndex = 0;
            ddlExcelVersions.SelectedIndex = 0;
            chkForceUpdate.Checked = false;
        }

        protected void btnDeleteExcelVersion_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdnObjectId.Value) && !string.IsNullOrEmpty(hdnVersion.Value))
            {
                DataTable dt = BLL.Domain.GetObjectVersionMappings(hdnObjectId.Value.ToString(), Convert.ToInt32(hdnVersion.Value));

                if (dt.Rows.Count > 0)
                {
                    BLL.Domain.SubmitObjectVersionMappings(dt.Rows[0]["ObjectID"].ToString(), Convert.ToInt32(dt.Rows[0]["ObjectVersion"].ToString()), Convert.ToBoolean(dt.Rows[0]["ForceUpdate"].ToString()), "D", CurrentSession.User.UserID.ToString());
                }
                hdnObjectId.Value = "";
                hdnVersion.Value = "";
                lblMessage.Text = "Record has been deleted successfully..";
                lblMessage.Visible = true;
                FillVersionGrid();
            }
        }
        private string GetRandomKey()
        {
            int length = 32;
            Random random = new Random(); ;
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString().ToUpper();
        }

        protected void btnUpdateVersion_Click(object sender, EventArgs e)
        {

            string guId = GetRandomKey(); // 83E56EC9AFC60F8812A533B5B5080388
            string fileName = txtFileName.Text.Trim();                                  // TheBeastAppsExcel_TWD_010106.zip
            string filePath = Server.MapPath("~/WWSApp/ExcelPkg/" + txtFilePath.Text.Trim());  // 1         
            //string filePath = Server.MapPath("~/ExcelVersion/ExcelVersionDownload/" + txtFilePath.Text.Trim());
            string pathStore = txtFilePath.Text.Trim().Replace("/", "\\\\") + "\\\\";
            string pathToStore = @"\\WWSApp\\ExcelPkg\\" + pathStore;

            try
            {
                ZipFile zipFile = new ZipFile();
                string serverSaveExeFilePath = Server.MapPath("~/ExcelVersion/ExcelVersionZip/" + guId);
                byte[] bt = ReadFully(fileUploadExe.PostedFile.InputStream);
                SaveBytesToFile(serverSaveExeFilePath + "/" + fileUploadExe.PostedFile.FileName, bt);
                string[] filezip = Directory.GetFiles(serverSaveExeFilePath);
                zipFile.AddFiles(filezip, fileName);
                if (!System.IO.Directory.Exists(filePath))
                    System.IO.Directory.CreateDirectory(filePath);


                zipFile.Save(filePath + "/" + fileName + ".zip");

                if (ConfigurationManager.AppSettings["SystemRunningOn"].ToLower() == "amazon" && !Request.Url.AbsoluteUri.Contains("demo"))
                {
                    #region [[ Production ]]

                    bool dirExist = false;
                    string pathProductionFirst = @"\\" + Convert.ToString(ConfigurationManager.AppSettings["Aws_Web01_Ip"]) + pathToStore;
                    string pathProductionSecond = @"\\" + Convert.ToString(ConfigurationManager.AppSettings["Aws_Web02_Ip"]) + pathToStore;

                    using (NetworkShareAccesser.Access(Convert.ToString(ConfigurationManager.AppSettings["Aws_Web01_Ip"]), Convert.ToString(ConfigurationManager.AppSettings["NetworkUserName"]), Convert.ToString(ConfigurationManager.AppSettings["NetworkPassword"])))
                    {
                        if (Directory.Exists(pathProductionFirst))
                        {
                            using (NetworkShareAccesser.Access(Convert.ToString(ConfigurationManager.AppSettings["Aws_Web02_Ip"]), Convert.ToString(ConfigurationManager.AppSettings["NetworkUserName"]), Convert.ToString(ConfigurationManager.AppSettings["NetworkPassword"])))
                            {
                                if (!Directory.Exists(pathProductionSecond))
                                    Directory.CreateDirectory(pathProductionSecond);
                                File.Copy(pathProductionFirst + "\\" + fileName + ".zip", pathProductionSecond + "\\" + fileName + ".zip");
                            }
                            dirExist = true;
                        }

                    }
                    if (!dirExist)
                    {
                        using (NetworkShareAccesser.Access(Convert.ToString(ConfigurationManager.AppSettings["Aws_Web02_Ip"]), Convert.ToString(ConfigurationManager.AppSettings["NetworkUserName"]), Convert.ToString(ConfigurationManager.AppSettings["NetworkPassword"])))
                        {
                            using (NetworkShareAccesser.Access(Convert.ToString(ConfigurationManager.AppSettings["Aws_Web01_Ip"]), Convert.ToString(ConfigurationManager.AppSettings["NetworkUserName"]), Convert.ToString(ConfigurationManager.AppSettings["NetworkPassword"])))
                            {
                                if (!Directory.Exists(pathProductionFirst))
                                    Directory.CreateDirectory(pathProductionFirst);
                                File.Copy(pathProductionSecond + "\\" + fileName + ".zip", pathProductionFirst + "\\" + fileName + ".zip");
                            }
                        }
                    }
                    #endregion

                }
                else if (ConfigurationManager.AppSettings["SystemRunningOn"].ToLower() == "demo")
                {
                    #region [[ Demo ]]

                    bool dirExist = false;
                    string pathDemoFirst = @"\\" + Convert.ToString(ConfigurationManager.AppSettings["Aws_Demo01_Ip"]) + pathToStore;
                    string pathDemoSecond = @"\\" + Convert.ToString(ConfigurationManager.AppSettings["Aws_Demo02_Ip"]) + pathToStore;

                    string networkUserName = Convert.ToString(ConfigurationManager.AppSettings["NetworkUserName_Demo"]);
                    string networkPassword = Convert.ToString(ConfigurationManager.AppSettings["NetworkPassword_Demo"]);


                    using (NetworkShareAccesser.Access(Convert.ToString(ConfigurationManager.AppSettings["Aws_Demo01_Ip"]), networkUserName, networkPassword))
                    {
                        if (Directory.Exists(pathDemoFirst))
                        {
                            using (NetworkShareAccesser.Access(Convert.ToString(pathDemoSecond), networkUserName, networkPassword))
                            {
                                if (!Directory.Exists(pathDemoSecond))
                                    Directory.CreateDirectory(pathDemoSecond);
                                File.Copy(pathDemoFirst + "\\" + fileName + ".zip", pathDemoSecond + "\\" + fileName + ".zip");
                            }
                            dirExist = true;
                        }

                    }
                    if (!dirExist)
                    {
                        using (NetworkShareAccesser.Access(Convert.ToString(ConfigurationManager.AppSettings["Aws_Demo02_Ip"]), networkUserName, networkPassword))
                        {
                            using (NetworkShareAccesser.Access(Convert.ToString(ConfigurationManager.AppSettings["Aws_Demo01_Ip"]), networkUserName, networkPassword))
                            {
                                if (!Directory.Exists(pathDemoFirst))
                                    Directory.CreateDirectory(pathDemoFirst);
                                File.Copy(pathDemoSecond + "\\" + fileName + ".zip", pathDemoFirst + "\\" + fileName + ".zip");
                            }
                        }
                    }

                    #endregion
                }

                BLL.Domain.SubmitExcelPackageInfo(txtVersionName.Text.Trim(), guId, fileName + ".zip", pathToStore, 1, Convert.ToInt32(CurrentSession.User.UserID), DateTime.Now);

                lblMsg.Text = "Successfully Done";

                txtVersionName.Text = "";
                txtFileName.Text = "";
                txtFilePath.Text = "";

                System.IO.DirectoryInfo deleteDir = new DirectoryInfo(serverSaveExeFilePath);
                foreach (FileInfo file in deleteDir.GetFiles())
                {
                    file.Delete();
                }

                foreach (DirectoryInfo dir in deleteDir.GetDirectories())
                {
                    dir.Delete(true);
                }
                Directory.Delete(serverSaveExeFilePath);

                fileUploadExe.PostedFile.InputStream.Dispose();
                FillVersionInfoGrid();
                ScriptManager.RegisterStartupScript(this, GetType(), "SelectTabPack", "SelectExcelPackageTab();", true);
            }
            catch (Exception ex)
            {
                LogUtility.Error("ExcelAutoUpdateConfig.aspx.cs", "btnUpdateVersion_Click", ex.Message, ex);
            }

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

        protected void FillVersionInfoGrid()
        {
            DataTable dt = BLL.Domain.GetVersionInfoMappings("");
            if (dt.Rows.Count > 0)
            {
                lblVersionMsg.Visible = false;
                rptVersionInfo.DataSource = dt;
            }
            else
            {
                lblVersionMsg.Visible = true;
                rptVersionInfo.DataSource = null;
            }
            rptVersionInfo.DataBind();

        }

        protected void imgBtnRight_Command(object sender, CommandEventArgs e)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string refKey = commandArgs[0];
            string versionName = commandArgs[1];
            string fileName = commandArgs[2];
            string filePath = commandArgs[3];
            int recordCreateBy = Convert.ToInt32(commandArgs[4]);
            DateTime recordCreateDtTime = Convert.ToDateTime(commandArgs[5]);
            if (e.CommandName == "ToDeactivate")
            {
                BLL.Domain.SubmitExcelPackageInfo(versionName, refKey, fileName, filePath, 0, recordCreateBy, recordCreateDtTime);
            }
            else if (e.CommandName == "ToActivate")
            {
                BLL.Domain.SubmitExcelPackageInfo(versionName, refKey, fileName, filePath, 1, recordCreateBy, recordCreateDtTime);
            }
            FillVersionInfoGrid();
            lblMsg.Text = "";
            ScriptManager.RegisterStartupScript(this, GetType(), "SelectTabPack", "SelectExcelPackageTab();", true);
        }

        protected void btnExcel_Products_Click(object sender, EventArgs e)
        {
            try
            {
                string txtCurrentUser = Convert.ToString(CurrentSession.User.UserID);
                string txtProductName = txtExcelProductName.Text;
                string txtPriority =  ddExcelPriority.SelectedItem.Text;
                string txtGroupName = ddExcelGroupName.SelectedItem.Text;
                string txtGroupValue = ddExcelGroupName.SelectedItem.Value;
                string txtInstalledName = txtExcelInstalledName.Text;

                
                BLL.Domain.SubmitExcelProductInfo(txtProductName, txtGroupValue, txtPriority, txtInstalledName, txtCurrentUser);
                // BLL.Domain.SubmitExcelProductInfo(VendorName, GUID, Name, Description, BasePermissions, GroupID);

               // lblClickOnceFileMsg.Text = "Successfully Done";

               //txtVendorName.Text = "";
               //txtGUID.Text = "";

                txtExcelProductName.Text = "";
                ddExcelPriority.SelectedIndex = 0;
                ddExcelGroupName.SelectedIndex = 0;
                txtExcelInstalledName.Text = "";
            }
            catch (Exception ex)
            {
                LogUtility.Error("ExcelAutoUpdateConfig.aspx.cs", "btnUpdateVersion_Click", ex.Message, ex);
            }

            FillExcelProductInfoGrid();
            ScriptManager.RegisterStartupScript(this, GetType(), "SelectTabPack", "SelectExcelProductTab();", true);
        }

        //protected void btnExcelNewGUID_Click(object sender, EventArgs e)
        //{
        //       try
        //       {

        //           BLL.Domain.SubmitExcelNewGUID_Click();

        //           lblClickOnceFileMsg.Text = "Successfully Done";

        //       }
        //       catch (Exception ex)
        //       {
        //           LogUtility.Error("ExcelAutoUpdateConfig.aspx.cs", "btnUpdateVersion_Click", ex.Message, ex);
        //       }

        //       ScriptManager.RegisterStartupScript(this, GetType(), "SelectTabPack", "SelectExcelProductTab();", true);
        //}

        protected void btnClickOnceFile_Click(object sender, EventArgs e)
        {
            string productValue = ddlProduct.SelectedItem.Value;
            string productName = ddlProduct.SelectedItem.Text;
            string productType = rbtProductType.SelectedItem.Value;
            string productVersion = txtProductVersion.Text;
            string ServerIP = txtServerIP.Text;
            string txtCurrentUser = Convert.ToString(CurrentSession.User.PrimaryEmailID);

            string filePath = Path.Combine(Server.MapPath("~/"), txtProductPath.Text.Trim());
            string filePathforStore = Path.Combine(Server.MapPath("~/"), txtProductPath.Text.Trim() + "\\" + productVersion + "\\" + productName + ".EXE");

            //string fileName = productName + "_" + productType + "_" + productVersion;
            //string fileName = Path.GetFileName(txtProductPath.Text);
            string fileName = prodFileUploadExe.FileName;

            string pathStore = txtProductPath.Text.Trim().Replace("/", "\\\\") + "\\\\" + productVersion + "\\\\" ;

            try
            {
                ZipFile zipFile = new ZipFile();

                //string serverSaveExeFilePath = Path.Combine(Server.MapPath("~/"), pathStore);
                byte[] bt = ReadFully(prodFileUploadExe.PostedFile.InputStream);

                if (!System.IO.Directory.Exists(pathStore))
                {
                    using (NetworkShareAccesser.Access(Convert.ToString(ConfigurationManager.AppSettings["ExcelServerIP1"]), Convert.ToString(ConfigurationManager.AppSettings["ExcelNetworkUserName"]), Convert.ToString(ConfigurationManager.AppSettings["ExcelNetworkPassword"])))
                    {
                        System.IO.Directory.CreateDirectory(pathStore);
                    }
                }

                SaveBytesToFile(filePath + "\\" + productVersion + "\\" + productName + ".EXE", bt);

                //SaveBytesToFile(serverSaveExeFilePath + "\\" + productVersion + "\\" + productName, bt);
                //string[] filezip = Directory.GetFiles(serverSaveExeFilePath);
                //zipFile.AddFile(filezip[0], "");

                //zipFile.Save(filezip[0]);

                //zipFile.Save(filePath + "\\" + fileName + ".zip");

                if (ConfigurationManager.AppSettings["SystemRunningOn"].ToLower() == "amazon" && !Request.Url.AbsoluteUri.Contains("demo"))
                {
                    bool dirExist = false;
                    string pathProductionFirst = @"\\" + Convert.ToString(ConfigurationManager.AppSettings["ExcelServerIP1"]) + pathStore;
                    string pathProductionSecond = @"\\" + Convert.ToString(ConfigurationManager.AppSettings["ExcelServerIP2"]) + pathStore;

                    using (NetworkShareAccesser.Access(Convert.ToString(ConfigurationManager.AppSettings["ExcelServerIP1"]), Convert.ToString(ConfigurationManager.AppSettings["ExcelNetworkUserName"]), Convert.ToString(ConfigurationManager.AppSettings["ExcelNetworkPassword"])))
                    {
                        if (Directory.Exists(pathProductionFirst))
                        {
                            using (NetworkShareAccesser.Access(Convert.ToString(ConfigurationManager.AppSettings["ExcelServerIP2"]), Convert.ToString(ConfigurationManager.AppSettings["ExcelNetworkUserName"]), Convert.ToString(ConfigurationManager.AppSettings["ExcelNetworkPassword"])))
                            {
                                if (!Directory.Exists(pathProductionSecond))
                                    Directory.CreateDirectory(pathProductionSecond);
                                File.Copy(pathProductionFirst + "\\" + fileName + ".zip", pathProductionSecond + "\\" + fileName + ".zip");
                            }
                            dirExist = true;
                        }

                    }
                    if (!dirExist)
                    {
                        using (NetworkShareAccesser.Access(Convert.ToString(ConfigurationManager.AppSettings["ExcelServerIP2"]), Convert.ToString(ConfigurationManager.AppSettings["ExcelNetworkUserName"]), Convert.ToString(ConfigurationManager.AppSettings["ExcelNetworkPassword"])))
                        {
                            using (NetworkShareAccesser.Access(Convert.ToString(ConfigurationManager.AppSettings["ExcelServerIP1"]), Convert.ToString(ConfigurationManager.AppSettings["ExcelNetworkUserName"]), Convert.ToString(ConfigurationManager.AppSettings["ExcelNetworkPassword"])))
                            {
                                if (!Directory.Exists(pathProductionFirst))
                                    Directory.CreateDirectory(pathProductionFirst);
                                File.Copy(pathProductionSecond + "\\" + fileName + ".zip", pathProductionFirst + "\\" + fileName + ".zip");
                            }
                        }
                    }

                }

                BLL.Domain.SubmitExcelClickOncePackageInfo(productValue, productVersion, filePathforStore, ServerIP, productType, txtCurrentUser);

                lblClickOnceFileMsg.Text = "Successfully Done";

                FillProductListDropdown();
                txtProductVersion.Text = "";
                txtProductPath.Text = "";
                txtServerIP.Text = "";

                //System.IO.DirectoryInfo deleteDir = new DirectoryInfo(serverSaveExeFilePath);
                //System.IO.DirectoryInfo deleteFile = new DirectoryInfo(pathStore);
                //foreach (FileInfo file in deleteFile.GetFiles())
                //{
                //    file.Delete();
                //}

                //foreach (DirectoryInfo dir in deleteDir.GetDirectories())
                //{
                //    dir.Delete(true);
                //}
                //Directory.Delete(serverSaveExeFilePath);
                //

                fileUploadExe.PostedFile.InputStream.Dispose();
                //FillExcelClickOnceInfoGrid();
                ScriptManager.RegisterStartupScript(this, GetType(), "SelectTabPack", "SelectClickOnceTab();", true);
            }
            catch (Exception ex)
            {
                LogUtility.Error("ExcelAutoUpdateConfig.aspx.cs", "btnUpdateVersion_Click", ex.Message, ex);
            }
        }

        protected void FillExcelClickOnceInfoGrid()
        {
            DataTable dt = BLL.Domain.GetExcelClickOnceInfoMappings();

            if (dt != null && dt.Rows.Count > 0)

            if (dt != null)

            {
                if (dt.Rows.Count > 0)
                {
                    lblClickOnceGridMsg.Visible = false;
                    rptClickOnceFile.DataSource = dt;
                }
                else
                {
                    lblClickOnceGridMsg.Visible = true;
                    rptClickOnceFile.DataSource = null;
                }
            }
            rptClickOnceFile.DataBind();

        }

        protected void FillExcelProductInfoGrid()
        {
            DataTable dt = BLL.Domain.GetExcelProductInfoMappings();
            if (dt != null && dt.Rows.Count > 0)
            {
                lblExcelProductGridMsg.Visible = false;
                rptExcelProductFile.DataSource = dt;
            }
            else
            {
                lblExcelProductGridMsg.Visible = true;
                rptExcelProductFile.DataSource = null;
            }
            rptExcelProductFile.DataBind();

        }

    }
}