<%@ Page Title="" Language="C#" MasterPageFile="~/Common.Master" AutoEventWireup="true" CodeBehind="sto.aspx.cs" Inherits="Administration.sto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/StyleSheet.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="width: 100%; margin: 0px auto; text-align: center;">
        <div style="text-align: left; font-family: Verdana, Calibri, 'Segoe UI'; font-size: 11px; width: 90%;">
            <br />
            <br />
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
            <br />
            <br />
            <asp:Panel ID="pnlAutoUrlInfo" runat="server" Width="100%">
                <table style="width: 100%;">
                    <thead>
                        <tr>
                            <th style="width: 15%;"></th>
                            <%--<th style="width: 5%;">
                                        </th>--%>
                            <th style="width: 85%;"></th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td style="text-align: left;">URL:
                            </td>
                            <td style="text-align: left;">
                                <asp:Label ID="lblUrl" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">User:
                            </td>
                            <td style="text-align: left;">
                                <asp:Label ID="lblUser" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">URL Sent By:
                            </td>
                            <td style="text-align: left;">
                                <asp:Label ID="lblSenderInfo" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="lblValidityPeriodTitle" runat="server"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:Label ID="lblValidityPeriod" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; padding: 15px 0px;" colspan="3">
                                <asp:Label ID="lblReqAccessMessage" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; padding: 5px 0px 15px 0px;" colspan="2">
                                <%--<asp:TextBox ID="txtRequestAccessMessage" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;--%>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
        </div>
    </div>

</asp:Content>
