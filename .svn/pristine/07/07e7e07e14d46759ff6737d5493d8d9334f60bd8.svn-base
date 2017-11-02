using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPWI.Components.Models
{
  public class MillionthPartViewModel
  {
    public int CustomerID { get; set; }
    public char CompanyCode { get; set; }
    public int ValidationCode { get; set; }
    public string UserID { get; set; }
    public int OrderNumber { get; set; }
    [Required(ErrorMessage = "Guess Date is required."), DataType(DataType.Date)]
    public DateTime? GuessDate { get; set; }
    public int GuessHour { get; set; }
    public int GuessMinute { get; set; }
    public string GuessAmPm { get; set; }

    public IEnumerable<int> Hours
    {
      get
      {
        for (int i = 1; i <= 12; i++)
        {
          yield return i;
        }
      }
    }

    public IEnumerable<string>  Minutes
    {
      get
      {
        for (int i = 0; i <= 59; i++)
        {
          yield return i.ToString().PadLeft(2, '0');
        }
      }
    }


  }
}
