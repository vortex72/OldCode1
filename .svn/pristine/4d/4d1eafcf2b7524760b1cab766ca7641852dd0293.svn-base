<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Administration
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Administration</h2>

    <h3>Site Maintenance</h3>
    <ul>
        <li><a href="<% = Url.Content("~/N2") %>" target="_blank">Content Management</a></li>
        <li><% = Html.ActionLink("Slideshow Maintenance", "Index", "SlideShow") %></li>
        <li><a href="<% = Url.Content("~/LegacyAdmin/data_import.asp") %>" target="_blank">Synchronize Data with AS/400</a></li>
        <li><% = Html.ActionLink("Table Maintenance", "TableMaintenance") %></li>
        
        
    </ul>

    <h3>User Maintenance</h3>
    <ul>
        <li><% = Html.ActionLink("Create User", "Create", "Account") %></li>
        <li><% = Html.ActionLink("User List", "UserList") %></li>
    </ul>

    <div>Site Version: <% = TempData["version"] %></div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
