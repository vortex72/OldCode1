<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EPWI.Web.Models.N2CMS.CatalogViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<% = Html.Encode((Model.Item?.Title) ?? "empty") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><% = Html.Encode((Model.Item?.Title) ?? "empty") %></h2>
    
    <% = Model.Item?.Text %>
    <% = Html.Hidden("BasePath", Url.Content("~")) %>
    <div>Search: <input type="text" id="CatalogSearchValue" /><input type="button" value="Search" id="Search" /><input type="button" value="Clear Search" id="ResetSearch" /></div>    
    <div><input type="checkbox" id="disableDialog" style="margin-bottom:-2px"/><label for="disableDialog">Use default browser PDF handling</label></div>
    <div class="yui-skin-sam">    
        <div id="Catalog"></div>
    </div>    
    <br />
    <div id="catalogDialog" style="display:none;" title=""></div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <!-- <link rel="stylesheet" type="text/css" href="http://yui.yahooapis.com/combo?2.8.0r4/build/paginator/assets/skins/sam/paginator.css&2.8.0r4/build/datatable/assets/skins/sam/datatable.css" /> -->
    <%--<link rel="Stylesheet" type="text/css" href="<% = Url.Content("~/Content/css/YUIPaginatorDatatable.css") %>" />--%>
    <!-- <script type="text/javascript" src="http://yui.yahooapis.com/combo?2.8.0r4/build/yahoo-dom-event/yahoo-dom-event.js&2.8.0r4/build/connection/connection-min.js&2.8.0r4/build/element/element-min.js&2.8.0r4/build/paginator/paginator-min.js&2.8.0r4/build/datasource/datasource-min.js&2.8.0r4/build/datatable/datatable-min.js&2.8.0r4/build/json/json-min.js"></script> -->
    <script type="text/javascript" src="<% = Url.Content("~/Scripts/site/YUIDatatable.js") %>"></script>
    <script type="text/javascript" src="<% = Url.Content("~/Scripts/site/Catalog.js?v=3") %>"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideBarContent" runat="server">
</asp:Content>
