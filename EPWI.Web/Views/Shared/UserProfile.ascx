<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Components.Models.UserProfile>" %>
<% =Html.ClientSideValidation<UserProfile>("profile")  /*.UseValidationSummary("validationSummary", "Please fix the following errors:")*/ %>
<fieldset>
    <legend>User Profile</legend>
    <div class="row">
        <span class="label">User Name:</span>
        <span class="field">
            <%= Html.TextBox("profile.UserName", Model.UserName) %>
            <%= Html.ValidationMessage("profile.UserName") %>
        </span>
    </div>
    <div class="row">
        <span class="label">Password:</span>
        <span class="field">
            <% = Html.Password("profile.Password", Model.Password) %>
            <%= Html.ValidationMessage("profile.Password") %>
        </span>
    </div>
    <div class="row">
        <span class="label">Confirm Password:</span>
        <span class="field">
            <% = Html.Password("profile.ConfirmPassword", Model.ConfirmPassword) %>
        </span>
    </div>
    <div class="row">
        <span class="label">First Name:</span>
        <span class="field">
            <%= Html.TextBox("profile.FirstName", Model.FirstName, new { @class="name", maxlength = 30 })%>
            <%= Html.ValidationMessage("profile.FirstName", "*") %>
        </span>
    </div>
    <div class="row">
        <span class="label">Last Name:</span>
        <span class="field">
            <%= Html.TextBox("profile.LastName", Model.LastName, new { @class="name", maxlength = 30 })%>
            <%= Html.ValidationMessage("profile.LastName", "*") %>
        </span>
    </div>
    <div class="row">
        <span class="label">Email Address:</span>
        <span class="field">
            <% = Html.TextBox("profile.EmailAddress", Model.EmailAddress, new { maxlength = 50 } ) %>
        
        </span>
    </div>
    <div class="row">
        <span class="label">Title:</span>
        <span class="field">
            <%= Html.TextBox("profile.Title", Model.Title, new { maxlength = 50 }) %>
            <%= Html.ValidationMessage("profile.Title", "*") %>
        </span>
    </div>
    <div class="row">
        <span class="label">Company:</span>
        <span class="field">
            <%= Html.TextBox("profile.Company", Model.Company, new { maxlength = 50 })%>
            <%= Html.ValidationMessage("profile.Company", "*") %>
        </span>
    </div>
    <div class="row">
        <span class="label">Address:</span>
        <span class="field">
            <%= Html.TextBox("profile.Address", Model.Address, new { @class="streetaddress", maxlength = 50 })%>
            <%= Html.ValidationMessage("profile.Address", "*") %>
        </span>
    </div>
    <div class="row">
        <span class="label">City:</span>
        <span class="field">
            <%= Html.TextBox("profile.City", Model.City, new { @class="city", maxlength = 50 })%>
            <%= Html.ValidationMessage("profile.City", "*") %>
        </span>
    </div>
    <div class="row">
        <span class="label">State/Province:</span>
        <span class="field">
            <%= Html.TextBox("profile.StateProvince", Model.StateProvince, new { @class="state", maxlength = 50 })%>
            <%= Html.ValidationMessage("profile.StateProvince", "*") %>
        </span>
    </div>
    <div class="row">
        <span class="label">Zip/Postal Code:</span>
        <span class="field">
            <%= Html.TextBox("profile.ZipPostal", Model.ZipPostal, new { @class="zip", maxlength = 50 })%>
            <%= Html.ValidationMessage("profile.ZipPostal", "*") %>
        </span>
    </div>
    <div class="row">
        <span class="label">Phone:</span>
        <span class="field">
            <%= Html.TextBox("profile.Phone", Model.Phone, new { @class="phone", maxlength = 50 })%>
            <%= Html.ValidationMessage("profile.Phone", "*") %>
        </span>
    </div>
    <div class="row">
        <span class="label">Fax:</span>
        <span class="field">
            <%= Html.TextBox("profile.Fax", Model.Fax, new { @class="phone", maxlength = 50 })%>
            <%= Html.ValidationMessage("profile.Fax", "*") %>
        </span>
    </div>
    <div class="row">
        <span class="label">Notes:</span>
        <span class="field">
            <%= Html.TextBox("profile.Notes", Model.Notes) %>
            <%= Html.ValidationMessage("profile.Notes", "*") %>
        </span>
    </div>            
</fieldset>

