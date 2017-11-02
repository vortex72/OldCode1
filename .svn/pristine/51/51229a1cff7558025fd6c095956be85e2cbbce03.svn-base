<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Components.Models.ICustomerData>" %>
<% using (Html.BeginForm("SearchByCustomer", "Quote", FormMethod.Post, new { id = "SearchByCustomerForm" })) { %>
    <div><strong>Search by Customer ID/Company Code:</strong></div>
    <div>
        Cusomter ID: <% = Html.TextBox("CustomerID", Model.CustomerID, new { tabindex="1", size="5", maxlength ="5", @class="numeric required" }) %>
        Company Code (N/S): <% = Html.DropDownList("CompanyCode", new SelectList(new[] { "N", "S" }, Model.CompanyCode.ToString()), new { tabindex = "2" })%>
        <input type="submit" value="Search" />
    </div>
<% } %>
<div>&nbsp;</div>
<% using (Html.BeginForm("SearchByQuoteID", "Quote", FormMethod.Post, new { id = "SearchByQuoteIDForm" }))
   { %>
    <div><strong>Search by Quote ID:</strong></div>
    <div>Quote ID: <% = Html.TextBox("QuoteID", null, new { size = "5", @class = "numeric required" })%> <input type="submit" value="Search" /></div>
<% } %>
