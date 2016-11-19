<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeductionDetail.aspx.cs"
    Inherits="DeductionDetail" EnableEventValidation="false" %>

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
            width: 145px;
            height: 26px;
        }
        .style4
        {
            width: 79%;
            height: 26px;
        }
    </style>
    <script type="text/javascript" language="javascript">
        
    
    </script>
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
                          
                                
                                    <table>
                                        <tr>
                                            <td>
                                                With Effect From Year :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="showYear" runat="server" Width="100px" AutoPostBack="True"
                                                    OnSelectedIndexChanged="showYear_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                
                           
                            <div id="div2">
                               <div>
                                    <div style="text-align: left; vertical-align: top; width: 100%; height: 100%;">
                                        <asp:DataList ID="showDeductions" runat="server" DataKeyField="slabId" OnItemDataBound="showDeductions_ItemDataBound"
                                            RepeatColumns="2" RepeatDirection="Horizontal" 
                                            OnItemCommand="showDeductions_ItemCommand" 
                                            onselectedindexchanged="showDeductions_SelectedIndexChanged">
                                            <ItemTemplate>
                                                <fieldset style="padding: 5px; margin-right: 5px; width: 450px; vertical-align: top;
                                                    float: left; margin-left: 0px;">
                                                    <legend runat="server" id="genericLegend" style="margin-bottom: 4px; font-weight: normal;
                                                        color: #808080;">
                                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("slabName") %>'></asp:Label>
                                                    </legend>
                                                    <%--<asp:Label ID="Label2" runat="server"  Text='<%# Bind("slabName") %>'></asp:Label>--%>
                                                    <asp:GridView ID="ShowDetails" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                        Width="450px" OnRowCommand="ShowDetails_RowCommand" OnRowDataBound="ShowDetails_RowDataBound"
                                                        DataKeyNames="slabId" OnSelectedIndexChanged="ShowDetails_SelectedIndexChanged">
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
                                                                    <asp:LinkButton ID="dgdffdhj0" runat="server" CausesValidation="False" CommandArgument="startRange"
                                                                        CommandName="sort" CssClass="gridlink">Start</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="startRangeLabel" runat="server" Text='<%# Bind("startRange") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Earnings">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="dhfgthf0" runat="server" CausesValidation="False" CommandArgument="endRange"
                                                                        CommandName="sort" CssClass="gridlink">End</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="endRangeLabel" runat="server" Text='<%# Bind("endRange") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Cont.">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="Liaafutton2" runat="server" CausesValidation="False" CommandArgument="contribution"
                                                                        CommandName="sort" CssClass="gridlink">Cont.</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="genericCont" runat="server" Text='<%# Bind("contribution") %>'></asp:Label>
                                                                    <asp:Label CssClass="hideselect" ID="genericFixed" runat="server" Text='<%# Bind("isFixed") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Earnings">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="dgdfdfffdhj0" runat="server" CausesValidation="False" CommandArgument="wef"
                                                                        CommandName="sort" CssClass="gridlink">Wef</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="genericWef" runat="server" Text='<%# Bind("wef") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                           
                                                            <asp:TemplateField HeaderText="Earnings">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="dgdfddfgklk0" runat="server" CausesValidation="False" CommandArgument="forTheMonth"
                                                                        CommandName="sort" CssClass="gridlink">Month</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="genericMonth" runat="server" Text='<%# Bind("forTheMonth") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Earnings">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="dggdhghsafdhj0" runat="server" CausesValidation="False" CommandArgument="applicableOn"
                                                                        CommandName="sort" CssClass="gridlink">On</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="genericApplyOn" runat="server" Text='<%# Bind("applicableOn") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Earnings">
                                                                <ControlStyle CssClass="hideselect" />
                                                                <FooterStyle CssClass="hideselect" />
                                                                <HeaderStyle CssClass="hideselect" />
                                                                <ItemStyle CssClass="hideselect" />
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="Liafdf32sdfsdf54tton2" runat="server" CausesValidation="False"
                                                                        CommandArgument="isFixed" CommandName="sort" CssClass="hideselect">Fixed/Per</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="fixedEsic" runat="server" Text='<%# Bind("isFixed") %>'></asp:Label>
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
                                                                    <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="deleteIt"
                                                                        Height="16px" ImageUrl="~/images/delete.ico" OnClientClick=" return confirm('Are you sure you want to delete the record?'); changeContent('DeductionDetail.aspx');"
                                                                        Width="16px" />
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
                                                </fieldset>
                                            </ItemTemplate>
                                            
                                        </asp:DataList>
                                    </div>
                                  </div>
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
                           
                            <fieldset style="padding: 5px; margin-top:5px;">
                              
                                <table style="margin: 10px 0px 0px 0px;">
                                    <tr>
                                        <td style="text-align: right;">
                                            Deduction For
                                        </td>
                                        <td>
                                            &nbsp;<asp:DropDownList ID="deductFor" runat="server" CssClass="ddl">
                                                <asp:ListItem Value="0">Labour Welfare</asp:ListItem>
                                                <asp:ListItem Value="1">Providend Fund</asp:ListItem>
                                                <asp:ListItem Value="2">ESIC</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 145px; text-align: right;">
                                            Wef
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
                                   <%-- <tr>
                                        <td style="text-align: right">
                                            Contribution From
                                        </td>
                                        <td style="margin-left: 40px">
                                            <asp:DropDownList runat="server" ID="contributionFor" CssClass="ddl">
                                                <asp:ListItem Value="0">Employee</asp:ListItem>
                                                <asp:ListItem Value="1">Employer</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td style="width: 145px; text-align: right;">
                                            Start Range
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
                                        <td style="text-align: right;" class="style3">
                                            End Range
                                        </td>
                                        <td class="style4">
                                            <asp:TextBox ID="endRange" runat="server" Width="180px"></asp:TextBox>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td style="width: 145px; text-align: right;">
                                            Contribution from&nbsp;&nbsp;&nbsp; </td>
                                        <td class="style1">
                                            <asp:RadioButton ID="gross" runat="server" Checked="true" 
                                                GroupName="citizenTypes" Text="Gross" />
                                            &nbsp;&nbsp;
                                            <asp:RadioButton ID="basic" runat="server" GroupName="citizenTypes" 
                                                oncheckedchanged="basic_CheckedChanged" Text="Basic" />
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:RadioButton ID="NA" runat="server" GroupName="citizenTypes" Text="NA" />
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td style="text-align: right">
                                            Deductible as
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="deductAs" runat="server" CssClass="ddl" onchange="if(this.value=='0') document.getElementById('RsPer').innerHTML = '%'; else document.getElementById('RsPer').innerHTML = 'Rs';">
                                                <asp:ListItem Value="0">Percentage</asp:ListItem>
                                                <asp:ListItem Value="1">Fixed</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td style="width: 145px; text-align: right;">
                                            Contribution <span class="style2">* </span>
                                        </td>
                                        <td class="style1">
                                            <asp:TextBox ID="contribution" runat="server" Width="180px"></asp:TextBox>
                                            &nbsp;
                                            <span runat="server" id="RsPer">%</span>
                                            </td>
                                    </tr>
                                   
                                     <tr>
                                        <td style="text-align: right">
                                            At Month&nbsp;
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="months" runat="server" CssClass="ddl">
                                                <asp:ListItem Value="0">Monthly</asp:ListItem>
                                                <asp:ListItem Value="1">Jan</asp:ListItem>
                                                <asp:ListItem Value="2">Feb</asp:ListItem>
                                                <asp:ListItem Value="3">Mar</asp:ListItem>
                                                <asp:ListItem Value="4">Apr</asp:ListItem>
                                                <asp:ListItem Value="5">May</asp:ListItem>
                                                <asp:ListItem Value="6">Jun</asp:ListItem>
                                                <asp:ListItem Value="7">Jul</asp:ListItem>
                                                <asp:ListItem Value="8">Aug</asp:ListItem>
                                                <asp:ListItem Value="9">Sep</asp:ListItem>
                                                <asp:ListItem Value="10">Oct</asp:ListItem>
                                                <asp:ListItem Value="11">nov</asp:ListItem>
                                                <asp:ListItem Value="12">Dec</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 145px;text-align:right">
                                            &nbsp;
                                            <asp:Button ID="ins" runat="server" CssClass="button" Font-Bold="False" 
                                                ForeColor="#333333" OnClick="ins_Click" Text="Save" />
                                        </td>
                                        <td class="style1">
                                            &nbsp;<asp:Button ID="cncl" runat="server" CausesValidation="False" CssClass="button" Font-Bold="False"
                                                ForeColor="#333333" OnClick="cncl_Click" Text="Reset"  />
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
