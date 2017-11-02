using System.Text;

namespace EPWI.Components.Models
{
  public class KitPart
  {
    //private int startYear;
    //private int endYear;

    public Kit Kit { get; set; }
    public int NIPCCode { get; set; }
    public string ItemNumber { get; set; }
    public string ItemDescription { get; set; }
    public string LineCode { get; set; }
    public string LineDescription { get; set; }
    public string SizeCode { get; set; }
    public decimal PricingPercentage { get; set; }
    public string JoinQualifier { get; set; }
    public string PreJoinQualifier { get; set; }
    public int GroupingMain { get; set; }
    public int GroupingOr { get; set; }
    public int GroupingAnd { get; set; }
    public string PartsToGroup { get; set; }
    public string PartsToDeselect { get; set; }
    public string PartsToSelect { get; set; }
    public int CategoryID { get; set; }
    public int LineType { get; set; } // this is primarily used to differentiate related kit parts
    public int SequenceNumber { get; set; }
    public int QuantityRequired { get; set; }
    public int QuantitySelected { get; set; }
    public int StartYear { get; set; }
    //{ 
    //  get
    //  {
    //    return startYear;
    //  }
    //  set
    //  {
    //    startYear = (value == 0 ? Kit.StartYear : value);
    //  }
    //}
    public int EndYear { get; set; } 
    //{
    //  get
    //  {
    //    return endYear;
    //  }
    //  set
    //  {
    //    endYear = (value == 0 ? Kit.EndYear : value);
    //  }
    //}
    public string Note { get; set; }
    public bool Selected { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public bool HasInterchange { get; set; }
    public string OriginalPartUniqueID { get; set; }
    public string InterchangeMethod { get; set; }
    public string OrderMethod { get; set; }
    private string shipWarehouse;
    public string ShipWarehouse 
    {
      set
      {
        shipWarehouse = value == null ? null : value.Trim();
      }
      get
      {
        return shipWarehouse;
      }
    }
    
    public bool IsMasterKitPart { get; set; }

    public string GroupName
    {
      get
      {
        return $"group_{this.GroupingMain}";
      }
    }

    public string UniqueKitPartIdentifier
    {
      get
      {
        return $"{this.NIPCCode}-{this.SequenceNumber}";
      }
    }

    public bool DisplayYears
    {
      get
      {
        // display the part years if different than the kit years 
        return ((this.StartYear > this.Kit.StartYear || this.EndYear < this.Kit.EndYear)  && this.Years != this.Note && (this.StartYear != 0 && this.EndYear != 0));
      }
    }

    public string Years
    {
      get
      {
        if (this.StartYear == this.EndYear)
        {
          return $"{this.StartYear.ToString("0000").Substring(2, 2)}";
        }
        else
        {
          return $"{this.StartYear.ToString("0000").Substring(2, 2)}/{this.EndYear.ToString("0000").Substring(2, 2)}";
        }
      }
    }

    public string PartString
    {
      get
      {
        StringBuilder sb = new StringBuilder();

        sb.Append(this.ItemNumber);

        sb.Append(" ");
        sb.Append(this.LineDescription);
        sb.Append(" ");

        // show quantity required if it's more than one
        if (this.QuantityRequired > 1)
        {
          sb.Append($" ({this.QuantityRequired}) ");
        }

        // if we have a note or need to display years, render parenthesis and appropriate content
        if (!string.IsNullOrEmpty(this.Note) || this.DisplayYears)
        {
          sb.Append("(");
          if (this.DisplayYears)
          {
            sb.Append(this.Years);
            if (!string.IsNullOrEmpty(this.Note))
            {
              sb.Append(" ");
            }
          }

          if (!string.IsNullOrEmpty(this.Note))
          {
            sb.Append(this.Note);
          }
          sb.Append(")");
        }

        return sb.ToString();

      }
    }

    public bool IsStartOfGroup
    {
      get
      {
        return string.IsNullOrEmpty(this.JoinQualifier.Trim()) && !string.IsNullOrEmpty(this.PreJoinQualifier.Trim());
      }
    }

    public bool IsEndOfGroup
    {
      get
      {
        return string.IsNullOrEmpty(this.PreJoinQualifier.Trim()) && !string.IsNullOrEmpty(this.JoinQualifier.Trim());
      }
    }

    public bool IsNotPartOfGroup
    {
      get
      {
        return string.IsNullOrEmpty(this.PreJoinQualifier.Trim()) && string.IsNullOrEmpty(this.JoinQualifier.Trim());
      }
    }

    public bool IsPartOfAndGroup
    {
      get
      {
        return this.PreJoinQualifier.Trim() == "A" || this.JoinQualifier.Trim() == "A";
      }
    }
    

  }
}
