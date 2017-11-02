<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AccountSettings>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Account Settings
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Account Settings</h2>
    <div id="validationSummary"></div>
    <% =Html.ClientSideValidation<AccountSettings>().UseValidationSummary("validationSummary") %>
    <div>Editing Account Settings for <% = Html.Encode(Model.UserName) %></div>
    <% using (Html.BeginForm()) { %>
    <table width="100%" cellspacing="0" cellpadding="2" class="standard">
    <tr style="height: 20px;">
        <th colspan="4">
            <div class="center">Pricing Calculation Preferences</div>
        </th>
    </tr>
    <tr>
        <th width="50%">
            &nbsp;
        </th>
        <th width="17%">
            Jobber&nbsp;Price
        </th>
        <th width="17%">
            Invoice&nbsp;Price
        </th>
        <th width="17%">
            Elite&nbsp;Price
        </th>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right">
            Base the Suggested Retail Pricing of <% = Html.TextBox("PricingFactor", Model.PricingFactor, new { @class = "pricingfactor" })%>% on:
        </td>
        <td align="center">
            <% = Html.RadioButton("CustomerPricingBasis", PriceType.Jobber)%>
        </td>
        <td align="center">
            <% = Html.RadioButton("CustomerPricingBasis", PriceType.Invoice)%>
        </td>
        <td align="center">
            <% = Html.RadioButton("CustomerPricingBasis", PriceType.Elite)%>
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right">
            Base the Street Price Margin on:
        </td>
        <td align="center">
            <% = Html.RadioButton("MarginPricingBasis", PriceType.Jobber)%>
        </td>
        <td align="center">
            <% = Html.RadioButton("MarginPricingBasis", PriceType.Invoice)%>
        </td>
        <td align="center">
            <% = Html.RadioButton("MarginPricingBasis", PriceType.Elite)%>
        </td>
    </tr>
</table>
&nbsp;<br />
<table width="100%" cellspacing="0" cellpadding="2" class="standard">
    <tr style="height: 20px;">
        <th colspan="4">
            <div class="center">Pricing Display Preferences</div>
        </th>
    </tr>
    <tr>
        <th width="50%">
            Price Type
        </th>
        <th width="25%">
            Display In View 1
        </th>
        <th width="25%">
            Display In View 2
        </th>
    </tr>
    <tr>
        <td>
            Suggested Retail Price
        </td>
        <td>
            <% = Html.Hidden("PriceTypeSelectionsView1[0].key", PriceType.Customer)%>
            <% = Html.CheckBox("PriceTypeSelectionsView1[0].value", Model.PriceTypeSelectionsView1[PriceType.Customer])%>
        </td>
            
        <td>
            <% = Html.Hidden("PriceTypeSelectionsView2[0].key", PriceType.Customer)%>
            <% = Html.CheckBox("PriceTypeSelectionsView2[0].value", Model.PriceTypeSelectionsView2[PriceType.Customer])%>
        </td>
    </tr>
    <tr>
        <td>
            Jobber Price
        </td>
        <td>
            <% = Html.Hidden("PriceTypeSelectionsView1[1].key", PriceType.Jobber)%>
            <% = Html.CheckBox("PriceTypeSelectionsView1[1].value", Model.PriceTypeSelectionsView1[PriceType.Jobber])%>
        </td>
            
        <td>
            <% = Html.Hidden("PriceTypeSelectionsView2[1].key", PriceType.Jobber)%>
            <% = Html.CheckBox("PriceTypeSelectionsView2[1].value", Model.PriceTypeSelectionsView2[PriceType.Jobber])%>
        </td>
    </tr>
    <tr>
        <td>
            Standard Invoice Price
        </td>
        <td>
            <% = Html.Hidden("PriceTypeSelectionsView1[2].key", PriceType.Invoice)%>
            <% = Html.CheckBox("PriceTypeSelectionsView1[2].value", Model.PriceTypeSelectionsView1[PriceType.Invoice])%>
        </td>
            
        <td>
            <% = Html.Hidden("PriceTypeSelectionsView2[2].key", PriceType.Invoice)%>
            <% = Html.CheckBox("PriceTypeSelectionsView2[2].value", Model.PriceTypeSelectionsView2[PriceType.Invoice])%>
        </td>
    </tr>
    <tr>
        <td>
            Elite Price
        </td>
        <td>
            <% = Html.Hidden("PriceTypeSelectionsView1[3].key", PriceType.Elite)%>
            <% = Html.CheckBox("PriceTypeSelectionsView1[3].value", Model.PriceTypeSelectionsView1[PriceType.Elite])%>
        </td>
            
        <td>
            <% = Html.Hidden("PriceTypeSelectionsView2[3].key", PriceType.Elite)%>
            <% = Html.CheckBox("PriceTypeSelectionsView2[3].value", Model.PriceTypeSelectionsView2[PriceType.Elite])%>
        </td>
    </tr>
    <tr>
        <td>
            Street Price
        </td>
        <td>
            <% = Html.Hidden("PriceTypeSelectionsView1[4].key", PriceType.Market)%>
            <% = Html.CheckBox("PriceTypeSelectionsView1[4].value", Model.PriceTypeSelectionsView1[PriceType.Market])%>
        </td>
            
        <td>
            <% = Html.Hidden("PriceTypeSelectionsView2[4].key", PriceType.Market)%>
            <% = Html.CheckBox("PriceTypeSelectionsView2[4].value", Model.PriceTypeSelectionsView2[PriceType.Market])%>
        </td>
    </tr>
    <tr>
        <td>
            Street Price Margin
        </td>
        <td>
            <% = Html.Hidden("PriceTypeSelectionsView1[5].key", PriceType.Margin)%>
            <% = Html.CheckBox("PriceTypeSelectionsView1[5].value", Model.PriceTypeSelectionsView1[PriceType.Margin])%>
        </td>
            
        <td>
            <% = Html.Hidden("PriceTypeSelectionsView2[5].key", PriceType.Margin)%>
            <% = Html.CheckBox("PriceTypeSelectionsView2[5].value", Model.PriceTypeSelectionsView2[PriceType.Margin])%>
        </td>
    </tr>
    
</table>
<input type="submit" value="Save" />
<% } %>
</asp:Content>

