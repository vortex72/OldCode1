<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Web.Models.MenuViewModel>" %>
<%@ Import Namespace="EPWI.Web.Models.N2CMS" %>
<%@ Import Namespace="N2.Collections" %>
<div id="menucontainer">
    <ul id="menu" class="sf-menu">
        <!-- TODO: N2 -->
        <li><% = N2.Web.Link.To(N2.Find.RootItem) %>
            <%--<% if (N2.Find.RootItem.GetChildPagesUnfiltered()
                    .Where(new TypeFilter(typeof (MenuPage))).Count > 0)
               { %>
            <ul>
                <% foreach (var item in N2.Find.RootItem.GetChildPagesUnfiltered().Where(new TypeFilter(typeof (MenuPage))))
                   { %>
                <li><% = N2.Web.Link.To(item) %></li>
                <% } %>
            </ul>
            <% } %>--%>
        </li>
        <% foreach (var menuItem in N2.Find.RootItem.GetChildPagesUnfiltered().Where(new TypeFilter(typeof (MenuPage))))
            { %>
        <li><a href="#"><% = Html.Encode(menuItem.Title) %></a>
            <ul>
                <% foreach (var item in menuItem.GetChildPagesUnfiltered().Where(new NavigationFilter()))
                    { %>
                <li><% = N2.Web.Link.To(item) %></li>
                <% } %>
            </ul>
        </li>
        <% } %>
        <% if (Page.User.Identity.IsAuthenticated)
            { %>
        <% if (Page.User.IsInRole("CUSTOMER") || Page.User.IsInRole("EMPLOYEE") || Page.User.IsInRole("ADMIN"))
            {  %>
        <li><a href="#">Engine Kits</a>
            <ul>
                <li><%= Html.ActionLink("Kit Catalog", "Index", "KitCatalog")%></li>
                <% if (Model.KitExists)
                    { %>
                <li><% = Html.ActionLink("Edit Kit In Progress", "Edit", "Kit") %></li>
                <% } %>
                <% if (Page.User.IsInRole("ACES_KIT_BUILDER"))
                    { %>
                <li><% = Html.ActionLink("ACES Kit Builder", "Aces", "Kit") %></li>
                <% } %>
            </ul>
        </li>
        <% } %>
        <li><a href="#">My Order</a>
            <ul>
                <li><% = Html.ActionLink("Stock Status Inquiry", "Search", "StockStatus") %></li>
                <% if (Page.User.IsInRole("LOOKUP"))
                    { %>
                <%--TODO: Lookup/Opticat
                    <li><% = Html.ActionLink("Year/Make/Model Lookup", "Index", "Lookup") %></li>--%>
                <% } %>
                <li><% = Html.ActionLink("View Order", "Index", "Order") %></li>
                <% if (Model.QuotesExist || Page.User.IsInRole("EMPLOYEE"))
                    { %>
                <li><% = Html.ActionLink("View Quotes", "Index", "Quote") %></li>
                <% } %>
                <% if (Page.User.IsInRole("DATA_DOWNLOADS"))
                    { %>
                <li><% = Html.ActionLink("Data Downloads", "Index", "DataDownload") %></li>
                <% } %>
            </ul>
        </li>
        <% } %>
        <% if (!Request.IsAuthenticated)
            { %>
        <li><% = Html.ActionLink("Register", "Register", "Account") %></li>
        <li><% = Html.ActionLink("Log In", "LogOn", "Account") %></li>
        <% }
            else { %>
        <li><a href="#">My Account</a>
            <ul>
                <% if (Page.User.IsInRole("ACCESS_ACCOUNT_STATUS"))
                    { %>
                <li><%= Html.ActionLink("View Account Status", "Status", "Account") %></li>
                <li><%= Html.ActionLink("Invoices / Statements", "Status", "Account") %></li>
                <% } %>
                <% if (Page.User.IsInRole("ACCESS_ACCOUNT_SETTINGS"))
                    { %>
                <li><%= Html.ActionLink("Edit Account Settings", "Settings", "Account") %></li>
                <% } %>
                <li><%= Html.ActionLink("Edit User Profile", "Edit", "Account") %></li>
                <li><%= Html.ActionLink("Log Off", "LogOff", "Account") %></li>
            </ul>
        </li>
        <% } %>
    </ul>
    <% if (Page.User.Identity.IsAuthenticated && Page.User.IsInRole("EMPLOYEE"))
        { %>
    <% Html.RenderPartial("UpdateCustomerID", Model.CustomerData); %>
    <% } %>
</div>
