<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CategorizeMe.aspx.cs" Inherits="MyWeb.CSharp.My53Finan.CategorizeMe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">

        .style3
        {
            width: 100%;
        }
        .style5
        {
            text-align: center;
            width: 173px;
        }
        .style4
        {
            text-align: center;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <asp:Panel ID="FilterPanel" runat="server">
        
            <h1>Build Your Filters</h1>
            <table class="style3">
                <tr>
                    <td class="style5">
                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" Height="113px" 
                            style="text-align: left" Width="165px">
                            <asp:ListItem Value="Insurance"></asp:ListItem>
                            <asp:ListItem Value="HomeBill"></asp:ListItem>
                            <asp:ListItem Value="CarBill"></asp:ListItem>
                            <asp:ListItem Value="Restaurant"></asp:ListItem>
                            <asp:ListItem Value="Grocery"></asp:ListItem>
                            <asp:ListItem>Other</asp:ListItem>
                            <asp:ListItem Value="Cable"></asp:ListItem>
                            <asp:ListItem Value="PetBill"></asp:ListItem>
                            <asp:ListItem Value="CommunicationBill"></asp:ListItem>
                            <asp:ListItem>Bank</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:Label ID="lblTransactionDescription" runat="server" 
                            Text="Transaction Description"></asp:Label>
                        <br />
                        <br />
                        <asp:Label ID="Label1" runat="server" 
                            Text="Choose an unique words above to serve as your filter."></asp:Label>
                        <br />
                        <br />
                        <asp:TextBox ID="txtFilter" runat="server" Width="361px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style5">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                            ControlToValidate="RadioButtonList1" 
                            ErrorMessage="Select one of the category above"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                            ControlToValidate="txtFilter" ErrorMessage="Unique Filter Need"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style4" colspan="2">
                        <asp:Button ID="bttnSubmitFilter" runat="server" style="text-align: center" 
                            Text="Submit" onclick="bttnSubmitFilter_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="style4" colspan="2">
                        <asp:Label ID="lblFilterError" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        
        <br />
        
    </asp:Panel>
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
</asp:Content>
