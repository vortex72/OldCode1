using System;
using System.Collections.Generic;
using System.Linq;

namespace EPWI.Components.Models
{
  public class Kit
  {
    public KitIdentifier KitIdentifier { get; set; }
    public int NIPCCode { get; set; }
    public bool CustomConfiguration { get; set; }
    public decimal StandardPrice { get; set; }
    public decimal ConfiguredPrice { get; set; }
    public decimal TypicallyConfiguredPrice { get; set; }
    public string OrderMethod { get; set; }
    public string GroupingList { get; set; }

    // kit header data
    public int Cylinders { get; set; }
    public string CylinderString { get; set; }
    public int StartYear { get; set; }
    public int EndYear { get; set; }
    public string Bore { get; set; }
    public string Stroke { get; set; }
    public string RodDiameter { get; set; }
    public string MainDiameter { get; set; }
    private List<string> applications = new List<String>();

    // aces kit identifier
    public string AcesID { get; set; }

    // Selection Filter by Year and Sizes
    public int SelectedYear { get; set; }
    private string selectedBoreSize;
    private string selectedRodBearingSize;
    private string selectedMainBearingSize;
    private string selectedThrustWasherSize;

    public int DeselectionLimit
    {
      get
      {
        switch (this.KitIdentifier.KitType)
        {
          case "MK":
          case "EK":
            return 3;
          default:
            return 0;
        }
      }
    }

    public decimal GetConfiguredPrice(bool asTypicallyConfigured)
    {
      decimal configuredPrice;

      if (asTypicallyConfigured)
      {
        configuredPrice = this.TypicallyConfiguredPrice;
      }
      else if (this.MasterKitParts == null)
      {
        configuredPrice = 0;
      }
      else
      { // changed to use linq instead of calculated column in DB because results were inconsistent
        configuredPrice = this.MasterKitParts.Where(kp => kp.Selected).Sum(kp => kp.Price * kp.QuantitySelected);
      }

      // per DVW, if the configured price is with 0.25% of
      // the standard kit price, then only return the standard price
      if (Math.Abs(configuredPrice - this.StandardPrice) < (this.StandardPrice * 0.0025M))
      {
        configuredPrice = StandardPrice;
      }

      return configuredPrice;
    }

    public string SelectedBoreSize 
    {
      get
      {
        return selectedBoreSize;
      }
      set
      {
        selectedBoreSize = value;
        updatePartSizes(new int[] { 10, 11, 15 }, selectedBoreSize);
      }
    }
    public string SelectedRodBearingSize 
    {
      get
      {
        return selectedRodBearingSize;
      }
      set
      {
        selectedRodBearingSize = value;
        updatePartSizes(new int[] { 20 }, selectedRodBearingSize);
      }
    }

    public string SelectedMainBearingSize
    {
      get
      {
        return selectedMainBearingSize;
      }
      set
      {
        selectedMainBearingSize = value;
        updatePartSizes(new int[] { 21 }, selectedMainBearingSize);
      }
    }
    
    public string SelectedThrustWasherSize 
    { 
      get
      {
        return selectedThrustWasherSize; 
      }
      set
      {
        selectedThrustWasherSize = value;
        updatePartSizes(new int[] { 23 }, selectedThrustWasherSize);
      }    
    }
   
    // display variables required for KTRACKs
    public bool IsYearRequired { get; set; }
    public bool RequiresSpecialDisplay { get; set; }
    public bool IsKTRACK { get; set; }

    // kit parts
    public IEnumerable<KitPart> MasterKitParts { get; set; }
    public IEnumerable<KitPart> RelatedKitParts { get; set; }

    // category notes
    public IEnumerable<KitCategoryNote> CategoryNotes { get; set; }
    
    // supporting data
    public IEnumerable<string> BoreSizes { get; set; }
    public IEnumerable<string> RodBearingSizes { get; set; }
    public IEnumerable<string> MainBearingSizes { get; set; }
    public IEnumerable<string> ThrustWasherSizes { get; set; }

    // crank kit information
    public int SelectedCrankKitNIPC { get; set; }
    public decimal SelectedCrankKitPrice { get; set; }
    public int SelectedCrankKitCoreNIPC { get; set; }
    public decimal SelectedCrankKitCorePrice { get; set; }

