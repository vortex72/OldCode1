<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Components.Models.Address>" %>
<% = Html.Encode(Model.Name) %><br />
<% = Html.Encode(Model.StreetAddress1) %><br />
<% if (!string.IsNullOrEmpty(Model.StreetAddress2)) { %>
    <% = Html.Encode(Model.StreetAddress2) %><br />
<% } %>
<% = Html.Encode(Model.City) %>, <% = Html.Encode(Model.State) %> 
<% if (Model.Zip.Length == 5) { %>
    <% = Html.Encode(Model.Zip) %><% if (Model.Zip4.Length == 4) {%>-<% = Html.Encode(Model.Zip4) %><% } %><br />
<% } %>
<% = Html.FormattedPhone(Model.Phone) %>
