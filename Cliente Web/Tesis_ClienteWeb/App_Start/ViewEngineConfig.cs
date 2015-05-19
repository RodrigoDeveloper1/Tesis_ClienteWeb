using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tesis_ClienteWeb.App_Start
{
    public class ViewEngineConfig : RazorViewEngine
    {
        public ViewEngineConfig()
        {
            ViewLocationFormats = new[] 
            {
                "~/RazorViews/{1}/{0}.cshtml", "~/RazorViews/{1}/{0}.vbhtml",
                "~/RazorViews/Common/{0}.cshtml", "~/RazorViews/Common/{0}.vbhtml"
            };

            MasterLocationFormats = new[] 
            {
                "~/RazorViews/{1}/{0}.cshtml", "~/RazorViews/{1}/{0}.vbhtml",
                "~/RazorViews/Common/{0}.cshtml", "~/RazorViews/Common/{0}.vbhtml"
            };

            PartialViewLocationFormats = new[] 
            {
                "~/RazorViews/{1}/{0}.cshtml", "~/RazorViews/{1}/{0}.vbhtml",
                "~/RazorViews/Common/{0}.cshtml", "~/RazorViews/Common/{0}.vbhtml"
            };
        }
    }
}