<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EPWI.Components.Models.InvoiceViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Invoice
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("InvoiceHeader", Model.CustomerData); %>
    <div id="EmailBody" style="display:none">Attached is the Invoice Detail for Invoice #<% = Html.Encode(Model.Invoice.InvoiceNumber) %></div>
    <div id="EmailDefaultSubject" style="display:none">Invoice Detail for Invoice #<% = Html.Encode(Model.Invoice.InvoiceNumber) %></div>
    <div id="FaxDefaultSubject" style="display:none">Invoice Detail for Invoice #<% = Html.Encode(Model.Invoice.InvoiceNumber) %></div>
    <div class="pageContent">
        <% Html.RenderPartial("InvoiceContent", Model.Invoice); %>
    </div>
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<% = Url.Content("~/Scripts/site/PageActions.js") %>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideBarContent" runat="server">
</asp:Content>
