<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/NoWidgets.Master" Inherits="System.Web.Mvc.ViewPage<EPWI.Components.Models.UserProfile>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Register
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/site/xVal.jquery.validate.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-migrate-1.2.1.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.2.custom.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/site/Register.js") %>" type="text/javascript" ></script>
    <h2>Register</h2>
     <% using (Html.BeginForm()) {%>
        <% Html.RenderPartial("UserProfile", Model); %>
        <% = Html.GenerateCaptcha() %>
        <% if (ViewData["recaptchaMessage"] != null) { %>
            <div class="StatusMessage">
                <% = Html.Encode(ViewData["recaptchaMessage"]) %>    
            </div>
        <% } %>
        <p>
            <input type="submit" value="Register" />
        </p>
     <% } %>
     <% if(ViewData["SuppressDialog"] == null || !(bool)ViewData["SuppressDialog"]) { %>
        <% Html.RenderPartial("RegisterDialog"); %>
     <% } %>
     <% = Html.Hidden("HomePageURL", Url.Action("Index", "Home")) %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
