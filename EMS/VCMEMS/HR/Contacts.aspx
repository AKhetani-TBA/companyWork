<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="Contacts.aspx.cs"
    Inherits="HR_Contacts" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
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
                                    <div style="height: 565px;width: 100%; overflow-y: auto; overflow-x: auto;" >                         
                                        <asp:Label ID="lblName" runat="server" Text="Label"></asp:Label>
             <table>
        <tr>
            <td>
                <b>Vyapar Capital Market Partners LLC</b><br />
            44 Wall Street, 21st Floor<br />
            New York NY 10005<br />
            Main: +1-646-688-7500<br />
            Email: info@thebeastapps.com<br /><br />
            </td>
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td>
                <b>VCM Securities, LLC®</b><br />
            44 Wall Street, 21st Floor<br />
            New York NY 10005<br />
            Main: +1-646-688-7500<br />
            Email: info@vcmsecurities.com<br /><br />
            </td>
            <td>
                &nbsp;</td>
            <td>
                <b>Credit products</b><br />
          Credit Default Swaps<br />
            Correlation<br />
            Credit exotics<br />
            +1-646-688-7550<br />
            cds@thebeastapps.com</td>
        </tr>
           <tr>
            <td>
                <b>United Kingdom</b><br />
           Vyapar Capital Market Partners (UK) Limited <br />

                30 Crown Place <br />
                London EC2A 4EB<br />
                United Kingdom<br />
                +44 (0)20 7398 2800<br />
                Fax: 020 7398 2801<br />
                VCMUK@thebeastapps.com <br /><br />
            </td>
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td>
                <b>India</b><br />
                Vyapar Capital Market Partners (India) Private Limited <br />
                104, 1st Floor, I.T. Tower 2, InfoCity,<br />
                Nr. Indroda Circle,<br />
                GandhiNagar - 382 009,<br />
                (Gujarat-India)<br />
                +1-646-688-7441<br />
                +91-79-30526909 <br /><br />
            </td>
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                &nbsp;</td>
            <td>
                <b>Weather derivatives</b><br /><br />
          +1-646-688-7565<br />
            weather@thebeastapps.com<br /><br />
                <b>Technology services</b><br />
         +1-646-688-7500<br />
        techservices@thebeastapps.com<br /><br />
            </td>
        </tr>
           <tr>
            <td>
                <b>Singapore</b><br />
           #30-02 Hitachi Tower<br />
            16 Collyer Quay<br />
            Singapore 049318<br />
            +65 6435 0500<br />
            Fax: +65 6435 0550<br />
                <br />
                <br />
                <br /><br />
            </td>
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td>
           
                <b>Interest rate products</b><br />
            Options and swaps&nbsp;
                +1-646-688-7400<br />
                options@thebeastapps.com<br />

                IR exotics&nbsp;&nbsp;
                +1-646-688-7400<br />
                irexotics@thebeastapps.com<br />

                Inflation&nbsp;&nbsp;
                +1-646-688-7515<br />
                inflation@thebeastapps.com<br />

                Real estate derivatives&nbsp;
                +1-646-688-7520<br />
                realestate1@thebeastapps.com<br /><br />
               
            </td>
            <td>
                &nbsp;</td>
            <td>
                <b>Liquidity services</b><br />
                +1-646-688-7520<br />
                liquidity@thebeastapps.com<br />
                <br />
                <br /><br />
                <b>Catastrophe products</b><br />
           +1-646-688-7565<br />
            cat@thebeastapps.com</td>
        </tr>
              </table>
              </div>
 </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>