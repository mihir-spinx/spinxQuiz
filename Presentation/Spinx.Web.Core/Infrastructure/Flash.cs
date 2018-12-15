using System.Web;

namespace Spinx.Web.Core.Infrastructure
{
    public static class Flash
    {
        public static void Success(string message)
        {
            CreateCookieWithFlashMessage(Notification.Success, message);
        }

        public static void Danger(string message)
        {
            CreateCookieWithFlashMessage(Notification.Danger, message);
        }

        public static void Warning(string message)
        {
            CreateCookieWithFlashMessage(Notification.Warning, message);
        }

        public static void Info(string message)
        {
            CreateCookieWithFlashMessage(Notification.Info, message);
        }
        
        private static void CreateCookieWithFlashMessage(Notification notification, string message)
        {
            HttpContext.Current.Response.Cookies.Add(new HttpCookie($"Flash.{notification}", message) { Path = "/" });
        }

        private enum Notification
        {
            Danger,
            Warning,
            Success,
            Info
        }
    }
}