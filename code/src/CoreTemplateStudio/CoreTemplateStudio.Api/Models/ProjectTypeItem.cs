using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreTemplateStudio.Api.Enumerables;

namespace CoreTemplateStudio.Api.Models
{
    public class ProjectTypeItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; } = @"C:\Some\Dummy\Path";

        public ProjectTypeItem(ShortProjectType type)
        {
            this.Name = EnumerablesHelper.GetDisplayName(type);
            this.Description = EnumerablesHelper.GetDescription(type);
        }
    }
}
