<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Web.Models.KitBuilderViewModel>" %>
<div class="FulfillmentMessage">
<% if (Model.FulfillmentProcessingResult.ResultCode == "M") { %>
    Multiple warehouses can provide a solution to fulfilling your order. Please select a secondary warehouse which will
    be used in combination with your primary warehouse in order to ship your order.<br />
    Warehouse: <% = Html.DropDownList("warehouse", Model.FulfillmentProcessingResult.ProcessedWarehouses.Select(w => new SelectListItem() { Value = w, Text = w })) %>
    <input type="submit" value="Submit" id="SubmitOrderWithWarehouse" />
<% } else if (Model.FulfillmentProcessingResult.ResultCode.Substring(0, 1) == "P" || Model.FulfillmentProcessingResult.ResultCode.Substring(0, 1) == "S" || Model.FulfillmentProcessingResult.ResultCode.Substring(0, 1) == "C" || Model.FulfillmentProcessingResult.ResultCode.Substring(0, 1) == "O") { %>
    <div>
        <span class="erroralert">
            Your kit has been modified to reflect changes in parts or shipping locations in order to most effectively fulfill your order.
            The changed items are highlighted in yellow below.
        </span>
    </div>
    <br />
    <div>
        Click the Add to Order button to add this kit to your order or choose an alternate option below.
        <% using (Html.BeginForm("ReloadSnapshot", "Kit", FormMethod.Post)) { %>
            <input type="submit" value="Reload kit as previously configured" class="ShowIndicator" />
        <% } %>
        <input id="AddAsPreviouslyConfigured" type="button" value="Add kit to order as previously configured and call me" />
    </div>
<% } else if (Model.FulfillmentProcessingResult.ResultCode.Substring(0, 1) == "X") { %>
    <span class="erroralert">
        Our kit wizard was unable to find an adequate solution that would allow us to automatically fulfill your order. 
        Unavailable items are highlighted in red. You can attempt to add the kit to your order again after adjusting the items in your kit or add the kit as 
        configured and request manual processing.
    </span>
<% } else { %>
    <span class="erroralert">
        An error has occurred while attempting to perform fulfillment processing on your configured kit.<br />
        Please contact EPWI directly for assistance.
     </span>
<% } %>
<% if (Model.FulfillmentProcessingResult.ResultCode.Length == 2 && Model.FulfillmentProcessingResult.ResultCode.Substring(1,1) == "X") { %>
    <br />
    <div>
        <span class="erroralert">
        Unfortunately, we were unable to effectively fulfill one or more items in your kit as indicated by the red highlighted items.
        Please either deselect the item, find an interchange, or add the kit as configured and 
        request manual processing.
        </span>
    </div>
<% } %>
</div>
