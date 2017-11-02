using System;
using System.Linq;
using System.Web.Security;
using EPWI.Components.Models;

namespace EPWI.Web.Providers
{
    public class EpwiRoleProvider : RoleProvider
    {
        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            var rep = new RoleRepository();

            var roles = rep.GetAllRoles().ToList();

            // add the "PRICING_ACCESS" virtual role so we can assign access in n2cms
            roles.Add("PRICING_ACCESS");

            return roles.ToArray();
        }

        public override string[] GetRolesForUser(string username)
        {
            var rep = new UserRepository();
            var user = rep.GetByUserName(username);

            if (user == null)
            {
                return new string[] {};
            }

            return user.Roles;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}