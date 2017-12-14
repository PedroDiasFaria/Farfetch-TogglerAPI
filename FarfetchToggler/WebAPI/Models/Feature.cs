using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebAPI.Models
{
    public class Feature
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string FeatureType { get; set; }
        public bool FeatureFlag { get; set; }
        public string FeaturePermissions { get; set; }

        public List<string> FeaturePermissionsList { get; set; }
        private char stringDelimiter = ';';

        public Feature()
        {
        }

        public Feature(string name, string type, bool featureFlag, string permissions)
        {
            Name = name;
            FeatureType = type;
            FeatureFlag = featureFlag;
            FeaturePermissions = permissions;
            FeaturePermissionsList = DeconstructPermissions();
        }        

        public virtual bool Toggle()
        {
            //FeatureFlag = !FeatureFlag;
            this.GetType().GetProperty("FeatureFlag").SetValue(this, !FeatureFlag, null);
            return FeatureFlag;
        }

        public void SetPermission(bool toggle, string userType)
        {
            List<string> permissionsList = DeconstructPermissions();

            if (permissionsList.Contains(userType) && !toggle)
            {
                permissionsList.Remove(userType);
            }
            else
            {
                permissionsList.Add(userType);
            }

            this.GetType().GetProperty("FeaturePermissions").SetValue(this, permissionsList.Aggregate((i, j) => i + stringDelimiter + j), null);
        }

        public List<string> DeconstructPermissions()
        {
            List<string> permissionsList = FeaturePermissions.Split(stringDelimiter).ToList();

            return permissionsList;
        }

        public bool HasPermision(string userType)
        {
            return FeaturePermissionsList.Contains(userType);
        }
    }
}