using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using EPWI.Components.Models;
using EPWI.Components.Proxies;

namespace EPWI.Components.Services
{
    public class PowerUserService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly PowerUserWarehouseProxy _powerUserWarehouseProxy;

        public PowerUserService() : this(new OrderRepository(), PowerUserWarehouseProxy.Instance)
        {
        }

        public PowerUserService(IOrderRepository orderRepository, PowerUserWarehouseProxy powerUserWarehouseProxy)
        {
            _orderRepository = orderRepository;
            _powerUserWarehouseProxy = powerUserWarehouseProxy;
        }

        public Order OpenPowerUserOrder(Address address, ICustomerData customerData)
        {
            var order = _orderRepository.OpenOrder(customerData, false);

            // delete the order if one is already open
            if (order != null)
            {
                _orderRepository.DeleteOrder(order);
                _orderRepository.Save();
            }

            order = _orderRepository.OpenOrder(customerData, true);

            var powerUserWarehouseProxyResult = _powerUserWarehouseProxy.SubmitRequest(customerData, address);

            if (powerUserWarehouseProxyResult != null)
            {
                order.AssignedWhse = powerUserWarehouseProxyResult.AssignedWarehouse;
                order.PrimaryWarehouse = powerUserWarehouseProxyResult.PrimaryWarehouse;
                order.SecondaryWarehouse = powerUserWarehouseProxyResult.SecondaryWarehouse;
            }

            order.ShipToAddress = address;

            _orderRepository.Save();

            return order;
        }

        public PowerUserWarehouseResult GetPowerUserWarehouseOverrides(ICustomerData customerData)
        {
            PowerUserWarehouseResult result = null;

            var order = _orderRepository.OpenOrder(customerData, false);

            if ((order != null) && order.IsPowerUserOrder)
                result = new PowerUserWarehouseResult
                {
                    PrimaryWarehouse = order.PrimaryWarehouse,
                    SecondaryWarehouse = order.SecondaryWarehouse,
                    AssignedWarehouse = order.AssignedWhse
                };

            return result;
        }

        public IEnumerable<ShipMethodDetail> GetPowerUserShipMethods()
        {
            using (
                var connection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["EPWIConnectionString"].ConnectionString))
            {
                return
                    connection.Query<ShipMethodDetail>(
                        "SELECT Whse AS [From], VC AS ShipMethod, VIA AS Description FROM tbl_kitcat_ZSHPVPF ORDER BY SEQ",
                        null);
            }
        }
    }
}