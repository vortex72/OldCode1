<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<EPWI.Components.Models.QuoteDetail>>" %>
<div style="overflow:auto" id="quoteList">
<% if (Model.Count() > 0) { %>
Click on an existing quote below to overwrite it with the current order.
    <table class="standard">
        <thead>
            <tr>
                <th>Description</th>
                <th>Date Created</th>
            </tr>
        </thead>
    <% foreach (var quote in Model) { %>
        <tbody>
            <tr class="selectable">
                <td><span class="QuoteDescription"><% = Html.Encode(quote.QuoteDescription) %></span></td>
                <td width="1%" nowrap="nowrap"><% = quote.QuoteDate.ToString("MM/dd/yyyy h:mmtt") %></td>
            </tr>
        </tbody>
    <% } %>
    </table>
    <% } %>
</div>