<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EPWI.Components.Models.AccountStatus>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Account Status
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<table width="600" border="0" cellspacing="0" cellpadding="2">
<tr>
<td>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td width="80%">
            <h2>Account Status</h2>
        </td>
        <td align="right">
            <% if (User.IsInRole("EMPLOYEE"))
               { %>
                <a href="#" class="FaxPage">Fax Page</a>&nbsp;
            <% } %>
            <a href="#" class="PrintPage">Print Page</a>
        </td>
    </tr>
</table>
<% if (User.IsInRole("EMPLOYEE"))
   { %>
    <% using (Html.BeginForm("Status", "Account", FormMethod.Post, new {id = "CustomerLookup"}))
       { %>
        <table width="100%" border="0" cellspacing="0" cellpadding="2">
            <tr>
                <td width="1%" nowrap="nowrap">Customer ID:</td>
                <td width="1%"><% = Html.TextBox("CustomerID", null, new {tabindex = "1", size = "5", maxlength = "5"}) %></td>
                <td width="1%" nowrap="nowrap">Company Code (N/S):</td>
                <td width="1%"><% = Html.DropDownList("CompanyCode", new SelectList(new[] {"N", "S"}), new {tabindex = "2"}) %></td>
                <td width="99%">
                    <input tabindex="3" type="submit" name="customerLookup" value="Lookup"/>
                </td>
            </tr>
        </table>
    <% } %>
<% } %>
<div id="accountstatuscontent" class="pageContent">
<table width="100%" border="0" cellspacing="0" cellpadding="0" style="border: solid 1px black; border-collapse: collapse;">
<tr>
    <td>
        <table class="lightborder" width="100%" border="1" cellspacing="0" cellpadding="2">
            <% if (Model.CurrentUser.CustomerID != Model.CustomerID)
               { %>
                <tr>
                    <td colspan="3">Viewing Data for <b>Customer #<% = Model.CustomerID %> (<% = Model.CompanyCode %>)</b></td>
                </tr>
            <% } %>
        </table>
    </td>
</tr>
<tr>
    <td>
        <table width="100%" border="1" cellspacing="0" cellpadding="0">
            <tr>
                <td colspan="4">
                    &nbsp;Elite Status Information<% if (Model.IsElite)
                                                     { %>:
                        <div class="StatusMessage highlight">Currently Obtained Elite Status</div><% }
                                                     else
                                                     { %>: <b>Current Elite Status Not Yet Reached</b><% } %>
                </td>
            </tr>
            <tr valign="top">
                <td align="left" class="headerbar" width="40%">Current Month To Date Net Purchases:</td>
                <td><% = Model.CurrentMonthSales.ToString("C2") %></td>
            </tr>
            <% if (Model.CurrentMonthSales >= Model.ELITE_SALES_LEVEL)
               { %>
                <tr valign="top">
                    <td align="left" class="headerbar" width="40%">Current Month Eligible Elite Discount:</td>
                    <td><% = (Model.CurrentMonthSales*Model.ELITE_DISCOUNT_PERCENT).ToString("C2") %></td>
                </tr>
            <% } %>
            <tr valign="top">
                <td align="left" class="headerbar" width="40%">Last Month Total Net Purchases:</td>
                <td><% = Model.LastMonthSales.ToString("C2") %></td>
            </tr>
            <% if ((Model.LastMonthSales >= 2500) && Model.BeforeEliteCutoff)
               { %>
                <tr valign="top">
                    <td align="left" class="headerbar" width="40%">Last Month Eligible Elite Discount:</td>
                    <td><% = (Model.LastMonthSales*Model.ELITE_DISCOUNT_PERCENT).ToString("C2") %></td>
                </tr>
            <% } %>
        </table>
    </td>
