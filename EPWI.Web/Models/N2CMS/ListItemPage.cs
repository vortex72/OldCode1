using N2;
using N2.Details;
using N2.Integrity;

namespace EPWI.Web.Models.N2CMS
{
    [PageDefinition("List Item")]
    [RestrictParents(typeof (ListItemContainer))]
    public class ListItemPage : AbstractPage
    {
        public ListItemPage()
        {
            Visible = false;
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


        [EditableFreeTextArea("Text", 100)]
        public virtual string Text
        {
            get { return (string) (GetDetail("Text") ?? string.Empty); }
            set { SetDetail("Text", value, string.Empty); }
        }
    }
}