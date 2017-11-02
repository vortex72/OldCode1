<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Web.Models.OrderViewModel>" %>
<script type="text/javascript">
    $(function() {
        $(document)
            .on('click', 'img.DeleteLink',
                function() {
                    $(this).siblings('form.Delete').find('#DeleteSubmit').click();
                });
    });
</script>
<table class="standard">
    <% var itemMessages = Model.Order.GetOrderItemMessages().ToList();
        if (Model.OrderError || itemMessages.Any())
        { %>
    <tr>
        <td colspan="10" class="error">Messages:<br />
            <% if (Model.OrderError)
                { %>
            <% if (Model.OrderErrorNotHandledByOrderWizard)
                { %>
                    Unable to Process order at This Time - Error Code: <% = Html.Encode(Model.Order.OrderStatus) %> - <% = Html.Encode(Model.Order.OrderStatusMessage) %><br />
            <% if (Model.Order.StatusFlag1.GetValueOrDefault(' ') != ' ')
                { %><% = Html.Encode(Model.Order.GetStatusFlagMessage(1)) %><br />
            <% } %>
            <% if (Model.Order.StatusFlag2.GetValueOrDefault(' ') != ' ')
                { %><% = Html.Encode(Model.Order.GetStatusFlagMessage(2)) %><br />
            <% } %>
            <% if (Model.Order.StatusFlag3.GetValueOrDefault(' ') != ' ')
                { %><% = Html.Encode(Model.Order.GetStatusFlagMessage(3)) %><br />
            <% } %>
            <% } %>
            <% } %>
            <% foreach (string message in itemMessages)
                { %>
            <% = Html.Encode(message) %><br />
            <% } %>
        </td>
    </tr>
    <% } %>
    <tr>
        <th>Quantity</th>
        <th>
            <% if (!Model.OrderAccepted)
                { %>Rem<% }
                           else
                           { %>&nbsp;<% } %>
        </th>
        <th>Item</th>
        <th>Line</th>
        <th>Description</th>
        <th>Size</th>
        <th>Whse</th>
        <th>Ship Method</th>
        <th>Indiv. Price</th>
        <th>Total Price</th>
    </tr>
    <% if (!Model.Order.OrderItems.Any())
        { %>
    <tr>
        <td colspan="10">You currently have no items in your cart.
        </td>
    </tr>
    <% }
        else
        { %>
    <% foreach (OrderItem item in Model.Order.OrderedOrderItems)
        { %>
    <tr class="<% = item.IsKitComponent ? "KitComponent" : "Component" %> <% if (item.ErrorFlag.HasValue && item.ErrorFlag.Value != ' ')
        { %>ItemError<% } %>">
        <td>
            <% = item.Quantity %>
        </td>
        <td>
            <% if (item.ZeroPrice || item.ParentItemID > 0 || Model.OrderAccepted)
                { %>
                    &nbsp;
                <% }
                    else
                    { %>
            <% using (Ajax.BeginForm("DeleteItem", "Order", new
                { id = item.OrderItemID }, new AjaxOptions
                {
                    UpdateTargetId = "OrderDetails",
                    OnBegin = "displayProcessingBlock",
                    OnComplete = "hideProcessingBlock",
                    OnSuccess = "itemDeleteSuccess",
                    OnFailure = "ajaxError",
                    InsertionMode = InsertionMode.Replace,
                }, new { @class = "Delete" }))
                { %><input type="submit" style="display: none" id="DeleteSubmit">
            <% } %>
            <img src="<% = Url.Content("~/Content/images/qty_rem.gif") %>" class="submitbutton DeleteLink" alt="Remove" />
            <!-- TODO: Only show change option for admin users? -->
            <% if (!item.IsKit || !string.IsNullOrEmpty(item.KitXml))
                { %>
            <img src="<% = Url.Content("~/Content/images/qty_change.gif") %>" class="submitbutton ChangeLink <% if (item.IsKit)
                { %>ChangeKit<% } %>"
                alt="Change" />
            <% using (Html.BeginForm("ChangeItem", "Order", new { id = item.OrderItemID }, FormMethod.Post, new { @class = "Change" }))
                {
                } %>
            <% } %>
            <% } %>
        </td>
        <td>
            <% = Html.Encode(item.ItemNumber) %>
        </td>
        <td>
            <% = Html.Encode(item.LineCode) %>
        </td>
        <td>
            <% if (item.CoreItem.GetValueOrDefault() == 'C')
                { %><em>- Core Item -</em><% } %>
            <% = Html.Encode(item.ItemDescription) %>
            <% if (new char[] { 'F', 'X' }.Contains(item.OrderMethod.GetValueOrDefault(' ')))
                { %>
            <span class="error">(Manual processing requested)</span>
            <% } %>

            <% if (item.IsKit && !string.IsNullOrEmpty(item.KitData))
                { %>
            <div>
                Year: <% = Html.Encode(item.KitYear) %> Bore: <% = Html.Encode(item.KitBoreSize) %><br />
                Rods: <% = Html.Encode(item.KitRodSize) %> Main: <% = Html.Encode(item.KitMainBearingSize) %><br />
                TWash: <% = Html.Encode(item.KitThrustWasherSize) %>
            </div>
            <% } %>
            <% if (item.ErrorFlag.HasValue && item.ErrorFlag.Value != ' ')
                { %>
            <span>[ERROR: <% = item.ErrorFlag %>]</span>
            <% } %>
            <% if (Model.OrderAccepted)
                { %>
            <div class="customer-reference-toggle"><% = Html.Encode(item.CustomerReference) %></div>
            <% }
                else if (!item.IsKitComponent && item.CoreItem != 'K' && item.CoreItem != 'C')
                { %>
            <div class="CustomerReference customer-reference-toggle" data-order-item-id="<% = item.OrderItemID %>">
                <% Html.RenderPartial("EditCustomerReference", item); %>
            </div>
            <% } %>
        </td>
        <td>
            <% = Html.Encode(item.SizeCode) %>
        </td>
        <td>
            <% = Html.Encode(item.Warehouse) %>
        </td>
        <td>
            <% if (item.CanSelectShipMethod)
                { %>
            <% if (!Model.OrderAccepted)
                { %>
            <% using (Ajax.BeginForm("SetItemShipMethod", new { id = item.OrderItemID }, new AjaxOptions { OnFailure = "handleError" }))
                { %>
            <% = Html.DropDownList("ItemShipMethod", Model.GetShippingMethods(false, item.ShipMethod, item.Warehouse), new {onchange = "$('#ItemShipMethodSubmit" + item.OrderItemID + "').click();"}) %>
            <input type="submit" value="Set" style="display: none" id="ItemShipMethodSubmit<% = item.OrderItemID %>" />
            <% } %>
            <% }
                else
                { %>
            <% = Html.Encode(Model.ShipMethodCodeToName(item.ShipMethod)) %>
            <% } %>
            <% }
                else
                { %>
                    &nbsp;
                <% } %>
        </td>
        <% if (item.ZeroPrice)
            { %>
        <% if (item.ParentItemID > 0)
            { %>
        <td colspan="2">&nbsp;</td>
        <% }
            else
            { %>
        <td colspan="2">N/A - Alt Ship Method</td>
        <% } %>
        <% }
            else
            { %>
        <td><% = item.DiscountedPrice.ToString("C4") %></td>
        <td><% = (item.Quantity*item.DiscountedPrice).ToString("C") %></td>
        <% } %>
    </tr>
    <% } %>
    <tr>
        <th colspan="9">Sub-Total:
        </th>
        <th>
            <% = Model.Order.SubTotalCalculated.GetValueOrDefault(0).ToString("C") %>
        </th>
    </tr>
    <% if (Model.Order.TaxValue.GetValueOrDefault(0) > 0)
        { %>
    <tr>
        <td colspan="9">Tax
                <% if (Model.Order.TaxPercent.GetValueOrDefault(0) > 0)
                    { %>(<% = Model.Order.TaxPercent.Value.ToString("P1") %>)<% } %>
        </td>
        <td>
            <% = Model.Order.TaxValue.Value.ToString("C") %>
        </td>
    </tr>
    <% } %>

    <% if (Model.Order.SpecialCharges.GetValueOrDefault(0) > 0)
        { %>
    <tr>
        <td colspan="9">Special Charges:
        </td>
        <td>
            <% = Model.Order.SpecialCharges.Value.ToString("C") %>
        </td>
    </tr>
    <% } %>

    <% if (Model.Order.OrderTotal.GetValueOrDefault(0) > 0)
        { %>
    <tr>
        <td colspan="9">Order Total:
        </td>
        <td>
            <% = Model.Order.OrderTotal.Value.ToString("C") %>
        </td>
    </tr>
    <% } %>
    <% } %>
    <tr>
        <td colspan="10">
            <% if (!Model.OrderAccepted)
                { %>
            Order Notes:
            <% = Html.TextArea("OrderNotes", Model.Order.OrderNotes, 2, 60, null) %>
            <div id="NotesStatus"></div>
            <input id="SubmitNotesChange" type="submit" value="Save" /><% = Html.AjaxIndicator("SaveNotesIndicator") %>
            <% }
                else
                { %>
            <% = Html.Encode(Model.Order.OrderNotes) %>
            <% } %>
        </td>
    </tr>
