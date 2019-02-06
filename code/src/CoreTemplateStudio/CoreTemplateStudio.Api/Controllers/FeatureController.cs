// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using CoreTemplateStudio.Api.Enumerables;
using CoreTemplateStudio.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreTemplateStudio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureController : Controller
    {
        private readonly IDictionary<Feature, FeatureModel> featureStore;

        public FeatureController()
        {
            featureStore = new Dictionary<Feature, FeatureModel>
            {
                // the supported frameworks are randomly added/removed to support development of functonality
                // while the engine is not ready.
                { Feature.ThreeDLauncher, new FeatureModel(Feature.ThreeDLauncher, Framework.CodeBehind, Framework.MVVMBasic, Framework.Prism, Framework.CaliburnMicro) },
                { Feature.BackgroundTask, new FeatureModel(Feature.BackgroundTask, Framework.CodeBehind, Framework.MVVMBasic, Framework.MVVMLight, Framework.Prism, Framework.CaliburnMicro) },
                { Feature.DragAndDrop, new FeatureModel(Feature.DragAndDrop, Framework.CodeBehind, Framework.MVVMBasic, Framework.MVVMLight, Framework.CaliburnMicro) },
                { Feature.FeedbackHub, new FeatureModel(Feature.FeedbackHub, Framework.MVVMLight, Framework.Prism, Framework.CaliburnMicro) },
                { Feature.FirstUsePrompt, new FeatureModel(Feature.FirstUsePrompt, Framework.CodeBehind, Framework.MVVMBasic, Framework.MVVMLight, Framework.Prism, Framework.CaliburnMicro) },
                { Feature.LiveTile, new FeatureModel(Feature.LiveTile, Framework.CodeBehind, Framework.MVVMBasic, Framework.MVVMLight) },
                { Feature.HubNotifications, new FeatureModel(Feature.HubNotifications, Framework.CodeBehind, Framework.Prism, Framework.CaliburnMicro) },
                { Feature.ToastNotifications, new FeatureModel(Feature.ToastNotifications, Framework.CodeBehind, Framework.MVVMBasic, Framework.MVVMLight, Framework.CaliburnMicro) },
                { Feature.WhatsNewPrompt, new FeatureModel(Feature.WhatsNewPrompt, Framework.MVVMBasic, Framework.MVVMLight, Framework.Prism, Framework.CaliburnMicro) },
                { Feature.AzureDatabase, new FeatureModel(Feature.AzureDatabase, Framework.ReactJS, Framework.VueJS, Framework.AngularJS, Framework.NodeJS, Framework.Django, Framework.SinglePageJS, Framework.MultiPageJS) },
                { Feature.AzureFunctions, new FeatureModel(Feature.AzureFunctions, Framework.VueJS, Framework.AngularJS, Framework.NodeJS, Framework.SinglePageJS) },
                { Feature.AzureIdentity, new FeatureModel(Feature.AzureIdentity,  Framework.ReactJS, Framework.Django, Framework.MultiPageJS) },
                { Feature.AzureKeyVault, new FeatureModel(Feature.AzureKeyVault,  Framework.ReactJS, Framework.VueJS, Framework.NodeJS, Framework.Django, Framework.SinglePageJS, Framework.MultiPageJS) },
            };
        }

        // GET api/feature?frameworks=<>&frameworks=<>
        // returns a list of features matching given frameworks
        [HttpGet]
        public JsonResult GetFeaturesForFrameworks(string[] frameworks)
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

            IDictionary<Feature, FeatureModel> validFeatures = new Dictionary<Feature, FeatureModel>();
            foreach (var item in featureStore)
            {
                if (item.Value.HasFrameworks(parsedFrameworks))
                {
                    validFeatures.Add(item);
                }
            }

            return Json(Ok(validFeatures));
        }
    }
}
