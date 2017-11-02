namespace EPWI.Components.Models
{
    using System.Data.Linq;
    using System.Data.Linq.Mapping;
    using System.Reflection;
    using System.Configuration;

    public partial class EPWIDataContext
    {
        public static EPWIDataContext GetInstance()
        {
            return GetInstance(false);
        }

        public static EPWIDataContext GetInstance(bool asAdmin)
        {
            return
                new EPWIDataContext(
                    ConfigurationManager.ConnectionStrings[
                        asAdmin ? "EPWIAdminConnectionString" : "EPWIConnectionString"].ConnectionString);
        }

        // Overload of default method to allow for omitting line code from sp call
        [Function(Name = "dbo.usp_LookupNIPCCode")]
        public ISingleResult<InventoryItemUnprocessed> usp_LookupNIPCCode(
            [Parameter(DbType = "Char(20)")] string sItemNumber)
        {
            var mi = ((MethodInfo)(MethodBase.GetCurrentMethod()));
            IExecuteResult result = this.ExecuteMethodCall(this, mi, sItemNumber);
            return ((ISingleResult<InventoryItemUnprocessed>)(result?.ReturnValue));
        }

        // Overload of default method to allow for omitting nipc from sp call
        [Function(Name = "dbo.usp_GetKitCatalogHeaderData")]
        public ISingleResult<usp_GetKitCatalogHeaderDataResult> usp_GetKitCatalogHeaderData(
            [Parameter(DbType = "Char(10)")] string sKitID)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodBase.GetCurrentMethod())), sKitID);
            return ((ISingleResult<usp_GetKitCatalogHeaderDataResult>)(result?.ReturnValue));
        }
    }
}