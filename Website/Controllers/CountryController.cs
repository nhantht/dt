using Admin.Models;
using Service.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website.Controllers
{
    public class CountryController : Controller
    {
        Country obj = new Country();
        // GET: Database
        public ActionResult Index(bool fromChild = false)
        {
            ViewBag.FromChild = fromChild;
            return View(GetList());
        }
        IEnumerable<CountryDetailViewModel> GetList()
        {
            return from x in obj.GetList()
                   orderby x.Name
                   select new CountryDetailViewModel
                   {
                       CountryId = x.CountryId,
                       Code = x.Code,
                       Name = x.Name,
                       TelephoneCode = x.TelephoneCode,
                       CityNumber = x.Cities.Count
                   };
        }

        // GET: Database/Details/5
        public ActionResult Details(decimal id, bool fromChild = false)
        {

            Data.Country Country = obj.GetByCountryId(id);
            CountryDetailViewModel result = new CountryDetailViewModel();
            if (Country != null)
            {
                result = new CountryDetailViewModel
                {
                    CountryId = Country.CountryId,
                    Code = Country.Code,
                    Name = Country.Name,
                    TelephoneCode = Country.TelephoneCode,
                    CityNumber = Country.Cities.Count,
                    FromChild = fromChild
                };
            }
            else
            {
                result.ErrorMessage = "The Country was deleted!";
            }

            return View(result);
        }

        // GET: Database/Create
        public ActionResult Create(bool fromChild = false)
        {
            return View(new CountryDetailViewModel()
            {
                FromChild = fromChild
            });
        }

        // POST: Database/Create
        [HttpPost]
        public ActionResult Create(CountryDetailViewModel input)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add insert logic here
                Data.Country Country = obj.Create(new Data.Country
                {
                    CountryId = 0,
                    Code = input.Code,
                    Name = input.Name,
                    TelephoneCode = input.TelephoneCode
                });
                if (Country != null)
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
            Data.Country Country = obj.GetByCountryId(id);
            CountryDetailViewModel result = new CountryDetailViewModel();
            if (Country != null)
            {
                result = new CountryDetailViewModel
                {
                    CountryId = Country.CountryId,
                    Code = Country.Code,
                    Name = Country.Name,
                    TelephoneCode = Country.TelephoneCode,
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
        public ActionResult Edit(CountryDetailViewModel input)
        {
            if (ModelState.IsValid)
            {
                //Search
                if (obj.GetByCountryId(input.CountryId) == null)
                {
                    input.ErrorMessage = "This Country was deleted!";
                    return View(input);
                }
                // TODO: Add insert logic here
                Data.Country Country = obj.Update(new Data.Country
                {
                    CountryId = input.CountryId,
                    Code = input.Code,
                    Name = input.Name,
                    TelephoneCode = input.TelephoneCode
                });
                if (Country != null)
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
            Data.Country Country = obj.GetByCountryId(id);
            CountryDetailViewModel result = new CountryDetailViewModel();
            if (Country != null)
            {
                result = new CountryDetailViewModel
                {
                    CountryId = Country.CountryId,
                    Code = Country.Code,
                    Name = Country.Name,
                    TelephoneCode = Country.TelephoneCode,
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
        public ActionResult Delete(CountryDetailViewModel input)
        {
            // TODO: Add insert logic here
            bool result = obj.Delete(input.CountryId);
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
