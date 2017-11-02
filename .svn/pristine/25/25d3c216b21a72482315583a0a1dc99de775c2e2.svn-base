<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Components.Models.ICustomerData>" %>
<div class="dialog" id="UpdateCustomerIDDialog">
<% using (Html.BeginForm("UpdateCustomerID", "Account", FormMethod.Post, new { id = "UpdateCustomerIDForm" })) { %>
<div>Customer ID: <% = Html.TextBox("customerID", Model.CustomerID, new { id = "UpdateCustomerIDText", @class = "numeric required" })%></div>
<div>Company Code: <% = Html.DropDownList("companyCode", new SelectList(new[] { "N", "S" }))%></div>
<input type="submit" value="Save" />
<% } %>
</div>

