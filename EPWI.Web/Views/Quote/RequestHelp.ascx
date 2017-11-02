<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Web.Models.QuoteHelpRequestViewModel>" %>
<% if (Model.QuoteSaved) { %>
    <p>Order successfully saved as quote.</p>
<% } %>
<% using (Ajax.BeginForm("RequestHelp", new { controller = "Quote" }, new AjaxOptions { OnFailure = "ajaxError", OnComplete = "hideProcessingBlock" }, new { id = "RequestHelpForm" }))
   { %>
    <p>
        Select the EPWI CSR you would like assistance from:<br />
        <% = Html.DropDownList("CSR", new SelectList(Model.CustomerServiceReps, "UserCode", "FormattedCSR"), "---SELECT AN EPWI CSR---") %>
    </p>
    <p>
        Enter any comments for the EPWI CSR (optional): <span id="RequestHelpMessageStatus"></span><br />
        <% = Html.TextArea("RequestHelpMessage", string.Empty, 4, 70, null) %>
    </p>
    <% = Html.Hidden("QuoteID", Model.QuoteID) %>
    <% = Html.Hidden("QuoteSaved", Model.QuoteSaved) %>
    <% = Html.Hidden("StockStatusURL", Url.Action("Search", "StockStatus")) %>
    <div class="buttons"><input type="submit" value="Request Help" id="SubmitHelpRequest" class="ShowIndicator" />&nbsp;<input type="button" class="cancel close" value="Cancel" /></div>
<% } %>
