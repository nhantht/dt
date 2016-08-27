using Admin.Models;
using Service.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website.Controllers
{
    public class CityController : Controller
    {
        City obj = new City();
        // GET: Database
        public ActionResult Index(decimal countryId, bool fromChild = false)
        {
            ViewBag.CountryId = countryId;
            ViewBag.FromChild = fromChild;

            return View(GetList(countryId));
        }
        IEnumerable<CityDetailViewModel> GetList(decimal countryId)
        {
            return from x in obj.GetList()
                   where x.CountryId == countryId
                   orderby x.Name
                   select new CityDetailViewModel
                   {
                       CityId = x.CityId,
                       Name = x.Name,
                       CountryId = x.CountryId
                   };
        }
        // GET: Database/Details/5
        public ActionResult Details(decimal id, bool fromChild = false)
        {

            Data.City City = obj.GetById(id);
            CityDetailViewModel result = new CityDetailViewModel();
            if (City != null)
            {
                result = new CityDetailViewModel
                {
                    CityId = City.CityId,
                    Name = City.Name,
                    CountryId = City.CountryId,
                    CountryName = City.Country.Name,
                    FromChild = fromChild
                };
            }
            else
            {
                result.ErrorMessage = "The City was deleted!";
            }

            return View(result);
        }

        // GET: Database/Create
        public ActionResult Create(decimal countryId, bool fromChild = false)
        {
            return View(new CityDetailViewModel
            {
                CountryId = countryId,
                FromChild = fromChild
            });
        }

        // POST: Database/Create
        [HttpPost]
        public ActionResult Create(CityDetailViewModel input)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add insert logic here
                Data.City City = obj.Create(new Data.City
                {
                    CityId = 0,
                    Name = input.Name,
                    CountryId = input.CountryId
                });
                if (City != null)
                {
                    ViewBag.CountryId = input.CountryId;
                    ViewBag.FromChild = input.FromChild;
                    return View("Index", GetList(input.CountryId));
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
            Data.City City = obj.GetById(id);
            CityDetailViewModel result = new CityDetailViewModel();
            if (City != null)
            {
                result = new CityDetailViewModel
                {
                    CityId = City.CityId,
                    Name = City.Name,
                    CountryId = City.CountryId,
                    FromChild = fromChild
                };
            }
            else
            {
                result.ErrorMessage = "Error on Updating!";
            }

            return View(result);
        }
        public ActionResult Back2List(decimal countryId, bool fromChild = false)
        {
            ViewBag.CountryId = countryId;
            ViewBag.FromChild = fromChild;

            return View("Index", GetList(countryId));
        }
        // POST: Database/Edit/5
        [HttpPost]
        public ActionResult Edit(CityDetailViewModel input)
        {
            if (ModelState.IsValid)
            {
                //Search
                if (obj.GetById(input.CityId) == null)
                {
                    input.ErrorMessage = "This City was deleted!";
                    return View(input);
                }
                // TODO: Add insert logic here
                Data.City City = obj.Update(new Data.City
                {
                    CityId = input.CityId,
                    Name = input.Name,
                    CountryId = input.CountryId
                });
                if (City != null)
                {
                    ViewBag.CountryId = input.CountryId;
                    ViewBag.FromChild = input.FromChild;

                    return View("Index", GetList(input.CountryId));
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
            Data.City City = obj.GetById(id);
            CityDetailViewModel result = new CityDetailViewModel();
            if (City != null)
            {
                result = new CityDetailViewModel
                {
                    CityId = City.CityId,
                    Name = City.Name,
                    CountryId = City.CountryId,
                    CountryName = City.Country.Name,
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
        public ActionResult Delete(CityDetailViewModel input)
        {
            // TODO: Add insert logic here
            bool result = obj.Delete(input.CityId);
            if (result)
            {
                ViewBag.CountryId = input.CountryId;
                ViewBag.FromChild = input.FromChild;

                return View("Index", GetList(input.CountryId));
            }
            else
            {
                input.ErrorMessage = "This is inuse!";
                return View(input);
            }
        }
    }
}
