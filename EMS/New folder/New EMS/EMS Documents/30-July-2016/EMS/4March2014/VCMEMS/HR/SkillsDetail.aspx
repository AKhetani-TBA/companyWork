<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SkillsDetail.aspx.cs" Inherits="HR_SkillsDetail" EnableEventValidation="true"   Async="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <title></title>

    <script src="../js/jquery.js" type="text/javascript"></script>

    <script src="../js/ajax.js" type="text/javascript"></script>

    <link href="../css/ems.css" rel="stylesheet" type="text/css" />
</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"   EnablePartialRendering="true" >
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="EMS_font">
                <br />
                <table id="Skill" runat="server">
                    <tr>
                        <td>
                            Department :
                        </td>
                        <td>
                            <asp:DropDownList ID="showDepartments" runat="server" AutoPostBack="True" Width="168px"
                                OnSelectedIndexChanged="showDepartments_SelectedIndexChanged" CssClass="ddl">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Employee :
                        </td>
                        <td>
                            <asp:DropDownList ID="showEmployees" runat="server" AutoPostBack="True" OnSelectedIndexChanged="showEmployees_SelectedIndexChanged"
                                Width="180px" CssClass="ddl">
                            </asp:DropDownList>
                        </td>
                         <td>
                    <asp:ImageButton ID="btnsearch" runat="server" ImageUrl="~/images/searchbtn.png"
                        OnClick="btnsearch_Click" CausesValidation="False" />
                </td>
                        <td>
                            <asp:Button ID="btnAdd" runat="server" Text="Add Skill" 
                                CausesValidation="False" onclick="btnAdd_Click"   />
                        </td>
                          <td > &nbsp;&nbsp;&nbsp;
                                  <asp:ImageButton ID="btnexcel" runat="server" ImageUrl="~/images/excelicon.png" Width="23px"   
                                        OnClick="btnexcel_Click" /><br />
                                </td>     
                    </tr>
                </table>
                <div id="addSkilldiv" runat="server">
                    <fieldset style="padding: 5px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Bank Details
                        </legend>
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 8%" align="left" >
                                    Employee :
                                </td>
                                <td style="width: 92%" align="left">
                                    <asp:DropDownList ID="ddlEmp" runat="server" AutoPostBack="false" Width="180px" CssClass="ddl">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvEmployeeName" runat="server" ErrorMessage="Please select employee name."
                                        InitialValue="0" ControlToValidate="ddlEmp" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Skills <span style="color: #FF0000">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSkill" runat="server" Width="650px" Height="75px" TextMode="MultiLine"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtSkill"
                                        CssClass="hideselect" ErrorMessage="Please enter skills"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                            <td>
                            &nbsp;
                            </td>
                                <td align="left">
                                    <asp:Button ID="btnSave" runat="server" Font-Bold="False" Font-Names="Tw Cen MT Condensed"
                                        Font-Size="18px" ForeColor="#333333" Height="28px" Text="Save" 
                                        onclick="btnSave_Click" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="cncl" runat="server" CausesValidation="False" Font-Names="Tw Cen MT Condensed"
                                        Font-Size="18px" ForeColor="#333333" Height="28px" OnClick="cncl_Click" Text="Cancel" />
                                    &nbsp;
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                                        ShowSummary="False" DisplayMode="List" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div id="DivGrid" runat="server">
                    <fieldset style="padding-top: 5px; padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Search Result
                        </legend>
                        <div style="height: 1050px; width: 100%; overflow-y: auto; overflow-x: auto;">
                            <asp:GridView ID="displayAll" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                OnPageIndexChanging="displayAll_PageIndexChanging" OnRowCommand="displayAll_RowCommand"
                                OnRowDataBound="displayAll_RowDataBound" Width="100%" PageSize="20"  OnSorting="displayAll_OnSorting" AllowSorting="true" EnableModelValidation="true">
                                <RowStyle BorderColor="#333333" />
                                <HeaderStyle BackColor="#959595" CssClass="gridheader" Font-Names="Tw Cen MT Condensed"
                                     ForeColor="White" Font-Bold="false"/>
                                <Columns>                                    
                                   <asp:BoundField DataField="EmpName" HeaderText="Employee Name" ItemStyle-Width="15%" SortExpression="EmpName"/>
                                   <asp:BoundField DataField="SkillName" HeaderText="Skill Name" ItemStyle-Width="75%" /> 
                                   <asp:TemplateField HeaderText="">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False"                                           
                                            CommandArgument='<%# Eval("SkillId")%>'
                                                CommandName="EditSkill" ImageUrl="~/images/edit_btn.png"   />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("SkillId") %>'
                                                CommandName="Deleteskill" ImageUrl="~/images/delete.ico" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                    </asp:TemplateField>                               
                                </Columns>
                                <EmptyDataTemplate>
                                    No skills Details Added...</EmptyDataTemplate>
                                <FooterStyle BackColor="White" ForeColor="#333333" />
                                <%--<SelectedRowStyle BackColor="Silver" ForeColor="Black" />--%>
                            </asp:GridView>
                            <asp:HiddenField ID="hidskillId" runat="server" />
                     <%--   </div>--%>
                    </fieldset>
                </div>
            </div>
        </ContentTemplate>
           <Triggers>
            <asp:PostBackTrigger ControlID="btnexcel" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
