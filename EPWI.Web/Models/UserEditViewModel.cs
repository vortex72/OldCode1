using EPWI.Components.Models;

namespace EPWI.Web.Models
{
  public class UserEditViewModel
  {
    public UserProfile UserProfile { get; set; }
    public UserAdminSettings UserAdminSettings { get; set; }
    public bool ShowReturnToUserList { get; set; }
  }
}
