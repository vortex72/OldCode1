using System.ComponentModel.DataAnnotations;

namespace EPWI.Components.Models
{
	public class Address
	{
    private string zip;
    private string zip4;
    private string phone;

		[Required(ErrorMessage = "Name is required.")] [StringLength(30)]
		public string Name { get; set; }
		
		[Required(ErrorMessage = "Address is required.")] [StringLength(30)]
		public string StreetAddress1 { get; set; }
		
		[StringLength(30)]
		public string StreetAddress2 { get; set; }

		[Required(ErrorMessage="City is required.")] [StringLength(20)] 
		public string City { get; set; }

		[Required(ErrorMessage="State is required.")] 
		[RegularExpression(@"^[\w\s]{2,2}$", ErrorMessage = "State is invalid.")]
		public string State { get; set; }

    [Required(ErrorMessage = "ZIP is required.")]
    [RegularExpression(@"^[\w\s]{5,5}$", ErrorMessage = "ZIP must be 5 characters")]
    [Range(0, 99999, ErrorMessage = "ZIP is invalid.")]
    public string Zip
    {
      get
      {
        return zip == "0" ? string.Empty : zip;
      }
      set
      {
        zip = value;
      }
    }

		[RegularExpression(@"^[\w\s]{4,4}$", ErrorMessage = "ZIP-4 must be 4 characters")]
		[Range(0,9999, ErrorMessage = "ZIP-4 is invalid.")]
    public string Zip4
    {
      get
      {
        return zip4 == "0" ? string.Empty : zip4;
      }
      set
      {
        zip4 = value;
      }
    }

    public string ZipRaw
    {
      get
      {
        return zip;
      }
    }

    public string Zip4Raw
    {
      get
      {
        return zip4;
      }
    }

    public string PhoneRaw
    {
      get
      {
        return phone;
      }
    }

		[Required(ErrorMessage= "Phone is required.")] [StringLength(12)]
		[RegularExpression(@"^(\d{3})[ -.]?(\d{3})[ -.]?(\d{4})$", ErrorMessage = "Phone number is invalid. Use format xxx-xxx-xxxx.")]
    public string Phone
    {
      get
      {
        return phone == "0" ? string.Empty : phone;
      }
      set
      {
        phone = value;
      }
    }

    // these are currently only used for company address on Account Statements / Invoices
    public string AlternatePhone { get; set; }
    public string Fax { get; set; }

	}
}
