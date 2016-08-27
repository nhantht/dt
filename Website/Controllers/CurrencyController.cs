using Admin.Models;
using Service.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website.Controllers
{
    public class CurrencyController : Controller
    {
        Currency obj = new Currency();
        // GET: Database
        public ActionResult Index(bool fromChild = false)
        {
            ViewBag.FromChild = fromChild;
            return View(GetList());
        }
        IEnumerable<CurrencyDetailViewModel> GetList()
        {
            return from x in obj.GetList().OrderBy(x => x.Code)
                   select new CurrencyDetailViewModel
                   {
                       Id = x.Id,
                       Name = x.Name,
                       Code = x.Code
                   };
        }
        // GET: Database/Details/5
        public ActionResult Details(decimal id, bool fromChild = false)
        {
            Data.Currency currency = obj.GetById(id);
            CurrencyDetailViewModel result = new CurrencyDetailViewModel();
            if (currency != null)
            {
                result = new CurrencyDetailViewModel
                {
                    Id = currency.Id,
                    Name = currency.Name,
                    Code = currency.Code,
                    FromChild = fromChild
                };
            }
            else
            {
                result.ErrorMessage = "The currency was deleted!";
            }

            return View(result);
        }

        // GET: Database/Create
        public ActionResult Create(bool fromChild = false)
        {
            return View(new CurrencyDetailViewModel()
            {
                FromChild = fromChild
            });
        }

        // POST: Database/Create
        [HttpPost]
        public ActionResult Create(CurrencyDetailViewModel input)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add insert logic here
                Data.Currency currency = obj.Create(new Data.Currency
                {
                    Id = 0,
                    Name = input.Name,
                    Code = input.Code
                });
                if (currency != null)
                {
                    ViewBag.FromChild = input.FromChild;

                    return View("Index", GetList());
                }
                else
                {
                    input.ErrorMessage = "Code is duplicated!";
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
            Data.Currency currency = obj.GetById(id);
            CurrencyDetailViewModel result = new CurrencyDetailViewModel();
            if (currency != null)
            {
                result = new CurrencyDetailViewModel
                {
                    Id = currency.Id,
                    Name = currency.Name,
                    Code = currency.Code,
                    FromChild = fromChild
                };
            }
            else
            {
                result.ErrorMessage = "This currency was deleted!";
            }

            return View(result);
        }

        // POST: Database/Edit/5
        [HttpPost]
        public ActionResult Edit(CurrencyDetailViewModel input)
        {
            if (ModelState.IsValid)
            {
                //Search
                if (obj.GetById(input.Id) == null)
                {
                    input.ErrorMessage = "This currency was deleted!";
                    return View(input);
                }
                // TODO: Add insert logic here
                Data.Currency currency = obj.Update(new Data.Currency
                {
                    Id = input.Id,
                    Name = input.Name,
                    Code = input.Code
                });
                if (currency != null)
                {
                    ViewBag.FromChild = input.FromChild;
                    return View("Index", GetList());
                }
                else
                {
                    input.ErrorMessage = "Code is duplicated! Please enter another.";
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
            Data.Currency currency = obj.GetById(id);
            CurrencyDetailViewModel result = new CurrencyDetailViewModel();
            if (currency != null)
            {
                result = new CurrencyDetailViewModel
                {
                    Id = currency.Id,
                    Name = currency.Name,
                    Code = currency.Code,
                    FromChild = fromChild
                };
            }
            else
            {
                result.ErrorMessage = "This currency was deleted!";
            }

            return View(result);
        }

        // POST: Database/Delete/5
        [HttpPost]
        public ActionResult Delete(CurrencyDetailViewModel input)
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
