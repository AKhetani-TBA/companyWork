using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace Administration
{
    public partial class iptest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DBService objService = new DBService();
                string[] arrIpInfo = objService.GetIp_Method_2();
                StringBuilder sbTable = new StringBuilder();
                string strIpTable = "<table><tr>";
                strIpTable += "<td colspan=\"2\" class=\"title\">If HttpContext.Current.Request.ServerVariables[HTTP_CLIENT_IP] is not NULL</td>"
                            + "</tr><tr>"
                            + "<td>" + arrIpInfo[0].Split('#')[0] + "</td><td>" + arrIpInfo[0].Split('#')[1] + "</td>"
                            + "</tr><tr>"
                            + "<td>" + arrIpInfo[1].Split('#')[0] + "</td><td>" + arrIpInfo[1].Split('#')[1] + "</td>"
                            + "</tr><tr>"
                            + "<td>" + arrIpInfo[2].Split('#')[0] + "</td><td>" + arrIpInfo[2].Split('#')[1] + "</td>"
                            + "</tr><tr>"
                            + "<td>" + arrIpInfo[3].Split('#')[0] + "</td><td>" + arrIpInfo[3].Split('#')[1] + "</td>"
                            + "</tr><tr>"
                            + "<td>" + arrIpInfo[4].Split('#')[0] + "</td><td>" + arrIpInfo[4].Split('#')[1] + "</td>"
                            + "</tr><tr>"
                            + "<td colspan=\"2\" class=\"title\">If HttpContext.Current.Request.Headers[X-Forwarded-For] is not NULL</td>"
                             + "</tr><tr>"
                            + "<td>" + arrIpInfo[5].Split('#')[0] + "</td><td>" + arrIpInfo[5].Split('#')[1] + "</td>"
                            + "</tr><tr>"
                            + "<td>" + arrIpInfo[6].Split('#')[0] + "</td><td>" + arrIpInfo[6].Split('#')[1] + "</td>"
                            + "</tr><tr>"
                            + "<td>" + arrIpInfo[7].Split('#')[0] + "</td><td>" + arrIpInfo[7].Split('#')[1] + "</td>"
                            + "</tr><tr>"
                            + "<td>" + arrIpInfo[8].Split('#')[0] + "</td><td>" + arrIpInfo[8].Split('#')[1] + "</td>"
                            + "</tr><tr>"
                            + "<td>" + arrIpInfo[9].Split('#')[0] + "</td><td>" + arrIpInfo[9].Split('#')[1] + "</td>"
                            + "</tr></table>";

                lblInfotbl.Text = strIpTable;
            }
            catch (Exception ex)
            {
                lblInfotbl.Text = ex.Message;
            }
        }
    }
}