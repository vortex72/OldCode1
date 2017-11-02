<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ICustomerData>" %>
<div id="FaxDialog" class="dialog">
    <% = Html.Hidden("siteBasePathFax", Url.Content("~")) %>
       <form id="FaxForm" class="default">
       <fieldset>
        <div id="faxValidationSummary"></div>
        <% /*=Html.ClientSideValidation<Address>().UseValidationSummary("validationSummary", "Please fix the following errors:")*/ %>
        <div class="row">
            <span class="emailLabel"><label for="FaxSenderName">Your Name:</label></span>
            <span class="emailField"><% = Html.TextBox("FaxSenderName", $"{Model.FirstName} {Model.LastName}", new { @class = "exclude" })%></span>
        </div>
        <div class="row">
            <span class="emailLabel"><label for="FaxRecipientName">Recipient's Name:</label></span>
            <span class="emailField"><% = Html.TextBox("FaxRecipientName")%></span>
        </div>
        <div class="row">
            <span class="emailLabel"><label for="FaxRecipientCompany">Recipient's Company:</label></span>
            <span class="emailField"><% = Html.TextBox("FaxRecipientCompany") %></span>
        </div>
        <div class="row">
            <span class="emailLabel"><label for="FaxRecipientNumber">Recipient's Fax Number:</label></span>
            <span class="emailField"><% = Html.TextBox("FaxRecipientNumber") %></span>
        </div>
        <div class="row">
            <span class="emailLabel"><label for="FaxSubject">Subject:</label></span>
            <span class="emailField"><% = Html.TextBox("FaxSubject") %></span>
        </div>
        <div class="row">
            <span class="emailLabel"><label for="FaxIncludeCoverSheet">Include Cover Sheet?</label></span>
            <span class="emailField"><% = Html.CheckBox("FaxIncludeCoverSheet", true, new { style = "width:10%" }) %><i>Required for Notes</i></span>
        </div>
        <div class="row">
            <span class="emailLabel"><label for="FaxNotes">Notes:</label></span>
            <span class="emailField"><textarea id="FaxNotes" rows="8"></textarea></span>
        </div>
        <br />
        <div class="buttons">
            <input type="button" value="Send" id="SendFax" class="default" />
            <input type="button" value="Cancel" id="CancelEmail" class="cancel" />
        </div>
       </fieldset>
       </form>
</div>
