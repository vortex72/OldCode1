<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<p>Enter the ship-to address for this Power User Order. All fields are required except the last 4 digits of the ZIP code.</p>
<div id="PowerUserValidationSummary"></div>
<% using (Html.BeginForm("OpenPowerUserOrder", "Order")) { %>
    <% =Html.ClientSideValidation<Address>().UseValidationSummary("PowerUserValidationSummary", "Please fix the following errors:") %>
    <% Html.RenderPartial("ShipToAddress"); %>
    <div>
        <input type="submit" value="Open Power User Order" />
    </div>
<% } %>

