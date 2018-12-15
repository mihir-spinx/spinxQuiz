using Newtonsoft.Json;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.AdminRolePermissions;
using Spinx.Data.Repository.AdminUsers;
using Spinx.Services.AdminUsers;
using System;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;

namespace Spinx.Web.Core.Authentication
{
    public class UserAuth
    {
        public const string CookieUser = ".ASPX_USER";
        public const string CookieAdminUser = ".ASPX_ADMINUSER";

        public static bool OnAuthorization(HttpCookieCollection cookies, string authCookieName)
        {
            var authCookie = cookies[authCookieName];

            if (authCookie == null || authCookie.Value == "")
                return false;

            try
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                if (authTicket != null && (authTicket.Expired || authTicket.Expiration <= DateTime.Now))
                {
                    Signout(authCookieName);
                    return false;
                }
            }
            catch { return false; }

            return true;
        }

        public static void Signout(string authCookieName)
        {
            HttpContext.Current.Response.Cookies.Add(
                new HttpCookie(authCookieName, string.Empty)
                {
                    Expires = DateTime.Now.AddYears(-1), 
                    Path = HttpContext.Current.Request.ApplicationPath
                }
            );
        }

        public static void SignIn(int userId, string name, string email, bool remember = false)
        {
            SignInSetCookie(CookieUser, new UserPrincipal(userId, name, email), remember);
        }

        public static void SignInAdmin(int userId, string name, string email, bool remember = false)
        {
            SignInSetCookie(CookieAdminUser, new UserPrincipal(userId, name, email), remember);
        }

        public static void SignInSetCookie(string authCookieName, UserPrincipal user, bool isRemember = false)
        {
            var userData = JsonConvert.SerializeObject(user);
            var authTicket = new FormsAuthenticationTicket(1, user.Name, DateTime.Now, DateTime.Now.AddMinutes(1440), isRemember, userData);
            var encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(authCookieName, encryptedTicket);
            authCookie.Path = HttpContext.Current.Request.ApplicationPath;
            HttpContext.Current.Response.Cookies.Add(authCookie);
        }

        public static UserPrincipal GetSignInUser(string authCookieName)
        {
            var serializeModel = new UserPrincipal();

            var authCookie = HttpContext.Current.Request.Cookies[authCookieName];
            if (authCookie != null)
            {
                try
                {
                    var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    serializeModel = JsonConvert.DeserializeObject<UserPrincipal>(authTicket.UserData);

                    if (authCookieName == CookieAdminUser)
                        SetAdminUserPermissions(ref serializeModel);
                }
                catch (CryptographicException)
                {
                    Signout(authCookieName);
                }
            }

            return serializeModel;
        }

        private static void SetAdminUserPermissions(ref UserPrincipal serializeModel)
        {
            IDatabaseFactory databaseFactory = new DatabaseFactory();
            IAdminUserRepository adminUserRepository = new AdminUserRepository(databaseFactory);
            IAdminRoleRepository adminRoleRepository = new AdminRoleRepository(databaseFactory);
            IUnitOfWork unitOfWork = new UnitOfWork(databaseFactory);

            IAdminUserService adminUserService = new AdminUserService(adminUserRepository, adminRoleRepository, unitOfWork);
            serializeModel.Permissions = adminUserService.GetPermissions(serializeModel.UserId);
            serializeModel.Roles = adminUserService.GetRoles(serializeModel.UserId);
        }

        public static UserPrincipal User => GetSignInUser(CookieUser);
        public static UserPrincipal AdminUser => GetSignInUser(CookieAdminUser);


        public static bool IsLogedIn()
        {
            return IsLogedIn(CookieUser);
        }

        public static bool IsAdminLogedIn()
        {
            return IsLogedIn(CookieAdminUser);
        }

        public static bool IsLogedIn(string authCookieName)
        {
            var adminUser = GetSignInUser(authCookieName);
            return adminUser.UserId > 0;
        }
    }
}