<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
if (typeof jQuery == 'undefined')
{
    document.write(unescape("%3Cscript src='<% = Url.Content("~/Scripts/jquery-3.0.0.min.js") %>' type='text/javascript'%3E%3C/script%3E"));
}
</script>
<script src="http://ajax.microsoft.com/ajax/jQuery.Validate/1.6/jQuery.Validate.js" type="text/javascript" ></script>
<script type="text/javascript">
if (!jQuery().validate)
{
    document.write(unescape("%3Cscript src='<% = Url.Content("~/Scripts/jQuery.validate.js") %>' type='text/javascript'%3E%3C/script%3E"));
}
</script>