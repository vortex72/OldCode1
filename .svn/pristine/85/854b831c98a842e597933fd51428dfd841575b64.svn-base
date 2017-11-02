<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Components.Models.Order>" %>
<div class="sideContainer roundedCorners">
    <h2 class="roundedCorners">Order Summary</h2>
    <div class="roundedCornersBody">
    <% if (Model != null) { %>
        <div>Number of items: <% = Model.OrderItems.Count(oi => oi.ParentItemID == null) %> <% = Html.ActionLink("View Order", "Index", "Order") %></div>
        <% var mostRecentItem = Model.MostRecentItem; if (mostRecentItem != null) { %>
            <div>Last item added: <% = Html.Encode(mostRecentItem.ItemNumber) %> <% = Html.Encode(mostRecentItem.ItemDescription) %> (<% = Html.Encode(mostRecentItem.Quantity) %>)</div>
        <% } %>
    <% } else if (ViewData["error"] != null && (bool)ViewData["error"]) { %>
        <div>Error getting order details</div>
    <% } %>
    </div>
    <div class="roundedCornersFooter">
        <p style="text-align:left;color:black"> 
            <% if (Model != null) { %>
                Order Total: <% = Model.SubTotalCalculated.GetValueOrDefault(0).ToString("C") %>
            <% } else if (ViewData["error"] == null) { %>
                You do not have an open order
            <% } %>
        </p>
        <% if (Page.User.IsInRole("POWER_USER")) { %>
               <% if (Model != null && Model.IsPowerUserOrder) { %> 
                    <p style="text-align:left;color:black;font-weight:bold;font-size:12px">Power User Order</p>
                    <p style="text-align:left;color:black">Primary: <%= Html.Encode(Model.PrimaryWarehouse) %> Secondary: <%= Html.Encode(Model.SecondaryWarehouse) %></p>
               <% } else if (Model == null || !Model.OrderItems.Any()) { %>
                   <p style="text-align:left;color:black">
                       <% = Html.ActionLink("Start a Power User Order", "OpenPowerUserOrder", "Order") %>
                   </p>
               <% } %>
        <% } %>
    </div>
</div>