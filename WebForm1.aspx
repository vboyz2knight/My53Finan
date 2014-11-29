<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="MyWeb.CSharp.My53Finan.WebForm1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

<style>
    

    .modalBackground {
        background: #333;
        filter:alpha(opacity = 70);
        opacity: 0.7;
        }
        

    #modalPopup {
        background: url(App_Gfx/PopupBackground.gif);
        width: 308px;
        height: 209px;
        padding: 10px 10px 10px 15px;
        font: 13px Arial;
        }
                

    

    </style>
    



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


                <!-- Here I test with a linkbutton, this button is the targetID for the ModalPopupExtender -->
        <asp:Button ID="Button1" runat="server" Text="CLick me" onclick="Button1_Click1"/>
    

        <asp:Panel ID="modalPopup" style="display: none;" CssClass="panel" runat="server">
        <asp:Button runat="server" ID="HiddenForModal" style="display: none" />


        <asp:Label runat="server" ID="Label"></asp:Label>
        

        <asp:LinkButton ID="btnSend" runat="server">Send</asp:LinkButton>
        <asp:LinkButton ID="btnClose" runat="server">Close</asp:LinkButton>
                        

        </asp:Panel>
    

        <asp:ModalPopupExtender ID="ModalPopupExtender1"  BehaviorID="ModalPopupExtender1"
        TargetControlID="HiddenForModal" OkControlID="btnSend" CancelControlID="btnClose" PopupControlID="modalPopup" 
        BackgroundCssClass="modalBackground"  runat="server">
        </asp:ModalPopupExtender>



</asp:Content>
