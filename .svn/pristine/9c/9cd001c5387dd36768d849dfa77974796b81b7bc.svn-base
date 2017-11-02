<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Web.Models.KitStockStatusViewModel>" %>
<% if (Model.StockStatus.OrderMethodAvailability(OrderMethod.MainWarehouse)) { %>    
        <div>
            <a href="#" id="AddPartMain">Add to Kit</a>
        </div>
<% } %>
 
<% if (Model.StockStatus.OrderMethodAvailability(OrderMethod.SecondaryWarehouse)) { %>    
        <div>
            <a href="#" id="AddPartSecondary">Add to Kit but Ship From <% = Model.StockStatus.CustomerSecondaryWarehouse %></a>
        </div>
<% } %>

<% if (Model.StockStatus.OrderMethodAvailability(OrderMethod.OtherWarehouse)) { %>
        <form id="ShipmentOptions">
            <div>
                <div id="ShipmentOptionErrors" style="display:none" class="erroralert">Please select a warehouse.</div>
                <a href="#" id="AddPartOther">Add to Kit but Ship From the Following Location</a>
                <div>
                    <% foreach(string warehouse in WarehouseList.Warehouses) { %>
                        <% = Html.WarehouseRadioButton("warehouse", Model.StockStatus, warehouse, false) %>
                    <% } %>
                </div>
            </div>
        </form>
<% } %>

<% if (Model.StockStatus.OrderMethodAvailability(OrderMethod.DropShip)) { %>
        <div>
            <a href="#" id="AddPartDropShip">Add to Kit but Request Factory Drop Ship</a>
        </div>
<% } %>

<% if (Model.StockStatus.OrderMethodAvailability(OrderMethod.Manual)) { %>
        <div>
            <a href="#" id="AddPartManual">Add to Kit but Request Manual Processing</a>
        </div>
<% } %>
