using System;
using System.Web.Mvc;

namespace EPWI.Web.ModelBinders
{
  public class SessionModelBinder<T> : IModelBinder where T : new()
  {
    private string sessionKey;

    public SessionModelBinder(string key)
    {
      sessionKey = key;
    }

    #region IModelBinder Members

    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
      if (bindingContext.Model != null)
        throw new InvalidOperationException("Cannot update instances");

      // Return the object from Session (creating it first if necessary)
      T item = (T)controllerContext.HttpContext.Session[sessionKey];

      if (item == null)
      {
        item = new T();
        controllerContext.HttpContext.Session[sessionKey] = item;
      }
      return item;
    }

    #endregion
  }
}