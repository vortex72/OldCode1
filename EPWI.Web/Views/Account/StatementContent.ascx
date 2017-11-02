<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Components.Models.Statement>" %>
<div name="statementcontent" id="statementcontent" class="pageContent">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td>
          <table width="100%" border="0" cellspacing="2" cellpadding="0">
            <tr> 
              <td width="1%" rowspan="2" align="center">
                <img src="<% = Url.Content("~/Content/images/invoice_std_logo.gif") %>" width="106" height="106" />
              </td>
              <td colspan="3" style="text-align:center;font-weight:bold;font-size:12pt;letter-spacing:0.1em;">
                ENGINE and PERFORMANCE WAREHOUSE<% if (Model.CompanyCode == "S") { %> SOUTH<% } %>, INC.
              </td>
            </tr>
            <tr>
                <td  align="center" valign="top" style="font-size:xx-small;">
                    <b>Corporate Office</b><br />
                    <% = Html.Encode(Model.CompanyAddress.StreetAddress1) %><br />
                    <% if (!string.IsNullOrEmpty(Model.CompanyAddress.StreetAddress2)) { %><% = Html.Encode(Model.CompanyAddress.StreetAddress2) %><br /><% } %>
                    <% = Html.Encode(Model.CompanyAddress.City) %>, <% = Html.Encode(Model.CompanyAddress.State) %> <% = Html.Encode(Model.CompanyAddress.Zip) %><br />
                    &nbsp;<br/>
                    <% = Html.Encode(Model.CompanyAddress.Phone) %>&nbsp;&nbsp;|&nbsp;&nbsp;<% = Html.Encode(Model.CompanyAddress.AlternatePhone) %><br />
                    FAX <% = Html.Encode(Model.CompanyAddress.Fax) %><br />
                </td>
            </tr>  
            </tr>
            <tr> 
              <td width="1%" colspan="4" style="text-align:center;font-weight:bold;font-size:12pt;letter-spacing:0.2em;">
                Statement
              </td>
            </tr>
          </table>
          <table width="100%" border="0" cellspacing="2" cellpadding="0">
            <tr> 
              <td valign="top" width="1%" nowrap="nowrap">Bill To:</td>
              <td valign="top" width="70%" nowrap="nowrap"><b>
                <% if (!string.IsNullOrEmpty(Model.Name)) { %> 
                    <% = Html.Encode(Model.Name) %>&nbsp;&nbsp; | &nbsp;&nbsp;<% = Html.Encode(Model.CustomerID) %> (<% = Html.Encode(Model.CompanyCode) %>)<br />                
                <% } %>
                <% if (!string.IsNullOrEmpty(Model.CustomerAddress.StreetAddress1))
                   { %>
                    <% = Html.Encode(Model.CustomerAddress.StreetAddress1) %><br />
                <% } %>
                <% if (!string.IsNullOrEmpty(Model.CustomerAddress.StreetAddress2)) { %>
                    <% = Html.Encode(Model.CustomerAddress.StreetAddress2)%><br />
                <% } %>
                <% if (!string.IsNullOrEmpty(Model.CustomerAddress.City)) { %>
                    <% = Html.Encode(Model.CustomerAddress.City)%>, <% = Html.Encode(Model.CustomerAddress.State)%> <% = Html.Encode(Model.CustomerAddress.Zip) %>
                <% } %>
                <% if (!string.IsNullOrEmpty(Model.CustomerAddress.Zip4) && Model.CustomerAddress.Zip4 != "0") { %>
                    -<% = Html.Encode(Model.CustomerAddress.Zip4) %>
                <% } %>
                </b>
              </td>
              <td valign="top" width="30%">
                <table width="100%" border="0" cellspacing="2" cellpadding="0" style="border:solid 1px black;">
                  <tr> 
                    <td align="right" nowrap="nowrap">Statement Date:</td>
                    <td align="right" nowrap="nowrap"><b><% = Html.Encode(Model.StatementDate.ToString("MM/yyyy")) %></b></td>
                  </tr>
                  <% = Html.ARField("Statement Balance", Model.TotalDue, false, false) %>
                </table>
                <table width="100%" border="0" cellspacing="2" cellpadding="0" style="border:solid 1px black;">
                  <% = Html.ARField("Current Balance", Model.CurrentBalance, false, true) %>
                  <% = Html.ARField("30 Days Past Due", Model.PastDue30, false, true) %>
                  <% = Html.ARField("60 Days Past Due", Model.PastDue60, false, true) %>
                  <% = Html.ARField("90 Days Past Due", Model.PastDue90, false, true) %>
                  <% = Html.ARField("120 Days Past Due", Model.PastDue120, false, true) %>
                </table>            
              </td>
            </tr>
            <tr>
                <td colspan="3" valign="top" nowrap="nowrap">
                Customer Notes:<br />
                <% foreach (var customerNote in Model.CustomerNotes) { %>
                    <% = Html.Encode(customerNote) %><br />
                <% } %>
                </td>
            </tr>
            <tr> 
              <td colspan="3" valign="top" nowrap>&nbsp;</td>
            </tr>
          </table>
          <table width="100%" border="0" cellspacing="0" cellpadding="1" class="statement">
            <tr> 
              <th align="left">Transaction Date</th>
              <th align="left">Transaction ID</th>
              <th align="left">Type</th>
              <th align="center">Terms Code</th>
              <th align="right">Charges</th>
              <th align="right">Credits</th>
              <th align="center">Notes</th>
            </tr>
            <tr>
              <% if (Model.PreviousBalance >= 0) { %>
              <td colspan="4" align="left">Previous Balance</td>
              <td align="right"><b><%= Html.Encode(Model.PreviousBalance.ToString("C2")) %></b></td>
              <td colspan="2">&nbsp;</td>
              <% } else { %>
              <td colspan="5" align="left">Previous Balance</td>
              <td align="right"><b><%= Html.Encode(Model.PreviousBalance.ToString("C2")) %></b></td>
              <td>&nbsp;</td>
              <% } %>
            </tr>        
    <% foreach(var record in Model.StatementDetails) { %>
    <tr>
    <td><%= Html.Encode(record.TransactionDate.ToString("M/d/yyyy")) %></td>
    <% if (record.TransactionType == "I" || record.TransactionType == "C") { %>
    <td><% = Html.ActionLink(record.ReferenceNumber, "Invoice", new { CustomerID = Model.CustomerID, CompanyCode = Model.CompanyCode, InvoiceNumber = record.ReferenceNumber })%></td>
    <% } else { %>
    <td><%= Html.Encode(record.ReferenceNumber) %></td>
    <% } %>
    <td><%= Html.Encode(record.TransactionTypeDescription) %></td>
    <td align="center">
    <% switch(record.TermsCode) { %>
        <% case "O": %>Open<% break; %>
        <% case "C": %>COD<% break; %>
        <% case "S": %>Cash Only<% break; %>
    <% } %>
    </td>
    <% if (record.TransactionAmount >= 0) { %>
    <td align="right"><%= record.TransactionAmount.ToString("C2") %></td>
    <td>&nbsp;</td>
    <% } else { %>
    <td>&nbsp;</td>
    <td align="right"><%= record.TransactionAmount.ToString("C2") %></td>
    <% } %>
    <td>&nbsp;&nbsp;&nbsp;<%= record.Note.Trim() %></td>
    </tr>
    <% } %>
            <tr> 
              <% if (Model.TotalDue >= 0) { %>
              <th colspan="4" align="left" style="font-size:10pt;">TOTAL DUE:</th>
              <th align="right" style="font-size:10pt;"><b><% = Model.TotalDue.ToString("C2") %></b></th>
              <th colspan="2">&nbsp;</th>
              <% } else { %>
              <th colspan="5" align="left" style="font-size:10pt;">TOTAL DUE:</th>
              <th align="right" style="font-size:10pt;"><b><%= Model.TotalDue.ToString("C2") %></b></th>
              <th>&nbsp;</th>
              <% } %>
            </tr>
          </table>    
        </td>
      </tr>
    </table>
    <br />
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td>
          <img src="<% = Url.Content("~/Content/images/invoice_net_logo.gif") %>" width="114" height="50" />
        </td>
        <td align="right">
          <img src="<% = Url.Content("~/Content/images/invoice_enginepro_logo.gif") %>" width="75" height="50" />
        </td>
      </tr>
    </table>
    </div>
