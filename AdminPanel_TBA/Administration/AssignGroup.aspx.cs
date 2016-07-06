using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
//using VCM.Common.Log;
using System.Net;
using System.Web.UI.HtmlControls;
using TBA.Utilities;

namespace Administration
{
    public partial class AssignGroup : System.Web.UI.Page
    {
        ArrayList arraylist1 = new ArrayList();
        ArrayList arraylist2 = new ArrayList();
        List<string> usridList = new List<string>();
        DataTable allusrImgs, AllUsrInGrp, imgsInGrp = new DataTable();
        Int32 groupid;

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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {              
                if (CurrentSession.User.IsTrader == "TRUE")
                {
                    string script = @"setMessage();";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "setM", script, true);

                    return;
                }

                if (!IsPostBack)
                {

                    System.Web.UI.HtmlControls.HtmlGenericControl li = (System.Web.UI.HtmlControls.HtmlGenericControl)this.Page.Master.FindControl("group");
                    li.Attributes.Add("class", "active");

                    discr.Visible = false;
                    GetGroupList();
                }
                lblAddImg.Text = "";
                lblAddInGrp.Text = "";
                lbldltFrmgrp.Text = "";
                lblRmvImg.Text = "";
            }
            catch (Exception ex)
            {
                LogUtility.Error("AssignGroup", "Page_Load", ex.Message, ex);
            }
        }

        private void GetGroupList()
        {
            try
            {
                DataTable dTable = BLL.Domain.GetAllGroups(null);
                if (dTable != null && dTable.Rows.Count > 0)

                    rptrGroup.DataSource = dTable;
                else
                    rptrGroup.DataSource = null;

                rptrGroup.DataBind();
            }
            catch (Exception ex)
            {
                LogUtility.Error("AssignGroup", "GetGroupList()", ex.Message, ex);
            }
        }

        protected void btnCreateGrp_Click(object sender, EventArgs e)
        {

            LogUtility.Info("AdminPanel.aspx.cs", "btnCreateUser_Click", "Clicked to create user");
            try
            {
                BASE.User objUser = new BASE.User();
                string groupName, grpDesc;
                groupName = txtGroupName.Text.Trim();
                grpDesc = txtGroupDesxtiption.Text.Trim();

                DataTable dt = new DataTable();
                dt = BLL.Domain.Submit_AppStore_Group(groupName, grpDesc);
                int _flag = Convert.ToInt32(dt.Rows[0]["MsgId"]);
                if (_flag == 0)
                {
                    lblMsgCreateGrp.Text = "Group created successfully";
                    lblMsgCreateGrp.ForeColor = System.Drawing.Color.Red;

                    GetGroupList();
                }
                else
                {
                    lblMsgCreateGrp.Text = "Group already  exists";
                    lblMsgCreateGrp.ForeColor = System.Drawing.Color.Red;
                    txtGroupName.Focus();
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("AssignGroup", "btnCreateGrp_Click", ex.Message, ex);
            }
        }

        protected void getGropDetails(object sender, EventArgs e)
        {
            try
            {
                ImageButton button = (sender as ImageButton);
                string commandArgument = button.CommandArgument;
                string[] commandArgs = commandArgument.ToString().Split(new char[] { ',' });
                groupid = Convert.ToInt32(commandArgs[0]);
                hdn_grp.Value = groupid.ToString(); ;
                string groupName = commandArgs[1];
                discr.Visible = true;
                lblGrpName.Text = groupName;
                lblGrpName.Font.Size = 20;

                DataTable dt = new DataTable();
                dt = BLL.Domain.GetAllUserInGroup(groupid);

                usrInGrpList.Items.Clear();
                usrInGrpList.DataSource = dt;
                usrInGrpList.DataValueField = "UserID";
                usrInGrpList.DataTextField = "Email_Name"; //column name in DT
                usrInGrpList.DataBind();
                //dt = BLL.Domain.GetAllImages();

                allusrImgs = BLL.Domain.GetAllImages();
                rptrAllImgs.DataSource = allusrImgs;
                rptrAllImgs.DataBind();

                imgsInGrp = BLL.Domain.GetImagesInGroup(groupid);
                rptrGrpImgs.DataSource = imgsInGrp;
                rptrGrpImgs.DataBind();

                string[] allitems = imgsInGrp.AsEnumerable().Select(s => s.Field<string>("ObjectId")).ToArray<string>();
                foreach (var item in allitems)
                {
                    string asd = item.ToString();
                    for (int i = 0; i < rptrAllImgs.Items.Count; i++)
                    {
                        HtmlInputCheckBox chkDisplayTitle = (HtmlInputCheckBox)rptrAllImgs.Items[i].FindControl("cb1");
                        string[] commandcheck = chkDisplayTitle.Value.ToString().Split(new char[] { ',' });

                        string imageid = commandcheck[0];
                        string check = imageid.Trim();
                        if (check.CompareTo(asd) == 0)
                        {
                            rptrAllImgs.Items[i].Visible = false;
                        }
                    }
                }

                AllUsrInGrp = BLL.Domain.Get_AppStore_User_List(null);
                AllUserLst.Items.Clear();
                AllUserLst.DataSource = AllUsrInGrp;
                AllUserLst.DataValueField = "UserID";
                AllUserLst.DataTextField = "Name"; //column name in DT
                AllUserLst.DataBind();

                var usrList = usrInGrpList.Items.Cast<ListItem>().ToList();

                foreach (var item in usrList)
                    AllUserLst.Items.Remove(item);
            }
            catch (Exception ex)
            {
                LogUtility.Error("AssignGroup", "getGropDetails()", ex.Message, ex);
            }
        }

        protected void btnDltFrmGrp_Click(object sender, EventArgs e)
        {
            lblAddInGrp.Text = "";
            lbldltFrmgrp.Text = "";
            try
            {
                DataTable usrdtl = new DataTable();
                usrdtl.Columns.Add("userid", typeof(Int32));
                lbldltFrmgrp.Visible = false;
                if (usrInGrpList.SelectedIndex >= 0)
                {
                    for (int i = 0; i < usrInGrpList.Items.Count; i++)
                    {
                        if (usrInGrpList.Items[i].Selected)
                        {
                            DataRow dr = usrdtl.NewRow();
                            if (!arraylist1.Contains(usrInGrpList.Items[i]))
                            {
                                arraylist1.Add(usrInGrpList.Items[i]);
                                Int32 asd = Convert.ToInt32(usrInGrpList.Items[i].Value);
                                dr["userid"] = asd;
                                usrdtl.Rows.Add(dr);
                            }
                        }
                    }

                    for (int i = 0; i < arraylist1.Count; i++)
                    {
                        AllUserLst.Items.Add(((ListItem)arraylist1[i]));
                        usrInGrpList.Items.Remove(((ListItem)arraylist1[i]));
                    }

                    DataTable addgrpUser = new DataTable();
                    int flag = 0;
                    Int32 grpId = Convert.ToInt32(hdn_grp.Value);
                    addgrpUser = BLL.Domain.submitUserInGroup(grpId, usrdtl, flag);
                    int _flag = Convert.ToInt32(addgrpUser.Rows[0]["MsgId"]);
                    if (_flag == 0)
                    {
                        AllUserLst.SelectedIndex = -1;
                        lbldltFrmgrp.Visible = true;
                        lbldltFrmgrp.Text = "User Removed from group";
                    }
                }
                else
                {
                    lbldltFrmgrp.Visible = true;
                    lbldltFrmgrp.Text = "Please select atleast one in User's In Group to Remove";
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("AssignGroup", "btnDltFrmGrp_Click", ex.Message, ex);
            }
        }

        protected void btnAddToGrp_Click(object sender, EventArgs e)
        {
            lblAddInGrp.Text = "";
            lbldltFrmgrp.Text = "";
            try
            {
                DataTable usrdtl = new DataTable();
                usrdtl.Columns.Add("userid", typeof(Int32));
                lblAddInGrp.Visible = false;
                if (AllUserLst.SelectedIndex >= 0)
                {

                    for (int i = 0; i < AllUserLst.Items.Count; i++)
                    {
                        if (AllUserLst.Items[i].Selected)
                        {
                            DataRow dr = usrdtl.NewRow();

                            arraylist2.Add(AllUserLst.Items[i]);

                            Int32 asd = Convert.ToInt32(AllUserLst.Items[i].Value);
                            dr["userid"] = asd;
                            usrdtl.Rows.Add(dr);
                        }
                    }

                    DataTable addgrpUser = new DataTable();
                    int flag = 1;
                    Int32 grpId = Convert.ToInt32(hdn_grp.Value);
                    addgrpUser = BLL.Domain.submitUserInGroup(grpId, usrdtl, flag);
                    for (int i = 0; i < arraylist2.Count; i++)
                    {
                        if (!usrInGrpList.Items.Contains(((ListItem)arraylist2[i])))
                        {
                            usrInGrpList.Items.Add(((ListItem)arraylist2[i]));
                        }
                        AllUserLst.Items.Remove(((ListItem)arraylist2[i]));
                    }
                    usrInGrpList.SelectedIndex = -1;
                    lblAddInGrp.Visible = true;

                    lblAddInGrp.Text = "User Added from group";
                }
                else
                {
                    lblAddInGrp.Visible = true;
                    lblAddInGrp.Text = "Please select atleast one in All Users to add";
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("AssignGroup", "btnAddToGrp_Click", ex.Message, ex);
            }
        }

        protected void rptrGrpImgs_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (rptrGrpImgs.Items.Count < 1)
            {
                if (e.Item.ItemType == ListItemType.Footer)
                {
                    Label lblFooter = (Label)e.Item.FindControl("lblEmptyData");
                    lblFooter.Text = "No item present";
                    lblFooter.Visible = true;
                }
            }
        }

        protected void rptrAllImgs_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (rptrAllImgs.Items.Count < 1)
            {
                if (e.Item.ItemType == ListItemType.Footer)
                {
                    Label lblFooter = (Label)e.Item.FindControl("lblEmptyData");
                    lblFooter.Text = "No item present";
                    lblFooter.Visible = true;
                }
            }
        }

        protected void btnRmveImgFrmGrp_Click(object sender, EventArgs e)
        {

            try
            {
                lblAddImg.Text = "";
                lblRmvImg.Text = "";
                DataTable imgdtl = new DataTable();
                imgdtl.Columns.Add("ObjectID", typeof(string));

                DataTable dt = (DataTable)rptrAllImgs.DataSource;
                int status = -1;
                for (int i = 0; i < rptrGrpImgs.Items.Count; i++)
                {

                    HtmlInputCheckBox chkDisplayTitle = (HtmlInputCheckBox)rptrGrpImgs.Items[i].FindControl("ImgInGrp");
                    if (chkDisplayTitle.Checked)
                    {
                        status = 0;
                        var csk = rptrGrpImgs.Items[i];
                        var data = chkDisplayTitle.Value;
                        rptrGrpImgs.Items[i].Visible = false;

                        string[] commandArgs = data.ToString().Split(new char[] { ',' });
                        string imageID = commandArgs[0];

                        string categoryName = commandArgs[1];
                        string AppName = commandArgs[2];

                        DataRow dr = imgdtl.NewRow();
                        dr["ObjectID"] = imageID;
                        imgdtl.Rows.Add(dr);

                    }
                }

                if (status == 0)
                {
                    DataTable delteImgs = new DataTable();
                    int flag = 0;

                    Int32 grpId = Convert.ToInt32(hdn_grp.Value);
                    delteImgs = BLL.Domain.submitImagesInGroup(grpId, imgdtl, flag);
                    int _flag = Convert.ToInt32(delteImgs.Rows[0]["MsgId"]);
                    if (_flag == 0)
                    {
                        lblRmvImg.Text = "Image Removed from Group";
                        lblMsgCreateGrp.ForeColor = System.Drawing.Color.Red;

                        allusrImgs = BLL.Domain.GetAllImages();
                        rptrAllImgs.DataSource = allusrImgs;
                        rptrAllImgs.DataBind();



                        imgsInGrp = BLL.Domain.GetImagesInGroup(grpId);
                        rptrGrpImgs.DataSource = imgsInGrp;
                        rptrGrpImgs.DataBind();

                        string[] allitems = imgsInGrp.AsEnumerable().Select(s => s.Field<string>("ObjectId")).ToArray<string>();
                        foreach (var item in allitems)
                        {
                            string asd = item.ToString();
                            for (int i = 0; i < rptrAllImgs.Items.Count; i++)
                            {

                                HtmlInputCheckBox chkDisplayTitle = (HtmlInputCheckBox)rptrAllImgs.Items[i].FindControl("cb1");
                                string[] commandcheck = chkDisplayTitle.Value.ToString().Split(new char[] { ',' });

                                string imageid = commandcheck[0];
                                string check = imageid.Trim();
                                if (check.CompareTo(asd) == 0)
                                {

                                    rptrAllImgs.Items[i].Visible = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        lblRmvImg.Text = "Image not Removed";
                        lblMsgCreateGrp.ForeColor = System.Drawing.Color.Red;
                        txtGroupName.Focus();
                    }
                }
                else
                {
                    lblRmvImg.Text = "Select atleast one image from images group to remove";
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("AssignGroup", "btnRmveImgFrmGrp_Click", ex.Message, ex);
            }
        }

        protected void btnAddImgToGrp_Click(object sender, EventArgs e)
        {
            try
            {
                lblAddImg.Text = "";
                lblRmvImg.Text = "";
                DataTable imgdtl = new DataTable();
                imgdtl.Columns.Add("ObjectID", typeof(string));

                DataTable dt = (DataTable)rptrAllImgs.DataSource;
                int status = -1;
                for (int i = 0; i < rptrAllImgs.Items.Count; i++)
                {
                    HtmlInputCheckBox chkDisplayTitle = (HtmlInputCheckBox)rptrAllImgs.Items[i].FindControl("cb1");
                    if (chkDisplayTitle.Checked)
                    {
                        status = 0;

                        var data = chkDisplayTitle.Value;
                        rptrAllImgs.Items[i].Visible = false;

                        string[] commandArgs = data.ToString().Split(new char[] { ',' });
                        string imageID = commandArgs[0];

                        string categoryName = commandArgs[1];
                        string AppName = commandArgs[2];

                        DataRow dr = imgdtl.NewRow();
                        dr["ObjectID"] = imageID;
                        imgdtl.Rows.Add(dr);
                    }
                }

                if (status == 0)
                {
                    DataTable dtAddImgs = new DataTable();
                    int flag = 1;

                    Int32 grpId = Convert.ToInt32(hdn_grp.Value);
                    dtAddImgs = BLL.Domain.submitImagesInGroup(grpId, imgdtl, flag);
                    int _flag = Convert.ToInt32(dtAddImgs.Rows[0]["MsgId"]);
                    if (_flag == 0)
                    {
                        lblAddImg.Text = "Image added to group";
                        lblMsgCreateGrp.ForeColor = System.Drawing.Color.Red;
                        allusrImgs = BLL.Domain.GetAllImages();

                        rptrAllImgs.DataSource = allusrImgs;
                        rptrAllImgs.DataBind();

                        imgsInGrp = BLL.Domain.GetImagesInGroup(grpId);
                        rptrGrpImgs.DataSource = imgsInGrp;
                        rptrGrpImgs.DataBind();

                        string[] allitems = imgsInGrp.AsEnumerable().Select(s => s.Field<string>("ObjectId")).ToArray<string>();
                        foreach (var item in allitems)
                        {
                            string asd = item.ToString();
                            for (int i = 0; i < rptrAllImgs.Items.Count; i++)
                            {

                                HtmlInputCheckBox chkDisplayTitle = (HtmlInputCheckBox)rptrAllImgs.Items[i].FindControl("cb1");
                                string[] commandcheck = chkDisplayTitle.Value.ToString().Split(new char[] { ',' });

                                string imageid = commandcheck[0];
                                string check = imageid.Trim();
                                if (check.CompareTo(asd) == 0)
                                {
                                    rptrAllImgs.Items[i].Visible = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        lblAddImg.Text = "Image not added";
                        lblMsgCreateGrp.ForeColor = System.Drawing.Color.Red;
                        txtGroupName.Focus();
                    }
                }
                else
                {
                    lblAddImg.Text = "Select atleast one image from All images to add";
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("AssignGroup", "btnAddImgToGrp_Click", ex.Message, ex);
            }
        }
    }
}