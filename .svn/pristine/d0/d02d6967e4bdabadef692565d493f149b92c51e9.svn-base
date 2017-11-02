using EPWI.Components.Proxies;

namespace EPWI.Components.Models
{
    public class StockStatusRepository
    {
        public static StockStatus GetStockStatusByNipc(int nipcCode, string size, int quantityRequested,
            ICustomerData customerData, PowerUserWarehouseResult powerUserWarehouses = null)
        {
            return GetStockStatusByNipc(nipcCode, 0, size, quantityRequested, customerData, powerUserWarehouses);
        }

        public static StockStatus GetStockStatusByNipc(int nipcCode, int applicableKitNipc, string size,
            int quantityRequested, ICustomerData customerData, PowerUserWarehouseResult powerUserWarehouses = null)
        {
            // standard size is empty string
            if (size == "STD")
                size = string.Empty;
            var stockStatusProxy = StockStatusProxy.Instance;
            var dataTable = stockStatusProxy.SubmitRequest(customerData.CompanyCode, customerData.CustomerID, nipcCode,
                size, quantityRequested, customerData.UserID);

            var stockStatus = new StockStatus();

            stockStatus.PopulateFromHost(dataTable, nipcCode, applicableKitNipc, quantityRequested, customerData,
                powerUserWarehouses);

            return stockStatus;
        }
    }
}