</table>
<br />
<div id="Actions">
    <% if (!Model.OrderAccepted)
        { %>
    <% using (Html.BeginForm("Process", "Order", FormMethod.Post, new { id = "ProcessOrderForm", @class = "inline" }))
        { %>
    <% if (Model.Order.OrderItems.Count > 0)
        { %>
    <input type="button" id="ProcessOrderButton" value="Process Order" class="ShowIndicator" />
    <% = Html.Hidden("HiddenPO") %>
    <% = Html.Hidden("HiddenNotes") %>
    <input type="button" id="SaveOrderAsQuoteButton" value="Save Order As Quote" />
    <% } %>
    <% if (Model.Order.OrderItems.Count > 0 || Model.Order.IsPowerUserOrder)
        { %>
    <input type="button" id="DeleteOrderButton" value="Delete Order" />
    <% } %>
    <% } %>
    <% using (Html.BeginForm("Delete", "Order", FormMethod.Post, new { @class = "inline", id = "DeleteOrderForm" }))
        {
        } %>
    <% } %>
    <% if (Model.OrderAccepted || Model.Order.OrderItems.Count == 0)
        { %>
    <p>
        <% = Html.ActionLink("Return to Stock Status", "Index", "StockStatus") %>
    </p>
    <% } %>
</div>
<script type="text/javascript">
    /*Moved from OrderForm-2.1.js by JH for EPWI-132*/
    $(document).on('click', 'input.UpdateCustomerReference', function () {
        var parent = $(this).parents('div.CustomerReference');
        var orderItemId = parent.attr('data-order-item-id');
        var customerReference = parent.find('input:text').val();
        parent.find('input').attr('disabled', 'disabled');
        $.ajax({ url: $('#UpdateCustomerReferenceAction').val(), type: 'post', data: ({ id: orderItemId, customerReference: customerReference }), complete: function () { updateCustomerReferenceComplete(parent); }, success: function (html) { updateCustomerReferenceSuccess(parent, html); } });
    });

   $(document).on('click', 'a.EditCustomerReference', function () {
        toggleCustomerReferenceElements($(this));
        return false;
    });

    $('#toggleCustomerReference').click(function () {
        toggleCustomerReference();
        return false;
    });

    $(document).on('click', '#SubmitNotesChange', function () {
        $('#SaveNotesIndicator').show();
        $.ajax({ url: $('#SaveNotesAction').val(), type: 'post', data: ({ orderNotes: $('#OrderNotes').val() }), error: handleJqueryAjaxError, complete: function () { $('#SaveNotesIndicator').hide(); } });
    });


    function toggleCustomerReference() {
        $('.customer-reference-toggle').toggle();
    }

    function updateCustomerReferenceComplete(parent) {
        parent.find('input').removeAttr('disabled');
    }

    function updateCustomerReferenceSuccess(target, html) {
        target.html(html);
    }

    function toggleCustomerReferenceElements(src) {
        $(src).parents('.CustomerReference').find('.EditCustomerReference').slideToggle();
    }

</script>