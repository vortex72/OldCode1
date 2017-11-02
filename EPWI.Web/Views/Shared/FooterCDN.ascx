<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script src="http://ajax.microsoft.com/ajax/3.5/MicrosoftAjax.js " type="text/javascript"></script>
<script type="text/javascript">
if (typeof Sys == 'undefined')
{
    document.write(unescape("%3Cscript src='<% = Url.Content("~/Scripts/MicrosoftAjax.js") %>' type='text/javascript'%3E%3C/script%3E"));
}
</script>
<script src="http://ajax.microsoft.com/ajax/mvc/1.0/MicrosoftMvcAjax.js" type="text/javascript"></script>
<script type="text/javascript">
if (typeof Sys.Mvc == 'undefined')
{
    document.write(unescape("%3Cscript src='<% = Url.Content("~/Scripts/MicrosoftMvcAjax.js") %>' type='text/javascript'%3E%3C/script%3E"));
}
</script>