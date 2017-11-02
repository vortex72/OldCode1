using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using xVal.ServerSide;
using System.Data.SqlClient;
using EPWI.Web.Models;
using DomainModel.Helpers;
using EPWI.Components.Utility;

namespace EPWI.Components.Models
{
  public class UserRepository : Repository
  {
    private const int DEFAULT_PASSWORD_LENGTH = 8;
    private const string PASSWORD_SALT = "lak$@!f32091";

    public void Add(User user)
    {
      Db.Users.InsertOnSubmit(user);
    }

    public void Delete(User user)
    {
      Db.DeleteUser(user.UserID);
    }

    public User GetByUserName(string username)
    {
        var foo = from u in Db.Users where u.UserName == username select u;

      return foo.SingleOrDefault();
     
    }

    public static User GetUserStatic(string username)
    {
      var db = EPWIDataContext.GetInstance();
      return (from u in db.Users where u.UserName == username select u).SingleOrDefault();
    }

    public User GetByUserID(int id)
    {
      return (from u in Db.Users where u.UserID == id select u).SingleOrDefault();
    }

    public IQueryable<User> GetAll()
    {
      return Db.Users;
    }

    public IList<RoleMembershipItem> GetUserRoleMembership(User user)
    {
      var roleMembershipList = from r in Db.Roles
                                select
                                  new RoleMembershipItem
                                  {
                                    RoleID = r.RoleID,
                                    RoleDescription = r.RoleName,
                                    RoleKey = r.RoleKey,
                                    IsInRole = r.RoleMemberships.Where(rm => rm.UserID == user.UserID).Count() > 0
                                  };
      return roleMembershipList.ToList();
    }

    public void UpdateUserProfile(User user, UserProfile profile)
    {
      if (profile.Password != profile.ConfirmPassword)
      {
        throw new RulesException("password", "Passwords must match.");
      }
      user.UserName = profile.UserName;
      user.FirstName = profile.FirstName;
      user.LastName = profile.LastName;
      if (profile.Password != UserProfile.PASSWORD_TEXT)
      {
        user.Password = hashPassword(profile.Password);
      }
      user.EmailAddress = profile.EmailAddress;
      user.Title = profile.Title;
      user.CompanyName = profile.Company;
      user.Address = profile.Address;
      user.City = profile.City;
      user.StateProvince = profile.StateProvince;
      user.ZipPostal = profile.ZipPostal;
      user.Phone = profile.Phone;
      user.Fax = profile.Fax;
      user.Notes = profile.Notes;
    }

    public void UpdateUserAdminSettings(User user, UserAdminSettings admin)
    {
      user.CommentsForUser = admin.CommentsForUser;
      user.CompanyCode = string.IsNullOrEmpty(admin.CompanyCode) ? (char?)null : admin.CompanyCode.ToCharArray()[0];
      user.CustomerID = admin.CustomerID;
      user.ActiveAccount = admin.IsActive;
      user.PricingFactor = admin.PricingFactor / 100M;

      foreach (var userRole in admin.RoleMembershipList)
      {
        var roleMembership = (from r in user.RoleMemberships where r.RoleID == userRole.RoleID select r).SingleOrDefault();

        // if the user is not in role but the role membership exists in db, remove it
        if (!userRole.IsInRole && roleMembership != null)
        {
          user.RoleMemberships.Remove(roleMembership);
        }
        else if (userRole.IsInRole && roleMembership == null)
        { // if the user is in role but role membership isn't in db, add it
          roleMembership = new RoleMembership() { UserID = user.UserID, RoleID = userRole.RoleID };
          Db.RoleMemberships.InsertOnSubmit(roleMembership);
        }
      }
    }

    public void UpdateAccountSettings(User user, AccountSettings accountSettings)
    {
      user.PricingFactor = accountSettings.PricingFactor / 100M;
      user.CustomerPricingBasis = accountSettings.CustomerPricingBasis;
      user.MarginPricingBasis = accountSettings.MarginPricingBasis;
      user.PricingDisplayBitmask = 0;
      user.PricingDisplayBitmask2 = 0;

      var view1Selections = accountSettings.PriceTypeSelectionsView1.Where(ps => ps.Value == true).Select(ps => ps.Key);

      foreach (var selection in view1Selections)
      {
        user.PricingDisplayBitmask |= (int)selection;
      }

      var view2Selections = accountSettings.PriceTypeSelectionsView2.Where(ps => ps.Value == true).Select(ps => ps.Key);

      foreach (var selection in view2Selections)
      {
        user.PricingDisplayBitmask2 |= (int)selection;
      }
    }


    public override void Save()
    {
      try
      {
        base.Save();
      }
      catch (SqlException ex)
      {
        if (ex.Message.Contains("UX_Users_UserName"))
        {
          throw new RulesException("UserName", "There is already an account with that User Name");
        }
        else
        {
          throw;
        }
      }
    }

    public bool ValidateAccount(string username, string password)
    {
      return (from u in Db.Users
              where u.UserName == username && u.Password == hashPassword(password) && u.ActiveAccount.GetValueOrDefault(false)
              select u).Count() == 1;
    }

    private string hashPassword(string password)
    {
      var bytes = SHA1Managed.Create().ComputeHash(Encoding.Default.GetBytes(password + PASSWORD_SALT));
      return Convert.ToBase64String(bytes);
    }


    public string ResetPassword(ResetPasswordModel model)
    {
      var errors = DataAnnotationsValidationRunner.GetErrors(model);
			
      if (errors.Any())
      {
			  throw new RulesException(errors);
			}

      var user = (from u in Db.Users
                  where u.UserName == model.Username
                  select u).SingleOrDefault();

      if (user == null)
      {
        throw new RulesException("Username", "The specified username does not exist.");
      }

      string newPassword = StringUtility.CreatePassword(DEFAULT_PASSWORD_LENGTH);
      user.Password = hashPassword(newPassword);
      this.Save();

      return newPassword;
    }
  }
}
