<%@ import namespace='EPWI.Web.Utility' %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<EPWI.Components.Models.Slideshow>>" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Slide List
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Slide List</h2>

    <div><% = Html.ActionLink("Add Slide", "Create") %></div>
    <table class="standard">
        <tr>
            <th>&nbsp;</th>
            <th>Link</th>
            <th>External</th>
            <th>Enabled</th>
            <th>Registered Users</th>
            <th>&nbsp;</th>
        </tr>
        
        <% foreach (var item in Model) { %>
            <tr>
                <td><% = ImageHelper.Image(Url.Action("UncachedSlide", new { id = item.SlideShowID }), new { width = 100 })%></td>
                <td><% = Html.Encode(item.Link) %></td>
                <td><div class="center"><% if (item.ExternalLink) { %><% = ImageHelper.Image(Url.Content("~/Content/images/check.gif"), new { width = 25 })%><% } else { %>&nbsp;<% } %></div></td>
                <td><div class="center"><% if (item.Enabled) { %><% = ImageHelper.Image(Url.Content("~/Content/images/check.gif"), new { width = 25 })%><% } else { %>&nbsp;<% } %></div></td>
                <td><div class="center"><% if (item.RegisteredOnly) { %><% = ImageHelper.Image(Url.Content("~/Content/images/check.gif"), new { width = 25 })%><% } else { %>&nbsp;<% } %></div></td>
                <td><% = Html.ActionLink("Edit", "Edit", new { id = item.SlideShowID }) %> <% = Html.ActionLink("Delete", "Delete", new { id = item.SlideShowID }) %></td>
            </tr>
        <% } %>

    </table>
    <div>&nbsp;</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideBarContent" runat="server">
</asp:Content>
