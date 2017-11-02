<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EPWI.Web.Models.KitCatalogListingViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Kit Catalog
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("KitCatalogHeader"); %>   
    <div class="breadcrumbs"><% = Html.ActionLink("Manufacturers", "Index") %> &gt; <% = Html.Encode(Model.ManufacturerName) %></div>
    <p></p>
    <table style="width:100%" class="standard">
        <tr>
            <th>Name</th>
            <th>eDirect Available</th>
            <th>Description</th>
            <th>Displacement</th>
            <th>Cylinders</th>
            <th>Years</th>
        </tr>
            
        <% foreach(var kit in Model.Kits) { %>
            <tr>
                <td><% = Html.ActionLink(kit.DisplayName, "View", "Kit", new { id = kit.KitID + "MK" }, new { @class = "em" })%></td>
                <td>
                    <% if (kit.IsEDirectKitAvailable) { %>
                        <% = Html.ActionLink("eDirect", "View", "Kit", new { id = "E" + kit.KitID + "MK" }, null) %>
                    <% } else { %>
                        &nbsp;
                    <% } %>
                </td>
                <td><% = Html.Encode(kit.Description) %></td>
                <td><% = Html.Encode(kit.Displacement) %></td>
                <td><% = Html.Encode(kit.Cylinders) %></td>
                <td><% = Html.Encode(kit.StartYear.ToShortYear()) %>/<% = Html.Encode(kit.EndYear.ToShortYear()) %></td>
            </tr>    
        <%} %>
    </table>
    <br />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideBarContent" runat="server">
</asp:Content>
