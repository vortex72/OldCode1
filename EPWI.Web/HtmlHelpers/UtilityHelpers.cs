using System.Collections.Generic;
using System.Web.Mvc;
using System.Text;
using System.Globalization;


namespace EPWI.Web.HtmlHelpers
{
	public static class UtilityHelpers
	{
		public static string AjaxIndicator(this HtmlHelper html, string id)
		{
			var Url = new UrlHelper(html.ViewContext.RequestContext);
			TagBuilder span = new TagBuilder("span");

			span.MergeAttribute("id", id);
			span.MergeAttribute("style", "display:none");

			TagBuilder img = new TagBuilder("img");
			img.MergeAttribute("src", Url.Content("~/Content/images/indicator_arrows.gif"));

			span.InnerHtml = img.ToString(TagRenderMode.SelfClosing);

			return span.ToString();
		}

    public static string ActionButton(this HtmlHelper helper, string value, string action, string controller, object routeValues)
    {
      UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
      var actionUrl = urlHelper.Action(action, controller, routeValues);

      var html = new StringBuilder();
      html.AppendFormat("<form method='get' action='{0}'>", actionUrl).AppendLine()
          .AppendFormat("    <input type='submit' value='{0}' />", value).AppendLine()
          .AppendFormat("</form>").AppendLine();

      return html.ToString();
    }

    public static IEnumerable<SelectListItem> MonthSelectList(int? monthToSelect)
    {
      var months = new List<SelectListItem>();

      for (int i = 1; i <= 12; i++)
      {
        months.Add(new SelectListItem() { Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i), Value = i.ToString(), Selected = (monthToSelect.GetValueOrDefault(0) == i) });
      }

      return months;
    }
	}
}
