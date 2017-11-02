<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ICustomerData>" %>
<div id="EmailDialog" class="dialog">
    <% = Html.Hidden("siteBasePathEmail", Url.Content("~")) %>
       <form id="EmailForm" class="default">
       <fieldset>
        <div id="emailValidationSummary"></div>
        <% /*=Html.ClientSideValidation<Address>().UseValidationSummary("validationSummary", "Please fix the following errors:")*/ %>
        <div class="row">
            <span class="emailLabel"><label for="EmailSenderName">Your Name:</label></span>
            <span class="emailField"><% = Html.TextBox("EmailSenderName", $"{Model.FirstName} {Model.LastName}", new { @class = "exclude" })%></span>
        </div>
        <div class="row">
            <span class="emailLabel"><label for="EmailSenderAddress">Your Email Address:</label></span>
            <span class="emailField"><% = Html.TextBox("EmailSenderAddress", Model.EmailAddress, new { @class="exclude"} )%></span>
        </div>
        <div class="row">
            <span class="emailLabel"><label for="EmailRecipientAddress">Recipient's Email Address:</label></span>
            <span class="emailField"><% = Html.TextBox("EmailRecipientAddress")%></span>
        </div>
        <div class="row">
            <span class="emailLabel"><label for="EmailSubject">Subject:</label></span>
            <span class="emailField"><% = Html.TextBox("EmailSubject") %></span>
        </div>
        <div class="row">
            <span class="emailLabel"><label for="EmailNotes">Notes:</label></span>
            <span class="emailField"><textarea id="EmailNotes" rows="8"></textarea></span>
        </div>
        <br />
        <div class="buttons">
            <input type="button" value="Send" id="SendEmail" class="default" />
            <input type="button" value="Cancel" id="CancelEmail" class="cancel" />
        </div>
       </fieldset>
       </form>
</div>

