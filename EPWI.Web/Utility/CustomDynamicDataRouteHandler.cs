using System;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Security;
using EPWI.Web.Utility;

/// <summary>
/// The CustomDynamicDataRouteHandler enables the user 
/// to access a table based on the following:
/// 1) The user's credentials as authenticated by 
/// ASP.NET forms authentication. The user's
/// authentication provides the first security level, 
/// which includes or excludes a table from being accessed.
/// 2) The SecurityAttribute role and action properties 
/// applicable to the table. This attribute provides the
/// second security level, that is it authorizes the 
/// authenticated users to perform certain operations
/// based on their role and allowed actions.
/// </summary>

public class CustomDynamicDataRouteHandler : 
  DynamicDataRouteHandler
{
  public CustomDynamicDataRouteHandler()
  { }

  /// <summary>
  /// Determine access to tables based on ASP.NET authentication
  /// and security attribute as applied to the tables in the
  /// data model.
  /// </summary>
  /// <param name="route">The route used by 
  /// ASP.NET Dynamic Data. This is in the format 
  /// {table}/{action}.aspx and is defined in Global.asax</param>
  /// <param name="table">The metadata that describes a table 
  /// for use by Dynamic Data pages.</param>
  /// <param name="action">The action allowed for the table from the
  /// route.</param>
  /// <returns></returns>
  public override IHttpHandler CreateHandler(
     DynamicDataRoute route, MetaTable table, string action)
  {
    // Store the ASP.NET authenticated roles 
    string[] roles = Roles.GetAllRoles();
    
    // Obtain the roles that have administrative
    // access rights.
    Attribute[] adminRoles =
              Attribute.GetCustomAttributes(typeof(AdminRoles));

    // Obtain the roles that have limited 
    // access rights.
    Attribute[] anonymousRoles =
              Attribute.GetCustomAttributes(typeof(AnonymousRoles));
   

    // Allow tables access based on the authenticated
    // roles and on the security attributes applied
    // to the tables.
    for (int i = 0; i < roles.Length; i++)
    {
     
      // Check if the user is an authenticated 
      // administrator.
      foreach (SecurityAttribute admin in 
          adminRoles.OfType<SecurityAttribute>())
        {
          if (
              (Roles.IsUserInRole(admin.Role)) &&
              (admin.Action == "All")
            )
            // Allow complete access.
            return base.CreateHandler(route, table, action);
        }
      // Instantiate the SecurityInformation 
      // utility object.
      DynamicDataSecurity secInfo =
        new DynamicDataSecurity();

      // Check if the user is an administrator
      // and is authenticated.
      if (secInfo.IsUserInAdmimistrativeRole())
        // Allow complete access.
        return base.CreateHandler(route, table, action);

      // Check if the user is authenticated.
      // Allow those actions permitted by the
      // security attributes.
      if (Roles.IsUserInRole(roles[i]))
      {
        foreach (SecurityAttribute attribute in
          table.Attributes.OfType<SecurityAttribute>())
        {
          // Allow access the permissible actions.
          if (attribute.Role == roles[i] &&
                attribute.Action == action)
          {
            return base.CreateHandler(route, table, action);
          }

        }
      }
    }
    // Search for roles that have limited access
    // to the database tables.
    // Allow access to those tables that 
    // are marked with the roles having limited access.
    // Note this check is important to allow
    // anonymous access; otherwise you would not
    // have any table showing in the scaffolded list
    // in the default.aspx page.
    foreach (SecurityAttribute anonymous in 
       anonymousRoles.OfType<SecurityAttribute>())
     {
        foreach (SecurityAttribute entityRole in
             table.Attributes.OfType<SecurityAttribute>())
        {
          if (entityRole.Role == anonymous.Role)
            // Allow limited access.
            return base.CreateHandler(route, table, anonymous.Action);
        }
     }

    // No role and no attribute exist; access is denied.
    return null;

  }
}

