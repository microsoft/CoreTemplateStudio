using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTemplateStudio.Api.Models
{
    public class ProjectTypeItem
    {   
        public long Id { get; set; }
        public string Name { get; set; }
        public string ShortProjectType { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; } = @"C:\Program File (x86)\VSCode\Extensions\Web Template Studio\Templates\Projects\";
    }
}
