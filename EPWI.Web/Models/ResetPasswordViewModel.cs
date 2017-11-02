using System.ComponentModel.DataAnnotations;

namespace EPWI.Web.Models
{
  public class ResetPasswordViewModel
  {
    [Required(ErrorMessage="Username is required."), DataType(DataType.EmailAddress)]
    public string Username { get; set; }
  }
}
