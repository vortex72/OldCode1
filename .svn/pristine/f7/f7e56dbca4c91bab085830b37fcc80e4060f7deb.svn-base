<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EPWI.Web.Models.OrderViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Order Form
</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="FirstScriptContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/underscore.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-3.0.0.min.js") %>" type="text/javascript"></script>
    <script src="<% = Url.Content("~/Scripts/jquery-migrate-1.2.1.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.12.0-rc.2.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/site/OrderForm-2.1.js?v=2") %>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Order Form</h2>
    <% = Html.Hidden("SetPOAction", Url.Action("SetPurchaseOrderNumber", "Order")) %>
    <% = Html.Hidden("SaveNotesAction", Url.Action("SaveNotes", "Order")) %>
    <% = Html.Hidden("GetQuotesAction", Url.Action("QuoteList", "Quote")) %>
    <% = Html.Hidden("UpdateCustomerReferenceAction", Url.Action("UpdateCustomerReference", "Order")) %>
    <% = Html.Hidden("SavedKitConfigurationID") %>
    <div>
        <% = Html.ValidationSummary("Please correct the following errors:") %>
    </div>
    <div>
        <div>
            <% if (Model.OrderAccepted)
               { %>
                Order Confirmation for Invoice # <% = Model.Order.InvoiceNumber.GetValueOrDefault(0) %>
                <% = Html.ActionLink("View Invoice", "Invoice", "Account", new {Model.CustomerData.CustomerID, Model.CustomerData.CompanyCode, InvoiceNumber = Model.Order.InvoiceNumber.GetValueOrDefault(0)}, null) %><br/>
                <% if ((bool) ViewData["MillionthPartEnabled"])
                   { %>
                    <!-- Millionth Part UI -->
                    <div class="dialog" id="MillionthPartDialog">
                        <% using (Ajax.BeginForm("Submit", "MillionthPart", null, new AjaxOptions {OnFailure = "ajaxError", OnComplete = "hideProcessingBlock", OnBegin = "beginMillionthPartEntry", OnSuccess = "contestEntryComplete"}, new {id = "MillionthPartForm"}))
                           { %>
                            <% Html.RenderAction("Index", "MillionthPart", new {UserID = Model.CustomerData.UserName, Model.CustomerData.CustomerID, Model.CustomerData.CompanyCode, OrderNumber = Model.Order.InvoiceNumber, ValidationCode = (Model.CustomerData.CustomerID*2 + Model.Order.InvoiceNumber - 3)*2, popup = true}); %>
                        <% } %>
                    </div>
                <% } %>
            <% } %>
        </div>
        <div style="clear: both">
            <div class="threecolumn">
                <div class="em">Sold To:</div>
                <% Html.RenderPartial("Address", Model.Order.SoldToAddress); %><br/>
                <% if (!string.IsNullOrEmpty(Model.Order.UserName))
                   { %>
                    Ordered By: <% = Html.Encode(Model.Order.UserName) %><br/>
                <% } %>
            </div>

            <div class="threecolumn">
                <div>
                    <span class="em">Ship To:</span>&nbsp;
                    <% if (!Model.OrderAccepted && !Model.Order.IsPowerUserOrder)
                       { %>
                        <a id="ChangeShipToAddress" href="#">Change</a>
                    <% } %>
                </div>
                <div id="ShipToAddress">
                    <% if (Model.Order.HasShipToAddress)
                       { %>
                        <% Html.RenderPartial("Address", Model.Order.ShipToAddress); %>
                    <% }
                       else
                       { %>
                        <% Html.RenderPartial("Address", Model.Order.SoldToAddress); %>
                    <% } %>
                </div>
                <% if (Model.Order.IsPowerUserOrder)
                   { %>
                    <div>
                        <b>Power User Order</b>
                    </div>
                    <div>Primary: <% = Html.Encode(Model.Order.PrimaryWarehouse) %> Secondary: <% = Html.Encode(Model.Order.SecondaryWarehouse) %></div>
                <% } %>
            </div>

            <div class="threecolumn">
                <div>
                    <label>Requestor:</label>
                    <% = Html.Encode(Model.Order.UserName) %>
                </div>
                <div>
                    <label>Date:</label>
                    <% = Html.Encode(DateTime.Now.ToShortDateString()) %>
                </div>
                <div>
                    <label>Ship Method:</label>
                    <% if (Model.OrderAccepted)
                       { %>
                        <% = Html.Encode(Model.ShipMethodCodeToName(Model.Order.RequestedShipMethod)) %>
                    <% }
                       else
                       { %>
                        <% using (Ajax.BeginForm("SetOrderShipMethod", null, new AjaxOptions {LoadingElementId = "SetShipMethodIndicator", OnFailure = "handleError"}, new {id = "ChangeOrderShipMethodForm"}))
                           { %>
                            <% = Html.DropDownList("OrderShipMethod", Model.GetShippingMethods(true, Model.Order.RequestedShipMethod, null), (string) null) %>
                            <input type="submit" value="Set" style="display: none" id="SubmitChangeOrderShipMethod"/>
                            <% = Html.AjaxIndicator("SetShipMethodIndicator") %>
                        <% } %>
                    <% } %>
                </div>

                <div>
                    <label>PO Number:</label>
                    <% if (Model.OrderAccepted)
                       { %>
                        <% = Html.Encode(Model.Order.PONumber) %>
                    <% }
                       else
                       { %>
                        <% = Html.TextBox("PONumber", Model.Order.PONumber, new {maxlength = 10, @class = "ponumber"}) %>
                        <input id="SubmitPOChange" type="submit" value="Set"/>
                        <% = Html.AjaxIndicator("SetPOIndicator") %>
                    <% } %>
                </div>
                <div>
                    <a href="#" id="toggleCustomerReference"><span class="customer-reference-toggle">Hide</span><span class="customer-reference-toggle customer-reference-hidden" style="display: none">Show</span> Customer Reference</a>
                </div>
            </div>
            <div class="clear"></div>
            <div>
                <% if (Model.Order.Quote != null)
                   { %>
                    Based on Quote: <% = Html.Encode(Model.Order.Quote.QuoteDescription) %>
                <% } %>
            </div>
        </div>
        <br/>
        <div id="OrderDetails">
            <% Html.RenderPartial("OrderDetails"); %>
        </div>

        <% Html.RenderPartial("EditAddress", Model.Order.ShipToAddress); %>
        <br/>
        <% /* if (!Model.OrderAccepted) { */ %>
        <div class="footnote">
            <div class="em">Shipping Notes:</div>
            <div>
                * Items eligible for “Pooled” shipping will be sent daily to your home EPWI location at no or reduced shipping cost. Exempt items do not qualify for Pool shipping.
            </div>
            <div>
                ** The "Other" shipping option requires that you enter in shipping information in the order notes section and that the
                order will require manual processing.
            </div>
        </div>
        <br/>
        <div class="footnote">
            <div class="em">Pricing Notes:</div>
            <div>
                The prices listed above reflect the standard invoice price as it was displayed when the stock status of the item was retreived.
                The price does not include any discounts for Elite status and it may have changed since the item was added to your order.
                When you are ready to complete your order, all items will be rechecked to verify stock status and current pricing.
                From that point, you may choose to finish your order, or to modify, interchange or delete any items.
                If you have any questions, please contact your EPWI warehouse.
            </div>
        </div>
        <% /* } */ %>
    </div>
    <% if (Model.JustLoadedFromQuote)
       { %>
        <div class="dialog" id="QuoteWarning">
            You are retrieving a quote that was created on <% = Model.QuoteCreateDate.Value.ToShortDateString() %>. Pricing and availability may have changed. These will be re-checked when you submit the order.
            <div class="buttons">
                <input type="button" value="Ok" class="close"/>
            </div>
        </div>
    <% } %>
    <div class="dialog" id="SaveOrderAsQuote">
        <% Html.RenderPartial("SaveAsQuoteForm"); %>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <%--<script src="<%= Url.Content("~/Scripts/jquery-ui-1.12.0-rc.2.js") %>" type="text/javascript"></script>--%>
    <%--<script src="<%= Url.Content("~/Scripts/site/OrderForm-2.1.js?v=2") %>" type="text/javascript"></script>--%>
    <script src="<%= Url.Content("~/Scripts/site/RequestQuoteHelp.js") %>" type="text/javascript"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="SideBarContent" runat="server">
    <% if (!Model.OrderAccepted)
       { %>
        <div class="sideContainer roundedCorners">
            <h2 class="roundedCorners">Need Help With Your Order?</h2>
            <div class="roundedCornersBody">
            </div>
            <div class="roundedCornersFooter" style="color: black">
                <p style="text-align: left">Click the "Save Order As Quote" button and check the "I would like help with this quote from an EPWI CSR" checkbox.</p>
            </div>
        </div>
    <% } %>

</asp:Content>