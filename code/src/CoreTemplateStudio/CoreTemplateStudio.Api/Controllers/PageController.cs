// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using CoreTemplateStudio.Api.Enumerables;
using CoreTemplateStudio.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreTemplateStudio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageController : Controller
    {
        private readonly IDictionary<Page, PageModel> pageStore;

        public PageController()
        {
            pageStore = new Dictionary<Page, PageModel>
            {
                // the supported frameworks are randomly added/removed to support development of functonality
                // while the engine is not ready. The templates would also only have one frontend framework (per page).
                // Multiple frontend frameworks here are only to make fake data initialization easier.
                { Page.BlankUwp, new PageModel(Page.BlankUwp, Framework.CodeBehind, Framework.MVVMBasic, Framework.Prism, Framework.CaliburnMicro) },
                { Page.Camera, new PageModel(Page.Camera, Framework.CodeBehind, Framework.MVVMBasic, Framework.MVVMLight, Framework.Prism, Framework.CaliburnMicro) },
                { Page.Chart, new PageModel(Page.Chart, Framework.CodeBehind, Framework.MVVMBasic, Framework.MVVMLight, Framework.CaliburnMicro) },
                { Page.ContentGrid, new PageModel(Page.ContentGrid, Framework.MVVMLight, Framework.Prism, Framework.CaliburnMicro) },
                { Page.DataGrid, new PageModel(Page.DataGrid, Framework.CodeBehind, Framework.MVVMBasic, Framework.MVVMLight, Framework.Prism, Framework.CaliburnMicro) },
                { Page.Grid, new PageModel(Page.Grid, Framework.CodeBehind, Framework.MVVMBasic, Framework.MVVMLight) },
                { Page.ImageGallery, new PageModel(Page.ImageGallery, Framework.CodeBehind, Framework.Prism, Framework.CaliburnMicro) },
                { Page.InkDraw, new PageModel(Page.InkDraw, Framework.CodeBehind, Framework.MVVMBasic, Framework.MVVMLight, Framework.CaliburnMicro) },
                { Page.InkSmartCanvas, new PageModel(Page.InkSmartCanvas, Framework.MVVMBasic, Framework.MVVMLight, Framework.Prism, Framework.CaliburnMicro) },
                { Page.Map, new PageModel(Page.Map, Framework.CodeBehind, Framework.MVVMBasic, Framework.MVVMLight, Framework.Prism, Framework.CaliburnMicro) },
                { Page.MasterDetail, new PageModel(Page.MasterDetail, Framework.CodeBehind, Framework.MVVMBasic, Framework.Prism, Framework.CaliburnMicro) },
                { Page.MediaPlayer, new PageModel(Page.MediaPlayer, Framework.CodeBehind, Framework.MVVMBasic, Framework.MVVMLight, Framework.Prism, Framework.CaliburnMicro) },
                { Page.Settings, new PageModel(Page.Settings, Framework.CodeBehind, Framework.MVVMBasic, Framework.MVVMLight, Framework.CaliburnMicro) },
                { Page.TabbedPivot, new PageModel(Page.TabbedPivot, Framework.CodeBehind, Framework.MVVMBasic, Framework.MVVMLight, Framework.Prism) },
                { Page.WebView, new PageModel(Page.WebView, Framework.MVVMBasic, Framework.MVVMLight, Framework.Prism, Framework.CaliburnMicro) },
                { Page.BlankWeb, new PageModel(Page.BlankWeb, Framework.ReactJS, Framework.VueJS, Framework.AngularJS, Framework.NodeJS, Framework.Django, Framework.SinglePageJS, Framework.MultiPageJS) },
                { Page.Blog, new PageModel(Page.Blog, Framework.VueJS, Framework.AngularJS, Framework.NodeJS, Framework.SinglePageJS) },
                { Page.Contact, new PageModel(Page.Contact,  Framework.ReactJS, Framework.Django, Framework.MultiPageJS) },
                { Page.Home, new PageModel(Page.Home,  Framework.ReactJS, Framework.VueJS, Framework.AngularJS, Framework.NodeJS, Framework.Django) },
                { Page.News, new PageModel(Page.News,  Framework.ReactJS, Framework.AngularJS, Framework.NodeJS, Framework.Django, Framework.SinglePageJS, Framework.MultiPageJS) },
                { Page.FAQs, new PageModel(Page.FAQs,  Framework.AngularJS, Framework.VueJS, Framework.SinglePageJS, Framework.MultiPageJS) },
            };
        }

        // GET api/feature?page=<>&page=<>
        // returns a list of pages matching given frameworks
        [HttpGet]
        public JsonResult GetPagesForFrameworks(string[] frameworks)
        {
            if (frameworks == null || frameworks.Length == 0)
            {
                return Json(BadRequest(new { message = "please specify valid frameworks" }));
            }

            HashSet<Framework> parsedFrameworks = new HashSet<Framework>();

            foreach (string framework in frameworks)
            {
                if (Enum.TryParse(framework, true, out Framework parsedFramework))
                {
                    parsedFrameworks.Add(parsedFramework);
                }
                else
                {
                    return Json(NotFound(new { message = "framework not found" }));
                }
            }

            IDictionary<Page, PageModel> validPages = new Dictionary<Page, PageModel>();
            foreach (var item in pageStore)
            {
                if (item.Value.HasFrameworks(parsedFrameworks))
                {
                    validPages.Add(item);
                }
            }

            return Json(Ok(validPages));
        }
    }
}
