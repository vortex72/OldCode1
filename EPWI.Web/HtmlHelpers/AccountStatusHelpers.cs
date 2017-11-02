using System.Text;
using System.Web.Mvc;

namespace EPWI.Web.HtmlHelpers
{
  public static class AccountStatusHelpers
  {
    public static string ARField(this HtmlHelper html, string text, decimal amount, bool addSeperatorLine, bool boldNumber)
    {
      var sb = new StringBuilder();

      if (amount != 0) 
      {
        sb.Append("<tr>");
        sb.AppendFormat("<td align=\"right\">{0}:&nbsp;</td>", text);
        if (addSeperatorLine)
        {
          sb.Append("<td align=\"right\" style=\"border-top:2px solid black\" nowrap=\"nowrap\">");
        } 
        else 
        {
          sb.Append("<td align=\"right\" nowrap=\"nowrap\">");
        }
        if (boldNumber)
        {
          sb.Append("<b>");
        }
        
        sb.Append(amount.ToString("C2"));

        if (boldNumber)
        {
          sb.Append("</b>");
        }
        sb.Append("</td></tr>");
      }

      return sb.ToString();
    }
  }
}
