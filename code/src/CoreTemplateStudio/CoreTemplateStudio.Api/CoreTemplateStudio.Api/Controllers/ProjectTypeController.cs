using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreTemplateStudio.Api.Models;
using CoreTemplateStudio.Api.Enumerables;

namespace CoreTemplateStudio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectTypeController : Controller
    {

        private readonly IDictionary<ShortProjectType, ProjectTypeItem> projectTypeStore;

        public ProjectTypeController()
        {
            projectTypeStore = new Dictionary<ShortProjectType, ProjectTypeItem>();

            projectTypeStore.Add(ShortProjectType.SPAFS, new ProjectTypeItem(ShortProjectType.SPAFS));
            projectTypeStore.Add(ShortProjectType.MPAFS, new ProjectTypeItem(ShortProjectType.MPAFS));
            projectTypeStore.Add(ShortProjectType.SPFE, new ProjectTypeItem(ShortProjectType.SPFE));
            projectTypeStore.Add(ShortProjectType.MPFE, new ProjectTypeItem(ShortProjectType.MPFE));
            projectTypeStore.Add(ShortProjectType.REST, new ProjectTypeItem(ShortProjectType.REST));

        }

        // GET: api/projectType 
        // returns all project types matching the given platform and language as JSON
        [HttpGet]
        public JsonResult GetProjectTypes(string platform, string language)
        {
            return Json(projectTypeStore);
        }

    }
}