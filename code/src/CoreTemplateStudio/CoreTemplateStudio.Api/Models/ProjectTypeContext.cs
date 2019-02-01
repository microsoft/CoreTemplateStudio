using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CoreTemplateStudio.Api.Models
{
    public class ProjectTypeContext: DbContext
    {
        public ProjectTypeContext(DbContextOptions<ProjectTypeContext> options) : base(options)
        {
        }

        public DbSet<ProjectTypeItem> ProjectTypeItems { get; set;}
    }
}
