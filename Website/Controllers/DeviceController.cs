using Admin.Models;
using Service.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website.Controllers
{
    public class DeviceController : Controller
    {
        Device obj = new Device();
        // GET: Database
        public ActionResult Index()
        {
            return View(obj.GetList().OrderBy(x => x.Name));
        }

        // GET: Database/Details/5
        public ActionResult Details(decimal id)
        {

            Data.Device Device = obj.GetByDeviceId(id);
            DeviceDetailViewModel result = new DeviceDetailViewModel();
            if (Device != null)
            {
                result = new DeviceDetailViewModel
                {
                    DeviceId = Device.DeviceId,
                    Name = Device.Name
                };
            }
            else
            {
                result.ErrorMessage = "The Device was deleted!";
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
        public ActionResult Create(DeviceDetailViewModel input)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add insert logic here
                Data.Device Device = obj.Create(new Data.Device
                {
                    DeviceId = 0,
                    Name = input.Name
                });
                if (Device != null)
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
            Data.Device Device = obj.GetByDeviceId(id);
            DeviceDetailViewModel result = new DeviceDetailViewModel();
            if (Device != null)
            {
                result = new DeviceDetailViewModel
                {
                    DeviceId = Device.DeviceId,
                    Name = Device.Name
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
        public ActionResult Edit(DeviceDetailViewModel input)
        {
            if (ModelState.IsValid)
            {
                //Search
                if (obj.GetByDeviceId(input.DeviceId) == null)
                {
                    input.ErrorMessage = "This Device was deleted!";
                    return View(input);
                }
                // TODO: Add insert logic here
                Data.Device Device = obj.Update(new Data.Device
                {
                    DeviceId = input.DeviceId,
                    Name = input.Name
                });
                if (Device != null)
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
            Data.Device Device = obj.GetByDeviceId(id);
            DeviceDetailViewModel result = new DeviceDetailViewModel();
            if (Device != null)
            {
                result = new DeviceDetailViewModel
                {
                    DeviceId = Device.DeviceId,
                    Name = Device.Name
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
        public ActionResult Delete(DeviceDetailViewModel input)
        {
            // TODO: Add insert logic here
            bool result = obj.Delete(input.DeviceId);
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
