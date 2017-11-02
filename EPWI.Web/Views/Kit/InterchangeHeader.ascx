<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Web.Models.KitInterchangeViewModel>" %>
<% = Html.Hidden("OriginalPartUniqueIdentifier", Model.OriginalPart.UniqueKitPartIdentifier) %>
<table class="standard" style="width:100%">
<tr>
    <th>Original Part</th>
    <th>Selected Part</th>
    <th>Qty</th>
    <th>Size</th>
</tr>
<tr>
    <td><% = Html.Encode(Model.OriginalPart.LineDescription) %> <% = Html.Encode(Model.OriginalPart.ItemNumber) %></td>
    <td><% if (Model.InterchangeItemExists) { %><% = Html.Encode(Model.InterchangeInventoryItem.LineDescription) %> <% = Html.Encode(Model.InterchangeInventoryItem.ItemNumber) %><% } else { %>&nbsp;<% } %></td>
    <td><% if (Model.InterchangeItemExists) { %><% = Html.Encode(Model.InterchangeStockStatus.QuantityRequested) %><% } else { %><% = Html.Encode(Model.OriginalPart.QuantitySelected) %><% } %></td>
    <td><% = Html.Encode(Model.OriginalPart.SizeCode) %></td>
</tr>
</table>
