using System.Web.Mvc;
using System.Web.Mvc.Html;
using EPWI.Components.Models;

namespace EPWI.Web.HtmlHelpers
{
	public static class WarehouseHelpers
	{
		public static string WarehouseRadioButton(this HtmlHelper html, string name, StockStatus stockStatus, string warehouse, bool withDefaultWarehouse)
		{
			string htmlOutput = string.Empty;

			if (warehouse != stockStatus.CustomerDefaultWarehouse && warehouse != stockStatus.CustomerSecondaryWarehouse && stockStatus.WarehouseAvailability(warehouse) + (withDefaultWarehouse ? stockStatus.WarehouseAvailability(stockStatus.CustomerDefaultWarehouse) : 0) >= stockStatus.QuantityRequested)
			{
				htmlOutput = html.RadioButton(name, warehouse) + warehouse;
			}

			return htmlOutput;
		}
	}
}
