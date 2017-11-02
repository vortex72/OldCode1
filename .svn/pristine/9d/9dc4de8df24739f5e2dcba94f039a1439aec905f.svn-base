<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Components.Models.OrderItem>" %>
<div>
    <% if (string.IsNullOrEmpty(Model.CustomerReference)) { %>
        <a href="#" class="EditCustomerReference">Add Customer Reference</a>
    <% } else { %>
        <span class="EditCustomerReference"><% = Html.Encode(Model.CustomerReference) %></span> <a href="#" class="EditCustomerReference">Edit</a>
    <% } %>
</div>
<div style="display:none" class="EditCustomerReference">
    <% = Html.TextBox("customerReference", Model.CustomerReference, new { maxlength = "30", style = "width:125px" }) %><input type="button" value="Save" class="UpdateCustomerReference" />
</div>