using System.ComponentModel.DataAnnotations;

namespace EPWI.Components.Models
{
  public class UserProfile
  {
    public static string PASSWORD_TEXT = "********";

    public UserProfile() 
    {
      this.UserName = string.Empty;
      this.FirstName = string.Empty;
      this.LastName = string.Empty;
      this.Password = string.Empty;
      this.EmailAddress = string.Empty;
      this.Title = string.Empty;
      this.Address = string.Empty;
      this.City = string.Empty;
      this.StateProvince = string.Empty;
      this.ZipPostal = string.Empty;
      this.Phone = string.Empty;
      this.Fax = string.Empty;
      this.Notes = string.Empty;
      this.Company = string.Empty;
    }

    public UserProfile(User user)
    {
      this.UserName = user.UserName;

      if (!string.IsNullOrEmpty(user.Password))
      {
        this.Password = PASSWORD_TEXT;
        this.ConfirmPassword = PASSWORD_TEXT;
      }

      this.FirstName = user.FirstName;
      this.LastName = user.LastName;
      this.EmailAddress = user.EmailAddress;
      this.Title = user.Title;
      this.Address = user.Address;
      this.City = user.City;
      this.StateProvince = user.StateProvince;
      this.ZipPostal = user.ZipPostal;
      this.Phone = user.Phone;
      this.Fax = user.Fax;
      this.Notes = user.Notes;
      this.Company = user.CompanyName;
    }

    [Required, RegularExpression(@"^[\w\.@]{4,50}$", ErrorMessage = "User Name must be alphanumeric and at least 4 characters long.")]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string ConfirmPassword { get; set; }
    [Required, StringLength(30)]
    public string FirstName { get; set; }
    [Required, StringLength(30)]
    public string LastName { get; set; }
    [Required, StringLength(50), DataType(DataType.EmailAddress)]
    public string EmailAddress { get; set; }
    [StringLength(50)]
    public string Title { get; set; }
    [Required, StringLength(50)]
    public string Company { get; set; }
    [StringLength(50)]
    public string Address { get; set; }
    [StringLength(50)]
    public string City { get; set; }
    [StringLength(50)]
    public string StateProvince { get; set; }
    [StringLength(50)]
    public string ZipPostal { get; set; }
    [Required, StringLength(50)]
    public string Phone { get; set; }
    [StringLength(50)]
    public string Fax { get; set; }
    public string Notes { get; set; }
    public bool EditingOwnProfile { get; set; }
  }
}
