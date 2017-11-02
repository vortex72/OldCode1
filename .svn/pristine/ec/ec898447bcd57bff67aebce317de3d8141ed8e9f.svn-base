<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EPWI.Web.Models.QuotesViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Saved Quotes
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Saved Quotes</h2>
    <% = Html.Hidden("OpenOrderExists", Model.OpenOrderExists) %>
    <% if (Model.IsEmployee) { Html.RenderPartial("Search", Model.CustomerData); } %>
    <br />
    <table class="standard" style="width:85%">
    <tr>
        <th>&nbsp;</th>
        <th>ID</th>
        <th>Description</th>
        <th>Date Created</th>
        <th>Created By</th>
    </tr>
    <% if (Model.Quotes.Count() > 0) { %>
        <% foreach (var quote in Model.Quotes) { %>
            <tr>
                <td width="1%" nowrap="nowrap"><% = Html.ActionLink("Load", "Load", new { id = quote.QuoteID }, new { @class = "LoadQuote" })%>&nbsp;
                    <% if (quote.CanDelete(Model.CustomerData)) { %>
                        <% = Html.ActionLink("Delete", "Delete", new { id = quote.QuoteID }, new { @class = "DeleteQuote" })%>&nbsp;
                    <% } %>
                    <% if (!User.IsInRole("EMPLOYEE") && quote.CanEdit(Model.CustomerData)) { %>
                        <% = Ajax.ActionLink("Help", "RequestHelp", new { id = quote.QuoteID }, new AjaxOptions() { OnFailure = "ajaxError", UpdateTargetId = "RequestHelpDialog", OnSuccess = "showRequestHelp", OnComplete = "hideProcessingBlock", HttpMethod = "Get" }, new { @class = "ShowIndicator" })%>
                    <% } %>
                </td>
                <td><% = Html.Encode(quote.QuoteID) %></td>
                <td><% = Html.Encode(quote.QuoteDescription) %></td>
                <td width="1%" nowrap="nowrap"><% = quote.QuoteDate.ToString("MM/dd/yyyy h:mmtt") %></td>
                <td><% = Html.Encode(quote.CreatedBy) %></td>
            </tr>
        <%} %>
    <% } else { %>
        <tr>
            <td colspan="4">No saved quotes exist.</td>
        </tr>
    <% } %>
    </table>
    <div>&nbsp;</div>
    <% if (Model.OpenOrderExists) { %>
        <div class="dialog" id="OpenOrderDialog">
            <div>You currently have an open order. Loading a quote will overwrite the current order. Please select an option below.</div>
            <br />
            <div class="buttons"><form method="get" action="<% = Url.Action("Index", "Order") %>"><input type="button" value="Load Quote" id="LoadQuote" /><input type="submit" value="View Order" /><input type="button" class="close" value="Cancel" /></form></div>        
        </div>
    <% } %>
    <div class="dialog" id="RequestHelpDialog"></div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
<script src="<% = Url.Content("~/Scripts/site/Quotes.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/site/RequestQuoteHelp.js") %>"type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideBarContent" runat="server">
</asp:Content>
