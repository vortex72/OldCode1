<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EPWI.Web.Models.UserEditViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit User
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Edit User</h2>
    <% if (Model.ShowReturnToUserList) { %>
        <div><% = Html.ActionLink("Return to User List", "UserList", "Admin") %></div>
    <% } %>
    <div id="validationSummary">
    </div>
    <% using (Html.BeginForm("Edit", "Account", FormMethod.Post, new { id = "EditAccount" })) {%>
        <% Html.RenderPartial("UserProfile", Model.UserProfile); %>
        <% if (User.IsInRole("ADMIN")) { %>
            <% Html.RenderPartial("UserAdminSettings", Model.UserAdminSettings); %>
        <% } %>
        <p>
            <input type="submit" name="submitButton" value="Save Changes" />
            <% if (User.IsInRole("ADMIN") && User.Identity.Name != Model.UserProfile.UserName) { %>
                <input type="submit" id="btnDelete" name="submitButton" value="Delete User" class="cancel" />
            <% } %>
        </p>
    <% } %>
   
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">    
    <script type="text/javascript">
        $(function() {
            $('#btnDelete').click(function() {
                return confirm('Are you sure you want to delete this user?');
            });
            $('#EditAccount').validate();
            $('#profile_ConfirmPassword').rules('add', { equalTo: '#profile_Password', messages: { equalTo: 'Passwords must match'} });
        });
    </script>
</asp:Content>
