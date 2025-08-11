using Equinox.Models.DataLayer;
using Equinox.Models.DataLayer.Repositories;
using Equinox.Models.DomainModels;
using Microsoft.AspNetCore.Mvc;

namespace Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class ManageClubController : Controller
    {
        private Repository<Club> clubRepo { get; set; }
        private Repository<Booking> bookingRepo { get; set; }

        public ManageClubController(EquinoxDbContext ctx)
        {
            clubRepo = new Repository<Club>(ctx);
            bookingRepo = new Repository<Booking>(ctx);
        }

        public ViewResult List()
        {
            var clubs = clubRepo.List(new QueryOptions<Club>
            {
                OrderBy = c => c.Name
            });
            return View(clubs);
        }

        // Add
        [HttpGet]
        public ViewResult Add()
        {
            ViewBag.Action = "Add";
            return View("Edit", new Club());
        }

        [HttpPost]
        public IActionResult Add(Club club)
        {
            if (ModelState.IsValid)
            {
                clubRepo.Insert(club);
                clubRepo.Save();
                TempData["message"] = $"{club.Name} Added Successfully";
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.Action = "Add";
                return View("Edit", club);
            }
        }

        // Edit
        [HttpGet]
        public ViewResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            return View(clubRepo.Get(id));
        }

        [HttpPost]
        public IActionResult Edit(Club club)
        {
            if (ModelState.IsValid)
            {
                clubRepo.Update(club);
                clubRepo.Save();
                TempData["message"] = $"{club.Name} Updated Successfully";
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.Action = "Edit";
                return View("Edit", club);
            }
        }

        // Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var club = clubRepo.Get(id);

            // Check if any bookings exist for this club
            bool hasBookings = bookingRepo.List(new QueryOptions<Booking>
            {
                Includes = "EquinoxClass",
                Where = b => b.EquinoxClass.ClubId == id
            }).Any();

            if (hasBookings)
            {
                TempData["message"] = $"Cannot delete {club.Name} because it has bookings in classes.";
                return RedirectToAction("List");
            }

            return View(club);
        }

        [HttpPost]
        public RedirectToActionResult Delete(Club club)
        {
            clubRepo.Delete(club);
            clubRepo.Save();
            TempData["message"] = $"{club.Name} Deleted Successfully";
            return RedirectToAction("List");
        }
    }
}
