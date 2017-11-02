<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Web.Models.StockStatusViewModel>" %>
<% if (Model.ShowInterchanges) { %>
    <div id="divAvailableInterchanges" class="dialog">
    <table class="standard">
        <tr>
            <th><% =Model.StockStatus.CustomerDefaultWarehouse %> Qty</th>
            <th>Line</th>
            <th>Item Number</th>
            <th>Sugg. Retail</th>
            <% if (Model.CustomerData.DisplayPriceType(Model.CurrentView, PriceType.Jobber)) { %>
            <th>Jobber Price</th>
            <% } %>
            <% if (Model.CustomerData.AccessInvoiceCost && Model.CustomerData.DisplayPriceType(Model.CurrentView, PriceType.Invoice)) { %>
            <th>Invoice Price</th>
            <% } %>
            <% if (Model.CustomerData.AccessEliteCost && Model.CustomerData.DisplayPriceType(Model.CurrentView, PriceType.Elite)) { %>
            <th>Elite Price</th>
            <% } %>
            <th>I/C Type</th>
            <th>Notes</th>
        </tr>
        <% foreach (var interchange in Model.Interchanges) { %>
            <tr class="InterchangePart">
                <td>
                    <% = Html.Hidden("InterchangeUrl" + interchange.NIPCCode, Url.Action("Search", "StockStatus", new { RequestedQuantity = Model.RequestedQuantity, RequestedItemNumber = interchange.ItemNumber, RequestedLineCode = interchange.LineCode }), new { @class = "InterchangeUrl" })%>
                    <% = Html.Encode(interchange.FulfillmentQuantity) %>
                </td>
                <td><% = Html.Encode(interchange.LineCode) %></td>
                <td><% = Html.Encode(interchange.ItemNumber) %></td>
                <td><% = interchange.Price[PriceType.Customer].ToString("C") %></td>
                <% if (Model.CustomerData.DisplayPriceType(Model.CurrentView, PriceType.Jobber)) { %>
                <td><% = interchange.Price[PriceType.Jobber].ToString("C") %></td>
                <% } %>
                <% if (Model.CustomerData.AccessInvoiceCost && Model.CustomerData.DisplayPriceType(Model.CurrentView, PriceType.Invoice)) { %>
                <td><% = interchange.Price[PriceType.Invoice].ToString("C") %></td>
                <% } %>
                <% if (Model.CustomerData.AccessEliteCost && Model.CustomerData.DisplayPriceType(Model.CurrentView, PriceType.Elite)) { %>
                <td><% = interchange.Price[PriceType.Elite].ToString("C") %></td>
                <% } %>
                <td><% = Html.Encode(interchange.InterchangeType) %></td>
                <td><% foreach (var note in interchange.Notes) { %><% = Html.Encode(note) %><br /><% } %></td>
            </tr>
        <% } %>
    
    
    </table>
    <div>
        <em>Many possible interchanges may not be available on-line. If you can't find exactly what you need, please call EPWI in 
            <% = Html.Encode(Model.StockStatus.CustomerDefaultWarehouse) %> for assistance.
        </em>
    </div>
    </div>
    <% } %>    
