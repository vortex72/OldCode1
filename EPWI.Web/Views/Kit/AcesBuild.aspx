<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EPWI.Web.Models.KitBuilderViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ACES Kit Builder
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>ACES Kit Builder</h2>
    <% using(Html.BeginForm(new { crankKitSelected = true })) { %>
        <div>
            Crank Kit: <% = Html.DropDownList("SelectedCrankKit", new SelectList(Model.Kit.MasterKitParts.Where(kp => kp.SequenceNumber >= 900), "NIPCCode", "PartString"), "NONE") %>
        </div>
        <div><input type="submit" value="Submit" /></div><br />
        <% = Html.Hidden("id", Model.Kit.KitIdentifier.KitID) %>
     <% } %>
    <% Html.RenderPartial("KitDisplay", Model); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <link type="text/css" href="<% = Url.Content("~/Content/css/kits.css") %>" rel="Stylesheet" />
   
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideBarContent" runat="server">
    <div id="KitHeader" class="widget">
        <% Html.RenderPartial("KitHeader"); %>
    </div>
</asp:Content>