using EPWI.Components.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace EPWI.Web.HtmlHelpers
{
    public static class FormattingHelpers
    {
        public static string FormattedPhone(this HtmlHelper html, string phone)
        {
            if (!string.IsNullOrEmpty(phone) && phone.Trim().Length > 3)
            {
                phone = phone.Trim();
                // remove the leading 1 if it exists
                if (phone.StartsWith("1"))
                {
                    phone = phone.Substring(1);
                }
                if (phone.Length >= 7)
                {
                    phone = $"({phone.Substring(0, 3)}) {phone.Substring(3, 3)}-{phone.Substring(6)}";
                }
            }
            else
            {
                phone = string.Empty;
            }
            return html.Encode(phone);
        }

        public static string KitLink(this HtmlHelper html, KitIdentifier kitID)
        {
            return
                html.ActionLink($"{kitID.KitID} {kitID.KitType}", "Search", "StockStatus",
                    new { RequestedQuantity = 1, RequestedItemNumber = kitID.KitPartNumber }, null).ToString();
        }

        public static string KitCategoryNoteLabel(this HtmlHelper html, Kit kit, KitCategory kitCategory)
        {
            var result = string.Empty;

            var notes = kit.GetCategoryNotes(kitCategory);

            if (notes.Count() > 0)
            {
                result = $"<span class=\"note\">NOTE {notes.First().NoteOrdinal}</span><br/>";
            }

            return result;
        }

        public static string KitPart(this HtmlHelper html, Kit kit, KitPart part)
        {
            return KitPart(html, kit, part, true, false, false, null);
        }

        public static string KitPart(this HtmlHelper html, Kit kit, KitPart part, bool masterKitPart, bool configuring,
            bool confirmingAvailability, FulfillmentProcessingResult result)
        {
            var Url = new UrlHelper(html.ViewContext.RequestContext);
            var displayProcessingAlert = false;
            var partClass = string.Empty;
            var partToolTip = string.Empty;
            var sb = new StringBuilder();
            var isCrankKit = !masterKitPart &&
                              RelatedCategoryMapping.Mappings[RelatedCategory.CrankKit].CategoryID.Contains(
                                  part.CategoryID);
            var displayPart = (configuring && isCrankKit && part.NIPCCode == kit.SelectedCrankKitNIPC) || !isCrankKit ||
                               (isCrankKit && !configuring) ||
                               (isCrankKit && configuring && kit.SelectedCrankKitNIPC == 0);

            // patch so that any Crank Kit Rebuilders of EPW parts under Crank Kits have the
            // LineCode and Manufacturer removed and forces the user to select a line
            if (part.CategoryID == 85 && (part.LineCode == "CRA" || part.LineCode == "EPK"))
            {
                part.LineCode = string.Empty;
                part.LineDescription = "CRANKSHAFT REBUILDERS INC. or EPW";
            }

            if (confirmingAvailability && result.ProcessedNipcCodes != null &&
                ((result.ProcessedNipcCodes.Contains(part.UniqueKitPartIdentifier) ||
                  result.ProcessedNipcCodes.Contains(part.NIPCCode.ToString())) && part.IsAvailable))
            {
                displayProcessingAlert = true;
                partClass = "class=\"ProcessingAlert\"";
            }
            else if (confirmingAvailability && !part.IsAvailable && part.Selected)
            {
                partClass = "class=\"Unavailable\"";
            }
            if (HttpContext.Current.User.IsInRole("ADMIN"))
            {

                if (!masterKitPart)
                {
                    partToolTip = $" title=\"NIPC={part.NIPCCode}\"";
                }
                else
                {
                    partToolTip = $" title=\"NIPC={part.NIPCCode} Price={part.Price} Avail={part.IsAvailable}\"";
                }
            }

            sb.AppendFormat("<span {0}{1}>", partClass, partToolTip);

            // for crank kits and additional parts, show disabled checkbox if it is selected
            if (isCrankKit && displayPart && configuring && kit.SelectedCrankKitNIPC == part.NIPCCode)
            {
                sb.Append(
                    @"<input type=""checkbox"" checked=""checked"" disabled=""disabled"" id=""CrankKitCheckbox"" />");
            }
            else if (configuring && (masterKitPart || part.CategoryID == 0))
            {
                if (string.IsNullOrEmpty(kit.AcesID))
                {
                    // show action button as appropriate, for non-aces kit builds
                    if (confirmingAvailability)
                    {
                        // when confirming availability, only show part actions if the part is not available
                        if (part.Selected && !part.IsAvailable)
                        {
                            sb.Append(getPartAction(part, html));
                        }
                    }
                    else
                    {
                        sb.Append(getPartAction(part, html));
                    }
                }

                // add checkbox        
                var checkboxAttributes = new Dictionary<string, object>();
                var checkboxClasses = new List<string> { "MasterKitPart", part.GroupName };

                if (confirmingAvailability && !part.IsAvailable && part.Selected)
                {
                    checkboxClasses.Add("unavailable");
                }

                if (part.IsNotPartOfGroup)
                {
                    checkboxClasses.Add("standalone");
                }

                if (part.IsPartOfAndGroup)
                {
                    checkboxClasses.Add("AndedPart");
                    checkboxAttributes.Add("data-andgroup", $"{part.GroupingMain}_{part.GroupingOr}");
                }

                checkboxAttributes.Add("class", string.Join(" ", checkboxClasses.ToArray()));
                checkboxAttributes.Add("id", $"{part.UniqueKitPartIdentifier}");

                if (part.CategoryID == 0 ||
                    (confirmingAvailability &&
                     (part.IsAvailable || !part.Selected || !string.IsNullOrEmpty(part.ShipWarehouse) ||
                      displayProcessingAlert)))
                {
                    checkboxAttributes.Add("disabled", "disabled");
                }

                // disable the checkbox if the part is an interchange, unless the part is not known, in which case it will be a reload from a quote
                // disabled -- this breaks all sorts of functionality
                //if (part.InterchangeMethod == "I" && !string.IsNullOrEmpty(part.OriginalPartUniqueID))
                //{
                //  checkboxAttributes.Add("disabled", "disabled");
                //}
                sb.Append("<span class=\"PartCheckbox\">");
                sb.Append(html.CheckBox(part.UniqueKitPartIdentifier, part.Selected, checkboxAttributes));
                sb.Append("</span>");
            }

            if (displayPart)
            {
                sb.Append("<span class=\"PartDescription\">");

                if (!configuring)
                {
                    sb.Append(html.ActionLink(part.ItemNumber, "Search", "StockStatus",
                        new
                        {
                            RequestedItemNumber = part.ItemNumber,
                            RequestedLineCode = part.LineCode,
                            RequestedQuantity = part.QuantityRequired
                        }, null));
                }
                else
                {
                    if (!confirmingAvailability && !masterKitPart && !isCrankKit && part.InterchangeMethod != "K")
                    {
                        // if the part is an additional part that hasn't been added to the kit, make it a link so it can be added to the kit
                        sb.AppendFormat(html.ActionLink(part.ItemNumber, "KitSearch", "StockStatus",
                            new
                            {
                                RequestedItemNumber = part.ItemNumber,
                                RequestedLineCode = part.LineCode,
                                RequestedQuantity = part.QuantityRequired
                            },
                            new { @class = "itemNumber additionalPart", style = "cursor:pointer" }).ToString());
                    }
                    else
                    {
                        sb.AppendFormat("<span class=\"itemNumber\">{0}</span>", part.ItemNumber);
                    }
                }
                sb.Append(" ");
                sb.Append(html.Encode(part.LineDescription));
                sb.Append(" ");

                // show quantity required if it's more than one
                if (part.QuantityRequired > 1)
                {
                    sb.Append(html.Encode($" ({part.QuantityRequired}) "));
                }

                // if we have a note or need to display years, render parenthesis and appropriate content
                if (!string.IsNullOrEmpty(part.Note) || part.DisplayYears)
                {
                    sb.Append("<span class=\"note\">");
                    sb.Append("(");
                    if (part.DisplayYears)
                    {
                        sb.Append(html.Encode(part.Years));
                        if (!string.IsNullOrEmpty(part.Note))
                        {
                            sb.Append(" ");
                        }
                    }

                    if (!string.IsNullOrEmpty(part.Note))
                    {
                        sb.Append(html.Encode(part.Note));
                    }
                    sb.Append(") ");
                    sb.Append("</span>");
                }

                if (!string.IsNullOrEmpty(part.ShipWarehouse) && part.ShipWarehouse.Trim() != string.Empty &&
                    part.OrderMethod != OrderMethod.MainWarehouse.ToCode())
                {
                    sb.Append("<span class=\"ShipWarehouse\">(");
                    if (part.ShipWarehouse == "XXX")
                    {
                        sb.Append("DROP SHIP");
                    }
                    else
                    {
                        sb.Append($"Shipping from: {part.ShipWarehouse}");
                    }
                    sb.Append(")</span> ");
                }

                // display size if we are at the configuring stage and the kit is not a KTRACK
                if (configuring && !kit.IsKTRACK && (new int[] { 10, 11, 15, 20, 21, 23, 0 }).Contains(part.CategoryID))
                {
                    if (part.CategoryID != 0 || (part.CategoryID == 0 && !string.IsNullOrEmpty(part.SizeCode)))
                    {
                        // category 0 is added related parts. Only show the size for them if it is not null/empty
                        sb.AppendFormat("<span class=\"sizeCode\">");
                        sb.AppendFormat("(Size:{0})", string.IsNullOrEmpty(part.SizeCode) ? "STD" : part.SizeCode);
                        sb.AppendFormat("</span>");
                    }
                }
                sb.Append("</span>");
            }
            sb.Append("</span>");
            return sb.ToString();
        }

        private static string getPartAction(KitPart part, HtmlHelper html)
        {
            var Url = new UrlHelper(html.ViewContext.RequestContext);
            string INTERCHANGE_LINK =
                $@"<img src=""{Url.Content("~/Content/images/kit_subst.gif")}"" alt=""Substitute Part"" class=""Substitute"" />";
            string REVERT_LINK =
                $@"<img src=""{Url.Content("~/Content/images/kit_revert.gif")}"" alt=""Revert Substitution"" class=""Revert"" />";

            switch (part.InterchangeMethod)
            {
                case "K": // The part has been added, so only allow the user to remove the part
                    return
                        $@"<img src=""{Url.Content("~/Content/images/kit_remove.gif")}"" alt=""Remove Part"" class=""Revert"" />";
                case "S":
                    return string.Format(INTERCHANGE_LINK);
                default: // the part has already been interchanged, so only allow the user to revert the interchange
                    return string.Format(REVERT_LINK);
            }
        }

        public static string AlphaPicker(this HtmlHelper html, string action, string filter)
        {
            var sb = new StringBuilder();

            if (string.IsNullOrEmpty(filter))
            {
                sb.Append("<strong>");
            }

            var Url = new UrlHelper(html.ViewContext.RequestContext);

            sb.AppendFormat("<a href=\"{0}\">ALL</a>", fixUrl(Url.Action(action, new { id = "ALL" })));

            if (string.IsNullOrEmpty(filter))
            {
                sb.Append("</strong>");
            }

            sb.Append("&nbsp;|&nbsp;");
            for (var i = 65; i < 91; i++)
            {
                var character = Convert.ToChar(i).ToString();
                if (filter == character)
                {
                    sb.Append("<strong>");
                }
                sb.AppendFormat("<a href=\"{0}\">{1}</a>", fixUrl(Url.Action(action, new { id = character })), Convert.ToChar(i).ToString());

                if (filter == character)
                {
                    sb.Append("</strong>");
                }
                if (i != 90)
                {
                    sb.Append("&nbsp;|&nbsp;");
                }
            }

            return sb.ToString();
        }

        public static string PageLinks(this HtmlHelper html, int currentPage, int totalPages, Func<int, string> pageUrl)
        {
            var sb = new StringBuilder();

            if (currentPage > 1)
            {
                var tag = new TagBuilder("a");
                var url = fixUrl(pageUrl(currentPage - 1));
                //if (currentPage == 2)
                //{ // hack--for some reason the route provider for n2cms doesn't like page = 1
                //  url = url.Replace("page=1", string.Empty);
                //}
                tag.MergeAttribute("href", url);
                tag.InnerHtml = "<";
                sb.Append(tag.ToString());
                sb.Append("&nbsp;");
            }
            else
            {
                sb.Append("&nbsp;&nbsp;");
            }

            sb.AppendFormat("Page {0} of {1}", currentPage, Math.Max(1, totalPages));

            if (currentPage < totalPages)
            {
                sb.Append("&nbsp;");
                var tag = new TagBuilder("a");
                tag.MergeAttribute("href", fixUrl(pageUrl(currentPage + 1)));
                tag.InnerHtml = ">";
                sb.Append(tag.ToString());
            }
            else
            {
                sb.Append("&nbsp;&nbsp;");
            }

            return sb.ToString();
        }

        /// <summary>
        /// For some reason, Url routes generated on the N2CMS pages include the application root twice
        /// for now, this hack removes the duplicate root if it exists
        /// </summary>
        /// <param name="url">the url to fix</param>
        /// <returns>fixed url</returns>
        private static string fixUrl(string url)
        {
            var tokens = url.Split('/');

            if (tokens.Length > 2 && tokens[1].ToLower() == tokens[2].ToLower())
            {
                url = url.Remove(0, tokens[1].Length + 1);
            }

            return url;
        }

        public static string SafeInternalLink(this HtmlHelper html, string url)
        {
            var Url = new UrlHelper(html.ViewContext.RequestContext);

            try
            {
                return Url.Content("~/" + url);
            }
            catch (System.Web.HttpException)
            { // don't let malformed urls crash the site. just return the url
                return url;
            }
        }
    }
}
