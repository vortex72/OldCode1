<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EPWI.Components.Models.InvoicePartSearch>" %>

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
            <b><% = Html.ActionLink("Account Status", "Status", new { customerID = Model.CustomerData.CustomerID, companyCode = Model.CustomerData.CompanyCode }) %></b>
          </td>
        </tr>
      </table>
    
    <% using (Html.BeginForm("MultipleInvoices", "Account", FormMethod.Post, new { id = "ViewMultipleInvoicesForm" })) { %>
    <% = Html.Hidden("companyCode", Model.CustomerData.CompanyCode) %>
    <% = Html.Hidden("customerID", Model.CustomerData.CustomerID) %>
    <div>Viewing Invoices Containing Part Number: <% = Html.Encode(Model.PartNumber) %>
    <br />
    <% if (Model.Invoices.Count() > 0) { %>
    <b><a href="#" class="ViewSelectedInvoices">View Selected Invoices</a>&nbsp;|&nbsp;<a href="#" id="ClearCheckboxes">Uncheck All Invoices</a></b>
    <% } %>
    </div>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
      <td>
        <% if (Model.Invoices.Count() > 0)
           { %>
        <table width="100%" border="1" cellspacing="0" cellpadding="5">
          <tr>
            <th>&nbsp;</th>
            <th>Ship Date</th>
            <th>Inv. #</th>
            <th>Line Code</th>
            <th>Item Number</th>
            <th>Size</th>
            <th>Type</th>
            <th>Order Date</th>
            <th>Whse</th>
            <th>Total Price</th>
            <th>Quanity</th>
            <th>Kit</th>
          </tr>
          <% int i = 0; foreach (var invoice in Model.Invoices) { %>
          <tr>
            <td><% = Html.CheckBox("InvoiceSelections[" + i + "].Selected")%></td>
            <td><% if (invoice.ShipmentPending)
                   { %>PENDING<% }
                   else
                   { %><%= Html.Encode(invoice.InvoiceDate.ToString("M/d/yyyy"))%><% } %></td>
            <td><% = Html.ActionLink(invoice.InvoiceNumber, "Invoice", new { customerID = Model.CustomerData.CustomerID, companyCode = Model.CustomerData.CompanyCode, invoiceNumber = invoice.InvoiceNumber })%><% = Html.Hidden("InvoiceSelections[" + i + "].InvoiceNumber", invoice.InvoiceNumber)%></td>
            <td align="center"><%= Html.Encode(invoice.LineCode)%></td>
            <td align="center"><%= Html.Encode(invoice.ItemNumber)%></td>
            <td align="center"><%= Html.Encode(invoice.SizeCode)%></td>
            <td align="center"><%= Html.Encode(invoice.Type)%></td>
            <td align="center"><%= Html.Encode(invoice.OrderDate.ToString("M/d/yyyy"))%></td>
            <td align="center"><%= Html.Encode(invoice.Warehouse)%></td>
            <td align="right"><% = Html.Encode(invoice.TotalPrice.ToString("C2"))%></td>
            <td align="center">&nbsp;<%= Html.Encode(invoice.Quantity)%></td>
            <td align="center">&nbsp;<%= Html.Encode(invoice.Kit)%></td>
          </tr>
          <% i++; } %>
        </table>
        <% }
           else
           { %>
        <b>No Records Found</b> <br />
        &nbsp; <br />
        The part number look up function of www.epwi.net can look up part numbers <br />
        that are not in our system. However, they must be entered exactly as they were <br />
        originally entered on your invoice.  This should be the same as they appear in  <br />
        the manufacturers price sheet including dashes but not sizes.  <br />
        <% } %>
      </td>
    </tr>
  </table>
  <% } %>
  <br />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideBarContent" runat="server">
</asp:Content>
