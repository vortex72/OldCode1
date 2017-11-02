<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Components.Models.ICustomerData>" %>
<% Html.RenderPartial("EmailDialog", Model); %>
<% Html.RenderPartial("FaxDialog", Model); %>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
      <td width="75%">
        <h2>Invoice</h2>
      </td>
      <td align="right">
        <% if (Page.User.IsInRole("EMPLOYEE")) { %>
            <a href="#" class="EmailPage">Email Page</a>&nbsp;
            <a href="#" class="FaxPage">Fax Page</a>&nbsp;
        <% } %>
        <a href="#" class="PrintPage">Print Page</a>
      </td>
    </tr>
</table>
