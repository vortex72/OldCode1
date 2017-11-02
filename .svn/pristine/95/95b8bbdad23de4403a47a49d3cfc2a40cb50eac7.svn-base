using System.Collections.Generic;
using N2;
using N2.Collections;
using N2.Details;

namespace EPWI.Web.Models.N2CMS
{
    namespace EPWI.Web.Models.N2CMS
    {
        public abstract class ItemContainer<T> : AbstractPage where T : N2.ContentItem
        {
            //[EditableFreeTextArea("Text", 100, EditorMode = EditorModeSetting.Full)]
            [EditableFreeTextArea("Header", 100)]
            public virtual string Header
            {
                get { return (string)(GetDetail("Header") ?? string.Empty); }
                set { SetDetail("Header", value, string.Empty); }
            }

            //[EditableFreeTextArea("Text", 100, EditorMode = EditorModeSetting.Full)]
            [EditableFreeTextArea("Footer", 900)]
            public virtual string Footer
            {
                get { return (string)(GetDetail("Footer") ?? string.Empty); }
                set { SetDetail("Footer", value, string.Empty); }
            }

            public virtual IEnumerable<T> GetItems()
            {
                return GetChildren(new AccessFilter(), new TypeFilter(typeof(T))).Cast<T>();
            }

        }
    }
}