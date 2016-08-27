using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Service;
using Admin.Models;
using System.Web.Security;
using System.Net;
using Newtonsoft.Json;
using System.Configuration;

namespace Admin.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private Service.User.Register register = new Service.User.Register();
        public AccountController()
        {
        }
        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View(new Models.RegisterViewModel());
        }
        //
        // GET: /Account/Register
        [AllowAnonymous]
        public void Active(string id)
        {
            if (register.Active(id))
            {
                Response.Redirect(Lib.Common.Application.ApplicationPath() + "/account/login");
            }
            else
            {
                Response.Redirect(Lib.Common.Application.ApplicationPath() + "/home/index");
            }
        }
        
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(Models.RegisterViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var crypto = new SimpleCrypto.PBKDF2();
                var encrypPass = crypto.Compute(model.Password);
                var client = new WebClient();
                var content = client.DownloadString("http://api.ipinfodb.com/v3/ip-city/?key=" + System.Configuration.ConfigurationManager.AppSettings["ApiKey"] + "&ip=14.175.136.156&format=json");
                Lib.Common.IPInfo info = JsonConvert.DeserializeObject<Lib.Common.IPInfo>(content);
                string ticket = Guid.NewGuid().ToString().Replace("-", "_");

                decimal id = register.CheckAndAddRegion(info.cityName, info.countryCode, info.countryName);
                string telephoneCode = register.GetTelephoneCode(info.countryCode);

                //Trim phone number
                while (model.Phone.StartsWith("+"))
                {
                    model.Phone = model.Phone.Substring(1);
                }
                while (model.Phone.StartsWith("0"))
                {
                    model.Phone = model.Phone.Substring(1);
                }

                Data.User user = new Data.User
                {
                    Phone = string.Format("+{0}{1}", telephoneCode, model.Phone),
                    Email = model.Email,
                    Password = encrypPass,
                    PasswordSalt = crypto.Salt,
                    CreatedDate = DateTime.Now,
                    IsActive = false,
                    IP = info.ipAddress,
                    StatusId = 1,
                    ActiveCode = ticket
                };
                register.Create(user);
                if (register.Error == null)
                {
                    Lib.Common.Email email = new Lib.Common.Email();
                    if (email.SendEmail(new string[] { ConfigurationManager.AppSettings["EMAIL"] }
                                                , "Registration"
                                                , "<p>Customer '" + model.Fullname + "' has registered a new account.</p>"
                                                    + "<p>Please click the <a href='" + Lib.Common.Application.ApplicationPath() + "/account/active/" + ticket + "' target='_blank'>link</a> to active your account.</p>"
                                                    + "<br><p><b>DT System</b></p>"
                                                ))
                    {
                        email.SendEmail(new string[] { model.Email }
                                        , "Registration"
                                        , "<p>Thank for your concern to DT system. Please click the <a href='" + Lib.Common.Application.ApplicationPath() + "/account/active/" + ticket + "' target='_blank'>link</a> to active your account.</p>"
                                            + "<br><p><b>DT System</b></p>"
                                        );
                    }

                    model.Message = "Your registration is successful now. Please check your email to active it.";
                }
                else
                {
                    model.Message = GetLabel(register.Error);
                }
                return View(model);
            }

            return View(model);
        }
        private string GetLabel(string key)
        {
            switch (key)
            {
                case "DuplicatedPhone":
                    {
                        return Admin.App_LocalResources.Error.DuplicatedPhone;
                    }
                default:
                    {
                        return Admin.App_LocalResources.Error.PerformmingError;
                    }
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string id, string returnUrl)
        {
            if (string.IsNullOrEmpty(id) == false)
            {
                Service.User.Manage manage = new Service.User.Manage();
                Data.User user = manage.GetUserByRegetpasswordCode(id);
                if (user != null && user.StatusId == 3)
                {
                    user.StatusId = 4;
                    manage.UpdateUserInfo(user);
                }
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string result = new Service.User.Manage().ValidateAccount(model.Phone, model.Password);
                if (result.Length == 0)
                {
                    FormsAuthentication.SetAuthCookie(model.Phone, false);
                    return RedirectToAction("index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", result);
                }
            }
            else
            {
                ModelState.AddModelError("", "Data is not correct");
            }
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult RegisterConditions()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Privacy()
        {
            return View();
        }
        public ActionResult LogOff()
        {
            Service.User.Manage manage = new Service.User.Manage();
            Data.User user = manage.GetUserInfo(User.Identity.Name);
            user.Logined = false;
            manage.UpdateUserInfo(user);

            Request.Cookies.Remove(Lib.Common.Variables.Fullname);
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}