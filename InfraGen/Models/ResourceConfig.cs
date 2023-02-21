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

        [Required]
        public GitServer GitServer { get; set; }

        [Required]
        public bool UseTemplate { get; set; }

        [Required]
        public string LocalProjectPath { get; set; }
    }
}
