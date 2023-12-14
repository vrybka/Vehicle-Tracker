using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleTracker.Models;
using VehicleTracker.Services;

namespace VehicleTracker.Controllers
{
    public class VehicleController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly FileUploadService _fileupload;

        private readonly IWebHostEnvironment _web;

        public VehicleController(ApplicationDbContext context, FileUploadService fileUploadService, IWebHostEnvironment web)
        {
            _context = context;
            _fileupload = fileUploadService;
            _web = web;
        }



        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            return _context.Vehicles != null ?
                        View(await _context.Vehicles.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Vehicles'  is null.");
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Vehicles == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Make,CarModel,Year")] Vehicle vehicle, IFormFile? file)
        {

            //var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", file.FileName);
            //using (System.IO.Stream stream = new FileStream(path, FileMode.Create))
            //{
            //    await file.CopyToAsync(stream);
            //}

            ///////

            //await _fileupload.UploadFile(file);


            //////

            if (ModelState.IsValid)
            {
                //string imgtxt = Path.GetExtension(file.FileName);
                //if (imgtxt.Equals(".jpg") || imgtxt.Equals(".JPG"))
                //{
                //    await _fileupload.UploadFile(file);
                //    //var path = Path.Combine(_web.WebRootPath, "Images", file.FileName);
                //    //var stream = new FileStream(path, FileMode.Create);
                //    //await file.CopyToAsync(stream);
                //    vehicle.Image = file.FileName;
                //}

                if (file != null)
                {
                    await _fileupload.UploadFile(file);
                    vehicle.Image = file.FileName;
                }


                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            //__ORIGINAL

            //if (ModelState.IsValid)
            //{

            //    _context.Add(vehicle);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}

            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vehicles == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Make,CarModel,Year")] Vehicle vehicle, IFormFile? file)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null)
                    {
                        await _fileupload.UploadFile(file);
                        vehicle.Image = file.FileName;
                    }

                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Vehicles == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Vehicles == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Vehicles'  is null.");
            }
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return (_context.Vehicles?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        public async Task<IActionResult> EditNotes(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        [HttpPost]
        public async Task<IActionResult> EditNotes(int id, string notes)
        {

            //ViewData["VehicleNotes"] = notes;

            var car = _context.Vehicles.FirstOrDefault(m => m.Id == id);
            car.Notes = notes;


            HttpContext.Session.SetString("VehicleNotes", notes);

            _context.Update(car);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Records", new { id = car.Id, notes = notes });
        }
    }
}
