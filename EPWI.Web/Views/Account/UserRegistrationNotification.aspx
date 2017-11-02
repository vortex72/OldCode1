<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<EPWI.Components.Models.UserProfile>" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head></head>
<body>
    <div>
    <% = Html.Encode(Model.FirstName) %> <% = Html.Encode(Model.LastName) %> (<b><% = Html.Encode(Model.UserName) %></b>) has registered at the
    Engine and Performance Warehouse site.
    </div>
    <div>Please review their information and approve them for access if required.</div>
    <div><a href="<% = Url.Action("Edit", "Account", new RouteValueDictionary { { "id", ViewData["userid"] } }, "http", Request.Url.Host) %>">Edit this user</a></div>
    <hr />
    DETAILS BELOW
    <hr />
    Username: <% = Html.Encode(Model.UserName) %><br />
    First Name: <% = Html.Encode(Model.FirstName) %><br />
    Last Name: <% = Html.Encode(Model.LastName) %><br />
    Email: <% = Html.Encode(Model.EmailAddress) %><br />
    Title: <% = Html.Encode(Model.Title) %><br />
    Company: <% = Html.Encode(Model.Company) %><br />
    Address: <% = Html.Encode(Model.Address) %><br />
    City: <% = Html.Encode(Model.City) %><br />
    State: <% = Html.Encode(Model.StateProvince) %><br />
    Zip: <% = Html.Encode(Model.ZipPostal) %><br />
    Phone: <% = Html.Encode(Model.Phone) %><br />
    Fax: <% = Html.Encode(Model.Fax) %><br />
    Notes: <% = Html.Encode(Model.Notes) %><br />
</body>
</html>
