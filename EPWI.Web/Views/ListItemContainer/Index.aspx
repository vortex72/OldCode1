<%@ import namespace='EPWI.Web.Utility' %>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EPWI.Web.Models.N2CMS.ContainerViewModel<EPWI.Web.Models.N2CMS.ListItemContainer, EPWI.Web.Models.N2CMS.ListItemPage>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Engine Performance Warehouse - <% = Html.Encode(Model.Container.Title) %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><% = Html.Encode(Model.Container.Title) %></h2>
    <div><% = Model.Container.Header %></div>
    <table>
    <% foreach(var item in Model.Items) { %> 
        <tr>
            <td><% if (!string.IsNullOrEmpty(item.Thumbnail)) { %><% = ImageHelper.Image(item.Thumbnail)%><% } else { %>&nbsp;<% } %></td>
            <td><h3><% = N2.Web.Link.To(item) %></h3>
            <% = item.Summary %>
            </td>
        </tr>
    <% } %>
    </table>
    <div><% = Model.Container.Footer %></div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideBarContent" runat="server">
</asp:Content>
