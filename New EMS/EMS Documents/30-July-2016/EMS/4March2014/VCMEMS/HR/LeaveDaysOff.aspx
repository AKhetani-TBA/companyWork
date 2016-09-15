<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeaveDaysOff.aspx.cs" Inherits="HR_LeaveDaysOff" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/ajax.js" type="text/javascript"></script>
    <style type="text/css">


        .style1
        {
            width: 728px;
            height: 100px;
        }
        .style2
        {
            width: 112px;
        }
        .style3
        {
            color: #FF0066;
        }
        </style>
</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    <div class="EMS_font">
    
         <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
     <ContentTemplate>
            <div>
             <br />
             <fieldset style="padding: 5px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Add/Update 
                            Holidays
                        </legend>
                    
                <table class="style1">
                    <tr>
                        <td class="style2">
                            Festival/Holiday&nbsp; <span class="style3">*</span></td>
                        <td>
                            <asp:TextBox ClientIDMode="Static" ID="HoidayName" runat="server" 
                                ></asp:TextBox>
                            
                            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ControlToValidate="HoidayName" CssClass="hideselect" 
                                ErrorMessage="Please specify a holiday"></asp:RequiredFieldValidator>
                            &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                ControlToValidate="HoidayName" 
                                ErrorMessage="It can only include aplhabets and underscore" 
                                ValidationExpression="^[a-zA-Z_ ]+$"></asp:RegularExpressionValidator>
                            
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            Type<span class="style3"> *</span></td>
                        <td  >
                       
                            <asp:RadioButton ID="isNational" runat="server"  Text="National" Checked="True" 
                                GroupName="holidayType" />
                            <asp:RadioButton ID="isOptional" runat="server"  Text="Optional" 
                                GroupName="holidayType"/>
                     
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            &nbsp;</td>
                        <td>
                            <asp:CheckBox ID="isPerpetual" runat="server" Text="Perpetual" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            Date <span class="style3">*</span></td>
                        <td>
                            <asp:TextBox ID="HolidayDate" runat="server" ></asp:TextBox>
                               
                            <asp:CalendarExtender ID="HolidayDate_CalendarExtender" runat="server" 
                                Enabled="True" Format="dd MMMM yyyy" TargetControlID="HolidayDate">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            </td>
                        <td>
                            <asp:Button ID="saveBtn" runat="server" CssClass="button" 
                                onclick="saveBtn_Click" Text="Save"  />
                            &nbsp;<asp:Button ID="resetBtn" runat="server"  onclick="resetBtn_Click" 
                                Text="Reset" CausesValidation="False" CssClass="button"  />
                          
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                    ShowMessageBox="True" ShowSummary="False" DisplayMode="List" />
                </fieldset>
                </div>
                
                
                   <div>
               <fieldset style="padding-top: 5px; padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Holidays
                        </legend>
                        <div style="margin:10px; height: 18px;">
                        <asp:DropDownList ID="showYear" runat="server" CssClass="ddl"  
                                OnSelectedIndexChanged="showDepartments_SelectedIndexChanged" Width="100px">
                            </asp:DropDownList>
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnview" runat="server" CssClass="button" Text="View" 
                                onclick="btnview_Click" CausesValidation="False" />
                           </div>
                        <asp:GridView ID="HolidayDetail" runat="server" AllowPaging="True" 
                            AutoGenerateColumns="False" HorizontalAlign="Justify" 
                            onpageindexchanging="HolidayDetail_PageIndexChanging" 
                            onrowcommand="HolidayDetail_RowCommand" 
                            onrowdatabound="HolidayDetail_RowDataBound" 
                            onselectedindexchanged="HolidayDetail_SelectedIndexChanged" Width="500px">
                            <RowStyle BorderColor="#333333" />
                            <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                            <HeaderStyle BackColor="#959595" Font-Names="Tw Cen MT Condensed" 
                                Font-Size="17px" ForeColor="White" Height="19px" />
                            <Columns>
                                <asp:BoundField DataField="HolidayId" HeaderText="HolidayId" >
                                    <ControlStyle CssClass="hideselect" />
                                    <FooterStyle CssClass="hideselect" />
                                    <HeaderStyle CssClass="hideselect" />
                                    <ItemStyle CssClass="hideselect" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Holiday Name">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                                            CommandArgument="HolidayName" CommandName="sort" CssClass="gridlink">Holiday Name</asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("HolidayName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("HolidayName") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dated on">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                                            CommandArgument="HolidayDate" CommandName="sort" CssClass="gridlink">Dated on</asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("HolidayDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("HolidayDate") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Type">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" 
                                            CommandArgument="isOptional" CommandName="sort" CssClass="gridlink">Type</asp:LinkButton>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("isOptional") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("isOptional") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Perpetual">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="LinkButt" runat="server" CausesValidation="False" 
                                            CommandArgument="LastAction" CommandName="sort" CssClass="gridlink">Perpetual</asp:LinkButton>
                                    </HeaderTemplate>
                                  
                                    <ItemTemplate>
                                        <asp:Label ID="lblperpetual" runat="server" Text='<%# Bind("LastAction") %>'></asp:Label>
                                    </ItemTemplate>
                                     <ItemStyle Width="50px" />
                                </asp:TemplateField>
                                <asp:CommandField ShowSelectButton="True">
                                    <ControlStyle CssClass="hideselect" />
                                    <FooterStyle CssClass="hideselect" />
                                    <HeaderStyle CssClass="hideselect" />
                                    <ItemStyle CssClass="hideselect" />
                                </asp:CommandField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="delCard0" runat="server" ImageUrl="~/images/delete.ico" 
                                            onclientclick="return confirm('Are you sure you want to delete?');" 
                                            Width="16px" />
                                    </ItemTemplate>
                                    <ItemStyle Width="16px" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                Holidays not&nbsp; added...
                            </EmptyDataTemplate>
                        </asp:GridView>
                       
                       
                            
                           
                            
                        
                </fieldset>
                <br />
                </div>
    </ContentTemplate>
     </asp:UpdatePanel>
    </div>
            
    </form>
</body>
</html>
