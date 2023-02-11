using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraGen.Models
{
    public class ResourceConfig
    {
        [Required]
        public string ProjectName { get; set; }
        public GitServer GitServer { get; set; }
        public bool UseTemplate { get; set; }
        public string LocalProjectPath { get; set; }
    }
}
