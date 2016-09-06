<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="SiteMap.aspx.cs"
    Inherits="HR_SiteMap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />

    <script src="../js/ajax.js" type="text/javascript"></script>

    <style type="text/css">
        .style1
        {
            width: 446px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div class="EMS_font">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <br />
    <table style="height: 105px">
        <tr>
            <td><div style="height: 100px">
                <b>Employees</b><br />
                &nbsp;- Employee List<br />
                &nbsp;- Personal <br />
                &nbsp;- Relation<br />
                &nbsp;- Banks<br />
                &nbsp;- Documents<br /><br /></div>
            </td>
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td><div style="height: 104px">
                <b>Cards</b><br />
           
            &nbsp;- Card Allotement<br />
            &nbsp;- Card Entries<br /><br />
            </div>
            </td>
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>
            <td>
                <b> Leaves</b><br />
                &nbsp;-  Entitlement<br />
                &nbsp;-  Summery<br />
                &nbsp;- Assign Leave<br />
                &nbsp;- Manage Leaves<br />
                &nbsp;- Leave Balance<br />
                &nbsp;- Leave Types<br />
            </td>
        
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                &nbsp;</td>
        
            <td><div style="height: 103px">
                <b>Attendance</b><br />
          &nbsp;- Daily <br />
           &nbsp;- Monthly <br />
            </td>
            </div>
            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td><div style="height: 101px">
                <b>Salary</b><br />
                &nbsp;-  Package<br />
                &nbsp;-  Deduction Details<br />
                &nbsp;- Income Tax<br />
                &nbsp;- Slabs<br /><br /></div>
            </td>
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td><div style="height: 100px">
                <b> Masters</b><br />
                &nbsp;-  Departments<br />
                &nbsp;-  Banks<br />
                &nbsp;- Designations<br />
                &nbsp;- Holidays<br /><br /></div>
            </td>
        </tr>
         
              </table>
</ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
