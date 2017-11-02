<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Web.Models.KitInterchangeViewModel>" %>

<% = Html.Hidden("DefaultWarehouse", Model.DefaultWarehouse) %>
<% Html.RenderPartial("InterchangeHeader"); %>
<div>
    You can opt to manually enter a part number below to interchange this item with, or select from a list of available 
interchanges below. Many possible interchanges may not be available on-line.  If you can't find exactly what you need please 
call EPWI directly for assistance.
</div>
<div>
    Enter Part Number:
    <% using (Ajax.BeginForm("KitSearch", "StockStatus", new AjaxOptions { UpdateTargetId = "InterchangeDialog", OnFailure = "ajaxError", OnBegin = "displayProcessingBlock", OnSuccess = "hideProcessingBlock" }))
        { %>
    <% = Html.TextBox("requestedItemNumber") %>
    <% = Html.Hidden("OriginalPartUniqueIdentifier", Model.OriginalPart.UniqueKitPartIdentifier) %>
    <% = Html.Hidden("RequestedSize", Model.OriginalPart.SizeCode) %>
    <% = Html.Hidden("OriginalPartPrice", Model.OriginalPart.Price) %>
    <% = Html.Hidden("KitNipc", Model.Kit.NIPCCode) %>
    <% = Html.Hidden("RequestedQuantity", Model.OriginalPart.QuantitySelected) %>
    <input type="submit" value="Submit" />
    <% } %>
</div>

<table class="standard">
    <tr>
        <th>Line</th>
        <th>Item Number</th>
        <th>Qty</th>
        <th>Price Difference</th>
        <th>I/C Type</th>
        <th>Notes</th>
    </tr>
    <% foreach (var interchange in Model.Interchanges)
        { %>
    <% if (interchange.NIPCCode != Model.OriginalPart.NIPCCode || Model.ShowOriginalPart)
        {%>
    <tr class="InterchangePart">
        <td class="LineCode"><% = Html.Encode(interchange.LineCode) %></td>
        <td class="PartNumber"><% = Html.Encode(interchange.ItemNumber) %><span style="display: none" class="PartNIPC"><% = interchange.NIPCCode %></span><span style="display: none" class="QuantityOnHand"><% = interchange.OnHandQuantity %></span><span style="display: none" class="InterchangeCode"><% = interchange.InterchangeCode %></span></td>
        <td class="InterchangeQuantity"><% = Html.Encode(interchange.InterchangeQuantity) %></td>
        <td><% if (Model.GetPriceDifference(interchange) == 0)
                { %><span>No difference</span><% }
                                                             else {
                                                                 if (Model.GetPriceDifference(interchange) > 0)
                                                                 { %><span class="increase">Increase by <% }
                                                       else { %><span class="decrease">Decrease by <% } %>$<% = Html.Encode(Math.Abs(Model.GetPriceDifference(interchange).Adjusted(Model.CurrentView, Model.CustomerData)).ToString("F2")) %></span><% } %></span></td>
        <td><% = Html.Encode(interchange.InterchangeType) %></td>
        <td><% = Html.Encode(interchange.LastNote) %></td>
    </tr>
    <% } %>
    <% } %>
    <% if (!Model.InterchangesExist)
        { %>
    <tr colspan="6">
        <td>No interchanges found</td>
    </tr>
    <% } %>
</table>

