using Admin.Models;
using Service.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website.Controllers
{
    public class TimeFrameController : Controller
    {
        TimeFrame obj = new TimeFrame();
        // GET: Database
        public ActionResult Index(bool fromChild = false)
        {
            ViewBag.FromChild = fromChild;
        
            return View(GetList());
        }
        IEnumerable<TimeFrameDetailViewModel> GetList()
        {
            return from x in obj.GetList()
                   orderby x.Name
                   select new TimeFrameDetailViewModel
                   {
                       Id = x.Id,
                       Name = x.Name,
                       FromHour = x.FromHour,
                       ToHour = x.ToHour
                   };
        }
        // GET: Database/Details/5
        public ActionResult Details(decimal id, bool fromChild = false)
        {

            Data.TimeFrame TimeFrame = obj.GetById(id);
            TimeFrameDetailViewModel result = new TimeFrameDetailViewModel();
            if (TimeFrame != null)
            {
                result = new TimeFrameDetailViewModel
                {
                    Id = TimeFrame.Id,
                    Name = TimeFrame.Name,
                    FromHour = TimeFrame.FromHour,
                    ToHour = TimeFrame.ToHour,
                    FromChild = fromChild
                };
            }
            else
            {
                result.ErrorMessage = "The TimeFrame was deleted!";
            }

            return View(result);
        }

        // GET: Database/Create
        public ActionResult Create(bool fromChild = false)
        {
            return View(new TimeFrameDetailViewModel()
            {
                FromChild = fromChild
            });
        }

        // POST: Database/Create
        [HttpPost]
        public ActionResult Create(TimeFrameDetailViewModel input)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add insert logic here
                Data.TimeFrame TimeFrame = obj.Create(new Data.TimeFrame
                {
                    Id = 0,
                    Name = input.Name,
                    FromHour = input.FromHour,
                    ToHour = input.ToHour
                });
                if (TimeFrame != null)
                {
                    ViewBag.FromChild = input.FromChild;

                    return View("Index", GetList());
                }
                else
                {
                    input.ErrorMessage = "Error on Creating!";
                    return View(input);
                }
            }
            else
            {
                return View(input);
            }
        }

        // GET: Database/Edit/5
        public ActionResult Edit(decimal id, bool fromChild = false)
        {
            Data.TimeFrame TimeFrame = obj.GetById(id);
            TimeFrameDetailViewModel result = new TimeFrameDetailViewModel();
            if (TimeFrame != null)
            {
                result = new TimeFrameDetailViewModel
                {
                    Id = TimeFrame.Id,
                    Name = TimeFrame.Name,
                    FromHour = TimeFrame.FromHour,
                    ToHour = TimeFrame.ToHour,
                    FromChild = fromChild
                };
            }
            else
            {
                result.ErrorMessage = "Error on Updating!";
            }

            return View(result);
        }

        // POST: Database/Edit/5
        [HttpPost]
        public ActionResult Edit(TimeFrameDetailViewModel input)
        {
            if (ModelState.IsValid)
            {
                //Search
                if (obj.GetById(input.Id) == null)
                {
                    input.ErrorMessage = "This TimeFrame was deleted!";
                    return View(input);
                }
                // TODO: Add insert logic here
                Data.TimeFrame TimeFrame = obj.Update(new Data.TimeFrame
                {
                    Id = input.Id,
                    Name = input.Name,
                    FromHour = input.FromHour,
                    ToHour = input.ToHour
                });
                if (TimeFrame != null)
                {
                    ViewBag.FromChild = input.FromChild;
                    return View("Index", GetList());
                }
                else
                {
                    input.ErrorMessage = "Error on Updating.";
                    return View(input);
                }
            }
            else
            {
                return View(input);
            }
        }

        // GET: Database/Delete/5
        public ActionResult Delete(decimal id, bool fromChild = false)
        {
            Data.TimeFrame TimeFrame = obj.GetById(id);
            TimeFrameDetailViewModel result = new TimeFrameDetailViewModel();
            if (TimeFrame != null)
            {
                result = new TimeFrameDetailViewModel
                {
                    Id = TimeFrame.Id,
                    Name = TimeFrame.Name,
                    FromHour = TimeFrame.FromHour,
                    ToHour = TimeFrame.ToHour,
                    FromChild = fromChild
                };
            }
            else
            {
                result.ErrorMessage = "Error on Deleting!";
            }

            return View(result);
        }

        // POST: Database/Delete/5
        [HttpPost]
        public ActionResult Delete(TimeFrameDetailViewModel input)
        {
            // TODO: Add insert logic here
            bool result = obj.Delete(input.Id);
            if (result)
            {
                ViewBag.FromChild = input.FromChild;
                return View("Index", GetList());
            }
            else
            {
                input.ErrorMessage = "This is inuse!";
                return View(input);
            }
        }
    }
}
