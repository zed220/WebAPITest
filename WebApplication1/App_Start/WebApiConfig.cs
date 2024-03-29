﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebAPIBooks {
    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "Sorted",
                routeTemplate: "api/{controller}/{sortMode}",
                defaults: new { sortMode = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "image",
                routeTemplate: "api/{controller}/{imageId}"
            );
        }
    }
}
