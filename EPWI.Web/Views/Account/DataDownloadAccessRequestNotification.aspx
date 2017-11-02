<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<ICustomerData>" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head></head>
<body>
    <div>
    <% = Html.Encode(Model.FirstName) %> <% = Html.Encode(Model.LastName) %> (<b><% = Html.Encode(Model.UserName) %></b>) has requested Data Download access.
    </div>
    <div><a href="<% = Url.Action("Edit", "Account", new RouteValueDictionary { { "id", Model.UserID } }, "http", Request.Url.Host) %>">Edit this user</a></div>
</body>
</html>