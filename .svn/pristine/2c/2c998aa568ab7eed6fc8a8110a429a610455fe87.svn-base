<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Web.Models.KitStockStatusViewModel>" %>
<div class="footnote">The prices listed below reflect the price adjustment to add the part in the quantity requested to your kit.</div>
<% if (Model.IsForInterchange) { %>
    <div>
        <% if (Model.StockStatus.Price[PriceType.KitPrice] > 0) { %>
            <% if (Model.PriceDifference(Model.StockStatus.Price[PriceType.KitPrice]) == 0) { %>
                <span>No difference</span>
            <% } else if(Model.PriceDifference(Model.StockStatus.Price[PriceType.KitPrice]) >0) { %><span class="increase">Increase by <% } else { %><span class="decrease">Decrease by <% } %>
                <% = Model.RequestComplete ? Math.Abs(Model.PriceDifference(Model.StockStatus.Price[PriceType.KitPrice]).Adjusted(Model.CurrentView, Model.CustomerData)).ToString("C") : string.Empty %>    
            </span> 
        <% } else { %>
            <% if (Model.PriceDifference(Model.StockStatus.Price[PriceType.Invoice]) == 0) { %>
                <span>No difference</span>
            <% } else if(Model.PriceDifference(Model.StockStatus.Price[PriceType.Invoice]) >0) { %><span class="increase">Increase by <% } else { %><span class="decrease">Decrease by <% } %>
                <% = Model.RequestComplete ? Math.Abs(Model.PriceDifference(Model.StockStatus.Price[PriceType.Invoice]).Adjusted(Model.CurrentView, Model.CustomerData)).ToString("C") : string.Empty %>    
            </span>
        <% } %>
    </div>
<% } else { %>
    <% if(Model.CustomerData.DisplayPriceType(Model.CurrentView, PriceType.Customer)) { %>
     <div>
        <label>Sugg. Retail:</label>
        <% = Model.RequestComplete ? Model.PriceDifference(Model.StockStatus.Price[PriceType.Customer]).ToString("C") : string.Empty %>
     </div>
    <% } %>

    <% if(Model.CustomerData.DisplayPriceType(Model.CurrentView, PriceType.Jobber)) { %>
        <% if (!Model.RequestComplete || Model.StockStatus.CustomerDefaultWarehouse != "ANC" || Model.StockStatus.Price[PriceType.Jobber] > Model.StockStatus.Price[PriceType.Invoice]) {
               // FB#57: For ANC customers, don't show jobber price if it is less than invoice price %>
             <div>
                <label><% = Model.InventoryItem.LineCode.StartsWith("EPW") || Model.InventoryItem.LineCode == "KIT" ? "EPWI Net Price" : "Jobber Price" %>:</label>
                <% = Model.RequestComplete ? Model.PriceDifference(Model.StockStatus.Price[PriceType.Jobber]).ToString("C") : string.Empty %>
             </div>
        <% } %>
    <% } %>

    <% if(Model.CustomerData.AccessInvoiceCost && Model.CustomerData.DisplayPriceType(Model.CurrentView, PriceType.Invoice)) { %>
             <div>
                <label>Std. Invoice:</label>
                <% = Model.RequestComplete ? Model.PriceDifference(Model.StockStatus.Price[PriceType.Invoice]).ToString("C") : string.Empty %>
             </div>
    <% } %>

    <% if(Model.CustomerData.AccessEliteCost && Model.CustomerData.DisplayPriceType(Model.CurrentView, PriceType.Elite)) { %>
     <div>
        <label>Elite Cost:</label>
        <%  = Model.RequestComplete ? (Model.PriceDifference(Model.StockStatus.Price[PriceType.Invoice]) * 0.9M).ToString("C") : string.Empty /*Manually compute the elite price and don't take the value from the object because it does not reflect the proper difference. FB #221 */  %>
     </div>
    <% } %>
<% } %>
