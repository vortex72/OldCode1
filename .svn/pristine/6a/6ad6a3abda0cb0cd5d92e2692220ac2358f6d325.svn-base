<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% using(Html.BeginForm("Delete", "Order")) { %>
<div>Your quote has been saved. Would you also like to process this order?</div><br />
<div class="buttons"><input type="button" value="Yes, process this order" id="PostQuoteProcessOrder" /><input type="submit" value="No, don't process this order" id="PostQuoteDoNotProcessOrder" class="close" />
<% = Html.Hidden("StockStatusURL", Url.Action("Search", "StockStatus")) %>
<input type="hidden" id="QuoteSaved" value="true" />
</div>
<% } %>