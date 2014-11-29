<%@ Page EnableViewState="true" EnableViewStateMac="true" ViewStateEncryptionMode="Always"
    Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="MyFinan53.aspx.cs" Inherits="MyWeb.CSharp.My53Finan.MyFinan53" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style3
        {
            height: 21px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="IntroductionPanel" runat="server" Width="603px">
        <p>
            This program will upload an <strong>*.csv</strong> file that have your monthly spendings.&nbsp;
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
    <asp:Panel ID="ViewPanel" runat="server">
    
    <div id="EntireTransactinsBarChart">
        <asp:Chart ID="Chart3" runat="server" Width="850px">           
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1">
                </asp:ChartArea>
            </ChartAreas>
            <Titles>
                <asp:Title Name="Spending History">
                </asp:Title>
            </Titles>
        </asp:Chart>
    </div>

    <div id="Navigations">
        <asp:Button ID="RecentMonth" runat="server" Text="Latest Month" 
            onclick="RecentMonth_Click" />
        <asp:Button ID="Latest3Month" runat="server" Text="Latest 3 Months" 
            onclick="Latest3Month_Click" />
        <asp:Button ID="Latest6Month" runat="server" Text="Latest 6 Months" 
            onclick="Latest6Month_Click" />
        <asp:Button ID="Latest12Month" runat="server" Text="Latest 12 Months" 
            onclick="Latest12Month_Click" />
        <fieldset>
        Start Date(mm/dd/yyyy):<asp:TextBox ID="txtStartDate" runat="server" Height="22px" 
            MaxLength="10" Width="70px"></asp:TextBox>
        End Date:<asp:TextBox ID="txtEndDate" runat="server" MaxLength="10" 
            Width="70px"></asp:TextBox>
        <asp:Button ID="bSelectDates" runat="server" Text="Select Dates" 
            onclick="bSelectDates_Click" />
        </fieldset>
        <asp:Label ID="lblErrorGraph" runat="server" CssClass="failureNotification"></asp:Label>

    </div>

        <div id="My_Chart">
            <table style="width: 100%;">
                <tr>
                    <td>
                        <asp:Chart ID="Chart1" runat="server" OnClick="Chart1_Click" Width="460px" >
                            <Series>
                                <asp:Series Name="Series1" ChartType="Pie" IsValueShownAsLabel="true"  IsVisibleInLegend="true">
                                </asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1" >
                                    <Area3DStyle Enable3D="True" />
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
                    </td>
                    <td>
                        <asp:Chart ID="Chart2" runat="server" Width="460px">
                            <Series>
                                <asp:Series Name="Series2" ChartType="Pie" IsValueShownAsLabel="false" IsVisibleInLegend="true" >
                                </asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea2">
                                    <Area3DStyle Enable3D="True" />
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Panel ID="Panel3" runat="server" ScrollBars="Both" Height="300px">
        
        <asp:ListView ID="ListView1" runat="server" >
            <LayoutTemplate>
                <table>
                    <tr>
                        <th align="left">
                            Date
                        </th>
                        <th align="left">
                            Description
                        </th>
                        <th align="left">
                            Check
                        </th>
                        <th align="left">
                            Amount
                        </th>
                        <th align="left">
                            Category
                        </th>
                    </tr>
                    <tr id="itemPlaceholder" runat="server">
                    </tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblDate"><%#Eval("myDate", "{0:M/dd/yyyy}")%></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblDescription"><%#Eval("Description")%></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblCheck"><%#Eval("check")%></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblAmount"><%#Eval("Amt")%></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblCategory"><%#Eval("myCategory")%></asp:Label>
                    </td>
            </ItemTemplate>
        </asp:ListView>

        </asp:Panel>

    </asp:Panel>
    </asp:Content>
