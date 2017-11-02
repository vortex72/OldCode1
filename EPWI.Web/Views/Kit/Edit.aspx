<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EPWI.Web.Models.KitBuilderViewModel>" %>
<asp:Content ID="Content5" ContentPlaceHolderID="FirstScriptContent" runat="server">
    <link rel="Stylesheet" type="text/css" href="<%= Url.Content("~/Content/css/kits.css") %>" />
    <link type="text/css" href="<% = Url.Content("~/Content/css/stockstatus.css") %>" rel="Stylesheet" />
    <script src="<%= Url.Content("~/Scripts/jquery-3.0.0.min.js") %>" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Kit Builder
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% = Html.Hidden("ajaxBasePath", Url.Content("~/Kit/")) %>
    <% = Html.Hidden("stockStatusPath", Url.Content("~/StockStatus/")) %>
    <% = Html.Hidden("confirmingAvailability", Model.ConfirmingAvailability ? "true" : "false")  %>
    <% = Html.Hidden("deselectionLimit", Model.DeselectionLimit) %>
    <% = Html.Hidden("kitType", Model.Kit.KitIdentifier.KitType) %>
    <% = Html.Hidden("previousCustomerReference", TempData["CustomerReference"]) %>
    <% if (!Model.Kit.IsKTRACK)
        { %>
    <div class="dialog" id="KitSizeSelection">
        <% Html.RenderPartial("KitSizeSelection"); %>
    </div>
    <% } %>
    <div class="opaque">
        <% if (Model.ConfirmingAvailability)
            { %>
        <% Html.RenderPartial("KitWizard", Model); %>
        <% } %>
        <br />
        <div class="kitContainer">
            <% if (Model.Kit.IsKTRACK) { Html.RenderPartial("KtrackDisplay"); } else { Html.RenderPartial("KitDisplay", Model); } %>
        </div>
    </div>
    <% Html.RenderPartial("EditDialogs"); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/site/KitEdit-2.1.js") %>"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideBarContent" runat="server">
    <% Html.RenderPartial("KitHeader"); %>

    <div id="KitPricing" class="widget">
        <% Html.RenderPartial("KitPricingAndSubmitButtons"); %>
    </div>
</asp:Content>
