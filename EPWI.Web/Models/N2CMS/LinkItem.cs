using N2;
using N2.Details;
using N2.Integrity;

namespace EPWI.Web.Models.N2CMS
{
    [PageDefinition("Link Item")]
    [RestrictParents(typeof (LinkItemContainer))]
    [AllowedZones("Links")]
    [WithEditableTitle, WithEditableName]
    public class LinkItem : ContentItem, INode
    {
        public override bool IsPage
        {
            get { return false; }
        }

        [EditableImage("Thumbnail", 25)]
        public virtual string Thumbnail
        {
            get { return (string) (GetDetail("Thumbnail") ?? string.Empty); }
            set { SetDetail("Thumbnail", value, string.Empty); }
        }

        [EditableTextBox("Summary", 50)]
        public virtual string Summary
        {
            get { return (string) (GetDetail("Summary") ?? string.Empty); }
            set { SetDetail("Summary", value, string.Empty); }
        }

        [EditableUrl("LinkUrl", 50)]
        public virtual string LinkUrl
        {
            get { return (string) (GetDetail("LinkUrl") ?? string.Empty); }
            set { SetDetail("LinkUrl", value, string.Empty); }
        }

        public virtual string PreviewUrl
        {
            get { return Url; }
        }

        //    return this.Parent.Url;
        //  {
        //  get
        //{

        //public override string PreviewUrl
        //  }
        //}
    }
}