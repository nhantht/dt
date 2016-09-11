using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Admin.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Security;
using PagedList;
using Newtonsoft.Json;

namespace Admin.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        Service.User.Manage manage = new Service.User.Manage();
        Service.Region.CityObj city = new Service.Region.CityObj();
        Service.Region.CountryObj country = new Service.Region.CountryObj();

        const string dateValueFormat = "MM/dd/yyyy";
        public ManageController()
        {
        }
        //
        // GET: /Manage/Index
        public ActionResult Index(string id, string task)
        {
            id = Lib.Common.Application.UnescapePhone(id);
            if (string.IsNullOrEmpty(id))
            {
                id = User.Identity.Name;
            }
            Data.User user = manage.GetUserInfo(id);
            if (string.IsNullOrEmpty(task) == false && task == "DELETE")
            {
                ViewBag.Title = "Delete User";
                ViewBag.Subtitle = "Would you like to delete the user?";
                ViewBag.Task = "DELETE";
            }
            else
            {
                ViewBag.Title = "User Management";
                ViewBag.Subtitle = "Detail Information";
                ViewBag.Task = string.Empty;
            }

            return View(new UserInfoViewModel
                        {
                            Phone = user.Phone,
                            Email = user.Email,
                            IsActive = user.IsActive,
                            StatusName = user.Status.Description,
                            Points = user.Points,
                            CreatedDate = user.CreatedDate,
                            FromDate = user.InActiveFrom,
                            ToDate = user.InActiveTo,
                            Session = user.Session
                        });
        }
        [Authorize(Users = Lib.Common.Variables.AdminUser)]
        public ActionResult ResetPassword(string id)
        {
            var crypto = new SimpleCrypto.PBKDF2();
            var newPass = Guid.NewGuid().ToString();
            var encrypPass = crypto.Compute(newPass);
            string message = string.Empty;
            if (manage.ChangePassword(id, encrypPass, crypto.Salt))
            {
                //Send email
                Lib.Common.Email email = new Lib.Common.Email();
                if (email.SendEmail(new string[] { ConfigurationManager.AppSettings["EMAIL"] }
                                            , "Reset Password"
                                            , "<p>The customer' password with the phone number '" + id + "' has just reset to '" + newPass + "'.</p>"
                                                + "<br><p><b>DT System</b></p>"
                                            ))
                {
                    email.SendEmail(new string[] { manage.GetUserInfo(id).Email }
                                    , "Reset Password"
                                    , "<p>Thank for your concern to DT system. Your password has just reset to '" + newPass + "', please click the <a href='" + Lib.Common.Application.ApplicationPath() + "/account/login/" + "' target='_blank'>link</a> to access the system.</p>"
                                        + "<br><p><b>DT System</b></p>"
                                    );
                }

                message = "This user' password has just been reset.";
            }
            else
            {
                message = "There are some errors, please recheck your system.";
            }
            return Json(new { result = message }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Users = Lib.Common.Variables.AdminUser)]
        public ActionResult Delete(string id)
        {
            id = Lib.Common.Application.UnescapePhone(id);

            if (manage.Delete(id))
            {
                return RedirectToAction("list");
            }
            else
            {
                ViewBag.StatusMessage = "This user created some transactions in the system, so you disable him/her instead of.";
                return Redirect(Request.Url.AbsolutePath);
            }
        }
        [Authorize]
        public ActionResult Update(string id)
        {
            id = Lib.Common.Application.UnescapePhone(id);
            if (string.IsNullOrEmpty(id))
            {
                id = User.Identity.Name;
            }
            Data.User user = manage.GetUserInfo(id);

            ViewBag.FromDate = ViewBag.ToDate = string.Empty;

            if (user.InActiveFrom == null)
            {
                user.InActiveFrom = DateTime.Now;
            }
            if (user.InActiveTo == null)
            {
                user.InActiveTo = DateTime.Now.AddYears(1);
            }

            ViewBag.FromDate = DateTime.Parse(user.InActiveFrom.ToString()).ToString(dateValueFormat);
            ViewBag.ToDate = DateTime.Parse(user.InActiveTo.ToString()).ToString(dateValueFormat);

            return View(new UpdateAccountViewModel
            {
                Phone = user.Phone,
                Email = user.Email,
                IsActive = user.IsActive,
                InActiveFrom = user.InActiveFrom,
                InActiveTo = user.InActiveTo,
                StatusName = user.Status.Description,
                Session = user.Session
            });
        }
        [Authorize]
        public ActionResult History(string currentFilter, int page = 1)
        {
            //Get filter data
            if (currentFilter == null)
            {
                currentFilter = string.Empty;
            }
            string[] parts = System.Uri.UnescapeDataString(currentFilter).Split(new string[] { "###" }, StringSplitOptions.None);
            string actionId = "0";
            string fromDate = string.Empty;
            string toDate = string.Empty;
            string relativeUser = string.Empty;
            if (parts.Length == 4)
            {
                //Set filters
                actionId = parts[0].Trim();
                fromDate = parts[1].Trim();
                toDate = parts[2].Trim();
                relativeUser = parts[3].Trim();
            }
            //Set viewbags
            ViewBag.ActionId = actionId;
            ViewBag.RelativeUser = relativeUser = string.Format("+{0}", relativeUser);
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;

            DateTime fromDate2 = DateTime.Parse(string.Format("{0} 00:00:00", fromDate));
            DateTime toDate2 = DateTime.Parse(string.Format("{0} 23:59:59", toDate));

            IEnumerable<HistoryViewModel> result = from s in manage.GetAllHistories()
                                                   where (short.Parse(actionId) == 0 || s.ActionId == short.Parse(actionId))
                                                      && (string.IsNullOrEmpty(fromDate) || s.NotedDate >= fromDate2)
                                                      && (string.IsNullOrEmpty(toDate) || s.NotedDate <= toDate2)
                                                      && (relativeUser == "+0" || s.CreatedUser == relativeUser)
                                                   select new HistoryViewModel
                                                      {
                                                          ActionId = s.ActionId,
                                                          ActionName = s.ActionList.Name,
                                                          NotedDate = s.NotedDate,
                                                          Description = s.Description,
                                                          RelativeUser = s.CreatedUser,
                                                          Object = s.Object
                                                      };


            int pageSize = 10;
            ViewBag.CurrentFilter = Uri.EscapeDataString(string.Format("{0}###{1}###{2}###{3}", actionId, fromDate, toDate, relativeUser));

            return View(result.ToPagedList(page, pageSize));
        }
        [Authorize]
        public ActionResult EmailAlerts(string currentFilter, int page = 1)
        {
            //Get filter data
            if (currentFilter == null)
            {
                currentFilter = string.Empty;
            }
            string[] parts = System.Uri.UnescapeDataString(currentFilter).Split(new string[] { "###" }, StringSplitOptions.None);
            string actionId = "0";
            string fromDate = string.Empty;
            string toDate = string.Empty;
            string relativeUser = string.Empty;
            if (parts.Length == 4)
            {
                //Set filters
                actionId = parts[0].Trim();
                fromDate = parts[1].Trim();
                toDate = parts[2].Trim();
                relativeUser = parts[3].Trim();
            }
            //Set viewbags
            ViewBag.ActionId = actionId;
            ViewBag.RelativeUser = relativeUser = string.Format("+{0}", relativeUser);
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;

            DateTime fromDate2 = DateTime.Parse(string.Format("{0} 00:00:00", fromDate));
            DateTime toDate2 = DateTime.Parse(string.Format("{0} 23:59:59", toDate));

            IEnumerable<EmailViewModel> result = from s in manage.GetAllEmailAlerts()
                                                 where (short.Parse(actionId) == 0 || s.ActionId == short.Parse(actionId))
                                                    && (string.IsNullOrEmpty(fromDate) || s.NotedDate >= fromDate2)
                                                    && (string.IsNullOrEmpty(toDate) || s.NotedDate <= toDate2)
                                                    && (relativeUser == "+0" || s.RelativeUser == relativeUser)
                                                 select new EmailViewModel
                                                   {
                                                       Id = s.Id,
                                                       ActionId = s.ActionId,
                                                       ActionName = s.ActionList.Name,
                                                       NotedDate = s.NotedDate,
                                                       Title = s.Title,
                                                       ToEmail = s.ToEmail,
                                                       RelativeUser = s.RelativeUser,
                                                       Message = s.Message
                                                   };


            int pageSize = 10;
            ViewBag.CurrentFilter = Uri.EscapeDataString(string.Format("{0}###{1}###{2}###{3}", actionId, fromDate, toDate, relativeUser));

            return View(result.ToPagedList(page, pageSize));
        }
        [Authorize]
        [HttpPost]
        public ActionResult Update(UpdateAccountViewModel model)
        {
            Data.User user = new Data.User();
            user.Phone = model.Phone;
            user.Email = model.Email;
            user.IsActive = model.IsActive;
            user.Session = null;

            if (model.IsActive)
            {
                user.InActiveFrom = null;
                user.InActiveTo = null;
            }
            else
            {
                user.InActiveFrom = model.InActiveFrom;
                user.InActiveTo = model.InActiveTo;
            }
            if (!manage.UpdateUserInfo(user))
            {
                model.Message = "Email is duplicated!";
            }
            else
            {
                //model.Message = "Successful!";
                return Redirect(Request.Url.AbsoluteUri.ToString().ToLower().Replace("update", "Index"));
            }

            if (user.InActiveFrom == null)
            {
                user.InActiveFrom = DateTime.Now;
            }
            if (user.InActiveTo == null)
            {
                user.InActiveTo = DateTime.Now.AddYears(1);
            }

            ViewBag.FromDate = DateTime.Parse(user.InActiveFrom.ToString()).ToString(dateValueFormat);
            ViewBag.ToDate = DateTime.Parse(user.InActiveTo.ToString()).ToString(dateValueFormat);

            return View(model);
        }
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            ViewBag.DisplayedCountryListData = manage.GetDisplayedCountryListData();
            return View(new ForgotPasswordViewModel());
        }
        [AllowAnonymous]
        public ActionResult GoToForgotPasswordMessage()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult GoToForgotPasswordMessage2()
        {
            return View();
        }
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(Models.ForgotPasswordViewModel model)
        {
            ViewBag.DisplayedCountryListData = manage.GetDisplayedCountryListData();
            if (ModelState.IsValid)
            {
                //Create regetpasswordcode
                //model.Phone = manage.GetPhoneNumber(model.CountryCode, model.Phone)[1];
                Data.User user = manage.GetUserInfo(model.Phone);
                if (user.Phone == null)
                {
                    model.Message = Website.App_LocalResources.Error.NotExistPhone;
                    return View(model);
                }
                user.RegetPasswordCode = Guid.NewGuid().ToString();
                user.StatusId = 3;
                if (manage.UpdateUserInfo(user))
                {
                    Lib.Common.Email email = new Lib.Common.Email();
                    try
                    {
                        if (email.SendEmail(new string[] { ConfigurationManager.AppSettings["EMAIL"] }
                                                    , "Forget Password"
                                                    , "<p>The user with Phone: " + user.Phone + " and email " + user.Email + " forgot his/her password.<p>"
                                                        + "<p>Please click the <a href='" + Lib.Common.Application.ApplicationPath() + "/manage/changepassword2/" + user.RegetPasswordCode + "' target='_blank'>link</a> to create a new password.</p>"
                                                        + "<br><p><b>DT System</b></p>"
                                                    ))
                        {
                            if (email.SendEmail(new string[] { user.Email }
                                            , "Forget Password"
                                            , "<p>Please click the <a href='" + Lib.Common.Application.ApplicationPath() + "/manage/changepassword2/" + user.RegetPasswordCode + "' target='_blank'>link</a> to create a new password.</p>"
                                              + "<br><p><b>DT System</b></p>"
                                            ))
                            {
                                return RedirectToAction("GoToForgotPasswordMessage");
                            }
                        }
                    }
                    catch
                    {
                        model.Message = "Email system is off. Please contact the administrator.";
                        return View(model);
                    }
                }
                else
                {
                    model.Message = "Cannot perform this action. Please contact the administrator.";
                    return View(model);
                }

                return View(model);
            }

            return View(model);
        }
        //
        // GET: /Manage/ChangePassword
        [Authorize]
        public ActionResult ChangePassword(string id)
        {
            return View(new ChangePasswordViewModel());
        }
        //
        // GET: /Manage/ChangePassword
        [AllowAnonymous]
        public ActionResult ChangePassword2(string id)
        {
            Data.User user = manage.GetUserByRegetpasswordCode(id);
            if (user == null)
            {
                return RedirectToAction("index", "home");
            }
            return View(new ChangePasswordViewModel2
            {
                Phone = user.Phone
            });
        }
        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (!manage.ValidatePassword(model.NewPassword))
                {
                    model.Message = Website.App_LocalResources.Error.InvalidPassword;
                }
                return View(model);
            }
            string result = manage.ValidateAccount(User.Identity.Name, model.OldPassword, null);
            if (result.Length > 0)
            {
                model.Message = "The current password is wrong!";
                return View(model);
            }
            if (model.OldPassword == model.NewPassword)
            {
                model.Message = "Please input another for the new password!";
                return View(model);
            }

            var crypto = new SimpleCrypto.PBKDF2();
            var encrypPass = crypto.Compute(model.NewPassword);

            if (manage.ChangePassword(User.Identity.Name, encrypPass, crypto.Salt))
            {
                Response.Redirect(Lib.Common.Application.ApplicationPath() + "/home/index");
            }

            return View(model);
        }
        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ChangePassword2(ChangePasswordViewModel2 model)
        {
            if (!manage.ValidatePassword(model.NewPassword))
            {
                model.Message = Website.App_LocalResources.Error.InvalidPassword;
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var crypto = new SimpleCrypto.PBKDF2();
            var encrypPass = crypto.Compute(model.NewPassword);
            if (manage.ChangePassword(model.Phone, encrypPass, crypto.Salt))
            {
                return View("GoToForgotPasswordMessage2");
            }

            return View(model);
        }
        [Authorize(Users = Lib.Common.Variables.AdminUser)]
        public ActionResult List(string sortOrder, string currentFilter, int page = 1)
        {
            //Get filter data
            if (currentFilter == null)
            {
                currentFilter = string.Empty;
            }
            string[] parts = System.Uri.UnescapeDataString(currentFilter).Split(new string[] { "###" }, StringSplitOptions.None);
            string statusId = "0";
            string email = string.Empty;
            string fullname = string.Empty;
            bool isActive = true;
            //Set filters
            if (parts.Length == 4)
            {
                statusId = parts[2].Trim();
                email = parts[0].Trim();
                fullname = parts[1].Trim();
                isActive = bool.Parse(parts[3].Trim());
            }
            //Set viewbags
            ViewBag.Email = email;
            ViewBag.Fullname = fullname;
            ViewBag.IsActive = isActive;

            List<SelectListItem> tempStatuses = (from s in manage.GetAllStatuses()
                                                 select new SelectListItem
                                                 {
                                                     Text = s.Description,
                                                     Value = s.StatusId.ToString(),
                                                     Selected = s.StatusId.ToString() == statusId
                                                 }).ToList();

            tempStatuses.Add(new SelectListItem
            {
                Text = "---Select a status---",
                Value = "0",
                Selected = "0" == statusId
            });

            ViewBag.Statuses = tempStatuses.AsEnumerable<SelectListItem>().OrderBy(s => int.Parse(s.Value));

            sortOrder = String.IsNullOrEmpty(sortOrder) ? "createddate_desc" : sortOrder;
            ViewBag.CreatedDateSortParm = sortOrder == "createddate" ? "createddate_desc" : "createddate";
            ViewBag.EmailSortParm = sortOrder == "email" ? "email_desc" : "email";
            ViewBag.PhoneSortParm = sortOrder == "phone" ? "phone_desc" : "phone";
            ViewBag.StatusSortParm = sortOrder == "status" ? "status_desc" : "status";
            ViewBag.CitySortParm = sortOrder == "city" ? "city_desc" : "city";
            ViewBag.CountrySortParm = sortOrder == "country" ? "country_desc" : "country";
            ViewBag.PointSortParm = sortOrder == "point" ? "point_desc" : "point";

            IEnumerable<AccoutListViewModel> result = from s in manage.GetAllUsers()
                                                      where (int.Parse(statusId) == 0 || s.StatusId == int.Parse(statusId))
                                                      && (email.Length == 0 || s.Email == email)
                                                      && s.IsActive == isActive
                                                      select new AccoutListViewModel
                                                      {
                                                          Phone = s.Phone,
                                                          Email = s.Email,
                                                          IsActive = s.IsActive,
                                                          InActiveFrom = s.InActiveFrom,
                                                          InActiveTo = s.InActiveTo,
                                                          StatusId = s.StatusId,
                                                          Status = s.Status.Description,
                                                          CreatedDate = s.CreatedDate,
                                                      };

            //Order by
            switch (sortOrder)
            {
                case "createddate_desc":
                    result = result.OrderByDescending(s => s.CreatedDate);
                    break;
                case "createddate":
                    result = result.OrderBy(s => s.CreatedDate);
                    break;
                case "fullname_desc":
                    result = result.OrderByDescending(s => s.Fullname);
                    break;
                case "fullname":
                    result = result.OrderBy(s => s.Fullname);
                    break;
                case "email_desc":
                    result = result.OrderByDescending(s => s.Email);
                    break;
                case "email":
                    result = result.OrderBy(s => s.Email);
                    break;
                case "phone_desc":
                    result = result.OrderByDescending(s => s.Phone);
                    break;
                case "phone":
                    result = result.OrderBy(s => s.Phone);
                    break;
                case "status_desc":
                    result = result.OrderByDescending(s => s.Status);
                    break;
                case "status":
                    result = result.OrderBy(s => s.Status);
                    break;
                case "city_desc":
                    result = result.OrderByDescending(s => s.City);
                    break;
                case "city":
                    result = result.OrderBy(s => s.City);
                    break;
                case "country_desc":
                    result = result.OrderByDescending(s => s.Country);
                    break;
                case "country":
                    result = result.OrderBy(s => s.Country);
                    break;
                case "point_desc":
                    result = result.OrderByDescending(s => s.Points);
                    break;
                case "point":
                    result = result.OrderBy(s => s.Points);
                    break;
            }

            int pageSize = 10;
            ViewBag.CurrentFilter = Uri.EscapeDataString(string.Format("{0}###{1}###{2}###{3}", email, fullname, statusId, isActive.ToString()));
            ViewBag.CurrentSort = sortOrder;

            return View(result.ToPagedList(page, pageSize));
        }
        public ActionResult GetData(string id)
        {
            object result = null;
            Service.User.Manage manage = new Service.User.Manage();

            switch (id.Trim().ToLower())
            {
                case "users":
                    {
                        result = manage.GetColumns("users");
                        break;
                    }
            }
            return Json(new { data = JsonConvert.SerializeObject(result) }, JsonRequestBehavior.AllowGet);
        }
    }
}