</tr>
<tr>
    <td>
        <table class="lightborder" width="100%" border="1" cellspacing="0" cellpadding="0">
            <tr>
                <td width="50%" valign="top">
                    <table width="100%" border="0" cellspacing="0" cellpadding="2">
                        <tr>
                            <td class="titlebar" colspan="2">Payment Due Information</td>
                        </tr>
                        <% if (!Model.CODCustomer || Model.MailStatementToCustomer)
                           { %>
                            <% = Html.ARField($"Bal. as of Last Statement ({Model.LastStatementDate.ToString("M/dd")})", Model.StatementBalance, false, false) %>
                            <% = Html.ARField("Payments &amp; Credits Applied", Model.PaymentsAndCreditsApplied, false, false) %>
                            <% if (Model.StatementBalance != Model.StatementRemainingBalance)
                               { %>
                                <% = Html.ARField("Remaining Balance", Model.StatementRemainingBalance, true, true) %>
                            <% } %>
                            <% if (Model.DiscountAmount > 0 && Model.BeforeEliteCutoff)
                               { %>
                                <% = Html.ARField($"Max. {Model.DiscountPercent}% Discount (<a href=\"#\" id=\"EliteHelpLink\">Details</a>)", Model.DiscountAmount, true, false) %>
                            <% } %>
                            <% if (Model.BeforeEliteCutoff)
                               { %>
                                <tr>
                                    <td align="right">Balance Due if Paid by <b><% = new DateTime(DateTime.Now.Year, DateTime.Now.Month, Model.ELITE_DISCOUNT_CUTOFF_DAY).ToString("M/dd/yyyy") %></b>:&nbsp;</td>
                                    <td align="right" style="border-top: 2px solid black" nowrap="nowrap">
                                        <b><% = (Model.StatementRemainingBalance - Model.DiscountAmount).ToString("C2") %></b>
                                    </td>
                                </tr>
                            <% } %>
                            <tr>
                                <td align="right">Balance Due if Paid by <% = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToString("M/dd/yyyy") %>:</td>
                                <td align="right" style="border-top: 2px solid black" nowrap="nowrap">
                                    <b><% = Model.StatementRemainingBalance.ToString("C2") %></b>
                                </td>
                            </tr>
                        <% }
                           else
                           { %>
                            <tr>
                                <td align="center">No Payment Due</td>
                            </tr>
                        <% } %>
                    </table>
                </td>
                <td width="50%" valign="top">
                    <table width="100%" border="0" cellspacing="0" cellpadding="2">
                        <tr>
                            <td class="titlebar" colspan="2">Current Account Summary</td>
                        </tr>
                        <% if (Model.TotalAccountBalance != 0)
                           { %>
                            <% = Html.ARField("Balance as of Last Statement", Model.StatementBalance, false, false) %>
                            <% = Html.ARField("Current Invoices", Model.CurrentInvoices, false, false) %>
                            <% = Html.ARField("Current Credits", Model.CurrentCredits, false, false) %>
                            <% = Html.ARField("Current Adjustments", Model.CurrentAdjustments, false, false) %>
                            <% = Html.ARField("Current NSF Checks", Model.CurrentNSFChecks, false, false) %>
                            <% = Html.ARField("Current NSF Charges", Model.CurrentNSFCharges, false, false) %>
                            <% = Html.ARField("Current Miscellaneous", Model.CurrentMiscellaenous, false, false) %>
                            <% = Html.ARField("Current Payments Made", Model.CurrentPaymentsMade, false, false) %>
                            <% = Html.ARField("Current Discounts Allowed", Model.CurrentDiscountsAllowed, false, false) %>
                            <% = Html.ARField("<b>Total Account Balance</b>", Model.TotalAccountBalance, true, true) %>
                        <% }
                           else
                           { %>
                            <tr>
                                <td align="center">No Account Information Available</td>
                            </tr>
                        <% } %>
                    </table>
                </td>
            </tr>
        </table>
    </td>
</tr>
<tr>
    <td>
        <table class="lightborder" width="100%" border="1" cellspacing="0" cellpadding="0">
            <tr align="center">
                <td class="titlebar" colspan="2">Aging</td>
                <td class="titlebar" colspan="3">Amount Past Due</td>
            </tr>
            <tr align="center">
                <td class="headerbar" width="20%">Current Month</td>
                <td class="headerbar" width="20%">Last Month</td>
                <td class="headerbar" width="20%">30 to 60 Days</td>
                <td class="headerbar" width="20%">60 to 90 Days</td>
                <td class="headerbar" width="20%">90+ Days</td>
            </tr>
            <tr align="center">
                <td><% = Model.PastDueCurrentMonth.ToString("C2") %></td>
                <td><% = Model.PastDueLastMonth.ToString("C2") %></td>
                <td><% = Model.PastDue30.ToString("C2") %></td>
                <td><% = Model.PastDue60.ToString("C2") %></td>
                <td><% = Model.PastDue90.ToString("C2") %></td>
            </tr>
        </table>
    </td>
