<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<p> Copyright &copy;<%= DateTime.Now.Year %>, Engine & Performance Warehouse. &nbsp;All Rights Reserved. 
<% if (Page.User.IsInRole("ADMIN")) { %>
    &nbsp;&nbsp; | &nbsp;&nbsp;<% = Html.ActionLink("Admin", "Index", "Admin", null, new { style = "color:#999;" })%>
<% } %>    
</p>
