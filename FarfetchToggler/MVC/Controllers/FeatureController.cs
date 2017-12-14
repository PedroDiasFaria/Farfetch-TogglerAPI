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
        /// <summary>
        /// Opens the list of features window
        /// </summary>
        /// <returns></returns>
        public ActionResult EditFeatures()
        {
            IEnumerable<mvcFeatureModel> featuresList;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Feature").Result;
            featuresList = response.Content.ReadAsAsync<IEnumerable<mvcFeatureModel>>().Result;
            return View(featuresList);
        }

        /// <summary>
        /// Toggles a feature on/off
        /// </summary>
        /// <param name="featureId"></param>
        /// <returns></returns>
        public ActionResult ToggleFeature(int featureId = 0)
        {
            if (featureId == 0)
                return RedirectToAction("EditFeatures");
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsync("Feature/ToggleFeature/" + featureId.ToString(), null).Result;
                return RedirectToAction("EditFeatures");
            }
        }

        /// <summary>
        /// Returns the features permited to an user
        /// </summary>
        /// <param name="userType"></param>
        /// <returns></returns>
        public ActionResult ViewFeatures(string userType = null, string userName = null)
        {
            IEnumerable<mvcFeatureModel> featuresList;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Feature/GetUserFeatures/" + userType).Result;
            featuresList = response.Content.ReadAsAsync<IEnumerable<mvcFeatureModel>>().Result;
            ViewBag.userType = userType;
            ViewBag.userName = userName;
            return View(featuresList);
        }

        /// <summary>
        /// Opens the new/edit feature window
        /// </summary>
        /// <param name="featureId"></param>
        /// <returns></returns>
        public ActionResult AddOrEditFeature(int featureId = 0)
        {
            if (featureId == 0)
                return View(new mvcFeatureModel());
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Feature/" + featureId.ToString()).Result;
                return View(response.Content.ReadAsAsync<mvcFeatureModel>().Result);
            }
        }

        /// <summary>
        /// Creates or Edits an existing feature
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddOrEditFeature(mvcFeatureModel feature)
        {
            if (feature.Id == 0)
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Feature", feature).Result;
                TempData["SuccessMessage"] = "Created w/ Success";
            }
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("Feature/" + feature.Id, feature).Result;
                TempData["SuccessMessage"] = "Updated w/ Success";
            }
            return RedirectToAction("EditFeatures");
        }

        /// <summary>
        /// Deleates a feature
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Feature/" + id.ToString()).Result;
            TempData["SuccessMessage"] = "Deleted w/ Success";
            return RedirectToAction("EditFeatures");
        }
    }
}
