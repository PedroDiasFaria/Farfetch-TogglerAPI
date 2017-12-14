using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            IEnumerable<mvcUserModel> userList;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("User").Result;
            userList = response.Content.ReadAsAsync<IEnumerable<mvcUserModel>>().Result;
            return View(userList);
        }

        public ActionResult AddOrEdit(int id = 0)
        {
            if(id == 0)
                return View(new mvcUserModel());
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("User/"+id.ToString()).Result;
                return View(response.Content.ReadAsAsync<mvcUserModel>().Result);
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(mvcUserModel user)
        {
            if(user.Id == 0)
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("User", user).Result;
                TempData["SuccessMessage"] = "Saved w/ Success";
            }
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("User/"+user.Id, user).Result;
                TempData["SuccessMessage"] = "Updated w/ Success";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("User/"+id.ToString()).Result;
            TempData["SuccessMessage"] = "Deleted w/ Success";
            return RedirectToAction("Index");
        }
    }
}