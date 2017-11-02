<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% using (Html.BeginForm("Search", "StockStatus", FormMethod.Post, new { id="StockStatusWidgetForm" } )){ %>
    <div class="sideContainer" id="StockStatusWidget">        
        <% = Html.TextBox("RequestedItemNumber", null, new { @class = "widgetfield infield", title = "Stock Status Inquiry", maxlength="25", id="SSWidgetRequestedItemNumber" })%>
        <% = Html.Hidden("RequestedQuantity", 1, new { id = "SSWidgetRequestedQuantity" })%>  
        <% = Html.Hidden("Lookup", false) %>
        <div id="StockStatusSearchIcon"></div>
        <input type="submit" value="Search" class="Search"/>
    </div>
<% } %>

