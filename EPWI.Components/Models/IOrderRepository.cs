namespace EPWI.Components.Models
{
  public interface IOrderRepository
  {
    Order OpenOrder(ICustomerData customerData, bool createIfNeeded);
    Order OpenOrder(ICustomerData customerData, bool createIfNeeded, bool retryOnDuplicateKey);
    int AddKitToOrder(Kit kit, ICustomerData customerData, string kitXml, string customerReference, string powerUserPrimaryWarehouse);
    int AddWarrantyToKitOrderItem(int kitOrderItemID, int warrantyNipc, decimal warrantyPrice);
    Order GetOrderFromHost(ICustomerData customerData);    
    Order UpdateOrderFromHost(Order order, ICustomerData customerData, bool flushData);
    Order GetOrderFromDatabase(int orderId);
    void DeleteOrder(Order order);
    void DeleteOrderItem(OrderItem orderItem);
    bool ProcessOrder(Order order, bool saveAsQuote, bool perpetualQuote, string quoteNumber, ICustomerData customerData);
    int SaveOrderAsQuote(ICustomerData customerData, Order order, string quoteDescription, bool shareQuote, bool readOnly);
    void Save();
  }
}