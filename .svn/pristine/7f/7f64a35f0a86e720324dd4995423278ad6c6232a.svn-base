<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	User List
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>User List</h2>
    <div class="yui-skin-sam">
        <div><% = Html.ActionLink("Create User", "Create", "Account") %></div><div><%=Html.ActionLink("Download User List", "DownloadUserList") %></div><br />
        <div>Search: <input type="text" id="UserSearchValue" /><input type="button" value="Search" id="Search" /><% = Html.Hidden("EditUserUrl", Url.Action("Edit","Account"))%></div>
        <div>User role filter: 
            <input type="radio" name="RoleFilter" value="ALL" checked="checked" />All
            <input type="radio" name="RoleFilter" value="CUSTOMER" />Customer
            <input type="radio" name="RoleFilter" value="EMPLOYEE" />Employee
            <input type="radio" name="RoleFilter" value="ADMIN" />Admin
            <input type="radio" name="RoleFilter" value="KIT_BUILDER" />Kit Builder 
            <input type="radio" name="RoleFilter" value="ACCESS_ACCOUNT_STATUS" />Access Account Status
            <input type="radio" name="RoleFilter" value="DATA_DOWNLOADS" />Data Downloads
        </div>
        <div id="UserList"></div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
<!-- <link rel="stylesheet" type="text/css" href="http://yui.yahooapis.com/combo?2.8.0r4/build/paginator/assets/skins/sam/paginator.css&2.8.0r4/build/datatable/assets/skins/sam/datatable.css" /> -->
<link rel="Stylesheet" type="text/css" href="<% = Url.Content("~/Content/css/YUIPaginatorDatatable.css") %>" />
<!-- <script type="text/javascript" src="http://yui.yahooapis.com/combo?2.8.0r4/build/yahoo-dom-event/yahoo-dom-event.js&2.8.0r4/build/connection/connection-min.js&2.8.0r4/build/element/element-min.js&2.8.0r4/build/paginator/paginator-min.js&2.8.0r4/build/datasource/datasource-min.js&2.8.0r4/build/datatable/datatable-min.js&2.8.0r4/build/json/json-min.js"></script> -->
<script type="text/javascript" src="<% = Url.Content("~/Scripts/site/YUIDatatable.js") %>"></script>
<style type="text/css">
/* custom styles for this example */
.yui-skin-sam .yui-dt-body { cursor:pointer; } /* when rows are selectable */
#single { margin-top:2em; }
</style>
<script type="text/javascript" src="<% = Url.Content("~/Scripts/site/userlist.js") %>"></script>
</asp:Content>
