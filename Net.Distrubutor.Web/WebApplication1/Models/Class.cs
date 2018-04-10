using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace WebApplication1.Models
{
    public static class LocalizedRouteExtensions
    {
        /// <summary>
        /// Adds a route to the route builder with the specified name and template
        /// </summary>
        /// <param name="routeBuilder">The route builder to add the route to</param>
        /// <param name="name">The name of the route</param>
        /// <param name="template">The URL pattern of the route</param>
        /// <returns>Route builder</returns>
        public static IRouteBuilder MapLocalizedRoute(this IRouteBuilder routeBuilder, string name, string template)
        {
            return MapLocalizedRoute(routeBuilder, name, template, defaults: null);
        }

        /// <summary>
        /// Adds a route to the route builder with the specified name, template, and default values
        /// </summary>
        /// <param name="routeBuilder">The route builder to add the route to</param>
        /// <param name="name">The name of the route</param>
        /// <param name="template">The URL pattern of the route</param>
        /// <param name="defaults">An object that contains default values for route parameters. 
        /// The object's properties represent the names and values of the default values</param>
        /// <returns>Route builder</returns>
        public static IRouteBuilder MapLocalizedRoute(this IRouteBuilder routeBuilder, string name, string template, object defaults)
        {
            return MapLocalizedRoute(routeBuilder, name, template, defaults, constraints: null);
        }

        /// <summary>
        /// Adds a route to the route builder with the specified name, template, default values, and constraints.
        /// </summary>
        /// <param name="routeBuilder">The route builder to add the route to</param>
        /// <param name="name">The name of the route</param>
        /// <param name="template">The URL pattern of the route</param>
        /// <param name="defaults"> An object that contains default values for route parameters. 
        /// The object's properties represent the names and values of the default values</param>
        /// <param name="constraints">An object that contains constraints for the route. 
        /// The object's properties represent the names and values of the constraints</param>
        /// <returns>Route builder</returns>
        public static IRouteBuilder MapLocalizedRoute(this IRouteBuilder routeBuilder,
            string name, string template, object defaults, object constraints)
        {
            return MapLocalizedRoute(routeBuilder, name, template, defaults, constraints, dataTokens: null);
        }

        /// <summary>
        /// Adds a route to the route builder with the specified name, template, default values, constraints anddata tokens.
        /// </summary>
        /// <param name="routeBuilder">The route builder to add the route to</param>
        /// <param name="name">The name of the route</param>
        /// <param name="template">The URL pattern of the route</param>
        /// <param name="defaults"> An object that contains default values for route parameters. 
        /// The object's properties represent the names and values of the default values</param>
        /// <param name="constraints">An object that contains constraints for the route. 
        /// The object's properties represent the names and values of the constraints</param>
        /// <param name="dataTokens">An object that contains data tokens for the route. 
        /// The object's properties represent the names and values of the data tokens</param>
        /// <returns>Route builder</returns>
        public static IRouteBuilder MapLocalizedRoute(this IRouteBuilder routeBuilder,
            string name, string template, object defaults, object constraints, object dataTokens)
        {
            if (routeBuilder.DefaultHandler == null)
                throw new ArgumentNullException(nameof(routeBuilder));

            //get registered InlineConstraintResolver
            var inlineConstraintResolver = routeBuilder.ServiceProvider.GetRequiredService<IInlineConstraintResolver>();

            //create new generic route
            routeBuilder.Routes.Add(new LocalizedRoute(routeBuilder.DefaultHandler, name, template,
                new RouteValueDictionary(defaults), new RouteValueDictionary(constraints), new RouteValueDictionary(dataTokens),
                inlineConstraintResolver));

            return routeBuilder;
        }

        /// <summary>
        /// Clear _seoFriendlyUrlsForLanguagesEnabled cached value for the routes
        /// </summary>
        /// <param name="routers">Routers</param>
        public static void ClearSeoFriendlyUrlsCachedValueForRoutes(this IEnumerable<IRouter> routers)
        {
            if (routers == null)
                throw new ArgumentNullException(nameof(routers));

            //clear cached settings
            foreach (var router in routers)
            {
                var routeCollection = router as RouteCollection;
                if (routeCollection == null)
                    continue;

                for (var i = 0; i < routeCollection.Count; i++)
                {
                    var route = routeCollection[i];
                }
            }
        }
    }

    public class LocalizedRoute : Route
    {
        #region Fields

        private readonly IRouter _target;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="target">Target</param>
        /// <param name="routeName">Route name</param>
        /// <param name="routeTemplate">Route template</param>
        /// <param name="defaults">Defaults</param>
        /// <param name="constraints">Constraints</param>
        /// <param name="dataTokens">Data tokens</param>
        /// <param name="inlineConstraintResolver">Inline constraint resolver</param>
        public LocalizedRoute(IRouter target, string routeName, string routeTemplate, RouteValueDictionary defaults,
            IDictionary<string, object> constraints, RouteValueDictionary dataTokens, IInlineConstraintResolver inlineConstraintResolver)
            : base(target, routeName, routeTemplate, defaults, constraints, dataTokens, inlineConstraintResolver)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }

        #endregion

        

        #region Methods

        /// <summary>
        /// Returns information about the URL that is associated with the route
        /// </summary>
        /// <param name="context">A context for virtual path generation operations</param>
        /// <returns>Information about the route and virtual path</returns>
        public override VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            //get base virtual path
            var data = base.GetVirtualPath(context);
            if (data == null)
                return null;
            //add language code to page URL in case if it's localized URL
            var path = context.HttpContext.Request.Path.Value;
            return data;
        }

        /// <summary>
        /// Route request to the particular action
        /// </summary>
        /// <param name="context">A route context object</param>
        /// <returns>Task of the routing</returns>
        public override Task RouteAsync(RouteContext context)
        {
            //if path isn't localized, no special action required
            var path = context.HttpContext.Request.Path.Value;

            //remove language code and application path from the path
            var newPath = path.RemoveLanguageSeoCodeFromUrl(context.HttpContext.Request.PathBase, false);

            //set new request path and try to get route handler
            context.HttpContext.Request.Path = newPath;
            base.RouteAsync(context).Wait();

            //then return the original request path
            context.HttpContext.Request.Path = path;
            return _target.RouteAsync(context);
        }

        /// <summary>
        /// Clear _seoFriendlyUrlsForLanguagesEnabled cached value
        /// </summary>

        #endregion

        #region Properties

        /// <summary>
        /// Gets value of _seoFriendlyUrlsForLanguagesEnabled settings
        /// </summary>


        #endregion
    }

    public static class Test1
    {
        public static string RemoveLanguageSeoCodeFromUrl(this string url, PathString pathBase, bool isRawPath)
        {
            if (string.IsNullOrEmpty(url))
                return url;

            //remove application path from raw URL
            if (isRawPath)
                url = url.RemoveApplicationPathFromRawUrl(pathBase);

            //get result URL
            url = url.TrimStart('/');
            var result = url.Contains('/') ? url.Substring(url.IndexOf('/')) : string.Empty;

            //and add back application path to raw URL
            if (isRawPath)
                result = pathBase + result;

            return result;
        }
        public static string RemoveApplicationPathFromRawUrl(this string rawUrl, PathString pathBase)
        {
            new PathString(rawUrl).StartsWithSegments(pathBase, out PathString result);
            return WebUtility.UrlDecode(result);
        }
    }


}
