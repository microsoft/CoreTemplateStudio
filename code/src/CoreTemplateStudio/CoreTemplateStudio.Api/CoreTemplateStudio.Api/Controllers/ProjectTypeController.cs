using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreTemplateStudio.Api.Models;

namespace CoreTemplateStudio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectTypeController : Controller
    {

        private readonly ProjectTypeContext _context;

        public ProjectTypeController(ProjectTypeContext context)
        {
            _context = context;

            _context.ProjectTypeItems.Add(new ProjectTypeItem { Name = "Single Page Full Stack Application", ShortProjectType = "SPAFS", Description = "A single page web application with a connected backend"});
            _context.ProjectTypeItems.Add(new ProjectTypeItem { Name = "Multi Page Full Stack Application", ShortProjectType = "MPAFS", Description = "A multi page web application with a connected backend" });
            _context.ProjectTypeItems.Add(new ProjectTypeItem { Name = "Single Page Frontend", ShortProjectType = "SPFE", Description = "A single page frontend only application" });
            _context.ProjectTypeItems.Add(new ProjectTypeItem { Name = "Multi Page Frontend", ShortProjectType = "MPFE", Description = "A multi page frontend only application" });
            _context.ProjectTypeItems.Add(new ProjectTypeItem { Name = "RESTFul API", ShortProjectType = "REST", Description = "A REST Application programming interface backend only" });

            _context.SaveChanges();
        }

        // GET: api/projectType 
        // returns all project types matching the given platform and language as JSON
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectTypeItem>>> GetProjectTypes([FromQuery(Name = "platform")] string platform, [FromQuery(Name = "language")] string language)
        {
            return await _context.ProjectTypeItems.ToListAsync();
        }

    }
}