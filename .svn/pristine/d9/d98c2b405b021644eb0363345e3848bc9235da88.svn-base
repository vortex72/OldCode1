<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Web.Models.StockStatusViewModel>" %>
<div id="OrderOptions">
    <%="" %>
    <table id="stockStatusOrderOptions" class="stockStatusBlock" style="white-space: normal" border="0" cellpadding="0"
        cellspacing="0" width="100%">
        <tr>
            <th colspan="2">Order Options
            </th>
        </tr>
        <tr>
            <td colspan="2">
                <%  
                    if (Model.InventoryItem.IsKit)
                    {
                %>
                <% if (!Model.StockStatus.IsCrankKit && Page.User.IsInRole("KIT_BUILDER"))
                    { %>
                <% if (Model.SavedKitConfigurationID == 0)
                    { %>
                <% = Html.ActionLink("Configure and Order this Kit", "Build", "Kit", new { id = Model.RequestedItemNumber }, null) %>
                <% }
                else { %>
                <a href="#" id="ConfigureKitLink">Configure and Order this Kit</a>
                <% Html.RenderPartial("KitExistsDialog", Model.RequestedItemNumber); %>
                <% } %><br />
                <% }
                else { %>
                <% using (Html.BeginForm("AddItem", "Order", new { orderMethod1 = OrderMethod.MainWarehouse, warehouse1 = Model.StockStatus.CustomerDefaultWarehouse, quantity1 = Model.RequestedQuantity }, FormMethod.Post, new { @class = "OrderForm" }))
                    { %>
                <div><% = Html.Hidden("localPickup", false, new { @class = "localpickup", id = "localpickup1" })%></div>
                <% Html.RenderPartial("CustomerReference"); %>
                <div>
                    <input type="submit" value="Add to Order" class="OrderButton" />
                </div>
                <% } %>
                <%  } %>
                <% if (!Model.StockStatus.IsCrankKit)
                    { %>
                <% = Html.ActionLink("View Kit as Online Catalog", "View", new { controller = "Kit", id = Model.RequestedItemNumber })%>
                <% } %>
                <% }
                    else {   // walk through order method availabilities for all non-kits %>
                <% Html.RenderPartial("CustomerReference"); %>
                <a href="#" id="AddToOrderLink">Add to Order</a><br />
                <div class="dialog" id="OrderOptionsDialog">
                    <div id="ShipmentOptionErrors" style="display: none"></div>
                    <div>Multiple ordering options exist. Please select one of the options below to add this item to your order.</div>
                    <br />
                    <% if (Model.StockStatus.OrderMethodAvailability(OrderMethod.MainWarehouse))
                        { %>
                    <% using (Html.BeginForm("AddItem", "Order", new { orderMethod1 = OrderMethod.MainWarehouse, warehouse1 = Model.StockStatus.CustomerDefaultWarehouse, quantity1 = Model.RequestedQuantity }, FormMethod.Post, new { @class = "OrderForm" }))
                        { %>
                    <div><% = Html.Hidden("localPickup", false, new { @class = "localpickup", id = "localpickup2" })%></div>
                    <div>
                        <input type="submit" value="Add to Order" class="OrderButton" />
                    </div>
                    <% } %>
                    <% } %>

                    <% if (Model.StockStatus.OrderMethodAvailability(OrderMethod.MainAndSecondaryWarehouse))
                        { %>
                    <% using (Html.BeginForm("AddItem", "Order", new { orderMethod1 = OrderMethod.MainWarehouse, warehouse1 = Model.StockStatus.CustomerDefaultWarehouse, quantity1 = Model.StockStatus.WarehouseAvailability(Model.StockStatus.CustomerDefaultWarehouse), orderMethod2 = OrderMethod.MainAndSecondaryWarehouse, warehouse2 = Model.StockStatus.CustomerSecondaryWarehouse, quantity2 = Model.RequestedQuantity - Model.StockStatus.WarehouseAvailability(Model.StockStatus.CustomerDefaultWarehouse) }, FormMethod.Post, new { @class = "OrderForm" }))
                        { %>
                    <% = Html.Hidden("localPickup", false, new { @class = "localpickup", id = "localpickup3" }) %>
                    <div>
                        <input type="submit" value="Add to Order but Ship From <% =Model.StockStatus.CustomerDefaultWarehouse %> and <% = Model.StockStatus.CustomerSecondaryWarehouse %>" class="OrderButton" />
                    </div>
                    <% } %>
                    <% } %>

                    <% if (Model.StockStatus.OrderMethodAvailability(OrderMethod.SecondaryWarehouse))
                        { %>
                    <% using (Html.BeginForm("AddItem", "Order", new { orderMethod1 = OrderMethod.SecondaryWarehouse, warehouse1 = Model.StockStatus.CustomerSecondaryWarehouse, quantity1 = Model.RequestedQuantity }, FormMethod.Post, new { @class = "OrderForm" }))
                        { %>
                    <div><% = Html.Hidden("localPickup", false, new { @class = "localpickup", id = "localpickup4" })%></div>
                    <div>
                        <input type="submit" value="Add to Order but Ship From <% = Model.StockStatus.CustomerSecondaryWarehouse %>" class="OrderButton" />
                    </div>
                    <% } %>
                    <% } %>

                    <% if (Model.StockStatus.OrderMethodAvailability(OrderMethod.MainAndOtherWarehouse))
                        { %>
                    <% using (Html.BeginForm("AddItem", "Order", new { orderMethod1 = OrderMethod.MainWarehouse, warehouse1 = Model.StockStatus.CustomerDefaultWarehouse, quantity1 = Model.StockStatus.WarehouseAvailability(Model.StockStatus.CustomerDefaultWarehouse), orderMethod2 = OrderMethod.MainAndOtherWarehouse, warehouse2 = "XXX", quantity2 = Model.RequestedQuantity - Model.StockStatus.WarehouseAvailability(Model.StockStatus.CustomerDefaultWarehouse) }, FormMethod.Post, new { id = "WarehouseOptions2", @class = "OrderForm" }))
                        { %>
                    <div><% = Html.Hidden("localPickup", false, new { @class = "localpickup", id = "localpickup5" })%></div>
                    <div>
                        <input type="submit" value="Add to Order but Ship From <% = Model.StockStatus.CustomerDefaultWarehouse %> and the Following Location:" class="OrderButton" />
                        <div>
                            <% foreach (string warehouse in WarehouseList.Warehouses)
                                { %>
                            <% = Html.WarehouseRadioButton("warehouse2", Model.StockStatus, warehouse, true) %>
                            <% } %>
                        </div>
                    </div>
                    <% } %>
                    <% } %>

                    <% if (Model.StockStatus.OrderMethodAvailability(OrderMethod.OtherWarehouse))
                        { %>
                    <% using (Html.BeginForm("AddItem", "Order", new { orderMethod1 = OrderMethod.OtherWarehouse, warehouse1 = "XXX", quantity1 = Model.RequestedQuantity }, FormMethod.Post, new { id = "WarehouseOptions1", @class = "OrderForm" }))
                        { %>
                    <div><% = Html.Hidden("localPickup", false, new { @class = "localpickup", id = "localpickup6" })%></div>
                    <div>
                        <input type="submit" value="Add to Order but Ship From the Following Location:" class="OrderButton" />
                        <div>
                            <% foreach (string warehouse in WarehouseList.Warehouses)
                                { %>
                            <% = Html.WarehouseRadioButton("warehouse1", Model.StockStatus, warehouse, false)%>
                            <% } %>
                        </div>
                    </div>
                    <% } %>
                    <% } %>

                    <% if (Model.StockStatus.OrderMethodAvailability(OrderMethod.DropShip))
                        { %>
                    <% using (Html.BeginForm("AddItem", "Order", new { orderMethod1 = OrderMethod.DropShip, quantity1 = Model.RequestedQuantity }, FormMethod.Post, new { @class = "OrderForm" }))
                        { %>
                    <div><% = Html.Hidden("localPickup", false, new { @class = "localpickup", id = "localpickup7" })%></div>
                    <div>
                        <input type="submit" value="Add to Order but Request Factory Drop Ship" class="OrderButton" />
                    </div>
                    <% } %>
                    <% } %>

                    <% if (Model.StockStatus.OrderMethodAvailability(OrderMethod.LocalPickup))
                        { %>
                    <div>
                        <% = Html.CheckBox("chkLocalPickup") %>Request Local Pickup As First Option To Ship<br />
                        (Please select an alternate option above if local pickup is not available)
                    </div>
                    <% } %>

                    <% if (Model.StockStatus.OrderMethodAvailability(OrderMethod.Manual))
                        { %>
                    <% using (Html.BeginForm("AddItem", "Order", new { orderMethod1 = OrderMethod.Manual, quantity1 = Model.RequestedQuantity }, FormMethod.Post, new { @class = "OrderForm" }))
                        { %>
                    <div><% = Html.Hidden("localPickup", false, new { @class = "localpickup", id = "localpickup8" })%></div>
                    <div>
                        <input type="submit" value="Add to Order but Request Manual Processing" class="OrderButton" />
                    </div>
                    <% } %>
                    <% } %>
                </div>
                <% }
                %>
            </td>
        </tr>
    </table>
</div>
