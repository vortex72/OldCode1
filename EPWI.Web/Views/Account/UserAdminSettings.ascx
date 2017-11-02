<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Components.Models.UserAdminSettings>" %>
        <% =Html.ClientSideValidation<UserAdminSettings>("admin") /*.UseValidationSummary("validationSummary", "Please fix the following errors:")*/ %>
        <fieldset>
            <legend>Administrative Settings</legend>
            <p>
                <label for="admin.IsActive">Activate Account:</label>
                <%= Html.CheckBox("admin.IsActive", Model.IsActive) %>
            </p>
            <% if (Model.UserID != 0) { %>
            <p>
                <label>User ID:</label>
                <% = Model.UserID %>
            </p>
            <% } %>
            <p>
                <label for="admin.CustomerID">EPWI User ID:</label>
                <%= Html.TextBox("admin.CustomerID", Model.CustomerID) %>
            </p>
            <p>
                <label for="admin.CompanyCode">EPWI Company Code:</label>
                <%= Html.DropDownList("admin.CompanyCode", new SelectList(new [] { "N", "S" }, Model.CompanyCode), string.Empty) %>
            </p>
            <p>
                <% for (int i = 0; i < Model.RoleMembershipList.Count; i++) { %>
                    <% = Html.Hidden("admin.RoleMembershipList[" + i + "].RoleID", Model.RoleMembershipList[i].RoleID) %>
                    <% = Html.Hidden("admin.RoleMembershipList[" + i + "].RoleDescription", Model.RoleMembershipList[i].RoleDescription) %>
                    <% = Html.CheckBox("admin.RoleMembershipList[" + i + "].IsInRole", Model.RoleMembershipList[i].IsInRole) %>
                    <% = Html.Encode(Model.RoleMembershipList[i].RoleDescription) %>
                    <% if(Model.RoleMembershipList[i].RoleKey == "ACCESS_ACCOUNT_SETTINGS" && Model.UserID > 0) { %>
                        <% = Html.ActionLink("Edit Account Settings", "Settings", new { id = Model.UserID }) %>
                    <% } %>
                    
                    <br />
                <% } %>
            </p>
            <p>
                <label for="admin.PricingFactor">Pricing Factor (%):</label>
                <%= Html.TextBox("admin.PricingFactor", Model.PricingFactor) %>
            </p>
            <p>
                <label for="admin.CommentsForUser">Comments For User:</label>
                <%= Html.TextArea("admin.CommentsForUser", Model.CommentsForUser, 5, 40, null) %>
            </p>
        </fieldset>



