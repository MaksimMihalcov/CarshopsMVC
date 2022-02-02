using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Carshops_MVC
{
    public class CarItemController : Controller
    {
        public CarItemController() { }
        public IActionResult CarItemsIndex(int? officeid)
        {
            using (MainOffice mainOffice = new MainOffice(0))
            {
                Office office = mainOffice.Dealerships.FirstOrDefault(of => of.OfficeId == officeid);
                if (office == null)
                {
                    return NotFound();
                }
                ViewBag.OfficeId = office.OfficeId;
                return View(office.CarsDataBase.GetItemsList());
            }
        }

        public IActionResult Create(int officeid)
        {
            ViewBag.OfficeId = officeid;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ItemId, Brand, Model, Price, StockBalance, OfficeId")] CarItem carItem, int officeid)
        {
            using (MainOffice mainOffice = new MainOffice(0))
            {
                if (ModelState.IsValid)
                {
                    Office office = mainOffice.Dealerships.FirstOrDefault(of => of.OfficeId == officeid);
                    if (office == null)
                    {
                        return NotFound();
                    }
                    office.CarsDataBase.Create(carItem);
                    return RedirectToAction(nameof(CarItemsIndex), new { officeid = officeid });
                }
                return View(carItem);
            }
        }
        public IActionResult Delete(int itemid, int officeid)
        {
            using (var mainOffice = new MainOffice(0))
            {
                ViewBag.OfficeId = officeid;
                var office = mainOffice.Dealerships.FirstOrDefault(of => of.OfficeId == officeid);
                if(office == null)
                {
                    return NotFound();
                }
                var item = office.CarsDataBase.GetItem(itemid);
                if (item == null)
                {
                    return NotFound();
                }
                return View(item);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int officeid, int itemid)
        {
            using (var mainOffice = new MainOffice(0))
            {
                var office = mainOffice.Dealerships.FirstOrDefault(of => of.OfficeId == officeid);
                if(office == null)
                {
                    return NotFound();
                }
                var item = office.CarsDataBase.GetItem(itemid);
                if (item == null)
                {
                    return NotFound();
                }
                office.CarsDataBase.Delete(item);
                return RedirectToAction(nameof(CarItemsIndex), new { officeid = officeid });
            }
        }
        public IActionResult Edit(int itemid, int officeid)
        {
            using (var mainOffice = new MainOffice(0))
            {
                var office = mainOffice.Dealerships.FirstOrDefault(of => of.OfficeId == officeid);
                if (office == null)
                {
                    return NotFound();
                }
                ViewBag.OfficeId = officeid;
                var item = office.CarsDataBase.GetItem(itemid);
                if (item == null)
                {
                    return NotFound();
                }
                return View(item);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("ItemId, Brand, Model, Price, StockBalance, OfficeId")] CarItem carItem, int officeid)
        {
            using (var mainOffice = new MainOffice(0))
            {
                var office = mainOffice.Dealerships.FirstOrDefault(of => of.OfficeId == officeid);
                if (office == null)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        office.CarsDataBase.Update(carItem);
                    }
                    catch (Exception)
                    {
                        return NotFound();
                    }
                    return RedirectToAction(nameof(CarItemsIndex), new { officeid = officeid });
                }
                return View(carItem);
            }
        }
    }
}
