<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EPWI.Web.Models.KitBuilderViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Kit Builder
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="opaque">
        <% if (!Model.YearSelected) { %>
            <div class="dialog" id="TypeAndYearSelection">
                <% Html.RenderPartial("KitTypeAndYearSelection"); %>
            </div>
            <div><a href="#" id="ConfigureKitYear">Configure Kit</a></div><p></p>
        <% } else { %>
            <div class="dialog" id="KitSizeSelection">
                <% Html.RenderPartial("KitSizeSelection"); %>
            </div>
            <div><a href="#" id="ConfigureKitSize">Configure Kit</a></div><p></p>
        <% } %>
    
        <div class="kitContainer">
            <% if (Model.Kit.IsKTRACK) { Html.RenderPartial("KtrackDisplay"); } else { Html.RenderPartial("KitDisplay", Model); } %>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <link type="text/css" href="<% = Url.Content("~/Content/css/kits.css") %>" rel="Stylesheet" />
    <link type="text/css" href="<% = Url.Content("~/Content/css/stockstatus.css") %>" rel="Stylesheet" />

    <script src="<%= Url.Content("~/Scripts/site/kitbuild.js") %>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideBarContent" runat="server">
    <div id="KitHeader" class="widget">
        <% Html.RenderPartial("KitHeader"); %>
    </div>
</asp:Content>