<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Components.Models.Invoice>" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td>
          <table width="100%" border="0" cellspacing="2" cellpadding="0">
            <tr> 
              <td width="1%" rowspan="2" align="center">
                <img src="<% = Url.Content("~/Content/images/invoice_std_logo.gif") %>" width="106" height="106">
              </td>
              <td colspan="3" style="text-align:center;font-weight:bold;font-size:12pt;letter-spacing:0.1em;">
                ENGINE and PERFORMANCE WAREHOUSE<% if (Model.CompanyCode == "S") { %> SOUTH<% } %>, INC.
              </td>
            </tr>
            <tr>
              <td  align="center" valign="top" style="font-size:x-small;">
                    <b>Corporate Office</b><br />
                    <% = Html.Encode(Model.CompanyAddress.StreetAddress1) %><br />
                    <% if (!string.IsNullOrEmpty(Model.CompanyAddress.StreetAddress2)) { %><% = Html.Encode(Model.CompanyAddress.StreetAddress2) %><br /><% } %>
                    <% = Html.Encode(Model.CompanyAddress.City) %>, <% = Html.Encode(Model.CompanyAddress.State) %> <% = Html.Encode(Model.CompanyAddress.Zip) %><br />
                    &nbsp;<br/>
                    <% = Html.Encode(Model.CompanyAddress.Phone) %>&nbsp;&nbsp;|&nbsp;&nbsp;<% = Html.Encode(Model.CompanyAddress.AlternatePhone) %><br />
                    FAX <% = Html.Encode(Model.CompanyAddress.Fax) %><br />
              </td> 
              <% if (Model.ShowCustomerWarehouse) { %>
                <td width="33%" align="center" valign="top" style="font-size:x-small">
                    <% = Html.Encode(Model.CustomerWarehouseAddress.StreetAddress1) %><br />
                    <% if (!string.IsNullOrEmpty(Model.CustomerWarehouseAddress.StreetAddress2)) { %><% = Html.Encode(Model.CustomerWarehouseAddress.StreetAddress2) %><br /><% } %>
                    <% = Html.Encode(Model.CustomerWarehouseAddress.City) %>, <% = Html.Encode(Model.CustomerWarehouseAddress.State) %> <% = Html.Encode(Model.CustomerWarehouseAddress.Zip) %><br />
                    &nbsp;<br />
                    <% = Html.Encode(Model.CustomerWarehouseAddress.Phone) %>&nbsp;&nbsp;|&nbsp;&nbsp;<% = Html.Encode(Model.CustomerWarehouseAddress.AlternatePhone) %><br />
                    FAX <% = Html.Encode(Model.CustomerWarehouseAddress.Fax) %><br />
                </td>
              <% } else { %>
                <td align="center" valign="top">&nbsp;</td>
              <% } %>
              <% if (Model.ShowShippingWarehouse) { %>
                <% = Html.Encode(Model.ShippingWarehouseAddress.StreetAddress1) %><br />
                <% if (!string.IsNullOrEmpty(Model.ShippingWarehouseAddress.StreetAddress2)) { %><% = Html.Encode(Model.ShippingWarehouseAddress.StreetAddress2) %><br /><% } %>
                <% = Html.Encode(Model.ShippingWarehouseAddress.City) %>, <% = Html.Encode(Model.ShippingWarehouseAddress.State) %> <% = Html.Encode(Model.ShippingWarehouseAddress.Zip) %><br />
                &nbsp;<br />
                <% = Html.Encode(Model.ShippingWarehouseAddress.Phone) %>&nbsp;&nbsp;|&nbsp;&nbsp;<% = Html.Encode(Model.ShippingWarehouseAddress.AlternatePhone) %><br />
                FAX <% = Html.Encode(Model.ShippingWarehouseAddress.Fax) %><br />
              <% } else { %>
                <td align="center" valign="top">&nbsp;</td>
              <% } %>               
            </tr>
            <tr> 
              <td width="1%" colspan="4" style="text-align:center;font-weight:bold;font-size:12pt;letter-spacing:0.2em;">
                <% if (Model.Type == "C") { %>CREDIT MEMO<% } else { %>INVOICE<% } %>
              </td>
            </tr>
          </table>
          <table width="100%" border="0" cellspacing="2" cellpadding="0">
            <tr> 
              <td valign="top" width="1%" nowrap="nowrap">Bill To:</td>
              <td valign="top" width="80%" nowrap="nowrap"><b>
                <% if (!string.IsNullOrEmpty(Model.BillToAddress.Name)) { %><% = Html.Encode(Model.BillToAddress.Name) %><br /><% } %>
                <% if (!string.IsNullOrEmpty(Model.BillToAddress.StreetAddress1)) { %><% = Html.Encode(Model.BillToAddress.StreetAddress1) %><br /><% } %>
                <% if (!string.IsNullOrEmpty(Model.BillToAddress.StreetAddress2)) { %><% = Html.Encode(Model.BillToAddress.StreetAddress2) %><br /><% } %>
                <% if (!string.IsNullOrEmpty(Model.BillToAddress.City)) { %><% = Html.Encode(Model.BillToAddress.City) %>, <% = Html.Encode(Model.BillToAddress.State) %> <% = Html.Encode(Model.BillToAddress.Zip) %><% if(!string.IsNullOrEmpty(Model.BillToAddress.Zip4) && Model.BillToAddress.Zip4 != "0") { %>-<% = Html.Encode(Model.BillToAddress.Zip4) %><% } %><% } %>
                <br /><% = Html.FormattedPhone(Model.BillToAddress.Phone) %>
                </b>
              </td>
              <td valign="top" width="20%" nowrap="nowrap">
                Create Date: <b><%= Html.Encode(Model.CreateDate.ToString("M/d/yyyy")) %></b><br />
                Ship Date: <b>
                <% if (Model.ShipmentPending) { %>PENDING<% } else { %><% = Html.Encode(Model.ShipDate.ToString("M/d/yyyy")) %><% } %>
                </b><br />
                Invoice Number: <b><%= Html.Encode(Model.InvoiceNumber) %></b><br />
                Associate: <b><%= Html.Encode(Model.Associate) %></b>
              </td>
            </tr>
            <tr> 
              <td colspan="3" valign="top" nowrap="nowrap">&nbsp;</td>
            </tr>
            <tr> 
              <td valign="top" width="1%" nowrap="nowrap">Sold To:</td>
              <td valign="top" width="80%" nowrap="nowrap"><b>
                <% if (!string.IsNullOrEmpty(Model.SoldToAddress.Name)) { %><% = Html.Encode(Model.SoldToAddress.Name) %><br /><% } %>
                <% if (!string.IsNullOrEmpty(Model.SoldToAddress.StreetAddress1)) { %><% = Html.Encode(Model.SoldToAddress.StreetAddress1) %><br /><% } %>
                <% if (!string.IsNullOrEmpty(Model.SoldToAddress.StreetAddress2)) { %><% = Html.Encode(Model.SoldToAddress.StreetAddress2) %><br /><% } %>
                <% if (!string.IsNullOrEmpty(Model.SoldToAddress.City)) { %><% = Html.Encode(Model.SoldToAddress.City) %>, <% = Html.Encode(Model.SoldToAddress.State) %> <% = Html.Encode(Model.SoldToAddress.Zip) %><% if(!string.IsNullOrEmpty(Model.SoldToAddress.Zip4) && Model.SoldToAddress.Zip4 != "0") { %>-<% = Html.Encode(Model.SoldToAddress.Zip4) %><% } %><% } %>
                <br /><% = Html.FormattedPhone(Model.SoldToAddress.Phone) %>
                </b>
              </td>
              <td valign="top" width="20%" nowrap="nowrap">&nbsp;</td>
            </tr>
            <tr> 
              <td colspan="3" valign="top" nowrap="nowrap">&nbsp;</td>
            </tr>
            <tr> 
              <td width="1%" valign="top" nowrap="nowrap">Ship To:</td>
              <td valign="top"><b>
                <% if (!string.IsNullOrEmpty(Model.ShipToAddress.Name)) { %><% = Html.Encode(Model.ShipToAddress.Name) %><br /><% } %>
                <% if (!string.IsNullOrEmpty(Model.ShipToAddress.StreetAddress1)) { %><% = Html.Encode(Model.ShipToAddress.StreetAddress1) %><br /><% } %>
                <% if (!string.IsNullOrEmpty(Model.ShipToAddress.City)) { %><% = Html.Encode(Model.ShipToAddress.City) %>, <% = Html.Encode(Model.ShipToAddress.State) %> <% = Html.Encode(Model.ShipToAddress.Zip) %><% if(!string.IsNullOrEmpty(Model.ShipToAddress.Zip4) && Model.ShipToAddress.Zip4 != "0") { %>-<% = Html.Encode(Model.ShipToAddress.Zip4) %><% } %><% } %>
                
                <% if (Model.TrackingNumbers.Any()) { %>
                    <br />Carrier Tracking Number(s):<br />
                <% } %>
                
                <% foreach (string trackingNumber in Model.TrackingNumbers) { %>
                    <% if (!string.IsNullOrEmpty(trackingNumber) && trackingNumber.Length > 1 && Char.IsLetter(trackingNumber[1])) { %>
                        <a href="http://wwwapps.ups.com/WebTracking/processRequest?tracknum=<% = Html.Encode(trackingNumber) %>" target="_blank"><% = Html.Encode(trackingNumber) %></a><br />
                    <% } else { %>
                        <a href="http://www.fedex.com/Tracking?action=track&tracknumbers=<% = Html.Encode(trackingNumber) %>" target="_blank"><% = Html.Encode(trackingNumber) %></a><br />                    
                    <% } %>
                <% } %>
                </b>
              </td>
              <td valign="top" nowrap="nowrap">
                Shipped From: <b><%= Html.Encode(Model.ShippingWarehouse) %></b><br />
                Shipped Via: <b><%= Html.Encode(Model.ShippingCarrier) %></b><br />
                Terms: <b><%= Html.Encode(Model.Terms) %></b><br/>
                PO Number: <b><%= Html.Encode(Model.PONumber) %></b>
              </td>
            </tr>
            <tr> 
              <td colspan="3" valign="top" nowrap="nowrap">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="3" valign="top" nowrap="nowrap">
                Customer Notes:<br />
                <% foreach (var customerNote in Model.CustomerNotes) { %>
                    <% = Html.Encode(customerNote) %><br />
                <% } %>
                </td>
            </tr>
            <tr> 
              <td colspan="3" valign="top" nowrap="nowrap">
              Order Notes:<br />
              <%= Html.Encode(Model.OrderNotes1) %><br />
              <%= Html.Encode(Model.OrderNotes2) %><br />
              </td>
            </tr>
            <tr> 
              <td colspan="3" valign="top" nowrap="nowrap">&nbsp;</td>
            </tr>
          </table>
          <table width="100%" border="0" cellspacing="0" cellpadding="1" class="invoice">
            <tr> 
              <th align="center">Qty.<br />Ordered</th>
              <th align="center">Qty.<br />Shipped</th>
              <th align="left">Line</th>
              <th align="left">Item Number</th>
              <th>Size</th>
              <th align="left">Description</th>
              <th>Whse</th>
              <th align="center">P.O.</th>
              <th align="right">Indiv. Price</th>
              <th align="right">Total Price</th>
            </tr>
            <% foreach (var item in Model.InvoiceDetails.Where(d => d.SequenceNumber < 9000)) { /* Sequence Numbers of 9000+ are special charges.  Don't display them here.*/  %>
                <% if (item.KitComponent != "C") { %>
                    <tr><td colspan="10" style="background-color:#CCCCCC;height:0em"></td></tr>    
                <% } %>
            <tr>
              <td class="invdetail" align="center"><%= Html.Encode(item.QuantityOrdered) %></td>
              <td class="invdetail" align="center"><%= Html.Encode(item.QuantityShipped) %></td>
              <% if (item.KitComponent == "K") { %>
              <td class="invdetail" style="font-weight:bold;"><%= Html.Encode(item.LineCode) %></td>
              <td class="invdetail" style="font-weight:bold;"><%= Html.Encode(item.ItemNumber) %></td>
              <% } else {  %>
              <td class="invdetail"><%= Html.Encode(item.LineCode) %></td>
              <td class="invdetail"><%= Html.Encode(item.ItemNumber) %></td>
              <% } %>
              <td class="invdetail" align="center"><%= Html.Encode(item.SizeCode) %></td>
              <td class="invdetail"><%= Html.Encode(item.Description) %></td>
              <% if (item.KitComponent != "C") { %>
              <td class="invdetail" align="center"><%= Html.Encode(item.Warehouse) %></td>
              <td class="invdetail" align="center"><%= Html.Encode(item.PONumber) %></td>
              <td class="invdetail" align="right"><%= Html.Encode(item.IndividualPrice.ToString("C2")) %></td>
              <td class="invdetail" align="right"><%= Html.Encode(item.NetPrice.ToString("C2")) %></td>
              <% } else { %>
              <td class="invdetail" colspan="6">&nbsp;</td>
              <% } %>
              <% if (!string.IsNullOrEmpty(item.Notes)) { %>
               <tr>
                <td colspan="2">&nbsp;</td>
                <td colspan="8" style="color:#990000;">Note: <%= Html.Encode(item.Notes) %></td>
              </tr>
              <% } %>
              <% if (item.ShowShipFromInfo) { %>
               <tr>
                <td>&nbsp;</td>
                <td colspan="9" style="color:#990000;">The above was shipped From: <% = Html.Encode(item.ShipFromCompany) %> <% = Html.Encode(item.ShipFromWarehouse) %></td>
               </tr>
              <% } %>
            <% } %>
            <tr> 
              <th colspan="9" align="right" style="font-size:10pt;">Merchandise Total:</th>
              <th align="right" style="font-size:10pt;"><%= Html.Encode(Model.MerchandiseTotal.ToString("C2")) %></th>
            </tr>
            <% int lineCode = 0; foreach (var item in Model.InvoiceDetails.Where(d => d.SequenceNumber >= 9000 && int.TryParse(d.LineCode, out lineCode) && lineCode < 9000))
               {
                   /* Sequence Numbers of 9000+ are special charges.  Do display them here. 
                     Line Numbers 8000 - 8999 are special charges */ 
            %>
            <tr>
              <td class="invdetail" align="right" colspan="7"><%= Html.Encode(item.LineCode)%></td>
              <td class="invdetail" align="right" colspan="2"><%= Html.Encode(item.ItemNumber)%>:</td>
              <td class="invdetail" align="right"><%= Html.Encode(item.NetPrice.ToString("C2"))%></td>
            </tr>
            <% } %>
            <tr> 
              <th colspan="9" align="right" style="font-size:10pt;">Invoice Total:</th>
              <th align="right" style="font-size:10pt;"><b><% = Html.Encode(Model.InvoiceTotal.ToString("C2")) %></b></th>
            </tr>
            <% lineCode = 0; foreach (var item in Model.InvoiceDetails.Where(d => d.SequenceNumber >= 9000 && int.TryParse(d.LineCode, out lineCode) && lineCode >= 9000))
               {
                   /* Sequence Numbers of 9000+ are special charges.  Do display them here. 
                     Line Numbers 9000+ are special charges */ 
            %>
            <tr>
              <td class="invdetail" align="right" colspan="7"><%= Html.Encode(item.LineCode)%></td>
              <td class="invdetail" align="right" colspan="2"><%= Html.Encode(item.ItemNumber)%>:</td>
              <td class="invdetail" align="right"><%= Html.Encode(item.NetPrice.ToString("C2"))%></td>
            </tr>
            <% } %>
            <tr> 
              <th colspan="9" align="right" style="font-size:10pt;">
                <% if (Model.ShipmentPending) { %>
                  <span style="color:#990000;">Pending invoices are likely to change and do not include freight.</span> &nbsp;
                <% } %>
                TOTAL DUE:
              </th>
              <th align="right" style="font-size:10pt;"><b><%= Model.TotalDue.ToString("C2") %></b></th>
            </tr>
          </table>    
        </td>
      </tr>
    </table>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td width="99%">
          Acceptance of the goods detailed in this invoice constitutes agreement that any past due amounts shall accrue interest at the rate of 1 1/2 percent per month, not to exceed the maximum rate allowed by law.<br />
          <b>THIS IS YOUR INVOICE - DUE AND PAYABLE IN <% if (Model.CompanyCode == "S") { %>DALLAS, TEXAS<% } else {%>DENVER, COLORADO<% } %></b>
        </td>
        <td width="1%">
          <img src="<% = Url.Content("~/Content/images/invoice_net_logo.gif")%>" width="114" height="50" />
        </td>
        <td width="1%">
          <img src="<% = Url.Content("~/Content/images/invoice_enginepro_logo.gif")%>" width="75" height="50" />
        </td>
      </tr>
    </table>


