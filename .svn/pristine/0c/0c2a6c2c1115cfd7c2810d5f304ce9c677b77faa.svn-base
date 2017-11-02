<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ResetPasswordModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Reset Password
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Reset Password</h2>
    
    <div id="validationsummary">
        <% =Html.ValidationSummary("Please correct the following errors:")%>
        <% =Html.ClientSideValidation(typeof(ResetPasswordModel)).UseValidationSummary("validationsummary") %>
    </div>
    
    <p>Enter your username and a new password will be e-mailed to you.</p>
    
    <% using (Html.BeginForm()) { %>
        <div>Username: <% = Html.TextBox("Username") %><input type="submit" value="Reset Password" /></div>
    <% } %>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SideBarContent" runat="server">
</asp:Content>
