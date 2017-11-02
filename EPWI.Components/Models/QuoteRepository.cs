using System;
using System.Linq;

namespace EPWI.Components.Models
{
  public class QuoteRepository : Repository
  {
    public QuoteRepository()
    { }
    public QuoteRepository(bool asAdmin) : base(asAdmin) { }

    public bool QuotesExist(ICustomerData customerData)
    {
      return (from q in Db.QuoteDetails
              where q.EPWCompCode == customerData.CompanyCode && q.EPWCustID == customerData.CustomerID && (q.UserID == customerData.UserID || q.Shared)
                select q).Any();
    }

    public QuoteDetail GetQuote(int quoteId, ICustomerData customerData)
    {
      return GetQuote(quoteId, customerData, false);
    }

    public QuoteDetail GetQuote(int quoteId, ICustomerData customerData, bool asEmployee)
    {
      return (from q in Db.QuoteDetails
              where (q.UserID == customerData.UserID || (q.EPWCompCode == customerData.CompanyCode && q.EPWCustID == customerData.CustomerID && q.Shared) || asEmployee)
                      && q.QuoteID == quoteId
              select q).SingleOrDefault();
    }

    public void LoadQuote(int quoteId, ICustomerData customerData)
    {
      Db.LoadQuote(quoteId, customerData.UserID);
    }

    public IQueryable<QuoteDetail> GetQuotesByUser(ICustomerData customerData)
    {
      return (from q in Db.QuoteDetails
              where q.UserID == customerData.UserID
              orderby q.QuoteDate
              select q);
    }

    public IQueryable<QuoteDetail> GetQuotesByCustomer(ICustomerData customerData)
    {
      return GetQuotesByCustomer(customerData, false);
    }

    public IQueryable<QuoteDetail> GetQuotesByCustomer(ICustomerData customerData, bool asEmployee)
    {
      return (from q in Db.QuoteDetails
              where (q.EPWCompCode == customerData.CompanyCode && q.EPWCustID == customerData.CustomerID) && (q.Shared || q.UserID == customerData.UserID || asEmployee)
              orderby q.QuoteDate
              select q);
    }

    public void DeleteQuote(int quoteId, ICustomerData customerData)
    {
      // can only delete quote if it belongs to current user, or the current user is deleted and the current user has the same company code / customer ID as the quote
      var quoteToDelete = (from q in Db.Quotes
                          where q.QuoteID == quoteId && 
                          ((!(from u in Db.Users select u.UserID).Contains(q.UserID) && q.EPWCompCode == customerData.CompanyCode && q.EPWCustID == customerData.CustomerID) 
                          || q.UserID == customerData.UserID)
                          select q).SingleOrDefault();

      if (quoteToDelete != null)
      {
        Db.Quotes.DeleteOnSubmit(quoteToDelete);
      }
    }

    public void UpdateQuoteFromOrder(Order order)
    {
      if (order.QuoteID.HasValue)
      {
        Db.UpdateQuoteFromOrder(order.OrderID);
      }
      else
      {
        throw new ApplicationException(
            $"Attempted to update a quote from an order not originally linked to a quote. Order ID: {order.OrderID}");
      }
    }
  }
}
