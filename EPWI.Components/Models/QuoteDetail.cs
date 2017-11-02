namespace EPWI.Components.Models
{
  public partial class QuoteDetail
  {
    public bool OwnedByUser(ICustomerData customerData)
    {
      return this.UserID == customerData.UserID;
    }

    public bool CanDelete(ICustomerData customerData)
    {
      return OwnedByUser(customerData) || (QuoteOwnerDeleted && customerData.CustomerID == this.EPWCustID && customerData.CompanyCode == this.EPWCompCode);
    }

    public bool CanEdit(ICustomerData customerData)
    {
      return (this.OwnedByUser(customerData) || (this.Shared && !this.ReadOnly));
    }

    public string CreatedBy
    {
      get
      {
        // if the quote owner is deleted, show the username from the quote instead
        return QuoteOwnerDeleted ? $"{this.UserName} - DELETED"
            : $"{this.CreatedByFirstName} {this.CreatedByLastName}";
      }
    }

    public bool QuoteOwnerDeleted
    {
      get
      {
        return this.CreatedByFirstName == null && this.CreatedByLastName == null;
      }
    }


  }
}
