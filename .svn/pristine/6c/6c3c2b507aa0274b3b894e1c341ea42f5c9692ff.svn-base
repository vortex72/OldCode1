<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Web.Models.KitInterchangeViewModel>" %>

<% Html.RenderPartial("InterchangeHeader"); %>
<% = Html.Hidden("InterchangeNIPC", Model.InterchangeInventoryItem.NIPCCode) %>
<% = Html.Hidden("InterchangeQuantity", Model.InterchangeStockStatus.QuantityRequested) %>
<% = Html.Hidden("InterchangeCode", Model.InterchangeCode) %>
<% if (Model.InterchangeStockStatus.IsQuantityAvailableAnywhere) { %>
<form id="ShipmentOptions">
    <div>The selected part is not available in your default warehouse, but it is available in the following warehouse locations:</div>
    <div id="ShipmentOptionErrors" style="display:none"></div>
    <div>
        <% foreach(string warehouse in WarehouseList.Warehouses) { %>
            <% = Html.WarehouseRadioButton("warehouse", Model.InterchangeStockStatus, warehouse, false) %>
        <% } %>
    </div>
<% } else { %>
    <div>
        The selected part is not available in full quantity from any of our warehouse locations<br />
        You may request that the part be drop shipped or select another interchange.~
    </div>
<% } %>
    
    <div class="buttons">
        <% if (Model.InterchangeStockStatus.IsQuantityAvailableAnywhere) { %>
            <input type="button" value="Submit Interchange Request" id="SubmitInterchange" />
        <% } else { %>
            <input type="button" value="Request Drop Shipment" id="DropshipInterchange" />
        <% } %>
        <input type="button" value="View Other Interchanges" id="ShowAllInterchanges" class="cancel" />
        <input type="button" value="Cancel Interchange Request" id="CancelInterchange" class="submit" />
    </div>
</form>