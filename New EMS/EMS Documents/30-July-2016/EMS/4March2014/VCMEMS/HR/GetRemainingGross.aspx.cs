using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class HR_GetRemainingGross : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["usertype"].ToString() != "4" && Session["usertype"].ToString() != "3")
            {
                Response.Redirect("../HR/LoginFailure.aspx");
            }
        }
        catch (Exception ex)
        {

        }
        if (!IsPostBack)
        {
            Response.Clear();
            string slabId = Request.QueryString["slabId"].ToString();
            string packId = Request.QueryString["packId"].ToString();
            string currentInput = Request.QueryString["currentInput"].ToString();
            string method = Request.QueryString["method"].ToString();
            string empId = Request.QueryString["empId"].ToString();
            
          
            string query = "";
            long ans = 0;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[1].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            query = "";
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PayslipGetRemainingGross";
            cmd.Parameters.AddWithValue("@packId", packId);
            cmd.Parameters.AddWithValue("@slabId", slabId);
            cmd.Parameters.AddWithValue("@empId", empId);
            try
            {
                con.Open();
                ans = Convert.ToInt64(cmd.ExecuteScalar().ToString());

            }
            catch (Exception exx)
            { }
            finally { con.Close(); }
           
            Response.Write(ans);
            Response.End();
        }
    }
}
