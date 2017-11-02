<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Web.Models.StockStatusViewModel>" %>
<table id="stockStatusPricing" class="stockStatusBlock stockStatusPricing" border="0" cellpadding="0"
    cellspacing="0" width="100%">
    <tr>
        <th colspan="2">
            Item Pricing
        </th>
    </tr>
    <% if (Model.CustomerData.DisplayPriceType(Model.CurrentView, PriceType.Customer))
       { %>
    <tr>
        <td>
            Sugg. Retail Price:
        </td>
        <td>
            <% = Model.RequestComplete ? Model.StockStatus.Price[PriceType.Customer].ToString("C") : string.Empty %>
        </td>
    </tr>
    <% } %>
    <% if (Model.CustomerData.DisplayPriceType(Model.CurrentView, PriceType.Jobber)) { %>
        <% if (!Model.RequestComplete || Model.StockStatus.CustomerDefaultWarehouse != "ANC" || Model.StockStatus.Price[PriceType.Jobber] > Model.StockStatus.Price[PriceType.Invoice]) { 
            // FB#57: For ANC customers, don't show jobber price if it is less than invoice price  %>
            <tr>
                <td>
                    <% = Model.InventoryItem.LineCode.StartsWith("EPW") || Model.InventoryItem.LineCode== "KIT" ? "EPWI Net Price" : "Jobber Price" %>:
                </td>
                <td>
                    <% = Model.RequestComplete ? Model.StockStatus.Price[PriceType.Jobber].ToString("C") : string.Empty %>
                </td>
            </tr>
        <% } %>
    <% } %>
    <% if (Model.CustomerData.AccessInvoiceCost && Model.CustomerData.DisplayPriceType(Model.CurrentView, PriceType.Invoice))
       { %>
    <tr>
        <td>
            Std. Invoice Price:
        </td>
        <td>
            <% = Model.RequestComplete ? Model.StockStatus.Price[PriceType.Invoice].ToString("C") : string.Empty %>
        </td>
    </tr>
    <% } %>
    <% if (Model.CustomerData.AccessEliteCost && Model.CustomerData.DisplayPriceType(Model.CurrentView, PriceType.Elite))
       { %>
    <tr>
        <td>
            Elite Cost:
        </td>
        <td>
            <% = Model.RequestComplete ? Model.StockStatus.Price[PriceType.Elite].ToString("C") : string.Empty %>
        </td>
    </tr>
    <% } %>
    <% if (Model.RequestComplete && Model.CustomerData.DisplayPriceType(Model.CurrentView, PriceType.Market) && Model.StockStatus.Price[PriceType.Market] > 0)
       { %>
    <tr>
        <td>
            Street Price:
        </td>
        <td>
            <% = Model.RequestComplete ? Model.StockStatus.Price[PriceType.Market].ToString("C") : string.Empty %>
        </td>
    </tr>
    <% if (Model.CustomerData.DisplayPriceType(Model.CurrentView, PriceType.Margin))
       { %>
    <tr>
        <td>
            Street Price Margin:
        </td>
        <td>
            (<% = (Model.StockStatus.Price[PriceType.Margin]).ToString("P1") %>)
        </td>
    </tr>
    <% } %>
    <% } %>
</table>
