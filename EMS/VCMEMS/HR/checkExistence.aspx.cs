using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VCM.EMS.Biz;
using System.Data.SqlClient;
using System.Configuration;

public partial class HR_checkExistence : System.Web.UI.Page
{
    VCM.EMS.Biz.Details adapt;
    public HR_checkExistence()
    {
       
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Response.Clear();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[1].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            
            string ans = System.DBNull.Value.ToString();
            string value = Request.QueryString["value"].ToString();
            try
            {
                cmd.CommandText = "SELECT empId from Emp_Details where empId=" + value;
                con.Open();
                //adapt = new Details();
                ans = cmd.ExecuteScalar().ToString();
                
                //ans = adapt.checkIdExistence(Convert.ToInt16(value)).ToString ();
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            if (ans == System.DBNull.Value.ToString())
            {
                Response.Write("Available");
            }
            else
            {
                Response.Write("Employee Id already assigned.");
            }
            Response.End();
        }

    }
}
