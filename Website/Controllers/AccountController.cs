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
        Service.User.Manage manage = new Service.User.Manage();
        public AccountController()
        {
        }
        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.DisplayedCountryListData = manage.GetDisplayedCountryListData();

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
            //ViewBag.DisplayedCountryListData = manage.GetDisplayedCountryListData();
            //Validate password
            if (!manage.ValidatePassword(model.Password))
            {
                model.Message = Website.App_LocalResources.Error.InvalidPassword;
                return View(model);
            }

            if (ModelState.IsValid)
            {
                //Create the new password
                var crypto = new SimpleCrypto.PBKDF2();
                var encrypPass = crypto.Compute(model.Password);
                string ipAddress = string.Empty;
                try
                {
                    var client = new WebClient();
                    var content = client.DownloadString("http://api.ipinfodb.com/v3/ip-city/?key=" + System.Configuration.ConfigurationManager.AppSettings["ApiKey"] + "&ip=14.175.136.156&format=json");
                    Lib.Common.IPInfo info = JsonConvert.DeserializeObject<Lib.Common.IPInfo>(content);
                    ipAddress = info.ipAddress;
                }
                catch
                {
                    ipAddress = "UNKNOWN";
                }
                string ticket = Guid.NewGuid().ToString().Replace("-", "_");
                //string[] country = new Service.User.Manage().GetPhoneNumber(model.CountryCode, model.Phone);

                //Validation
                //Data.User user = manage.GetUserInfo(country[1]);
                Data.User user = manage.GetUserInfo(model.Phone);
                if (user.Phone != null)
                {
                    model.Message = Website.App_LocalResources.Error.DuplicatedPhone;
                    return View(model);
                }
                user = manage.GetUserByEmail(model.Email);
                if (user != null)
                {
                    model.Message = Website.App_LocalResources.Error.DuplicatedEmail;
                    return View(model);
                }

                user = new Data.User
                {
                    Phone = model.Phone,
                    Email = model.Email,
                    Password = encrypPass,
                    PasswordSalt = crypto.Salt,
                    CreatedDate = DateTime.Now,
                    IsActive = false,
                    IP = ipAddress,
                    StatusId = 1,
                    ActiveCode = ticket,
                    CountryCode = model.CountryCode
                };
                register.Create(user);
                if (register.Error == null)
                {
                    Lib.Common.Email email = new Lib.Common.Email();
                    if (email.SendEmail(new string[] { ConfigurationManager.AppSettings["EMAIL"] }
                                                , "Registration"
                                                , "<p>Customer with phone number '" + model.Phone + "' has registered a new account.</p>"
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
                    return RedirectToAction("GoToSuccessfulRegister");
                }
                else
                {
                    model.Message = GetLabel(register.Error);
                }
                return View(model);
            }

            return View(model);
        }
        [AllowAnonymous]
        public ActionResult GoToSuccessfulRegister()
        {
            return View();
        }
        private string GetLabel(string key)
        {
            switch (key)
            {
                case "DuplicatedPhone":
                    {
                        return Website.App_LocalResources.Error.DuplicatedPhone;
                    }
                case "DuplicatedEmail":
                    {
                        return Website.App_LocalResources.Error.DuplicatedEmail;
                    }
                case "InuseAccount":
                    {
                        return Website.App_LocalResources.Error.InuseAccount;
                    }
                case "LockedAccount":
                    {
                        return Website.App_LocalResources.Error.LockedAccount;
                    }
                case "ActiveAccount":
                    {
                        return Website.App_LocalResources.Error.ActiveAccount;
                    }
                case "RenewPassword":
                    {
                        return Website.App_LocalResources.Error.RenewPassword;
                    }
                case "InvalidAccount":
                    {
                        return Website.App_LocalResources.Error.InvalidAccount;
                    }

                default:
                    {
                        return Website.App_LocalResources.Error.PerformmingError;
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
                //model.Phone = new Service.User.Manage().GetPhoneNumber(model.CountryCode, model.Phone)[1];
                string session = Guid.NewGuid().ToString();
                string result = new Service.User.Manage().ValidateAccount(model.Phone, model.Password, session);

                if (result.Length == 0)
                {
                    Response.Cookies.Add(new HttpCookie(Lib.Common.Variables.SessionKey, session));
                    FormsAuthentication.SetAuthCookie(model.Phone, false);
                    return RedirectToAction("index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", GetLabel(result));
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
            user.Session = null;
            manage.UpdateUserInfo(user);

            Request.Cookies.Remove(Lib.Common.Variables.Fullname);
            Request.Cookies.Remove(Lib.Common.Variables.SessionKey);
            FormsAuthentication.SignOut();
 
            return RedirectToAction("Index", "Home");
        }
    }
}