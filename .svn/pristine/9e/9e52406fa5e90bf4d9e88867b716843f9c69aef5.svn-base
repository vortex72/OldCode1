using System.Collections.Generic;
using System.Linq;
using EPWI.Components.Proxies;
using System.Data;

namespace EPWI.Components.Models
{
	public class InterchangeRepository 
	{
    public static IEnumerable<Interchange> GetInterchangeData(ICustomerData customerData, int nipcCode, string size, int quantityRequested, int cylinders, string customerDefaultWarehouse)
    {
      return GetInterchangeData(customerData, nipcCode, size, quantityRequested, cylinders, customerDefaultWarehouse, false);
    }

		public static IEnumerable<Interchange> GetInterchangeData(ICustomerData customerData, int nipcCode, string size, int quantityRequested, int cylinders, string customerDefaultWarehouse, bool includeOriginalPart)
		{
			EPWIDataContext db = EPWIDataContext.GetInstance();
			var interchanges = new List<Interchange>();

			var interchangeProxy = InterchangeProxy.Instance;
			var dataTable = interchangeProxy.SubmitRequest(customerData.CompanyCode, customerData.CustomerID.GetValueOrDefault(0), nipcCode, size, quantityRequested, cylinders);

			foreach (DataRow row in dataTable.Rows)
			{
				// exclude first row because it is the item being inquired on, unless specified to be included
				if (int.Parse(row["ZICSEQ"].ToString()) > 1 || includeOriginalPart)
				{
					var interchange = new Interchange();
					interchange.PopulateFromHost(row, customerData, quantityRequested);
					// If a line and warehouse combination exist in the interchange exception table,
					// exclude that item from the list of interchanges
					var isException = (from ie in db.InterchangeExceptions
															where ie.Warehouse == customerDefaultWarehouse && ie.LineCode == interchange.LineCode
															select ie).Count() > 0;
					if (!isException)
					{
						interchanges.Add(interchange);
					}
				}
			}

			return interchanges;
		}
	}
}
