<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Earning.aspx.cs" Inherits="HR_Earning"
    EnableEventValidation="false" %>

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
        .style3
        {
            width: 259px;
        }
        .style4
        {
            color: #006699;
        }
        .style5
        {
            width: 68px;
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
                <asp:PostBackTrigger ControlID="ins" />
            </Triggers>
            <ContentTemplate>
                <div><asp:HiddenField ID="hideValue"  runat="server" />
                <asp:HiddenField ID="hideEmpId"  runat="server" />
                 
                      <asp:HiddenField ID="hideGross"  runat="server" />
                    <div>
                        <div>
                            <%-- <fieldset style="padding: 5px;">
                    <legend style="margin-bottom: 2px; font-weight: normal; color: #808080;">Search </legend>--%>
                            <fieldset style="padding: 2px;">
                                <div>
                                    <table>
                                        <tr>
                                            <td class="style5">
                                                Employee : </td>
                                            <td class="style3">
                                                <asp:Label ID="lblempname" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style5">
                                                Gross :
                                            </td>
                                            <td class="style3">
                                                <asp:Label ID="grossLabel" ClientIDMode="Static" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        
                                    </table>
                                </div>
                            </fieldset>
                           
                                <fieldset style="padding: 5px; padding-left: 6px; height: auto;">
                                    <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Earnings</legend>
                                    <%--    <fieldset style="padding: 5px; width :340px;float:left;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;float: left">ESIC</legend>--%>
                                    <div style="float:left;text-align: left; vertical-align: top; width: 470px; height: 100%;">
                                        <%-- <fieldset style="padding: 5px; margin-right: 5px; width: 450px; height: auto; vertical-align: top;
                                                    float: left; margin-left: 0px;">
                                            <legend ID="genericLegend0" runat="server" style="margin-bottom: 4px; font-weight: normal;
                                                        color: #808080;">
                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("slabName") %>'></asp:Label>--%>
                                        </legend>
                                        <%--<asp:Label ID="Label2" runat="server"  Text='<%# Bind("slabName") %>'></asp:Label>--%>
                                        <asp:GridView ID="ShowEarning" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            DataKeyNames="slabId" OnRowCommand="ShowEarning_RowCommand" OnRowDataBound="ShowEarning_RowDataBound"
                                            OnSelectedIndexChanged="ShowEarning_SelectedIndexChanged" Width="450px" 
                                            PageSize="25">
                                            <RowStyle BorderColor="#333333" />
                                            <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                            <Columns>
                                                <asp:BoundField DataField="earningId">
                                                    <ControlStyle CssClass="hideselect" />
                                                    <FooterStyle CssClass="hideselect" />
                                                    <HeaderStyle CssClass="hideselect" />
                                                    <ItemStyle CssClass="hideselect" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="packageId">
                                                    <ControlStyle CssClass="hideselect" />
                                                    <FooterStyle CssClass="hideselect" />
                                                    <HeaderStyle CssClass="hideselect" />
                                                    <ItemStyle CssClass="hideselect" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="slabDetailId">
                                                    <ControlStyle CssClass="hideselect" />
                                                    <FooterStyle CssClass="hideselect" />
                                                    <HeaderStyle CssClass="hideselect" />
                                                    <ItemStyle CssClass="hideselect" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Fixed/Per">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="dgdffdhj1" runat="server" CausesValidation="False" CommandArgument="slabName"
                                                            CommandName="sort" CssClass="gridlink">Earnings</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="slabNameLabel" runat="server" Text='<%# Bind("slabName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                  <%--
                                                <asp:TemplateField HeaderText="Earnings">
                                                    <ControlStyle CssClass="hideselect" />
                                                    <FooterStyle CssClass="hideselect" />
                                                    <HeaderStyle CssClass="hideselect" />
                                                    <ItemStyle CssClass="hideselect" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="dhfdfdfgthf1" runat="server" CausesValidation="False" CommandArgument="startRange"
                                                            CommandName="sort" CssClass="gridlink">Start</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="startLabel" runat="server" Text='<%# Bind("startRange") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Earnings">
                                                    <ControlStyle CssClass="hideselect" />
                                                    <FooterStyle CssClass="hideselect" />
                                                    <HeaderStyle CssClass="hideselect" />
                                                    <ItemStyle CssClass="hideselect" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="dhfgtdffdghf1" runat="server" CausesValidation="False" CommandArgument="endRange"
                                                            CommandName="sort" CssClass="gridlink">End</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="endLabel" runat="server" Text='<%# Bind("endRange") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                             
                                                <asp:TemplateField HeaderText="Earnings">
                                                    <ControlStyle CssClass="hideselect" />
                                                    <FooterStyle CssClass="hideselect" />
                                                    <HeaderStyle CssClass="hideselect" />
                                                    <ItemStyle CssClass="hideselect" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="dgdfddfgklk1" runat="server" CausesValidation="False" CommandArgument="forTheMonth"
                                                            CommandName="sort" CssClass="gridlink">Month</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="genericMonth" runat="server" Text='<%# Bind("forTheMonth") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="rupiya" HeaderText="Rs." />  
                                                  
                                                <asp:TemplateField HeaderText="Earnings">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="dggdhghsafdhj1" runat="server" CausesValidation="False" CommandArgument="applicableOn"
                                                            CommandName="sort" CssClass="gridlink">Applicable On</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="applicableOnLabel" runat="server" Text='<%# Bind("applicableOn") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Earnings">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="Liaafutton3" runat="server" CausesValidation="False" CommandArgument="contribution"
                                                            CommandName="sort" CssClass="gridlink">Percentage</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="contributionLabel" runat="server" Text='<%# Bind("contribution") %>'></asp:Label>
                                                        <asp:Label ID="isFixedLabel" runat="server" CssClass="hideselect" Text='<%# Bind("isFixed") %>'></asp:Label>
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
                                                        <asp:ImageButton ID="deldfId" runat="server" CausesValidation="False" CommandName="delId"
                                                            Height="16px" ImageUrl="~/images/delete.ico" Width="16px" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="16px" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                Details not Added...
                                            </EmptyDataTemplate>
                                            <HeaderStyle BackColor="#959595" Font-Names="Tw Cen MT Condensed" Font-Size="17px"
                                                ForeColor="White" Height="19px" HorizontalAlign="Left" />
                                        </asp:GridView>
                                    </div>
                                    <div style="padding:5px 5px 5px 5px;float:left;width:200px;height:100px;background-color:#FFF8DC;border:1px solid grey">
                                        <b><span class="style4">Gross Amount Remaining:</span><br class="style4" />
                                        <br />
                                        <div id="getAmount"></div>
                                        <div id="flagAmount" style="display:none;"></div>
                                        </b>
                                    </div>
                                    <%--  </fieldset> --%>
                                </fieldset>
                            
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
                            <fieldset style="padding: 5px; margin-top:5px">
                               
                                <table style="margin: 10px 0px 0px 0px;">
                                    <tr>
                                        <td style="text-align: right;">
                                            Earning :&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            &nbsp;<asp:DropDownList ID="deductFor" runat="server" OnSelectedIndexChanged="deductFor_SelectedIndexChanged">
                                                <asp:ListItem Value="0">Labour Welfare</asp:ListItem>
                                                <asp:ListItem Value="1">Providend Fund</asp:ListItem>
                                                <asp:ListItem Value="2">ESIC</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                      <tr>
                                        <td style="width: 145px; text-align: right;">
                                            &nbsp;</td>
                                        <td class="style1">
                                            <asp:CheckBox ID="isConditionCheck" runat="server" 
                                                Text="Apply condition for Gross &gt; " AutoPostBack="True" 
                                                oncheckedchanged="isConditionCheck_CheckedChanged" />
                                            <asp:TextBox ID="CondtionAmount" runat="server" Width="180px" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 145px; text-align: right;">
                                            Lower Limit&nbsp;&nbsp;
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
                                            Upper Limit&nbsp;&nbsp;
                                        </td>
                                        <td class="style1">
                                            <asp:TextBox ID="endRange" runat="server" Width="180px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            Applicable On :&nbsp;&nbsp;</td>
                                        <td>
                                            <asp:RadioButton ID="gross" runat="server" Checked="true" 
                                                GroupName="citizenTypes" Text="Gross" />
                                            &nbsp;&nbsp;
                                            <asp:RadioButton ID="NA" runat="server" GroupName="citizenTypes" Text="NA" />
                                            &nbsp;&nbsp;
                                            <asp:RadioButton ID="basic" runat="server" GroupName="citizenTypes" 
                                                Text="Basic" />
                                        </td>
                                    </tr>
                                    
                                  
                                    <tr>
                                        <td style="text-align: right">
                                            Deductible as :&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <asp:DropDownList ClientIDMode="Static" ID="deductAs" runat="server" onChange="getGrossRemaining(document.getElementById('deductFor').value,document.getElementById('contribution').value,this.value,document.getElementById('hideValue').value,document.getElementById('grossLabel').innerHTML);">
                                                <asp:ListItem Value="0">Percentage</asp:ListItem>
                                                <asp:ListItem Value="1">Fixed</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hidenValue" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 145px; text-align: right;">
                                            Contribution :&nbsp;&nbsp; <span class="style2">&nbsp;</span>
                                        </td>
                                        <td class="style1">
                                            <asp:TextBox ID="contribution" ClientIDMode="Static" runat="server" Width="180px" onKeyUp="getGrossRemaining(document.getElementById('deductFor').value,this.value,document.getElementById('deductAs').value,document.getElementById('hideValue').value,document.getElementById('grossLabel').innerHTML,document.getElementById('hideEmpId').value);"></asp:TextBox>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 145px;">
                                            &nbsp;
                                        </td>
                                        <td class="style1">
                                            <asp:Button ID="ins" runat="server" CssClass="button" Font-Bold="False" ForeColor="#333333"
                                                OnClick="ins_Click" Text="Save" OnClientClick="if(parseInt(document.getElementById('flagAmount').innerHTML)<0){alert('Amount Exceeded than Gross');return false;}" />
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
