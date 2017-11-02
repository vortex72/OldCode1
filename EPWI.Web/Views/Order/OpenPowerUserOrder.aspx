<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	OpenPowerUserOrder
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Open Power User Order</h2>
    <% if ((bool) ViewData["KitInProgress"]) { %>
        <p class="error">Warning: Starting a power user order will delete the <%= Html.ActionLink("kit", "Edit", "Kit") %> that is currently in progress.</p>
    <% } %>
    <% Html.RenderPartial("PowerUserAddress"); %>    
    <br/>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FirstScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideBarContent" runat="server">
</asp:Content>
