using System;
using System.Linq;
using System.Web.Security;
namespace EPWI.Web.Utility
{
#region SecurityAttribute
  /// <summary>
  /// The SecurityAttribute enables you to specify, in the data model, 
  /// what role and actions are allowed for a table. 
  /// <remarks> 
  /// Note the following:
  /// a) You must specify the List action for a table, 
  /// to show the table link in the default.aspx start page. 
  /// This is because list is the default action and that's what is 
  /// used by the route handler. 
  /// b) Only the actions specified by the SecurityAttribute are 
  /// allowed for a certain table.</remarks>
  /// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
  public class SecurityAttribute : Attribute
  {

    // Contains the user's role.
    public string Role { get; set; }

    // Contains the allowed action for the
    // role. The values for the action
    // are the ones defined in Global.asax
    // for the routes.
    public string Action { get; set; }

    public SecurityAttribute()
    { }

    // This instructs Dynamic Data to return
    // the entire attribute collection not 
    // just the first attribute.
    public override object TypeId
    {
      get
      { return this; }
    }

  }
  #endregion

  #region Auxiliary Classes
  /// <summary>
  /// This class enables you to communicate the 
  /// administrative roles to Dynamic Data using
  /// the SecurityAttribute.
  /// <remarks> 
  /// It is important to note that with this class you have
  /// the felxibility to list the roles declaratively via
  /// the SecurityAttribute without hardcoding them.
  /// The values given to the Role in the attribute 
  /// must be the same values used for ASP.NET 
  /// authentication.
  /// The value for the Action is All (for now). 
  /// This means that all the acctions, as defined 
  /// in Global.asax, are allowed
/// </remarks>
  /// </summary>
  [Security(Role = "ADMIN", Action = "All")]
  public class AdminRoles
  {

  }

  /// <summary>
  /// Define the anonymous roles.
  /// The value given to the Role in the attribute 
  /// is not used for ASP.NET authentication.
  /// The value for the Action is AnonymousList. This means that
  /// only the actions, as defined in Global.asax 
  /// page template are allowed
  /// </summary>
  [Security(Role = "Anonymous", Action = "AnonymousList")]
  public class AnonymousRoles
  {

  }
  #endregion

  #region DynamicDataSecurity Class
  /// <summary>
  /// The DynamicDataSecurity class enables you to query
  /// the authentication roles, as defined in the ASP.NET
  /// security database.
  /// <remarks>The class also enables you to define the
  /// administrative roles that have full action 
  /// capabilities.</remarks>
  /// </summary>
  public class DynamicDataSecurity
  {
    public string[] roles;
    public Attribute[] adminRoles;
    public Attribute[] anonymousRoles;

    public DynamicDataSecurity()
    {
      // Store the ASP.NET authenticated roles 
      roles = Roles.GetAllRoles();

      // Obtain the roles that have administrative
      // access rights.
      adminRoles = Attribute.GetCustomAttributes(
        typeof(AdminRoles));

      // Obtain the roles that have limited 
      // access rights.
      anonymousRoles = Attribute.GetCustomAttributes(
        typeof(AnonymousRoles));
    }

    // Check if the logged user role belongs to the
    // administrative roles and is authenticated.
    public bool IsUserInAdmimistrativeRole()
    {
      bool result = false;

      // For each authentication role check if the 
      // logged user is in that role
      for (int i = 0; i < roles.Length; i++)
      {
        // For each administrative role
        // check if the user is in the role and 
        // is authenticated.
        foreach (SecurityAttribute admin in
            adminRoles.OfType<SecurityAttribute>())
        {
          if ((Roles.IsUserInRole(admin.Role)) &&
              (admin.Action == "All"))
          {
            // User is in authenticated administrative role.
            result = true;
            break;
          }
        }
      }
      return result;
    }

    // Check if the logged user role belongs to 
    // an authenticated role.
    public bool IsUserInAuthenticatedRole()
    {
      bool result = false;

      // For each authentication role check if the 
      // logged user is in the role..
      for (int i = 0; i < roles.Length; i++)
      {
        if (Roles.IsUserInRole(roles[i]))
        {
          result = true;
          break;
        }

      }
      return result;
    }
  }
  #endregion
}
