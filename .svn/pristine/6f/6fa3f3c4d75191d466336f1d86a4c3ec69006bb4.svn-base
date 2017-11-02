<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Components.Models.ICustomerData>" %>
<div class="sideContainer roundedCorners">
    <h2 class="roundedCorners">Account Information</h2>
    <div class="roundedCornersBody"> 
        <%
            if (Request.IsAuthenticated) {
        %>
                <% = Html.Encode(Page.User.Identity.Name) %><br />
                <% = Html.Encode(Model.CompanyName) %>(<% = Html.Encode(Model.CustomerID) %> <% = Html.Encode(Model.CompanyCode) %>)<br />
                <% if (Page.User.IsInRole("ACCESS_ACCOUNT_STATUS")) { %>
                    <%= Html.ActionLink("View Account Status", "Status", "Account") %><br />
                <% } %>
                <% if (Page.User.IsInRole("ACCESS_ACCOUNT_SETTINGS")) { %>
                    <%= Html.ActionLink("Edit Account Settings", "Settings", "Account") %><br />
                <% } %>
                <%= Html.ActionLink("Edit User Profile", "Edit", "Account") %>
                <% if (Page.User.IsInRole("EMPLOYEE")) { %>
                    <br /><a href="#" id="UpdateCustomerID">Update Customer ID</a>
                    <% Html.RenderPartial("UpdateCustomerID", Model); %>
                <% } %>
                <div class="center">[ <%= Html.ActionLink("Log Off", "LogOff", "Account") %> ]</div>
        <% } else { %>
            [ <%= Html.ActionLink("Log On", "LogOn", "Account") %> ]
        <%
            }
        %>
    </div>
    <div class="roundedCornersFooter"><p></p></div>
</div>
