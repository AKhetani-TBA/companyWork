using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_sample : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        TextBox1.Text = (TextBox1.Text).Substring(0, ((TextBox1.Text).Length - (((TextBox1.Text).Length) - (TextBox1.Text).IndexOf(@"@"))));
                               

    }
}
