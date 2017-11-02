<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EPWI.Components.Models.InvoiceDateSearch>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Invoice Search
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <table width="100%" border="0" cellspacing="0" cellpadding="5">
        <tr>
          <td>
            <h2>Invoice Search</h2>
          </td>
          <td align="right">
            Return to<br/>
            <b><% = Html.ActionLink("Account Status", "Status", new { customerID = Model.CustomerData.CustomerID, companyCode = Model.CustomerData.CompanyCode }) %></b><br />
          </td>
        </tr>
      </table>
      
      <% using (Html.BeginForm("MultipleInvoices", "Account", FormMethod.Post, new { id = "ViewMultipleInvoicesForm" })) { %>
      <% = Html.Hidden("companyCode", Model.CustomerData.CompanyCode) %>
      <% = Html.Hidden("customerID", Model.CustomerData.CustomerID) %>
      <table width="100%" border="1" cellspacing="0" cellpadding="4">
          <tr>
            <td colspan="12" align="center">
                &nbsp;&nbsp;&nbsp;Viewing <% = Model.Invoices.Count() %> Invoices <% = Html.Encode(Model.SearchDirection.ToString())%> <% = Html.Encode(Model.SearchDate.ToString("M/d/yyyy")) %><br />                
                <% Html.RenderPartial("DateSearchNavigation"); %><br />
            </td>
          </tr>
          <tr>
            <th>&nbsp;</th>
            <th>Ship Date</th>
            <th>Order Date</th>
            <th>Inv. #</th>
            <th>Type</th>
            <th>Statement Date</th>
            <th>Ship Whse.</th>
            <th>Inv. Total</th>
            <th># Line Items</th>
            <th>Ship Method</th>
            <th>Sold To Acct.</th>
            <th>P.O. Number</th>
          </tr>
          <% if (Model.Invoices.Count() > 0) { %>
              <% int i = 0;  foreach (var invoice in Model.Invoices) { %>
              <tr>
                <td><% = Html.CheckBox("InvoiceSelections[" + i + "].Selected")%></td>
                <td><% if (invoice.ShipmentPending)
                       { %>PENDING<% }
                       else
                       { %><%= Html.Encode(invoice.ShipmentDate.ToString("M/d/yyyy"))%><% } %></td>
                <td><% = Html.Encode(invoice.OrderDate.ToString("M/d/yyyy"))%></td>
                <td><% = Html.ActionLink(invoice.InvoiceNumber, "Invoice", new { customerID = Model.CustomerData.CustomerID, companyCode = Model.CustomerData.CompanyCode, invoiceNumber = invoice.InvoiceNumber })%><% = Html.Hidden("InvoiceSelections[" + i + "].InvoiceNumber", invoice.InvoiceNumber) %></td>
                <td align="center"><%= Html.Encode(invoice.Type)%></td>
                <td align="center"><% if (invoice.StatementDate == DateTime.MaxValue)
                                      { %>&nbsp;<% }
                                      else
                                      { %><% = Html.Encode(invoice.StatementDate.ToString("M/yyyy"))%><% } %></td>
                <td align="center"><%= Html.Encode(invoice.ShippingWarehouse)%></td>
                <td align="right"><%= Html.Encode(invoice.InvoiceTotal.ToString("C2"))%></td>
                <td align="center">&nbsp;<% = Html.Encode(invoice.LineItemCount)%></td>
                <td><%= Html.Encode(invoice.ShipmentMethod)%></td>
                <td align="center"><%= Html.Encode(invoice.SoldToAccount)%></td>
                <td align="center"><%= Html.Encode(invoice.PONumber)%></td>
              </tr>
              <% i++; } %>
          <% } else { %>
          <tr>
            <td colspan="12" align="center"><b>No Records Found</b></td>
          </tr>
          <% } %>
          <tr>
            <td colspan="12" align="center">
              <% Html.RenderPartial("DateSearchNavigation"); %>
            </td>
          </tr>
        </table>
  &nbsp;<br/>
        <% } %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideBarContent" runat="server">
</asp:Content>
