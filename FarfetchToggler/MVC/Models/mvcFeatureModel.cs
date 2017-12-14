using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class mvcFeatureModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required")]
        public string FeatureType { get; set; }
        public bool FeatureFlag { get; set; }
        public string FeaturePermissions { get; set; }
        public List<string> FeaturePermissionsList { get; set; }
    }
}