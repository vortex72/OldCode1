<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EPWI.Web.Models.KitCatalogIndexViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Kit Catalog
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("KitCatalogHeader"); %> 
    <div class="wrapper" style="position:relative;margin-left:-75px" >
    <ul id="kits" >
    <% foreach (var m in Model.Manufacturers) { %>
        <li style="list-style-type:none"><% = Html.ActionLink(m.Name, "Make", new { id = m.Make })%></li>
    <% } %>
    </ul>
    </div>   
    <div>&nbsp;</div> 
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<% = Url.Content("~/Scripts/site/columnizelist.js") %>" type="text/javascript"></script>
    <script type="text/javascript">        $(function () { $('#kits').columnizeList({ cols: 4 }); });</script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideBarContent" runat="server">
</asp:Content>
