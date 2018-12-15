using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Spinx.Web.Core.ViewEngine
{
    public abstract class CustomVirtualPathProviderViewEngine : VirtualPathProviderViewEngine
    {
        #region Fields

        internal Func<string, string> GetExtensionThunk;

        private readonly string[] _emptyLocations = null;

        #endregion

        #region Ctor

        protected CustomVirtualPathProviderViewEngine()
        {
            GetExtensionThunk = VirtualPathUtility.GetExtension;
        }

        #endregion

        #region Utilities

        protected virtual string GetPath(ControllerContext controllerContext, string[] locations, string[] areaLocations, string locationsPropertyName, string name, string controllerName, string cacheKeyPrefix, bool useCache, out string[] searchedLocations)
        {
            searchedLocations = _emptyLocations;
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }
            var areaName = GetAreaName(controllerContext.RouteData);

            //little hack to get admin area to be in /Administration/ instead of /Spinx/Admin/ or Areas/Admin/
            //if (!string.IsNullOrEmpty(areaName) && areaName.Equals("admin", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    //var newLocations = areaLocations.ToList();

            //    var newLocations = new List<string>();
            //    newLocations.Insert(0, "~/Administration/Views/{1}/{0}.cshtml");
            //    newLocations.Insert(0, "~/Administration/Views/Shared/{0}.cshtml");
            //    areaLocations = newLocations.ToArray();
            //}

            var flag = !string.IsNullOrEmpty(areaName);
            var viewLocations = GetViewLocations(locations, flag ? areaLocations : null);
            if (viewLocations.Count == 0)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Properties cannot be null or empty.", new object[] { locationsPropertyName }));
            }
            var flag2 = IsSpecificPath(name);
            var key = CreateCacheKey(cacheKeyPrefix, name, flag2 ? string.Empty : controllerName, areaName);
            if (useCache)
            {
                var cached = ViewLocationCache.GetViewLocation(controllerContext.HttpContext, key);
                if (cached != null)
                {
                    return cached;
                }
            }
            return !flag2 ? GetPathFromGeneralName(controllerContext, viewLocations, name, controllerName, areaName, key, ref searchedLocations) : GetPathFromSpecificName(controllerContext, name, key, ref searchedLocations);
        }

        protected virtual bool FilePathIsSupported(string virtualPath)
        {
            if (FileExtensions == null)
            {
                return true;
            }
            var str = GetExtensionThunk(virtualPath).TrimStart('.');
            return FileExtensions.Contains(str, StringComparer.OrdinalIgnoreCase);
        }

        protected virtual string GetPathFromSpecificName(ControllerContext controllerContext, string name, string cacheKey, ref string[] searchedLocations)
        {
            var virtualPath = name;
            if (!FilePathIsSupported(name) || !FileExists(controllerContext, name))
            {
                virtualPath = string.Empty;
                searchedLocations = new[] { name };
            }
            ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, virtualPath);
            return virtualPath;
        }

        protected virtual string GetPathFromGeneralName(ControllerContext controllerContext, List<ViewLocation> locations, string name, string controllerName, string areaName, string cacheKey, ref string[] searchedLocations)
        {
            var virtualPath = string.Empty;
            searchedLocations = new string[locations.Count];
            for (var i = 0; i < locations.Count; i++)
            {
                var str2 = locations[i].Format(name, controllerName, areaName);
                if (FileExists(controllerContext, str2))
                {
                    searchedLocations = _emptyLocations;
                    virtualPath = str2;
                    ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, virtualPath);
                    return virtualPath;
                }
                searchedLocations[i] = str2;
            }
            return virtualPath;
        }

        protected virtual string CreateCacheKey(string prefix, string name, string controllerName, string areaName)
        {
            return string.Format(CultureInfo.InvariantCulture, ":ViewCacheEntry:{0}:{1}:{2}:{3}:{4}", GetType().AssemblyQualifiedName, prefix, name, controllerName, areaName);
        }

        protected virtual List<ViewLocation> GetViewLocations(string[] viewLocationFormats, string[] areaViewLocationFormats)
        {
            var list = new List<ViewLocation>();
            if (areaViewLocationFormats != null)
            {
                list.AddRange(areaViewLocationFormats.Select(str => new AreaAwareViewLocation(str)));
            }
            if (viewLocationFormats != null)
            {
                list.AddRange(viewLocationFormats.Select(str2 => new ViewLocation(str2)));
            }
            return list;
        }

        protected virtual bool IsSpecificPath(string name)
        {
            var ch = name[0];
            if (ch != '~')
            {
                return (ch == '/');
            }
            return true;
        }

        protected virtual string GetAreaName(RouteData routeData)
        {
            if (routeData.DataTokens.TryGetValue("area", out var obj2))
            {
                return (obj2 as string);
            }
            return GetAreaName(routeData.Route);
        }

        protected virtual string GetAreaName(RouteBase route)
        {
            switch (route)
            {
                case IRouteWithArea area:
                    return area.Area;
                case Route route2 when (route2.DataTokens != null):
                    return (route2.DataTokens["area"] as string);
            }

            return null;
        }

        protected virtual ViewEngineResult FindThemeableView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (string.IsNullOrEmpty(viewName))
            {
                throw new ArgumentException("View name cannot be null or empty.", "viewName");
            }
            var requiredString = controllerContext.RouteData.GetRequiredString("controller");
            var str2 = GetPath(controllerContext, ViewLocationFormats, AreaViewLocationFormats, "ViewLocationFormats", viewName, requiredString, "View", useCache, out string[] strArray);
            var str3 = GetPath(controllerContext, MasterLocationFormats, AreaMasterLocationFormats, "MasterLocationFormats", masterName, requiredString, "Master", useCache, out string[] strArray2);
            if (!string.IsNullOrEmpty(str2) && (!string.IsNullOrEmpty(str3) || string.IsNullOrEmpty(masterName)))
            {
                return new ViewEngineResult(CreateView(controllerContext, str2, str3), this);
            }
            if (strArray2 == null)
            {
                strArray2 = new string[0];
            }
            return new ViewEngineResult(strArray.Union(strArray2));
        }

        protected virtual ViewEngineResult FindThemeablePartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (string.IsNullOrEmpty(partialViewName))
            {
                throw new ArgumentException("Partial view name cannot be null or empty.", "partialViewName");
            }
            var requiredString = controllerContext.RouteData.GetRequiredString("controller");
            var str2 = GetPath(controllerContext, PartialViewLocationFormats, AreaPartialViewLocationFormats, "PartialViewLocationFormats", partialViewName, requiredString, "Partial", useCache, out string[] strArray);
            if (string.IsNullOrEmpty(str2))
            {
                return new ViewEngineResult(strArray);
            }
            return new ViewEngineResult(CreatePartialView(controllerContext, str2), this);
        }

        #endregion

        #region Methods

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            var result = FindThemeableView(controllerContext, viewName, masterName, useCache);
            return result;

        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            var result = FindThemeablePartialView(controllerContext, partialViewName, useCache);
            return result;
        }

        #endregion
    }

    public class AreaAwareViewLocation : ViewLocation
    {
        public AreaAwareViewLocation(string virtualPathFormatString)
            : base(virtualPathFormatString)
        {
        }

        public override string Format(string viewName, string controllerName, string areaName)
        {
            return string.Format(CultureInfo.InvariantCulture, VirtualPathFormatString, viewName, controllerName, areaName);
        }
    }

    public class ViewLocation
    {
        protected readonly string VirtualPathFormatString;

        public ViewLocation(string virtualPathFormatString)
        {
            VirtualPathFormatString = virtualPathFormatString;
        }

        public virtual string Format(string viewName, string controllerName, string areaName)
        {
            return string.Format(CultureInfo.InvariantCulture, VirtualPathFormatString, viewName, controllerName);
        }
    }
}
