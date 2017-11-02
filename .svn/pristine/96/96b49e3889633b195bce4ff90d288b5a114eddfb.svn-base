<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Components.Models.InvoiceDateSearch>" %>
<b>
    <% = Html.ActionLink($"View Invoices Before {Model.FirstDate.ToString("M/d/yyyy")}", "InvoiceSearch", new { invoiceDateDirection = "P", invoiceDate = Model.FirstDate, customerID = Model.CustomerData.CustomerID, companyCode = Model.CustomerData.CompanyCode, searchSelection = "date" } ) %>&nbsp;|&nbsp;<% = Html.ActionLink($"View Invoices After {Model.LastDate.ToString("M/d/yyyy")}", "InvoiceSearch", new { invoiceDateDirection = "N", invoiceDate = Model.LastDate, customerID = Model.CustomerData.CustomerID, companyCode = Model.CustomerData.CompanyCode, searchSelection = "date" })%><br />
    <% if (Model.Invoices.Count() > 0) { %>
        <b><a href="#" class="ViewSelectedInvoices">View Selected Invoices</a>&nbsp;|&nbsp;<a href="#" id="ClearCheckboxes">Uncheck All Invoices</a></b>
    <% } %>

</b>

