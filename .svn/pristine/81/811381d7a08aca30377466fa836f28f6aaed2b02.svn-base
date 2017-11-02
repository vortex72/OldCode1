using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPWI.Components.Models
{
  public class UserAdminSettings
  {
    public UserAdminSettings() 
    {
    }

    public UserAdminSettings(User user, IList<RoleMembershipItem> roleMembershipList)
    {
      this.UserID = user.UserID;
      this.IsActive = user.ActiveAccount.GetValueOrDefault(false);
      this.CustomerID = user.CustomerID;
      this.CompanyCode = user.CompanyCode.HasValue ? user.CompanyCode.Value.ToString() : null;
      this.PricingFactor = user.UserID == 0 ? 135 : Convert.ToInt32(user.PricingFactor * 100);  // for new users, default to 135
      this.CommentsForUser = user.CommentsForUser;
      this.RoleMembershipList = roleMembershipList;
    }

    public int UserID { get; set; }
    public bool IsActive { get; set; }
    public int? CustomerID { get; set; }
    public string CompanyCode { get; set; }
    [Required]
    public int PricingFactor { get; set; }
    public string CommentsForUser { get; set; }
    public IList<RoleMembershipItem> RoleMembershipList { get; set; }
  }
}
