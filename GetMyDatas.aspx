<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GetMyDatas.aspx.cs" Inherits="MyWeb.CSharp.My53Finan.Choice" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style3
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        This program intend to get datas from your yearly spending from either a 
        database or a *.csv file in format Date,Description,&quot;Check Number&quot;,Amount 
        and shows a simple analysis of your spendings.</p>
    <asp:Panel ID="ChoicePanel" runat="server">
        <table class="style3" align="center">
            <tr>
                <td align="center">
                    <asp:Button ID="UseDemo" runat="server" Text="Use demo data" 
                        onclick="UseDemo_Click" style="text-align: center" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="UploadCSV" runat="server" Text="Upload a *.csv file" 
                        onclick="UploadCSV_Click" style="text-align: center" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="UploadCSVPanel" runat="server">
        <p>
            Upload an <strong>*.csv</strong> file that have your monthly spendings.&nbsp;
            File format will be <strong>Date,Description,&quot;Check Number&quot;,Amount</strong>.
            Description is a brief description of the transaction. The program will take the
            informations and display an analysis of your spendings.</p>
        <table style="width: 100%;">
            <tr>
                <td>
                    <asp:Label ID="lblUploadFileCSV" runat="server" Text="Upload Your CSV file: "></asp:Label>
                    <asp:FileUpload ID="FileUpload1" runat="server" type="file" name="fileUpload" />
                    <asp:Button ID="bttnUpload" runat="server" OnClick="bttnUpload_Click" Text="Upload" />
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="text-align: left" class="style3">
                    <asp:Label ID="lblError" runat="server" CssClass="failureNotification"></asp:Label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="FileUpload1"
                        ErrorMessage="Need a file to upload."></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="FileUpload1"
                        ErrorMessage="Only *.csv file allow." ValidationExpression="^.*\.(csv|CSV)$"></asp:RegularExpressionValidator>
                </td>
                <td style="text-align: center" class="style3">
                </td>
            </tr>
        </table>
    </asp:Panel>
    </asp:Content>
