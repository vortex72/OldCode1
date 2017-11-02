<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EPWI.Web.Models.MultipleInvoiceViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Invoices
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("InvoiceHeader", Model.CustomerData); %>
    <div id="EmailBody" style="display:none">Attached is Invoice Detail for multiple invoices</div>
    <div id="EmailDefaultSubject" style="display:none">Invoice Detail for multiple invoices</div>
    <div id="FaxDefaultSubject" style="display:none">Invoice Detail for multiple invoices</div>
    <div class="pageContent">
        <% if (Model.Invoices.Count() == 0) { %>
            No invoices selected
        <% } else { %>
            <% int i = 0;  foreach (var invoice in Model.Invoices) { %>
                <% Html.RenderPartial("InvoiceContent", invoice); %>
                <br />
                <% if (i < Model.Invoices.Count() - 1) { %><div style="page-break-after:always">&nbsp;</div><% } %>
            <% i++; 
               } %>
        <% } %>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<% = Url.Content("~/Scripts/site/PageActions.js") %>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideBarContent" runat="server">
</asp:Content>
