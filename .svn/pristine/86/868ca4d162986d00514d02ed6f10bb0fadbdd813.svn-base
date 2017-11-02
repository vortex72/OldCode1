<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EPWI.Web.Models.N2CMS.MenuPage>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Engine Performance Warehouse - <%= Model.Title %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1><%= Model.Title %></h1>
    <%= Model.Text %>
    <div>
    <ul>
    <% = N2.Web.Tree.From(N2.Find.CurrentPage, 2).Filters(new N2.Collections.NavigationFilter()).ExcludeRoot(true) %>
    </ul>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideBarContent" runat="server">
</asp:Content>
