using System;
using System.Collections.Generic;
using System.Data;

namespace EPWI.Components.Models
{
	public class Interchange
	{
		private Dictionary<PriceType, decimal> price = new Dictionary<PriceType, decimal>();
		private List<string> notes = new List<string>();

		public int FulfillmentQuantity { get; private set; }  // lesser of quantity requested or quantity on hand
    public int InterchangeQuantity { get; private set; }
    public int OnHandQuantity { get; private set; }
		public string LineCode { get; private set; }
		public string ItemNumber { get; private set; }
    public string InterchangeCode { get; private set; }
		public string InterchangeType { get; private set; }
    public int NIPCCode { get; private set; }

		public Dictionary<PriceType, decimal> Price
		{
			get
			{
				return price;
			}
		}

		public IEnumerable<string> Notes
		{
			get
			{
				return notes;
			}
		}

    public string LastNote
    {
      get
      {
        string lastNote = string.Empty;
        foreach (var note in Notes)
        {
          if (!string.IsNullOrEmpty(note))
          {
            lastNote = note;
          }
        }
        return lastNote;
      }
    }

		/// <summary>
		/// Returns a hydrated Interchange object based on a dataset from the AS/400
		/// </summary>
		/// <param name="dsStockStatus">The DataSet returned by the AS/400</param>
		/// <returns>A hydrated Interchange object</returns>
		public void PopulateFromHost(DataRow interchangeRow, ICustomerData customerData, int quantityRequested)
		{
			//TODO: Handle empty table
			this.LineCode = interchangeRow["SSSLI"].ToString().Trim();
			this.FulfillmentQuantity = Math.Min(int.Parse(interchangeRow["ICOH"].ToString()), quantityRequested);
      this.OnHandQuantity = int.Parse(interchangeRow["ICOH"].ToString());
      this.InterchangeQuantity = int.Parse(interchangeRow["ICQTY"].ToString());
			this.ItemNumber = interchangeRow["SSSIT"].ToString().Trim();
			this.InterchangeType = interchangeRow["ICDESC"].ToString().Trim();
      this.InterchangeCode = interchangeRow["ICTYPE"].ToString().Trim();
      this.NIPCCode = int.Parse(interchangeRow["SSSNI"].ToString());
			for (int i = 1; i <= 9; i++)
			{
				string note = interchangeRow[$"ICNTE{i}"].ToString().Trim();

				if (note != string.Empty)
				{
					notes.Add(note);
				}
			}
						
			this.Price[PriceType.Jobber] = decimal.Parse(interchangeRow["SSP1"].ToString());
			this.Price[PriceType.Invoice] = decimal.Parse(interchangeRow["SSP3N"].ToString());
			this.Price[PriceType.Elite] = this.Price[PriceType.Invoice] * 0.9M;	//TODO: Should this be configurable?
			
			//Adjust prices based on customer pricing basis
			this.Price[PriceType.Customer] = this.Price[customerData.CustomerPricingBasis] * customerData.PricingFactor;

		}
	}
}
