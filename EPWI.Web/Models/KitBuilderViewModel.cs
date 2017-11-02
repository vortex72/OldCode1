namespace EPWI.Web.Models
{
	public class KitBuilderViewModel : KitModelBase
	{
    public bool NewConfiguration { get; set; }

    public bool YearSelected
    {
      get
      {
        return Kit.SelectedYear > 0;
      }
    }

    public int DeselectionLimit { get; set; }

    public bool SizesSelected { get; set; }
    
    public bool CanSelectSize
    {
      get
      {
        return (!Editing || (Editing && !CrankKitSelected));
      }
    }

    public bool ShowSizeDialog
    {
      get
      {
        return this.Kit.KitIdentifier.KitType != "CK" || this.Kit.HasCrankKit;
      }
    }
	}
}
