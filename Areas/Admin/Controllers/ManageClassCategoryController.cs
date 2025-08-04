using Equinox.Models;
using Microsoft.AspNetCore.Mvc;

namespace Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class ManageClassCategoryController : Controller
    {
        private EquinoxDbContext context { get; set; }
        public ManageClassCategoryController(EquinoxDbContext ctx) => context = ctx;
        public IActionResult List()
        {
            var classCategories = context.ClassCategory
                .OrderBy(m => m.Name)
                .ToList();

            return View(classCategories);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.action = "Add";
            return View("Edit", new ClassCategory());
        }

        [HttpPost]
        public IActionResult Edit(ClassCategory classCategory)
        {
            if (ModelState.IsValid)
            {
                if (classCategory.ClassCategoryId == 0)
                {
                    context.ClassCategory.Add(classCategory);
                    TempData["message"] = $"{classCategory.Name} Added Successfully";
                }
                else
                {
                    context.ClassCategory.Update(classCategory);
                    TempData["message"] = $"{classCategory.Name} Updated Successfully";
                }
                context.SaveChanges();
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.action = (classCategory.ClassCategoryId == 0) ? "Add" : "Edit";
                return View("Edit", classCategory);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.action = "Edit";
            var classCategory = context.ClassCategory.Find(id);
            return View(classCategory);
        }

        [HttpPost]
        public IActionResult Delete(ClassCategory classCategory)
        {
            context.ClassCategory.Remove(classCategory);
            TempData["message"] = $"{classCategory.Name} Deleted Successfully";
            context.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            ClassCategory classCategory = context.ClassCategory
                    .FirstOrDefault(p => p.ClassCategoryId == id) ?? new ClassCategory();
            return View(classCategory);
        }

    }
}
