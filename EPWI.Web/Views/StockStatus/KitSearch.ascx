<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Web.Models.KitStockStatusViewModel>" %>

<% if (!string.IsNullOrEmpty(Model.RequestedItemNumber)) { %>
<% using (Ajax.BeginForm("KitSearch", new { OriginalPartUniqueIdentifier = Model.OriginalPartUniqueIdentifier, KitNipc = Model.KitNipc, OriginalPartPrice = Model.OriginalPartPrice }, new AjaxOptions { UpdateTargetId = "InterchangeDialog", OnFailure = "ajaxError" }, new { id = "NewKitSearch", margin = "0", padding = "0" })) {%>
<div id="NewKitSearchContent" style="display:none">
<input type="submit" id="HiddenSubmit" />
</div>
<% } %>
<% } %>
<% using (Ajax.BeginForm("KitSearch", "StockStatus", new AjaxOptions { UpdateTargetId = "InterchangeDialog", OnFailure = "ajaxError", OnBegin = "displayProcessingBlock", OnSuccess = "hideProcessingBlock" }, new { margin = "0", padding = "0", id = "KitSearch" })) { %>
<table border="0" cellpadding="0" cellspacing="0" width="100%" class="InterchangeStockStatus">  
  <tr>
    <td>Quantity:</td>
    <td>
        <% = Html.TextBox("RequestedQuantity", null, new { @class = "SearchField", maxlength = "3" })%>
        <% if (Model.RequestComplete || !Model.CanPlaceOrder) { %><input type="submit" value="Search" /><% } %>        
    </td>
  </tr>
  <tr>
    <td>Item Number:</td>
    <td>
        <% if (Model.InventoryItems == null || Model.InventoryItems.Count() == 0) { %>
            <%=Html.TextBox("RequestedItemNumber", null, new { @class = "SearchField", maxlength = "25" } )%>
            <input type="submit" value="Submit" /> 
        <% } else { %>
            <% = Html.Hidden("RequestedItemNumber", Model.RequestedItemNumber)%>
            <% = Html.Encode(Model.RequestedItemNumber)%>
            <% if (Model.ItemNumberChanged) { %>
                <div class="highlight">Note: The Item Wizard has modified the part number entered (<% = Html.Encode(Model.OriginalItemNumber)  %>)</div>
            <% } %>
        <% } %>
    </td>
  </tr>
  <tr>
    <td>Size:</td>
    <td>
        <% if (Model.NeedToSelectSize) { %>
            <% = Html.DropDownList("RequestedSize", Model.InventoryItem.Sizes.Select(s => new SelectListItem() { Text = (s == "STD" ? "Standard" : s.Substring(0, 3).Trim()), Value = s.Substring(0, 3).Trim() }), new { @class = "SearchField" } )%>
            <input type="submit" value="Submit" /> 
        <% } else { %>
            <%  if (!string.IsNullOrEmpty(Model.RequestedSize)) { %> 
                <% = Html.Hidden("RequestedSize", Model.RequestedSize)%> 
            <% } %>
            <% = Html.Encode(Model.RequestedSize)%>
        <% } %>
    </td>
  </tr>
  <tr>
    <td>Line Code:</td>
    <td>
        <% if (Model.NeedToSelectLineCode) { %>
            <% =Html.DropDownList("RequestedLineCode", Model.InventoryItems.Select(i => new SelectListItem() { Text = $"{i.LineDescription} (PN:{i.ItemNumber})", Value = $"{i.LineCode}|{i.ItemNumber}"}), "--- Please Select a Product Line ---", new { @class = "SearchField" })%>
            <input type="submit" value="Submit" /> 
        <% } else { %>
            <%   if (!string.IsNullOrEmpty(Model.RequestedLineCode)){ %> 
                <% = Html.Hidden("RequestedLineCode", Model.RequestedLineCode)%> 
            <% } %>
            <% = Html.Encode(Model.RequestedLineCodeDescription)%>
        <% } %>
    </td>
  </tr>                      
  <% if (!string.IsNullOrEmpty(Model.RequestedItemNumber)) { %>
  <tr>
    <td>Item Options:</td>
    <td>
        <a href="#" id="CheckAnotherPart">Check Another Part</a>
    </td>
  </tr>
  <% } %>
  <tr>

    <td>Description:</td>
    <td>
        <% if (Model.RequestComplete) { %>
            <%= Html.Encode(Model.InventoryItem.ItemDescription) %>
        <% } %> 
    </td>
  </tr>
  <tr>
    <td>Messages:</td>
    <td>
        <% = Html.ValidationSummary(string.Empty, new { @class = "highlight error" })%>
        <% if (Model.StockStatus != null && Model.StockStatus.IsSuperseded && !string.IsNullOrEmpty(Model.StockStatus.SupersededPartNumber)) { %>
                <div>This Part is Superseded by: <a href="#" id="ReplacementPart"><% = Html.Encode(Model.StockStatus.SupersededPartLine) %> <% = Html.Encode(Model.StockStatus.SupersededPartNumber) %></a></div>           
                <div style="display:none" id="SupersededPartNumber"><% = Html.Encode(Model.StockStatus.SupersededPartNumber) %></div>
                <div style="display:none" id="SupersededLineCode"><% = Html.Encode(Model.StockStatus.SupersededPartLine) %></div>
        <% } %>
        <% foreach (string message in Model.GetMessages(this.ViewContext.RequestContext)) { %>
            <div><% = message%></div>
        <% } %>
        <% if (Model.StockStatus != null && Model.StockStatus.DenyPurchase) { %>
            <div>Once registered as part of their MVP program, MSD and Superchips will allow you to purchase products.</div>
            <div>Signing up is easy, simply visit <a href="http://www.msdpmvp.com" target="_blank"><b>www.msdmvp.com</b></a> to sign up.</div>
            <div>After signing up, please contact EPWI by phone with your MSD or Supership order.</div>
        <% } %>
        <% = Html.Hidden("OriginalPartUniqueIdentifier", Model.OriginalPartUniqueIdentifier)%>
        <% = Html.Hidden("OriginalPartPrice", Model.OriginalPartPrice)%>
        <% = Html.Hidden("KitNipc", Model.KitNipc)%>
        <% = Html.Hidden("CurrentView", Model.CurrentView) %>
    </td>
  </tr>
  <% if (Model.RequestComplete) { %>
  <tr>
    <td>Pricing:</td>
    <td> 
        <% Html.RenderPartial("KitPrices"); %>
        <% = Html.Hidden("RequestedItemNumber", Model.RequestedItemNumber) %>
        <% = Html.Hidden("RequestedItemNipc", Model.InventoryItem.NIPCCode) %>
        <% = Html.Hidden("DefaultWarehouse", Model.StockStatus.CustomerDefaultWarehouse) %>
        <% = Html.Hidden("SecondaryWarehouse", Model.StockStatus.CustomerSecondaryWarehouse) %>
        <% = Html.Hidden("ItemRequestedQuantity", Model.StockStatus.QuantityRequested) %>   
    </td>
  </tr>
  <% } %>
  </table>
  <% } // end main form %> 
  <% if (Model.RequestComplete) { %>   
  <table border="0" cellpadding="0" cellspacing="0" width="100%" class="InterchangeOrderOptions">
  <tr>
    <td style="width:17%">Order Options:</td>
    <td>
        <% Html.RenderPartial("KitOrderOptions"); %>
    </td>
  </tr>
  </table>
  <% } %>
  
