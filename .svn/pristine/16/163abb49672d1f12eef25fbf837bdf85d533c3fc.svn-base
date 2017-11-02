<%@ import namespace='EPWI.Web.Utility' %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EPWI.Web.Models.N2CMS.ContainerViewModel<EPWI.Web.Models.N2CMS.LinkItemContainer, EPWI.Web.Models.N2CMS.LinkItem>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Engine Performance Warehouse - <% = Html.Encode(Model.Container.Title) %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><% = Html.Encode(Model.Container.Title) %></h2>
    <div><% = Model.Container.Header %></div>
    <% if (Model.Container.ShowAlphabetically) { %>    
        <div class="center"><% = Html.AlphaPicker("List", (string)ViewData["filter"]) %></div>
    <% } %>
    <% if (Model.Container.EnablePaging) { %>
        <div class="center"><% = Html.PageLinks((int)ViewData["CurrentPage"], (int)ViewData["TotalPages"], x=> Url.Action("List", new { p = x, id = (string)ViewData["filter"] })) %></div>
    <% } %>
    <table>
    <% if (Model.Items.Any()) { %>
        <% foreach(var item in Model.Items) { %> 
            <tr>
                <td><% if (!string.IsNullOrEmpty(item.Thumbnail)) { %><a href="<% = item.LinkUrl %>" target="_blank"><% = ImageHelper.Image(item.Thumbnail, new { style = "border:0px" })%></a><% } else { %>&nbsp;<% } %></td>
                <td><h3><a href="<% = item.LinkUrl %>" target="_blank"><% = item.Title %></a></h3>
                <% = item.Summary %>
                </td>
            </tr>
        <% } %>
    <% } else { %>
        <tr>
            <td colspan="2">No Matching Items</td>
        </tr>
    <% } %>
    </table>
    <div><% = Model.Container.Footer %></div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideBarContent" runat="server">
</asp:Content>
