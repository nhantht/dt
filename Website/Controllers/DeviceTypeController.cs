using Admin.Models;
using Service.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website.Controllers
{
    public class DeviceTypeController : Controller
    {
        DeviceType obj = new DeviceType();
        // GET: Database
        public ActionResult Index()
        {
            return View(obj.GetList().OrderBy(x => x.Name));
        }

        // GET: Database/Details/5
        public ActionResult Details(decimal id)
        {

            Data.DeviceType DeviceType = obj.GetById(id);
            DeviceTypeDetailViewModel result = new DeviceTypeDetailViewModel();
            if (DeviceType != null)
            {
                result = new DeviceTypeDetailViewModel
                {
                    Id = DeviceType.Id,
                    Name = DeviceType.Name
                };
            }
            else
            {
                result.ErrorMessage = "The DeviceType was deleted!";
            }

            return View(result);
        }

        // GET: Database/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Database/Create
        [HttpPost]
        public ActionResult Create(DeviceTypeDetailViewModel input)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add insert logic here
                Data.DeviceType DeviceType = obj.Create(new Data.DeviceType
                {
                    Id = 0,
                    Name = input.Name
                });
                if (DeviceType != null)
                {
                    return RedirectToAction("Index");
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
        public ActionResult Edit(decimal id)
        {
            Data.DeviceType DeviceType = obj.GetById(id);
            DeviceTypeDetailViewModel result = new DeviceTypeDetailViewModel();
            if (DeviceType != null)
            {
                result = new DeviceTypeDetailViewModel
                {
                    Id = DeviceType.Id,
                    Name = DeviceType.Name
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
        public ActionResult Edit(DeviceTypeDetailViewModel input)
        {
            if (ModelState.IsValid)
            {
                //Search
                if (obj.GetById(input.Id) == null)
                {
                    input.ErrorMessage = "This DeviceType was deleted!";
                    return View(input);
                }
                // TODO: Add insert logic here
                Data.DeviceType DeviceType = obj.Update(new Data.DeviceType
                {
                    Id = input.Id,
                    Name = input.Name
                });
                if (DeviceType != null)
                {
                    return RedirectToAction("Index");
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
        public ActionResult Delete(decimal id)
        {
            Data.DeviceType DeviceType = obj.GetById(id);
            DeviceTypeDetailViewModel result = new DeviceTypeDetailViewModel();
            if (DeviceType != null)
            {
                result = new DeviceTypeDetailViewModel
                {
                    Id = DeviceType.Id,
                    Name = DeviceType.Name
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
        public ActionResult Delete(DeviceTypeDetailViewModel input)
        {
            // TODO: Add insert logic here
            bool result = obj.Delete(input.Id);
            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                input.ErrorMessage = "This is inuse!";
                return View(input);
            }
        }
    }
}
