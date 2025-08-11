using Equinox.Models.DataLayer;
using Equinox.Models.DomainModels;
using Equinox.Models.Validations;
using Microsoft.AspNetCore.Mvc;

namespace Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class ManageUserController : Controller
    {
        private EquinoxDbContext context { get; set; }
        public ManageUserController(EquinoxDbContext ctx) => context = ctx;
        public IActionResult List()
        {
            var users = context.User
                .OrderBy(m => m.Name)
                .ToList();

            return View(users);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            return View("Edit", new User());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            var user = context.User.Find(id);
            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            // Check for duplicate phone number if not explicitly verified
            if (TempData["okPhoneNumber"] == null)
            {
                string msg = Check.PhoneNumberExists(context, user.PhoneNumber);
                if (!string.IsNullOrEmpty(msg))
                {
                    ModelState.AddModelError(nameof(user.PhoneNumber), msg);
                    TempData["message"] = "Please fix the error";
                }
            }

            if (ModelState.IsValid)
            {
                if (user.UserId == 0)
                {
                    context.User.Add(user);
                    TempData["message"] = $"{user.Name} added successfully.";
                }
                else
                {
                    context.User.Update(user);
                    TempData["message"] = $"{user.Name} updated successfully.";
                }

                context.SaveChanges();
                return RedirectToAction("List", "ManageUser");
            }
            else
            {
                ViewBag.Action = user.UserId == 0 ? "Add" : "Edit";
                return View(user);
            }
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var user = context.User.Find(id);
            return View(user);
        }

        [HttpPost]
        public IActionResult Delete(User user)
        {
            var existinguser = context.User.FirstOrDefault(c => c.UserId == user.UserId);
            if (existinguser == null)
            {
                TempData["message"] = "User not found.";
                return RedirectToAction("List");
            }

            bool hasBookings = context.Booking.Any(b => b.EquinoxClass.UserId == user.UserId);

            if (hasBookings)
            {
                TempData["message"] = $"Cannot delete {existinguser.Name} because it has bookings in classes.";
                return RedirectToAction("List");
            }

            context.User.Remove(existinguser);
            context.SaveChanges();

            TempData["message"] = $"{existinguser.Name} Deleted Successfully";
            return RedirectToAction("List");
        }
    }
}
