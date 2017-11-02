using System.Web.Mvc;
using System.Web.Routing;

namespace EPWI.Web.Utility
{
    public static class ImageHelper
    {
        public static string Image(string url,
            object htmlAttributes = null)
        {
            TagBuilder builder = new TagBuilder("img");
            builder.Attributes.Add("src", url);
            //builder.Attributes.Add("alt", altText);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return builder.ToString(TagRenderMode.SelfClosing);
        }


        //public static string Image(this HtmlHelper helper,
        //    string url,
        //    string altText,
        //    object htmlAttributes)
        //{
        //    TagBuilder builder = new TagBuilder("img");
        //    builder.Attributes.Add("src", url);
        //    builder.Attributes.Add("alt", altText);
        //    builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
        //    return builder.ToString(TagRenderMode.SelfClosing);
        //}

        //public static string Image<T>(this HtmlHelper<T> helper,
        //    string url,
        //    string altText,
        //    object htmlAttributes)
        //{
        //    TagBuilder builder = new TagBuilder("img");
        //    builder.Attributes.Add("src", url);
        //    builder.Attributes.Add("alt", altText);
        //    builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
        //    return builder.ToString(TagRenderMode.SelfClosing);
        //}

    }

}