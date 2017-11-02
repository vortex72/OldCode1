using EPWI.Web.Models.N2CMS.EPWI.Web.Models.N2CMS;
using N2;
using N2.Details;
using N2.Integrity;

namespace EPWI.Web.Models.N2CMS
{

  [PageDefinition("Link Item Collection")]
  [AvailableZone("Links","Links")]
    public class LinkItemContainer : ItemContainer<LinkItem>
  {
    [EditableCheckBox("Show Alphabetically", 50)]
    public virtual bool ShowAlphabetically
    {
      get { return (bool)(GetDetail("ShowAlphabetically") ?? false); }
      set { SetDetail("ShowAlphabetically", value, false); }
    }

    [EditableCheckBox("Enable Paging", 75)]
    public virtual bool EnablePaging
    {
      get { return (bool)(GetDetail("EnablePaging") ?? false); }
      set { SetDetail("EnablePaging", value, false); }
    }
  }
}
