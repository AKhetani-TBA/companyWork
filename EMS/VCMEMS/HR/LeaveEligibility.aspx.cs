using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VCM.EMS.Biz;
using System.Data;
using System.Diagnostics;
using System.Security.Principal;
using System.Collections;

public partial class HR_LeaveEligibility : System.Web.UI.Page
{
    WindowsPrincipal winPrincipal = (WindowsPrincipal)HttpContext.Current.User;
    VCM.EMS.Base.Leave_TakenDetails prop;
    VCM.EMS.Biz.Leave_TakenDetails adapt;
    public HR_LeaveEligibility()
    {
        prop = new VCM.EMS.Base.Leave_TakenDetails();
        adapt = new VCM.EMS.Biz.Leave_TakenDetails();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["usertype"].ToString() != "0" && Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "2" && Session["usertype"].ToString() != "3")
        //{
        //    Response.Redirect("../HR/LoginFailure.aspx");
        //}       
        if (!IsPostBack)
        {
            if ((winPrincipal.Identity.IsAuthenticated == true))
            {
                ViewState["UserName"] = winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1));
                VCM.EMS.Biz.MstUser objMst = new VCM.EMS.Biz.MstUser();
                string userid = objMst.GetUserId(Session["UserName"].ToString());
                ViewState["uid"] = userid;
                Details obj = new Details();
                VCM.EMS.Base.Details prop = new VCM.EMS.Base.Details();
                prop = obj.GetDetailsByID(Convert.ToInt64(userid));
                ViewState["DeptId"] = prop.DeptId;
                ViewState["usertype"] = objMst.GetUserType(Session["UserName"].ToString());
                if (ViewState["usertype"].ToString() == "1" || ViewState["usertype"].ToString() == "3")
                {
                    BindEmployees();
                    //  string[] stremphiredate = ddlEmpName.SelectedValue.Split('#');
                    //lblDoj.Text = Convert.ToDateTime(stremphiredate[0].ToString()).ToString("dd-MMM-yyyy");
                }
                else
                {
                    if (ViewState["usertype"].ToString() == "0")
                    {

                        BindIndividualEmployees();
                    }
                    else
                    {
                        BindEmployees();

                    }
                    //string[] stremphiredate = ddlEmpName.SelectedValue.Split('#');
                    //lblDoj.Text = Convert.ToDateTime(stremphiredate[0].ToString()).ToString("dd-MMM-yyyy");

                }
            }
            BindYear();
            BindHRGrid();

            //        BindEmployees();
            //searchPane.Visible = true;

            //searchResults.Visible = true;
            //assignLeave.Visible = false;
            //Clear();

            //    //DateTime.ParseExact(stremphiredate[1].ToString(), "MM/dd/yyyy", null).ToString("dd/MM/yyyy");
            //    //stremphiredate[1].ToString();
            //string stryear;
            //if (string.IsNullOrEmpty(txtFormDate.Text) || string.IsNullOrEmpty(txtTodate.Text))
            //    stryear = System.DateTime.Now.Year.ToString();
            //else if (Convert.ToDateTime(txtFormDate.Text).Year == Convert.ToDateTime(txtTodate.Text).Year)
            //    stryear = Convert.ToDateTime(txtFormDate.Text).Year.ToString();
            //else
            //    stryear = Convert.ToDateTime(txtTodate.Text).Year.ToString();
            //lblsys.Text = "System Suggested" + " - " + stryear;
            //lblhr.Text = "HR Applied" + " - " + stryear;
            //lbladmin.Text = "Admin Applied" + " - " + stryear;
        }
    }


    protected void gvleavehr_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        VCM.EMS.Biz.Leave_TakenDetails objLeaveinfo = new Leave_TakenDetails();
        DataSet dsLeave = null;
        try
        {
            if (e.CommandName == "Editleavehr")
            {
                // lblleave.Text = "Edit Business Tour / Benefit Day Information";
                //hidhrleaveID.Value = e.CommandArgument.ToString();
                //dsLeave = objLeaveinfo.GetHrLeaveEligibilityDetails(0,0,Convert.ToInt32(hidhrleaveID.Value));
                //txtCL.Text = dsLeave.Tables[0].Rows[0]["CL"].ToString();
                //txtSL.Text = dsLeave.Tables[0].Rows[0]["SL"].ToString();
                //txtPL.Text = dsLeave.Tables[0].Rows[0]["PL"].ToString();
                //txtVPL.Text = dsLeave.Tables[0].Rows[0]["VPL"].ToString();
                //txtVOL.Text = dsLeave.Tables[0].Rows[0]["VOL"].ToString();
                //txtTotal.Text = dsLeave.Tables[0].Rows[0]["Total"].ToString();

                //searchPane.Visible = false;
                //searchResults.Visible = false;
                //assignLeave.Visible = true;

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objLeaveinfo = null;
            if (dsLeave != null)
                dsLeave.Dispose(); dsLeave = null;
        }
    }
    decimal totalcl = 0, totalsl = 0, totalpl = 0, totalvpl = 0, totalvol = 0, totalco = 0, total = 0;
    decimal totalavol = 0, totalaleaeve = 0, totalalwp = 0, totalatdo = 0;

    protected void gvleavehr_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //ImageButton Editbtn = (ImageButton)e.Row.FindControl("ibtnEdithr");
                //string[] strname = { "amehta", "g", "smittal","kpatel" };
                //for( int i=0;i<strname.Length ;i++)
                //{
                //     if(strname[0].ToLower() ==  ViewState["UserName"].ToString().ToLower())
                //        Editbtn.Enabled = true;
                //    else
                //        Editbtn.Enabled = false;
                //}
                decimal dCl = 0, dSl = 0, dPl = 0, dVpl = 0, dVol = 0, dCo = 0;
                decimal davol = 0, daleave = 0, dalwo = 0, datdo;

                Label strcl = (Label)e.Row.FindControl("lblCL");
                Label strsl = (Label)e.Row.FindControl("lblSL");
                Label strpl = (Label)e.Row.FindControl("lblPL");
                Label strvpl = (Label)e.Row.FindControl("lblVPL");
                Label strvop = (Label)e.Row.FindControl("lblVOL");
                Label strco = (Label)e.Row.FindControl("lblCO");
                Label strtotal = (Label)e.Row.FindControl("lblTotal");


                Label straol = (Label)e.Row.FindControl("lblAOL");
                Label straleave = (Label)e.Row.FindControl("lblALeave");
                Label stralwp = (Label)e.Row.FindControl("lblALWP");
                Label stratdo = (Label)e.Row.FindControl("lblATDO");

                //Label strvop = (Label)e.Row.FindControl("lblVOL");
                //Label strco = (Label)e.Row.FindControl("lblCO");
                //Label strtotal = (Label)e.Row.FindControl("lblTotal");



                if (!String.IsNullOrEmpty(strcl.Text))
                {
                    dCl = Convert.ToDecimal(strcl.Text);
                    totalcl += Convert.ToDecimal(strcl.Text);
                }
                if (!String.IsNullOrEmpty(strsl.Text))
                {
                    dSl = Convert.ToDecimal(strsl.Text);
                    totalsl += Convert.ToDecimal(strsl.Text);
                }
                if (!String.IsNullOrEmpty(strpl.Text))
                {
                    dPl = Convert.ToDecimal(strpl.Text);
                    totalpl += Convert.ToDecimal(strpl.Text);
                }
                if (!String.IsNullOrEmpty(strvpl.Text))
                {
                    dVpl = Convert.ToDecimal(strvpl.Text);
                    totalvpl += Convert.ToDecimal(strvpl.Text);
                }
                if (!String.IsNullOrEmpty(strvop.Text))
                {
                    dVol = Convert.ToDecimal(strvop.Text);
                    totalvol += Convert.ToDecimal(strvop.Text);
                }
                if (!String.IsNullOrEmpty(strco.Text))
                {
                    dCo = Convert.ToDecimal(strco.Text);
                    totalco += Convert.ToDecimal(strco.Text);
                }

                //strtotal.Text = Convert.ToString(dCl + dSl + dPl + dVpl + dVol + dCo);
                total += dCl + dSl + dPl + dVpl + dVol + dCo;

                if (!String.IsNullOrEmpty(straol.Text))
                {
                    davol = Convert.ToDecimal(straol.Text);
                    totalavol += Convert.ToDecimal(straol.Text);
                }
                if (!String.IsNullOrEmpty(straleave.Text))
                {
                    daleave = Convert.ToDecimal(straleave.Text);
                    totalaleaeve += Convert.ToDecimal(straleave.Text);
                }
                if (!String.IsNullOrEmpty(stralwp.Text))
                {
                    dalwo = Convert.ToDecimal(stralwp.Text);
                    totalalwp += Convert.ToDecimal(stralwp.Text);
                }
               // totalatdo += davol + daleave + dalwo;
                totalatdo = davol + daleave + dalwo;
                stratdo.Text = totalatdo.ToString();

                Label lblMonth = (Label)e.Row.FindControl("lblMonth");
                if (lblMonth.Text.ToUpper() == Convert.ToDateTime(DateTime.Now.Date).ToString("MMMM").ToUpper())
                {
                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        e.Row.Cells[i].BackColor = System.Drawing.Color.Silver;
                    }
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblMth = (Label)e.Row.FindControl("lblfMonth");
                lblMth.Text = "Total";
                Label strfcl = (Label)e.Row.FindControl("lblfCL");
                strfcl.Text = totalcl.ToString();
                Label strfsl = (Label)e.Row.FindControl("lblfSL");
                strfsl.Text = totalsl.ToString();
                Label strfpl = (Label)e.Row.FindControl("lblfPL");
                strfpl.Text = totalpl.ToString();
                Label strfvpl = (Label)e.Row.FindControl("lblfVPL");
                strfvpl.Text = totalvpl.ToString();
                Label strfvop = (Label)e.Row.FindControl("lblfVOL");
                strfvop.Text = totalvol.ToString();
                Label strfco = (Label)e.Row.FindControl("lblfCO");
                strfco.Text = totalco.ToString();
                Label strftotal = (Label)e.Row.FindControl("lblfTotal");
                strftotal.Text = total.ToString();

                Label strfaol = (Label)e.Row.FindControl("lblfAOL");
                strfaol.Text = totalavol.ToString();
                Label strfaleave = (Label)e.Row.FindControl("lblfALeave");
                strfaleave.Text = totalaleaeve.ToString();
                Label strfalwp = (Label)e.Row.FindControl("lblfALWP");
                strfalwp.Text = totalalwp.ToString();
                Label lblatdo = (Label)e.Row.FindControl("lblfATDO");
                lblatdo.Text = (Convert.ToDecimal(strfaol.Text) + Convert.ToDecimal(strfaleave.Text) + Convert.ToDecimal(strfalwp.Text)).ToString();

                //lblamount.Text = total.ToString();
                //strfcl.Text = strfcl.Text;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //string[] stremphiredate = ddlEmpName.SelectedValue.Split('#');
            //lblDoj.Text = Convert.ToDateTime(stremphiredate[1].ToString()).ToString("dd-MMM-yyyy"); 
            BindHRGrid();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
    }
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        // searchPane.Visible = false;
        //// searchResults.Visible = false;
        // assignLeave.Visible = true;
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        //BindEmployees();        
        //BindHRGrid();        
        //searchPane.Visible = true;
        //searchResults.Visible = true;
        //assignLeave.Visible = false;         
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //VCM.EMS.Biz.Leave_TakenDetails objLeaveinfo = null;
        //VCM.EMS.Base.Leave_TakenDetails objLeave = null;        
        //try
        //{
        //    objLeaveinfo = new Leave_TakenDetails();
        //    objLeave = new VCM.EMS.Base.Leave_TakenDetails();
        //    int LeaveId =0;

        //    //if( !string.IsNullOrEmpty(hidadminleaveID.Value))
        //    //    LeaveId = Convert.ToInt32(hidadminleaveID.Value);
        //    //else
        //    //    LeaveId = Convert.ToInt32(hidhrleaveID.Value);

        //    objLeave.LeaveID = LeaveId;
        //    objLeave.CL = Convert.ToDecimal(txtCL.Text);            
        //    objLeave.SL = Convert.ToDecimal(txtSL.Text);
        //    objLeave.PL = Convert.ToDecimal(txtPL.Text);
        //    objLeave.VPL = Convert.ToDecimal(txtVPL.Text);            
        //    objLeave.VOL =  Convert.ToDecimal(txtVOL.Text);            
        //    objLeave.Total = Convert.ToDecimal(txtTotal.Text);

        //    if (hidhrleaveID.Value != "")
        //        objLeave.ModifyBy = winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1));
        //    else
        //        objLeave.CreatedBy = winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1));

        //    //if (!string.IsNullOrEmpty(hidadminleaveID.Value))
        //    //    objLeaveinfo.Update_AdminLeaveDetails(objLeave);
        //    //else
        //    //    objLeaveinfo.Update_HRLeaveDetails(objLeave);          

        //        Clear();                
        //        BindHRGrid();                
        //        searchPane.Visible = true;
        //        searchResults.Visible = true;
        //        assignLeave.Visible = false;                         

        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}
        //finally
        //{
        //    objLeaveinfo = null;
        //    objLeave = null;
        //}
    }


    private void BindHRGrid()
    {
        DataSet srch = null;
        VCM.EMS.Biz.Leave_TakenDetails srchdt = null;
        try
        {
                  Details empdt = new Details();
                 DataSet empds = new DataSet();
                //string[] stremphiredate = ddlEmpName.SelectedValue.Split('#');
                //lblDoj.Text = Convert.ToDateTime(stremphiredate[1].ToString()).ToString("dd-MMM-yyyy"); 
                empds = empdt.GetByEmpId(0,Convert .ToInt32  (ddlEmpName.SelectedValue.ToString()));
                lblDoj.Text = Convert.ToDateTime(empds.Tables[0].Rows[0]["empHireDate"].ToString()).ToString("dd-MMM-yyyy");

                srch = new DataSet();
                srchdt = new Leave_TakenDetails();
                //int iyear=0;
                //if (string.IsNullOrEmpty(txtFormDate.Text) || string.IsNullOrEmpty(txtTodate.Text))
                //    iyear = System.DateTime.Now.Year;
                //else if (Convert.ToDateTime(txtFormDate.Text).Year == Convert.ToDateTime(txtTodate.Text).Year)
                //    iyear = Convert.ToInt32(Convert.ToDateTime(txtFormDate.Text).Year);
                //else
                //    iyear = Convert.ToInt32(Convert.ToDateTime(txtTodate.Text).Year);

                //if (string.IsNullOrEmpty(txtFormDate.Text) || string.IsNullOrEmpty(txtTodate.Text))
                //    iyear = System.DateTime.Now.Year;
                //else if (Convert.ToDateTime(DateTime.ParseExact(txtFormDate.Text, "dd/MM/yyyy", null).ToString("MM/dd/yyyy")).Year == Convert.ToDateTime(DateTime.ParseExact(txtTodate.Text, "dd/MM/yyyy", null).ToString("MM/dd/yyyy")).Year)
                //    iyear = Convert.ToDateTime(DateTime.ParseExact(txtFormDate.Text, "dd/MM/yyyy", null).ToString("MM/dd/yyyy")).Year;
                //else
                //    iyear = Convert.ToDateTime(DateTime.ParseExact(txtTodate.Text, "dd/MM/yyyy", null).ToString("MM/dd/yyyy")).Year;


            string[] strempId = ddlEmpName.SelectedValue.Split('#');
            srch = srchdt.GetLeaveBalance(Convert.ToInt32(strempId[0]), Convert.ToInt32(ddlYear.SelectedValue.ToString()));
            //GetHrLeaveEligibilityDetails(iyear, Convert.ToInt32(strempId[0]),0);
            gvleavehr.DataSource = srch;
            gvleavehr.DataBind();

            //if (srch.Tables[1].Rows.Count > 0)
            //{
            //    lblhrcl.Text = srch.Tables[1].Rows[0]["CL"].ToString();
            //    lblhrsl.Text = srch.Tables[1].Rows[0]["SL"].ToString();
            //    lblhrpl.Text = srch.Tables[1].Rows[0]["PL"].ToString();
            //    lblhrvpl.Text = srch.Tables[1].Rows[0]["VPL"].ToString();
            //    lblhrvol.Text = srch.Tables[1].Rows[0]["VOL"].ToString();
            //    lblhrtotal.Text = srch.Tables[1].Rows[0]["Total"].ToString();
            //}
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (srch != null) srch.Dispose(); srch = null;
            srchdt = null;
        }
    }
    private void Clear()
    {
        // hidadminleaveID.ID = string.Empty;
        //hidhrleaveID.ID = string.Empty;
        //txtCL.Text = string.Empty;
        //txtSL.Text = string.Empty;
        //txtPL.Text = string.Empty;
        //txtVOL.Text = string.Empty;
        //txtVPL.Text = string.Empty;
    }
    private void BindEmployees()
    {
        Details empdt = new Details();
        DataSet empds = new DataSet();
        if (ViewState["usertype"].ToString() == "2")
        {
            empds = empdt.GetByEmpId(Convert.ToInt32(ViewState["DeptId"].ToString()), 0);
            ddlEmpName.DataSource = empds;
            ddlEmpName.DataTextField = "empName";
            ddlEmpName.DataValueField = "empId";
            ddlEmpName.DataBind();
            lblDoj.Text = Convert.ToDateTime(empds.Tables[0].Rows[0]["empHireDate"].ToString()).ToString("dd-MMM-yyyy");
        }
        else
        {
            empds = empdt.GetAll2();
            ddlEmpName.DataSource = empds;
            ddlEmpName.DataTextField = "empName";
            ddlEmpName.DataValueField = "empId";
            ddlEmpName.DataBind();
            lblDoj.Text = Convert.ToDateTime(empds.Tables[0].Rows[0]["empHireDate"].ToString()).ToString("dd-MMM-yyyy");
        }
        ddlEmpName.SelectedValue = ViewState["uid"].ToString();

    }

    private void BindIndividualEmployees()
    {
        Details empdt = new Details();
        DataSet empds = new DataSet();
        this.ViewState["SortExp"] = "empName";
        this.ViewState["SortOrder"] = "ASC";
        //if (showDepartments.SelectedIndex == 0)
        //    empds = empdt.GetAll2();
        //else
        //   empds = empdt.GetByDept2(Convert.ToInt32(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), "1", System.DateTime.Now.Month.ToString(), System.DateTime.Now.Year.ToString(), "1");
        empds = empdt.GetByEmpId(0, Convert.ToInt32(ViewState["uid"]));
        ddlEmpName.DataSource = empds;
        ddlEmpName.DataTextField = "empName";
        ddlEmpName.DataValueField = "empId";
        ddlEmpName.DataBind();
        lblDoj.Text = Convert.ToDateTime(empds.Tables[0].Rows[0]["empHireDate"].ToString()).ToString("dd-MMM-yyyy");
        //ddlemp.Items.Insert(0, "- Select Employee -");
        //ddlemp.SelectedIndex = 0;
    }

    private void BindYear()
    {
        ArrayList AListYear = new ArrayList();

        for (int i = 2013; i <= DateTime.Now.Year; i++)
        {
            AListYear.Add(i);
        }
        ddlYear.DataSource = AListYear;
        ddlYear.DataBind();
        //ddlYear.SelectedIndex = 0;
        ddlYear.SelectedValue = Convert.ToString(DateTime.Now.AddDays(-1).Year);

    }
 
}