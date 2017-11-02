<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Table Maintenance
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Table Maintenance</h2>

    <div>
        <% foreach (MetaTable table in (IEnumerable<MetaTable>)Model) { %>
            <a href="<% =table.GetActionPath("List") %>"><% =table.DisplayName %></a><br />
        <% } %>
    </div>
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideBarContent" runat="server">
</asp:Content>
