using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Xml.Schema;
using VehicleTracker.Models;

namespace VehicleTracker.Controllers
{
    public class RecordsController : Controller
    {
        private readonly ApplicationDbContext carRep;

        public RecordsController(ApplicationDbContext context)
        {
            carRep = context;
        }

        public async Task<IActionResult> Index(int id, int? category, string? SearchString, string? SortOrder, string? notes)
        {
            ViewBag.carId = id;
            ViewBag.CurrentCategory = category;
            ViewBag.CurrentSearch = SearchString;

            //ViewData["VehicleNotes"] = notes;
            if (notes != null)
            {
                HttpContext.Session.SetString("VehicleNotes", notes);
            }

            //else
            //{
            //    HttpContext.Session.SetString("VehicleNotes", "Nothing here yet..");
            //}


            HttpContext.Session.SetInt32("carid", (int)id);

            if (id == null || carRep.Records == null)
            {
                return NotFound();
            }


            var records = from b in carRep.Records.Where(m => m.VehicleId == id).Where(m => category == null || m.CategoryId == category) select b;

 
            if (!String.IsNullOrEmpty(SearchString))
            {
                records = records.Where(b => b.Name.Contains(SearchString) || b.Notes.Contains(SearchString));
            }

            // Calculate total cost
            double totalCost = (double)records.Sum(m => m.Cost);
            HttpContext.Session.SetString("TotalCost", totalCost.ToString("C2"));


            ViewBag.SortParam = String.IsNullOrEmpty(SortOrder) ? "asc" : "";
            switch (SortOrder)
            {
                case "asc":
                    records = records.OrderBy(b => b.Date);
                    break;
                default:
                    records = records.OrderByDescending(b => b.Date);
                    break;
            }

            //ViewData["VehicleNotes"] = records;

            if (records == null)
            {
                return NotFound();
            }

            return View(records);
        }


        // GET: Records/Create
        [HttpGet]
        public IActionResult Create(int id)
        {
            ViewBag.vehicleId = id;

            //HttpContext.Session.SetInt32("carid", (int)id);
            //ViewBag.Categories = GetCategories();


            var vm = new RecordsViewModel();
            vm.Categories = new List<SelectListItem>
            {
                new SelectListItem {Text="", Value = null},
                new SelectListItem {Text="Maintenance", Value = "1"},
                new SelectListItem {Text="Repair", Value = "2" },
                new SelectListItem {Text="Upgrade", Value = "3" },
                new SelectListItem {Text="Cosmetics", Value = "4" },
                new SelectListItem {Text="Other", Value = "5" }
            };


            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RecordsViewModel addRecord)
        {

            var carid = HttpContext.Session.GetInt32("carid");

            var record = new Records()
            {
                VehicleId = (int)carid,
                Date = addRecord.Date,
                Mileage = addRecord.Mileage,
                Name = addRecord.Name,
                Notes = addRecord.Notes,
                Cost = addRecord.Cost,
                CategoryId = addRecord.CategoryId,
            };

            await carRep.Records.AddAsync(record);
            await carRep.SaveChangesAsync();
            return RedirectToAction("Create");

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || carRep.Records == null)
            {
                return NotFound();
            }

            var records = await carRep.Records.FindAsync(id);
            if (records == null)
            {
                return NotFound();
            }
            var vm = new RecordsViewModel();
            vm.Categories = new List<SelectListItem>
            {
                new SelectListItem {Text="", Value = null},
                new SelectListItem {Text="Maintenance", Value = "1"},
                new SelectListItem {Text="Repair", Value = "2" },
                new SelectListItem {Text="Upgrade", Value = "3" },
                new SelectListItem {Text="Cosmetics", Value = "4" },
                new SelectListItem {Text="Other", Value = "5" }
            };

            vm.Date = records.Date;
            vm.Mileage = records.Mileage;
            vm.Name = records.Name;
            vm.Notes = records.Notes;
            vm.Cost = records.Cost;



            return View(vm);

            //return View(records);
        }

        // POST: Records/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleId,Date,Mileage,Name,Notes,Cost,CategoryId")] Records records)
        {
            var CarID = HttpContext.Session.GetInt32("carid");
            records.VehicleId = (int)CarID;
            if (id != records.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    carRep.Update(records);
                    await carRep.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecordsExists(records.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { id = CarID });
            }
            return View(records);
        }

        // GET: Records/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || carRep.Records == null)
            {
                return NotFound();
            }

            var records = await carRep.Records
                .FirstOrDefaultAsync(m => m.Id == id);
            if (records == null)
            {
                return NotFound();
            }

            return View(records);
        }

        // POST: Records/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var CarID = HttpContext.Session.GetInt32("carid");
            if (carRep.Records == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Records'  is null.");
            }
            var records = await carRep.Records.FindAsync(id);
            if (records != null)
            {
                carRep.Records.Remove(records);
            }

            await carRep.SaveChangesAsync();
            return RedirectToAction("Index", new { id = CarID });
        }

        //GET: Records/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || carRep.Records == null)
            {
                return NotFound();
            }

            var records = await carRep.Records
                .FirstOrDefaultAsync(m => m.Id == id);
            if (records == null)
            {
                return NotFound();
            }

            return View(records);
        }

        private bool RecordsExists(int id)
        {
            return (carRep.Records?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private List<SelectListItem> GetCategories()
        {
            var lstCategories = new List<SelectListItem>();

            List<Category> categories = carRep.Category.ToList();

            lstCategories = categories.Select(ct => new SelectListItem()
            {
                Value = ct.Name,
            }).ToList();

            return lstCategories;
        }
    
    }
}
