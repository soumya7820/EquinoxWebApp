using Equinox.Models.DataLayer;
using Equinox.Models.DataLayer.Repositories;
using Equinox.Models.DomainModels;
using Microsoft.AspNetCore.Mvc;

namespace Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class ManageClassCategoryController : Controller
    {
        private Repository<ClassCategory> data { get; set; }
        private Repository<Booking> bookingData { get; set; }

        public ManageClassCategoryController(EquinoxDbContext ctx)
        {
            data = new Repository<ClassCategory>(ctx);
            bookingData = new Repository<Booking>(ctx);
        }

        public ViewResult List()
        {
            var classCategories = data.List(new QueryOptions<ClassCategory>
            {
                OrderBy = c => c.Name
            });
            return View(classCategories);
        }

        // Add
        [HttpGet]
        public ViewResult Add()
        {
            ViewBag.Action = "Add";
            return View("Edit", new ClassCategory());
        }

        [HttpPost]
        public IActionResult Add(ClassCategory classCategory)
        {
            if (ModelState.IsValid)
            {
                data.Insert(classCategory);
                data.Save();
                TempData["message"] = $"{classCategory.Name} Added Successfully";
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.Action = "Add";
                return View("Edit", classCategory);
            }
        }

        // Edit
        [HttpGet]
        public ViewResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            return View(data.Get(id));
        }

        [HttpPost]
        public IActionResult Edit(ClassCategory classCategory)
        {
            if (ModelState.IsValid)
            {
                data.Update(classCategory);
                data.Save();
                TempData["message"] = $"{classCategory.Name} Updated Successfully";
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.Action = "Edit";
                return View(classCategory);
            }
        }

        // Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var classCategory = data.Get(id);

            // Check if this category has bookings
            bool hasBookings = bookingData.List(new QueryOptions<Booking>
            {
                Includes = "EquinoxClass",
                Where = b => b.EquinoxClass.ClassCategoryId == id
            }).Any();

            if (hasBookings)
            {
                TempData["message"] = $"Cannot delete {classCategory.Name} because it has bookings in classes.";
                return RedirectToAction("List");
            }

            return View(classCategory);
        }

        [HttpPost]
        public RedirectToActionResult Delete(ClassCategory classCategory)
        {
            data.Delete(classCategory);
            data.Save();
            TempData["message"] = $"{classCategory.Name} Deleted Successfully";
            return RedirectToAction("List");
        }
    }
}
