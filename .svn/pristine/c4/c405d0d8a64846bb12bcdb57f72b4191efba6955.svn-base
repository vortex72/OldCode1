<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div class="sideContainer roundedCorners">
    <h2 class="roundedCorners">View Invoice</h2>
    <div class="roundedCornersBody">
        <% using (Html.BeginForm("InvoiceByNumber", "Account", FormMethod.Post, new { Id = "InvoiceByNumberForm" } )){ %>        
            <% = Html.TextBox("InvoiceNumber", null, new { @class = "numeric", maxlength="7", style="width:50%" }) %>
            <input type="submit" value="View"/>
        <% } %>
    </div>
    <div class="roundedCornersFooter"><p style="text-align:left">&nbsp;</p></div>
</div>
