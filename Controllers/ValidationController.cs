using Microsoft.AspNetCore.Mvc;
using Equinox.Models;

namespace Equinox.Controllers
{
    public class ValidationController : Controller
    {
        private EquinoxDbContext context;
        public ValidationController(EquinoxDbContext ctx) => context = ctx;

        public JsonResult CheckPhoneNumber(string phoneNumber)
        {
            string msg = Check.PhoneNumberExists(context, phoneNumber);
            if (string.IsNullOrEmpty(msg))
            {
                TempData["okPhoneNumber"] = true;
                return Json(true);
            }
            else return Json(msg);
        }
    }
}
