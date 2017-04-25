using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineBankingSystem.Models;
using System.Web.Security;
using OnlineBankingSystem.UsersReference;

namespace OnlineBankingSystem.Controllers
{
    public class UserAuthenticationController : Controller
    {
        //
        // GET: /UserAuthentication/

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel data)
        {
            try
            {
                if (data.Username == null || data.Token == null)
                {
                    ViewData["error"] = "Please enter all information";
                }
                else if(new UsersReference.UsersServiceClient().Test(data.Username, data.Token) == true)
                {
                    //valid login
                    FormsAuthentication.RedirectFromLoginPage(data.Username, true);
                    new UsersServiceClient().UpdateToken(data.Username);
                }
                else
                {                //if invalid
                    ViewData["error"] = "Authentication failed";
                }
                
            }
            catch (HttpException e)
            {
                ViewData["error"] = "Authentication failed";
            
            }

            
            return View(data);
        }

        [HttpPost]
        public ActionResult LoginOut(LoginModel data)
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Account");
        }             
    }
}
