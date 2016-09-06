<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EarningDetail.aspx.cs"
    Inherits="HR_EarningDetail" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" id="Head1">
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/ajax.js" type="text/javascript"></script>

    <style type="text/css">
        .style1
        {
            width: 79%;
        }
        .style2
        {
            color: #FF0000;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="EMS_font">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="fireEvent" />
            </Triggers>
            <ContentTemplate>
                <div>
                    <div>
                        <div>
                            <%-- <fieldset style="padding: 5px;">
                    <legend style="margin-bottom: 2px; font-weight: normal; color: #808080;">Search </legend>--%>
                            <fieldset style="padding: 2px;">
                                <div>
                                    <table>
                                        <tr>
                                            <td>
                                                WEF. Year :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="showYear" runat="server" Width="100px" AutoPostBack="True"
                                                    OnSelectedIndexChanged="showYear_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </fieldset>
                            <div id="div2">
                                <fieldset style="padding: 5px; padding-left: 6px;">
                                    <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Deductions</legend>
                                    <%--    <fieldset style="padding: 5px; width :340px;float:left;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;float: left">ESIC</legend>--%>
                                    <div style="text-align: left; vertical-align: top; width: 100%; height: 100%;">
                                        <fieldset style="padding: 5px; margin-right: 5px; width: 450px; height: 150px; vertical-align: top;
                                                    float: left; margin-left: 0px;">
                                            <legend ID="genericLegend0" runat="server" style="margin-bottom: 4px; font-weight: normal;
                                                        color: #808080;">
                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("slabName") %>'></asp:Label>
                                            </legend>
                                            <%--<asp:Label ID="Label2" runat="server"  Text='<%# Bind("slabName") %>'></asp:Label>--%>
                                            <asp:GridView ID="ShowDetails0" runat="server" AllowPaging="True" 
                                                AutoGenerateColumns="False" DataKeyNames="slabId" 
                                                OnRowCommand="ShowDetails_RowCommand" OnRowDataBound="ShowDetails_RowDataBound" 
                                                OnSelectedIndexChanged="ShowDetails_SelectedIndexChanged" Width="450px">
                                                <RowStyle BorderColor="#333333" />
                                                <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                                <Columns>
                                                    <asp:BoundField DataField="slabDetailId">
                                                        <ControlStyle CssClass="hideselect" />
                                                        <FooterStyle CssClass="hideselect" />
                                                        <HeaderStyle CssClass="hideselect" />
                                                        <ItemStyle CssClass="hideselect" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Earnings">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="dgdffdhj1" runat="server" CausesValidation="False" 
                                                                CommandArgument="startRange" CommandName="sort" CssClass="gridlink">Start</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Labgsfggel3" runat="server" Text='<%# Bind("startRange") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Earnings">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="dhfgthf1" runat="server" CausesValidation="False" 
                                                                CommandArgument="endRange" CommandName="sort" CssClass="gridlink">End</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="fdshgjghjhl3" runat="server" Text='<%# Bind("endRange") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Earnings">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="Liaafutton3" runat="server" CausesValidation="False" 
                                                                CommandArgument="contribution" CommandName="sort" CssClass="gridlink">Contribution</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="genericCont0" runat="server" Text='<%# Bind("contribution") %>'></asp:Label>
                                                            <asp:Label ID="genericFixed0" runat="server" CssClass="hideselect" 
                                                                Text='<%# Bind("isFixed") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Earnings">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="dgdfdfffdhj1" runat="server" CausesValidation="False" 
                                                                CommandArgument="wef" CommandName="sort" CssClass="gridlink">Wef</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="genericWef0" runat="server" Text='<%# Bind("wef") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                  
                                                    <asp:TemplateField HeaderText="Earnings">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="dgdfddfgklk1" runat="server" CausesValidation="False" 
                                                                CommandArgument="forTheMonth" CommandName="sort" CssClass="gridlink">Month</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="genericMonth0" runat="server" Text='<%# Bind("forTheMonth") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Earnings">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="dggdhghsafdhj1" runat="server" CausesValidation="False" 
                                                                CommandArgument="applicableOn" CommandName="sort" CssClass="gridlink">Applicable On</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="genericApplyOn0" runat="server" 
                                                                Text='<%# Bind("applicableOn") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Earnings">
                                                        <ControlStyle CssClass="hideselect" />
                                                        <FooterStyle CssClass="hideselect" />
                                                        <HeaderStyle CssClass="hideselect" />
                                                        <ItemStyle CssClass="hideselect" />
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="Liafdf32sdfsdf54tton3" runat="server" 
                                                                CausesValidation="False" CommandArgument="isFixed" CommandName="sort" 
                                                                CssClass="hideselect">Fixed/Per</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="fixedEsic0" runat="server" Text='<%# Bind("isFixed") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:CommandField ShowSelectButton="True">
                                                        <ControlStyle CssClass="hideselect" />
                                                        <FooterStyle CssClass="hideselect" />
                                                        <HeaderStyle CssClass="hideselect" />
                                                        <ItemStyle CssClass="hideselect" />
                                                    </asp:CommandField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" 
                                                                CommandName="deleteIt" Height="16px" ImageUrl="~/images/delete.ico" 
                                                                OnClientClick="this.Parent.style.display='none'; return confirm('Are you sure you want to delete the record?'); //ParentNode.removeChild(this);" 
                                                                Width="16px" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="16px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    Details not Added...
                                                </EmptyDataTemplate>
                                                <HeaderStyle BackColor="#959595" Font-Names="Tw Cen MT Condensed" 
                                                    Font-Size="17px" ForeColor="White" Height="19px" HorizontalAlign="Left" />
                                            </asp:GridView>
                                        </fieldset></div>
                                    <%--  </fieldset> --%>
                                </fieldset>
                            </div>
                            <div id="div1">
                                <asp:Button ID="fireEvent" Style="display: none" OnClick="EvenHandler" runat="server"
                                    Text="Button" />
                                   <asp:HiddenField ID="hdfSlabDetId" runat="server" />
                                <script language="javascript">
                                    function HandleEvent(slabId) {
                                        var t = document.getElementById('<%= fireEvent.ClientID %>');
                                        t.value = slabId;
                                        document.getElementById('<%= hdfSlabDetId.ClientID %>').value = slabId;
                                        
                                        t.click();
                                        //alert(t.value);
                                    }
    
                                </script>

                            </div>
                            <%-- </fieldset>--%>
                            <fieldset style="padding: 5px;">
                                <legend style="margin-bottom: 2px; font-weight: normal; color: #808080;"></legend>
                                <table style="margin: 10px 0px 0px 0px;">
                                    <tr>
                                        <td style="text-align: right;">
                                            Deduction For :&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            &nbsp;<asp:DropDownList ID="deductFor" runat="server">
                                                <asp:ListItem Value="0">Labour Welfare</asp:ListItem>
                                                <asp:ListItem Value="1">Providend Fund</asp:ListItem>
                                                <asp:ListItem Value="2">ESIC</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 145px; text-align: right;">
                                            Wef :&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td class="style1">
                                            <table style="width: 402px">
                                                <tr>
                                                    <td style="width: 150px">
                                                        <asp:TextBox ID="wef" runat="server"></asp:TextBox>
                                                        <asp:CalendarExtender ID="wef_CalendarExtender" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                            TargetControlID="wef">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                    <td style="text-align: right;">
                                                        &nbsp;</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td style="width: 145px; text-align: right;">
                                            Start Range :&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td class="style1">
                                            <asp:TextBox ID="startRange" runat="server" Width="180px"></asp:TextBox>
                                            <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                    ControlToValidate="startRange" 
                                    ErrorMessage="Bonus name only includes digits" 
                                    ValidationExpression="[0-9]"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="startRange"
                                    ErrorMessage="Please enter bank name" CssClass="hideselect"></asp:RequiredFieldValidator>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 145px; text-align: right;">
                                            End Range :&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td class="style1">
                                            <asp:TextBox ID="endRange" runat="server" Width="180px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table style="width: 70%; height: 100%;">
                                                <tr>
                                                    <td style="width: 25%; text-align: center">
                                                        <asp:RadioButton GroupName="citizenTypes" Text="Gross" ID="gross" runat="server"
                                                            Checked="true" />
                                                    </td>
                                                    <td style="width: 15%; text-align: center">
                                                        <asp:RadioButton GroupName="citizenTypes" ID="basic" Text="Basic" runat="server" />
                                                    </td>
                                                    <td style="width: 30%; text-align: center">
                                                        <asp:RadioButton GroupName="citizenTypes" ID="NA" Text="NA" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            Deductible as :&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="deductAs" runat="server">
                                                <asp:ListItem Value="0">Percentage</asp:ListItem>
                                                <asp:ListItem Value="1" Selected >Fixed</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td style="width: 145px; text-align: right;">
                                            Contribution : <span class="style2">* </span>
                                        </td>
                                        <td class="style1">
                                            <asp:TextBox ID="contribution" runat="server" Width="180px"></asp:TextBox>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 145px;">
                                            &nbsp;
                                        </td>
                                        <td class="style1">
                                            <asp:Button ID="ins" runat="server" CssClass="button" Font-Bold="False" ForeColor="#333333"
                                                OnClick="ins_Click" Text="Save" />
                                            &nbsp;
                                            <asp:Button ID="cncl" runat="server" CausesValidation="False" CssClass="button" Font-Bold="False"
                                                ForeColor="#333333" OnClick="cncl_Click" Text="Reset" Visible="False" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
                                    ShowMessageBox="True" ShowSummary="False" />
                            </fieldset>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