    public List<string> Applications
    {
      get
      {
        return applications;
      }
    }

    public bool HasCrankKit
    {
      get
      {
        // kit has a crank kit if any of the related parts have a category of 5 or 85
        return RelatedKitParts.Any(kp => kp.CategoryID == 5 || kp.CategoryID == 85);
      }
    }

    public IEnumerable<KitPart> GetCategoryParts(KitCategory category)
    {
      var categoryParts = from kp in MasterKitParts
                          where (category == KitCategory.Ktrack || (KitCategoryMapping.Mappings[category].CategoryID.Contains(kp.CategoryID)
                          && KitCategoryMapping.Mappings[category].KitType.Contains(this.KitIdentifier.KitType))) &&
                          kp.InterchangeMethod != "X" && kp.QuantityRequired > 0
                          select kp;

      return categoryParts;
    }

    public IEnumerable<int> GetKitYears()
    {
      var years = new List<int>();

      for (int year = this.StartYear; year <= this.EndYear; year++)
      {
        years.Add(year);
      }

      return years;
    }

    public IEnumerable<KitPart> GetRelatedCategoryParts(RelatedCategory category)
    {
      var categoryParts = from kp in RelatedKitParts
                          where RelatedCategoryMapping.Mappings[category].CategoryID.Contains(kp.CategoryID)
                          && (!RelatedCategoryMapping.Mappings[category].LineType.HasValue || RelatedCategoryMapping.Mappings[category].LineType.Value == kp.LineType)
                          select kp;

      return categoryParts;
    }

    public IEnumerable<KitCategoryNote> GetCategoryNotes(KitCategory category)
    {
      var categoryNotes = from n in CategoryNotes
                          where KitCategoryMapping.Mappings[category].CategoryID.Contains(n.CategoryCode)
                          select n;
      // if this category has notes, update the note with a counter so that we can track how many categories with notes we have displayed
      if (categoryNotes.Count() > 0)
      {
        int ordinal = (from n in CategoryNotes
                       select n.NoteOrdinal).Max() + 1;

        // update each note with the counter value so we can display the notes with correct counter
        foreach (var note in categoryNotes)
        {
          note.NoteOrdinal = ordinal;
        }
      }

      return categoryNotes;
    }

    public void PopulateHeaderData(usp_GetKitCatalogHeaderDataResult headerData)
    {
      this.CylinderString = headerData.OCYL;
      
      this.StartYear = headerData.KSYY.GetValueOrDefault(0);
      this.EndYear = headerData.KEYY.GetValueOrDefault(0);
      this.Bore = headerData.OBORE;
      this.Stroke = headerData.OSTRK;
      this.RodDiameter = headerData.KZRD;
      this.MainDiameter = headerData.KZMD;

      var type = headerData.GetType();

      for (int i = 1; i <= 6; i++)
      {
        var application = (string)type.GetProperty("KZAP" + i).GetValue(headerData, null);
        if (!string.IsNullOrEmpty(application))
        {
          Applications.Add(application);
        }
      }
    }

    public void PopulateInventoryItemData(InventoryItem inventoryItem)
    {
      this.NIPCCode = inventoryItem.NIPCCode;
      this.RequiresSpecialDisplay = inventoryItem.RequiresSpecialDisplay;
      this.IsYearRequired = inventoryItem.IsYearRequired;
      this.IsKTRACK = inventoryItem.IsKTRACK;
    }

    public KitPart GetKitPartByUniqueIdentifier(string uniqueIdentifier)
    {
      var part = this.MasterKitParts.Where(kp => kp.UniqueKitPartIdentifier == uniqueIdentifier).SingleOrDefault();

      return part;
    }

    private void updatePartSizes(int[] categoryIDs, string size)
    {
      if (this.MasterKitParts != null)
      {
        var partsToUpdate = from kp in this.MasterKitParts
                            where categoryIDs.Contains(kp.CategoryID)
                            select kp;
        foreach (KitPart part in partsToUpdate)
        {
          part.SizeCode = size;
          //FB597: When changing sizes, change the ship method to main
          part.OrderMethod = "M";
          part.ShipWarehouse = null;
        }
      }
    }
  }
}
