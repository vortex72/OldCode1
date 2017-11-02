using System;
using N2;

namespace EPWI.Web.HtmlHelpers
{
  public static class ContentMangementHelpers
  {
    public static bool AnyParentIsType(this ContentItem item, Type searchType, bool includeSelf)
    {
      if (includeSelf && item.GetType() == searchType)
      {
        return true;
      }

      return AnyParentIsType(item, searchType);
    }

    public static bool AnyParentIsType(this ContentItem item, Type searchType)
    {
      if (item.Parent != null)
      {
        if (item.Parent.GetType() == searchType)
        {
          return true;
        }
        else
        {
          return AnyParentIsType(item.Parent, searchType);
        }
      }
      return false;
    }
  }
}
