<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Account Information
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Account Information</h2>
    
    <div>
        <b>Account Information is Currently Unavailable.</b><br>
        We apologize for the inconvenience.  Please try again later.
        <% if (ViewData["ErrorDescription"] != null) { %><div>Error Description: <% = Html.Encode(ViewData["ErrorDescription"]) %></div><% } %>
    </div>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideBarContent" runat="server">
</asp:Content>