</tr>
<tr>
    <td>
        <input type="hidden" name="statementdate" value=""/>
        <table width="100%" border="1" cellspacing="0" cellpadding="0" bordercolor="white">
            <tr>
                <td class="titlebar" colspan="6">View Account Statements</td>
            </tr>
            <% if (User.IsInRole("EMPLOYEE"))
               { %>
                <tr align="left">
                    <td>
                        <%-- <% using (Html.BeginForm("Statement", "Account", new {customerID = Model.CustomerID, companyCode = Model.CompanyCode}, FormMethod.Post, new {id = "GetStatement"})) --%>
                        <% using (Html.BeginForm("Statement", "Account", FormMethod.Post, new {id = "GetStatement"}))
                           { %>
                            <% = Html.HiddenFor(x => x.CustomerID, new {@class = "customerIDhidden"}) %>
                            <% = Html.HiddenFor(x => x.CompanyCode, new {@class = "companyCodehidden"}) %>
                            View statement for: <% = Html.DropDownList("month", UtilityHelpers.MonthSelectList(DateTime.Now.AddMonths(-1).Month)) %>
                            <% = Html.TextBox("year", DateTime.Now.AddMonths(-1).Year, new {@class = "overwrite numeric required", maxlength = 4, style = "width:4em"}) %>
                            <input type="submit" value="Get Statement"/>
                        <% } %>
                    </td>
                </tr>
            <% }
               else
               { %>
                <tr align="center">
                    <% var currentDate = DateTime.Now; %>
                    <% for (var i = -6; i <= -1; i++)
                       { %>
                        <td>
                            <% var statementDate = currentDate.AddMonths(i); %>
                            <% = Html.ActionLink(statementDate.ToString("MMM. yyyy"), "Statement", new {customerID = Model.CustomerID, companyCode = Model.CompanyCode, year = statementDate.Year, month = statementDate.Month}) %>
                        </td>
                    <% } %>
                </tr>
            <% } %>
        </table>
    </td>
</tr>
<tr>
    <td>
        <% using (Html.BeginForm("InvoiceSearch", "Account", FormMethod.Post, new {id = "InvoiceSearch"}))
           { %>
            <table class="lightborder" width="100%" border="1" cellspacing="0" cellpadding="0">
                <tr>
                    <td class="titlebar" colspan="5">
                        Search for Invoices<% = Html.Hidden("companyCode", Model.CompanyCode, new {@class = "companyCodehidden"}) %>
                        <% = Html.Hidden("customerID", Model.CustomerID, new {@class = "customerIDhidden"}) %>
                    </td>
                </tr>
                <tr align="center">
                    <td width="20%">
                        <label><% = Html.RadioButton("searchselection", "number") %>Invoice Number</label>
                    </td>
                    <td width="40%">
                        <label><% = Html.RadioButton("searchselection", "date", true) %>View 100 Invoices</label>
                    </td>
                    <td width="20%">
                        <label><% = Html.RadioButton("searchselection", "part") %>Part Number</label>
                    </td>
                    <td width="20%" rowspan="2" valign="bottom">
                        <div id="errorContainer"></div>
                        <input class="button" type="submit" style="width: 100%" id="searchInvoices" value="Search Invoices"/>
                    </td>
                </tr>
                <tr align="center">
                    <td>
                        <input tabindex="5" name="invnum" type="text" class="inputbox invoicesearch" id="invnum" size="10" maxlength="10"/>
                    </td>
                    <td nowrap="nowrap">
                        <label><% = Html.RadioButton("invoiceDateDirection", "P", true) %>Prior to</label>, or
                        <label><% = Html.RadioButton("invoiceDateDirection", "N", false) %>After</label> &nbsp;
                        <% = Html.TextBox("invoiceDate", DateTime.Now.ToString("M/d/yyyy"), new {size = "8", maxlength = "10", tabindex = "6", @class = "inputbox invoicesearch"}) %>
                    </td>
                    <td><% = Html.TextBox("partNum", null, new {@class = "inputbox invoicesearch", size = "10", maxlength = "20"}) %></td>
                </tr>
            </table>
        <% } %>
    </td>
</tr>
</table>
</div>
</td>
</tr>
</table>

<div class="dialog" id="EliteHelp">
    Payments shown are maximum discounts available for prompt pay or EPWI's Elite Customer Program. Customers paying with
    credit cards are eligible for smaller discounts. Please see <a href="http://www.epwi.net/about.asp?numperpage=1&images=on&display=detail&categoryid=0&searchstr=191&itemname=Company+Policies" target="_blank">About Us/Company Policies</a>
    for more details.
</div>

<% Html.RenderPartial("FaxDialog", Model.CurrentUser); %>
<div id="FaxDefaultSubject" style="display: none">Account Status</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<% = Url.Content("~/Scripts/site/PageActions.js") %>" type="text/javascript"></script>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideBarContent" runat="server">
</asp:Content>