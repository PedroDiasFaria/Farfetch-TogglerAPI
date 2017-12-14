using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class FeatureController : Controller
    {
        public ActionResult EditFeatures()
        {
            IEnumerable<mvcFeatureModel> featuresList;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Feature").Result;
            featuresList = response.Content.ReadAsAsync<IEnumerable<mvcFeatureModel>>().Result;
            return View(featuresList);
        }

        public ActionResult ToggleFeature(int featureId = 0)
        {
            if (featureId == 0)
                return RedirectToAction("EditFeatures");
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Feature/ToggleFeature/" + featureId.ToString()).Result;
                return RedirectToAction("EditFeatures");
            }
        }

        public ActionResult ViewFeatures(string userType = null)
        {
            IEnumerable<mvcFeatureModel> featuresList;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Feature/GetUserFeatures/" + userType).Result;
            featuresList = response.Content.ReadAsAsync<IEnumerable<mvcFeatureModel>>().Result;
            ViewBag.userType = userType;
            return View(featuresList);
        }
    }
}
