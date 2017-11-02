<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div class="StatusMessage" id="StatusMessage">
        <% if (TempData["message"] != null) { %><% = Html.Encode(TempData["message"]) %><% } %>
        <% if (TempData["unencodedmessage"] != null) { %><% = TempData["unencodedmessage"] %><% } %>
</div>
