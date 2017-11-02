<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Web.Models.OrderViewModel>" %>
<div id="contentContainer">
<% using (Ajax.BeginForm("SaveOrderAsQuote", new { controller = "Quote" }, new AjaxOptions { UpdateTargetId = "contentContainer", OnFailure = "ajaxError", OnComplete = "hideProcessingBlock" }, new { id = "SaveQuoteForm" }))
   { %>
    <% if (Model.LoadedFromQuote)
       { %>
        <div>This order was loaded from the quote "<% = Html.Encode(Model.Order.Quote.QuoteDescription)%>"</div>
        <% if (Model.CanOverwriteQuote)
           { %>
            Save Options:<br />
            <input type="radio" name="SaveOption" checked="checked" value="overwriteQuote" id="overwriteQuoteOption" /><label for="overwriteQuoteOption">Update Original Quote</label><br />
            <input type="radio" name="SaveOption" value="saveQuote" id="saveQuoteOption"/><label for="saveQuoteOption">Save As New Quote</label>
        <% }
           else
           { %>
            <div>This quote is marked as read-only and cannot be updated. Please enter a new name for the quote to save it.</div>
            <br />
        <% } %>
    <% } %>
    
    <div class="togglePanel" style="display:<% = Model.CanOverwriteQuote ? "none" : "block" %>">
        <% if (Model.CanOverwriteQuote)
           { %>
            <br />
            <div>
                To save this order as a new quote, enter a new name below or click one of the quotes listed to overwrite it with this order.
            </div>
            <br />
        <% } %>
            
        Quote Description: <% = Html.TextBox("QuoteDescription", null, new { style = "width:275px", maxlength = "300" })%><br />
        Quote Type:<br />
        <% = Html.RadioButton("ShareQuote", "Private", true)%>Private<br />
        <% = Html.RadioButton("ShareQuote", "SharedReadOnly")%>Share with company employees (no updates allowed)<br />
        <% = Html.RadioButton("ShareQuote", "Shared")%>Share with company employees (updates allowed)<br />
        <% = Html.Hidden("HiddenQuotePO")%>
        <% = Html.Hidden("HiddenQuoteNotes")%>
        <br />
    </div>
    <% if (!Page.User.IsInRole("EMPLOYEE")) { %>
        <div><% = Html.CheckBox("helpRequested")%> I would like help with this quote from an EPWI CSR</div>
    <% } %>
    <div class="buttons">
            <input type="submit" value="Save" id="SaveQuote" />
            <input type="button" value="Cancel" class="cancel close" />
    </div>
    <div class="togglePanel" style="display:<% = Model.CanOverwriteQuote ? "none" : "block" %>">
        <div id="ExistingQuotes"></div>
    </div>
<% } %>
</div>