using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class mvcFeatureModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string FeatureType { get; set; }
        public bool FeatureFlag { get; set; }
        public string FeaturePermissions { get; set; }
        public List<string> FeaturePermissionsList { get; set; }
    }
}