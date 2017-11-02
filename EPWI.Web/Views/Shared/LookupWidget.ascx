<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div class="sideContainer roundedCorners">
    <h2 class="roundedCorners">
        <% = Html.ActionLink("Year/Make/Model Lookup", "Index", "Lookup") %>
    </h2>
    <div class="roundedCornersFooter">
        <p style="text-align: left; height: 1px">
            &nbsp;</p>
    </div>
</div>
