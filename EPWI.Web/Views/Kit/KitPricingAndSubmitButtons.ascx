<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Web.Models.KitBuilderViewModel>" %>
<div class="sideContainer roundedCorners">
    <h2 class="roundedCorners">Kit Pricing</h2>
    <div class="roundedCornersBody"> 
        <div class="row underline">
            <span class="widgetlabel"><% if (Model.CurrentView == 1) { %>Standard Net Price:<% } else { %>Standard Retail Price:<% } %></span><span class="widgetfield em"><% = Html.Encode(Model.Kit.StandardPrice.Adjusted(Model.CurrentView, Model.CustomerData).ToString("C"))%></span>
        </div>
        <%--<% if (Model.Kit.ConfiguredPrice > 0) { %>--%>
            <div class="row underline">
                <span class="widgetlabel">Configured Price:</span> <span id="PriceAsConfigured" class="widgetfield em"><% = Html.Encode(Model.Kit.GetConfiguredPrice(Model.NewConfiguration).Adjusted(Model.CurrentView, Model.CustomerData).ToString("C")) %></span>
            </div>
            <% if (Model.Kit.HasCrankKit) { %>
                <div class="row underline"><span class="widgetlabel">Crank Kit Pricing:</span> <span class="widgetfield em"><% = Html.Encode(Model.Kit.SelectedCrankKitPrice.Adjusted(Model.CurrentView, Model.CustomerData).ToString("C")) %></span></div>
                <% if (Model.Kit.SelectedCrankKitCorePrice > 0) {  %>
                    <div class="row underline"><span class="widgetlabel">Crank Kit Core Pricing:</span> <span class="widgetfield em"><% = Html.Encode(Model.Kit.SelectedCrankKitCorePrice.Adjusted(Model.CurrentView, Model.CustomerData).ToString("C")) %></span></div>
                <% } %>
            <% } %>
        <%--<% } %>--%>
        <% if(Model.NewConfiguration && (bool)TempData["PartsNotIncludedForYear"]) {  %>
            <div class="dialog" id="MissingParts">
                <div>Some component(s) listed in the catalog are not available for the year selected and the price has been adjusted accordingly.</div>
                <div class="buttons"><input type="button" class="close" value="OK" /></div>
            </div>
        <% } %>

        <% = Html.Hidden("resultCode", Model.ConfirmingAvailability ? Model.FulfillmentProcessingResult.ResultCode : string.Empty ) %>
        <br />
        <div class="buttons">
            <% Html.RenderPartial("CustomerReference"); %>
            <input id="btnAddToOrder" type="button" value="Add Kit To Order" />
            <% if (Model.ShowForceToOrderOption) { %>
                <div><% = Html.CheckBox("ForceToOrder") %> Ignore availability issues and request manual processing</div>    
            <% } %>
        </div>
        <div class="buttons">
            <% = Html.ActionLink($"Switch to View {(Model.CurrentView == 1 ? 2 : 1)}", "Edit", new { switchView = "switch"}, new { @class = "ShowIndicator" }) %>
        </div>
    </div>
    <div class="roundedCornersFooter"><p>&nbsp;</p></div>
</div>

