<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Web.Models.StockStatusViewModel>" %>
<% if ((Model.StockStatus.OrderMethodAvailability(OrderMethod.MainAndOtherWarehouse) || Model.StockStatus.OrderMethodAvailability(OrderMethod.OtherWarehouse)) && !Model.InventoryItem.IsKit) {  %>
    <table id="stockStatusWarehouses" class="stockStatusBlock" border="0" cellpadding="0" cellspacing="0" width="100%" >
                      <tr>
                        <th>Warehouse Availability</th>
                      </tr>
                	    <tr>
                  	        <td style="white-space:normal;">
                                <% if (Model.StockStatus.CustomerDefaultWarehouse != "DAL" && Model.StockStatus.WarehouseAvailability("DAL") + (Model.StockStatus.OrderMethodAvailability(OrderMethod.MainAndOtherWarehouse) ? Model.StockStatus.WarehouseAvailability(Model.StockStatus.CustomerDefaultWarehouse) : 0) >= Model.RequestedQuantity) 
                                   { %><span>DAL </span><% } %>
                                <% if (Model.StockStatus.CustomerDefaultWarehouse != "HOU" && Model.StockStatus.WarehouseAvailability("HOU") + (Model.StockStatus.OrderMethodAvailability(OrderMethod.MainAndOtherWarehouse) ? Model.StockStatus.WarehouseAvailability(Model.StockStatus.CustomerDefaultWarehouse) : 0) >= Model.RequestedQuantity) 
                                   { %><span>HOU </span><% } %>
                                <% if (Model.StockStatus.CustomerDefaultWarehouse != "OAK" && Model.StockStatus.WarehouseAvailability("OAK") + (Model.StockStatus.OrderMethodAvailability(OrderMethod.MainAndOtherWarehouse) ? Model.StockStatus.WarehouseAvailability(Model.StockStatus.CustomerDefaultWarehouse) : 0) >= Model.RequestedQuantity) 
                                   { %><span>OAK </span><% } %>
                                <% if (Model.StockStatus.CustomerDefaultWarehouse != "OKC" && Model.StockStatus.WarehouseAvailability("OKC") + (Model.StockStatus.OrderMethodAvailability(OrderMethod.MainAndOtherWarehouse) ? Model.StockStatus.WarehouseAvailability(Model.StockStatus.CustomerDefaultWarehouse) : 0) >= Model.RequestedQuantity)
                                   { %><span>OKC </span><% } %>
                                <% if (Model.StockStatus.CustomerDefaultWarehouse != "SAN" && Model.StockStatus.WarehouseAvailability("SAN") + (Model.StockStatus.OrderMethodAvailability(OrderMethod.MainAndOtherWarehouse) ? Model.StockStatus.WarehouseAvailability(Model.StockStatus.CustomerDefaultWarehouse) : 0) >= Model.RequestedQuantity)
                                   { %><span>SAN </span><% } %>
                                <% if (Model.StockStatus.CustomerDefaultWarehouse != "ALB" && Model.StockStatus.WarehouseAvailability("ALB") + (Model.StockStatus.OrderMethodAvailability(OrderMethod.MainAndOtherWarehouse) ? Model.StockStatus.WarehouseAvailability(Model.StockStatus.CustomerDefaultWarehouse) : 0) >= Model.RequestedQuantity)
                                   { %><span>ALB </span><% } %>
                                <% if (Model.StockStatus.CustomerDefaultWarehouse != "DEN" && Model.StockStatus.WarehouseAvailability("DEN") + (Model.StockStatus.OrderMethodAvailability(OrderMethod.MainAndOtherWarehouse) ? Model.StockStatus.WarehouseAvailability(Model.StockStatus.CustomerDefaultWarehouse) : 0) >= Model.RequestedQuantity)
                                   { %><span>DEN </span><% } %>
                                <% if (Model.StockStatus.CustomerDefaultWarehouse != "PHX" && Model.StockStatus.WarehouseAvailability("PHX") + (Model.StockStatus.OrderMethodAvailability(OrderMethod.MainAndOtherWarehouse) ? Model.StockStatus.WarehouseAvailability(Model.StockStatus.CustomerDefaultWarehouse) : 0) >= Model.RequestedQuantity)
                                   { %><span>PHX </span><% } %>
                                <% if (Model.StockStatus.CustomerDefaultWarehouse != "ANC" && Model.StockStatus.WarehouseAvailability("ANC") + (Model.StockStatus.OrderMethodAvailability(OrderMethod.MainAndOtherWarehouse) ? Model.StockStatus.WarehouseAvailability(Model.StockStatus.CustomerDefaultWarehouse) : 0) >= Model.RequestedQuantity)
                                   { %><span>ANC </span><% } %>
                                <% if (Model.StockStatus.CustomerDefaultWarehouse != "PDX" && Model.StockStatus.WarehouseAvailability("PDX") + (Model.StockStatus.OrderMethodAvailability(OrderMethod.MainAndOtherWarehouse) ? Model.StockStatus.WarehouseAvailability(Model.StockStatus.CustomerDefaultWarehouse) : 0) >= Model.RequestedQuantity)
                                   { %><span>PDX </span><% } %>
                                <% if (Model.StockStatus.CustomerDefaultWarehouse != "TAC" && Model.StockStatus.WarehouseAvailability("TAC") + (Model.StockStatus.OrderMethodAvailability(OrderMethod.MainAndOtherWarehouse) ? Model.StockStatus.WarehouseAvailability(Model.StockStatus.CustomerDefaultWarehouse) : 0) >= Model.RequestedQuantity)
                                   { %><span>TAC </span><% } %>
                                <% if (Model.StockStatus.CustomerDefaultWarehouse != "LA1" && Model.StockStatus.WarehouseAvailability("LA1") + (Model.StockStatus.OrderMethodAvailability(OrderMethod.MainAndOtherWarehouse) ? Model.StockStatus.WarehouseAvailability(Model.StockStatus.CustomerDefaultWarehouse) : 0) >= Model.RequestedQuantity)
                                   { %><span>LA1 </span><% } %>
                  	        </td>
                      </tr>
                    </table>
                    <div>&nbsp;</div>
<% } %>
