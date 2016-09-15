<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="HR_test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:GridView ID="grid" runat="server" AutoGenerateColumns="False" AllowPaging="true" PageSize="5"
            onrowcommand="grid_RowCommand" onrowdatabound="grid_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Name">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("deptName") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <HeaderTemplate>
                        <asp:LinkButton ID="nameBtn" runat="server" CausesValidation="False" 
                            CommandArgument="deptName" CommandName="sort">Name</asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("deptName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="LastAction">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("LastAction") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lasstBtn" runat="server" CausesValidation="False" 
                            CommandArgument="LastAction" CommandName="sort">LAst Action</asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("LastAction") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    
    </div>
                                                  <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" 
        DataSourceID="years" DataTextField="wef" DataValueField="taxMasterId" 
        ondatabound="DropDownList1_DataBound">
                                                  </asp:DropDownList>
                                              <asp:ObjectDataSource ID="years" 
        runat="server" OldValuesParameterFormatString="original_{0}" 
        SelectMethod="GetAllDs" TypeName="VCM.EMS.Biz.TaxMaster">
    </asp:ObjectDataSource>
    </form>
</body>
</html>
