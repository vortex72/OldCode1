<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EPWI.Web.Models.StockStatusViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Stock Status
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="FirstScriptContent" runat="server">
    <script type="text/javascript">
        var opticatPartInfo = <% = Model.OpticatPartInfoJson %>;
    </script>
    <script src="<%= Url.Content("~/Scripts/underscore.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-3.0.0.min.js") %>" type="text/javascript"></script>
    <script src="<% = Url.Content("~/Scripts/jquery-migrate-1.2.1.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.12.0-rc.2.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.colorbox-min.js")%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/knockout-3.4.0.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/site/PartViewer.js?v=9") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/knockout-jqueryui.min.js") %>" type="text/javascript"></script>    

    <script type="text/javascript">
        (function ($, ko) {

            $(function () {
                if (opticatPartInfo) {
                    var partInfoModel = { partData: opticatPartInfo };
                    ko.applyBindings(partInfoModel, $('#part-details')[0]);
                }
            });

        })(jQuery, ko);

    </script>
    <link href="<%= Url.Content("~/Content/css/colorbox.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/css/Smoothness/jquery-ui-1.12.0-rc.2.css") %>" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        //var newjQuery = jQuery.noConflict();
    </script>    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<% = Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<% = Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>
    <link type="text/css" href="<%= Url.Content("~/Content/css/StockStatus.css") %>" rel="stylesheet" />
	<script src="<% = Url.Content("~/Scripts/site/StockStatus-2.1.js") %>" type="text/javascript"></script>
	<script src="<% = Url.Content("~/Scripts/site/kitdialog.js") %>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%="" %>
    <h2>Stock Status Inquiry</h2>
    <% if (!string.IsNullOrEmpty(Model.LastViewedKit)) { %>
        <div><% = Html.ActionLink("Return to last viewed kit", "View", "Kit", new { id = Model.LastViewedKit }, null) %></div>
    <% } %>
         <table id="stockStatusContainer" border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                  <td>
                    <% using (Html.BeginForm("Search", "StockStatus")) { %>
                    <div><% = Html.Hidden("CurrentView", Model.CurrentView) %></div>
              	    <table id="stockStatusInformation" class="stockStatusBlock" border="0" cellpadding="0" cellspacing="0" width="100%" >  
                      <tr>
                        <th colspan="2">Item Information</th>
                      </tr>
                      <tr>
                        <td>Item&nbsp;Number:</td>
                        <td>
                            <% if (Model.InventoryItems == null || !Model.InventoryItems.Any()) { %>
                                <%=Html.TextBox("RequestedItemNumber", null, new { @class = "overwrite", tabindex= "1", maxlength = "25" })%>
                                <input type="submit" value="Submit" class="ShowIndicator" tabindex="3" /> 
                            <% } else { %>
                                <input id="RequestedItemNumber" name="RequestedItemNumber" type="hidden" value="<% = Html.Encode(Model.RequestedItemNumber)%>" />
                                <% = Html.Encode(Model.RequestedItemNumber)%>&nbsp;
                                <% if (Model.ItemNumberChanged) { %>
                                    <div class="highlight">Note: The Item Wizard has modified the part number entered (<% = Html.Encode(Model.OriginalItemNumber)  %>)</div>
                                <% } %>
                            <% } %>
                        </td>
                      </tr>
                      <tr>
                        <td>Quantity:</td>
                        <td>
                            <% = Html.TextBox("RequestedQuantity", null, new { @class = "overwrite", tabindex = "2", maxlength = "3"})%>
                            <% if (Model.RequestComplete || !Model.CanPlaceOrder) { %><input type="submit" value="Submit" class="ShowIndicator" tabindex = "4"/><% } %>        
                        </td>
                      </tr>                      
                      <tr>
                        <td>Size:</td>
                        <td>
                            <% if (Model.NeedToSelectSize) { %>
                                <% = Html.DropDownList("RequestedSize", Model.InventoryItem.Sizes.Select(s => new SelectListItem() { Text = (s == "STD" ? "Standard" : s.Substring(0, 3).Trim()), Value = s.Substring(0, 3).Trim() }))%>
                                <input type="submit" value="Submit" class="ShowIndicator" /> 
                            <% } else { %>
                                <%  if (!string.IsNullOrEmpty(Model.RequestedSize)) { %> 
                                    <% = Html.Hidden("RequestedSize", Model.RequestedSize)%> 
                                <% } %>
                                <% = Html.Encode(Model.RequestedSize)%>&nbsp;
                            <% } %>
                        </td>
                      </tr>
                      <tr>
                        <td>Line&nbsp;Code:</td>
                        <td>
                            <% if (Model.NeedToSelectLineCode) { %>
                                <% =Html.DropDownList("RequestedLineCode", Model.InventoryItems.Select(i => new SelectListItem() { Text = $"{i.LineDescription} (PN:{i.ItemNumber})", Value = $"{i.LineCode}|{i.ItemNumber}"}), "--- Please Select a Product Line ---")%>
                                <input type="submit" value="Submit" class="ShowIndicator" /> 
                            <% } else { %>
                                <%   if (!string.IsNullOrEmpty(Model.RequestedLineCode)){ %> 
                                    <input id="RequestedLineCode" name="RequestedLineCode" type="hidden" value="<% = Html.Encode(Model.RequestedLineCode)%>" />
                                <% } %>
                                <% = Html.Encode(Model.RequestedLineCodeDescription)%>&nbsp;
                            <% } %>
                        </td>
                      </tr>
                      
                      <tr>
                        <td>Item&nbsp;Options:</td>
                        <td>
                            <% if (!string.IsNullOrEmpty(Model.RequestedItemNumber)) { %>
                                <span class="nowrap"><% = Html.ActionLink("Check Another Part", "Index") %></span>
                            <% } %>
                            <% if (Model.ShowInterchanges) { %>
                                &nbsp;|&nbsp; <span class="nowrap"><a href="#" id="ShowInterchanges">View Interchanges</a> </span>
                            <% } %>
                            <% if (Model.RequestComplete && System.IO.File.Exists(Server.MapPath($"~/Content/Products/{Model.InventoryItem.NIPCCode}.jpg"))) { %>
                                &nbsp;|&nbsp; <span class="nowrap"><a href="#" id="ViewImage">View Image</a></span>
                            <% } %>
                            
                            <% if (Model.SearchSubmitted) { %> &nbsp;|&nbsp; <% } %>
                            <span class="nowrap"><a href="#" id="SwitchView">Switch to View <% = Model.CurrentView == 1 ? 2 : 1 %></a></span>
                            <input type="submit" style="display:none" name="switchView" id="SwitchViewButton" />
                        </td>
                      </tr>
                      <tr>
                        <td>Description:</td>
                        <td>
                            <% if (Model.RequestComplete) { %>
                                <%= Html.Encode(Model.InventoryItem.ItemDescription) %>
                            <% } else { %> 
                                &nbsp;
                            <% } %>
                        </td>
                      </tr>
                      <% if (Model.RequestComplete && Model.OpticatPartInfoJson != "null") { %>
                      <tr>
                        <td>Details: </td>
                        <td id="part-details" data-bind="partViewer: partData"></td>
                      </tr>
                      <% } %>
                      <tr>
                        <td>Messages:</td>
                        <td>
                            <% = Html.ValidationSummary(string.Empty, new { @class = "highlight error" })%>
                            <% foreach (string message in Model.GetMessages(this.ViewContext.RequestContext)) { %>
                                <div><% = message%></div>
                            <% } %>
                            <% if (Model.StockStatus != null && Model.StockStatus.DenyPurchase) { %>
                                <div>Once registered as part of their MVP program, MSD and Superchips will allow you to purchase products.</div>
                                <div>Visit <a href="http://www.msdpmvp.com" target="_blank"><b>www.msdmvp.com</b></a> to sign up.</div>
                                <div>After signing up, please contact EPWI by phone with your MSD or Supership order.</div>
                            <% } %>
                        </td>
                      </tr>
                    </table>
                    <% } // end main form %>
                  </td>
                  <td>
                    <% if(Model.RequestComplete) { %>
                        <% Html.RenderPartial("Prices"); %><div>&nbsp;</div>
              	        <% Html.RenderPartial("WarehouseAvailability", Model); %>
              	        <% Html.RenderPartial("OrderOptions", Model); %>
                    <% } %>
                  </td>
                </tr>
              </table>
        <%  if (Model.RequestComplete) {
               Html.RenderPartial("Interchanges", Model);
        %>
            <div class="dialog" id="ProductImage"><img src="<% = Url.Action("ProductImage", "StockStatus", new { id = Model.InventoryItem.NIPCCode }) %>" />
            <div><% = Html.ActionLink("Download Image", "ProductImage", "StockStatus", new { id = Model.InventoryItem.NIPCCode, download = true }, null) %></div>
            </div>  
        <% } %>
        <% Html.RenderPartial("PartViewer"); %>
</asp:Content>
