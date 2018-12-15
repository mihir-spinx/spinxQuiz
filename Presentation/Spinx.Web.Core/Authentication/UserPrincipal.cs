using System.Collections.Generic;

namespace Spinx.Web.Core.Authentication
{
    public class UserPrincipal
    {
        public int UserId { get; set; }
        public string Slug { get; set; }
        public string ProfileImage { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public IList<string> Permissions { get; set; }
        public IList<string> Roles { get; set; }

        public UserPrincipal()
        {
            
        }

        public UserPrincipal(int userId, string name, string email)
        {
            UserId = userId;
            Name = name;
            Email = email;
        }

        public bool IsLogedIn()
        {
            return UserId > 0;
        }

        public bool HasPermission(string permission)
        {
            return true;
            //return permission != null && Permissions.Contains(permission.ToLower());
        }

        public bool HasPermission(IList<string> permissions)
        {
            return true;
            //return permissions != null && Permissions.Intersect(permissions).Any();
        }

        public bool HasRole(string role)
        {
            return true;
            //return role != null && Roles.Contains(role.NullSafeToLower());
        }
    }
}