<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EPWI.Web.Models.KitViewViewModel>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Kit View
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="opaque">
        <% if (Model.AllowKitConfiguration)
           { %>
            <% if (Model.SavedConfigurationID == 0)
               { %>
                <% = Html.ActionLink("Configure and Order this Kit", "Build", new {id = Model.Kit.KitIdentifier.KitPartNumber}) %>
            <% }
               else
               { %>
                <a href="#" id="ConfigureKitLink">Configure and Order this Kit</a>
                <% Html.RenderPartial("KitExistsDialog", Model.Kit.KitIdentifier.KitPartNumber); %>
            <% } %>
            <br/><br/>
        <% } %>
        <div class="kitContainer">
            <% if (Model.Kit.IsKTRACK)
               {
                   Html.RenderPartial("KtrackDisplay");
               }
               else
               {
                   Html.RenderPartial("KitDisplay", Model);
               } %>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <link type="text/css" href="<% = Url.Content("~/Content/css/kits.css") %>" rel="Stylesheet" />
    <link type="text/css" href="<% = Url.Content("~/Content/css/stockstatus.css") %>" rel="Stylesheet" />

    <script src="<% = Url.Content("~/Scripts/site/kitdialog.js") %>" type="text/javascript"></script>

</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="SideBarContent" runat="server">
    <div id="KitHeader" class="widget">
        <% Html.RenderPartial("KitHeader"); %>
    </div>
</asp:Content>