
<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<EPWI.Components.Models.MillionthPartViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Millionth Part Contest</title>
    <style type="text/css">
        body { font: 0.7em/1.5 Geneva, Arial, Helvetica, sans-serif; }
        .error { color: Red }
    </style>
<%--    <link type="text/css" href="<%= Url.Content("~/Content/css/smoothness/jquery-ui-1.7.2.custom.css") %>" rel="stylesheet" />--%>
    <script src="http://ajax.microsoft.com/ajax/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="http://ajax.microsoft.com/ajax/jQuery.Validate/1.6/jQuery.Validate.js" type="text/javascript" ></script>
    <%--<script src="<% = Url.Content("~/Scripts/jquery-ui-1.7.2.custom.min.js") %>" type="text/javascript" ></script>--%>
    <script src="<% = Url.Content("~/Scripts/jquery-ui-1.12.0-rc.2.js") %>" type="text/javascript" ></script>

    <script type="text/javascript">
        $(function() {
            $('#GuessDate').datepicker();
            $('#MillionthPartForm').validate();
        });
    </script>
</head>
<body>
    <div>
        <div><img src="<% = Url.Content("~/Content/images/epwi_logo.gif") %>" /><br /><br /></div>
        <% using (Html.BeginForm("Submit", "MillionthPart", FormMethod.Post, new { id = "MillionthPartForm" })) { %>
            <% Html.RenderPartial("GuessFormFields"); %>
        <% } %>
    </div>
</body>
</html>
