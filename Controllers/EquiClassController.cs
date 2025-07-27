using System.Text.Json;
using Equinox.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Equinox.Controllers
{
    public class EquiClassController : Controller
    {
        private EquinoxDbContext _context;
        public EquiClassController(EquinoxDbContext context)
        {
            _context = context;
        }

        public ViewResult Index(EquinoxViewModel model)
        {
            var filterList = new Filter($"{model.ActiveClub}-{model.ActiveClassCategory}");
            var filtereds = new Filter(filterList.filter);
            ViewBag.Filter = filterList;
            ViewBag.Club = _context.Club.ToList();
            ViewBag.ClassCategory = _context.ClassCategory.ToList();
            var session = new EquinoxSession(HttpContext.Session);
            session.SetActiveClubs(model.ActiveClub);
            session.SetActiveClassCategory(model.ActiveClassCategory);
            var cart = EquinoxCookies.GetReservationItemsFromCookie(HttpContext.Request, "PreReservationCart");
            var cartCount = cart.Count;

            IQueryable<EquinoxClass> query = _context.EquinoxClass
               .Include(t => t.Club)
               .Include(t => t.ClassCategory)
               .Include(t => t.User);

            if (model.ActiveClub != "all")
            {
                query = query.Where(r => r.Club.ClubId.ToString() == model.ActiveClub.ToLower());
            }
            if (model.ActiveClassCategory != "all")
            {
                query = query.Where(r => r.ClassCategory.ClassCategoryId.ToString() == model.ActiveClassCategory.ToLower());
            }
            var Equiclasses = query.OrderBy(t => t.Name).ToList();

            EquinoxViewModel equinoxViewModel = new EquinoxViewModel
            {
                EquinoxClass = Equiclasses,
                Club = ViewBag.Club,
                ClassCategory = ViewBag.ClassCategory,
                ActiveClub = session.GetActiveClubs(),
                ActiveClassCategory = session.GetActiveClassCategory(),
            };
            return View(equinoxViewModel);
        }
        public IActionResult Detail(int id, DateTime? date)
        {
            var session = new EquinoxSession(HttpContext.Session);

            var Equiclasses = _context.EquinoxClass
                                    .Include(t => t.Club)
                                    .Include(t => t.ClassCategory)
                                    .Include(t => t.User)
                                    .FirstOrDefault(t => t.EquinoxClassId == id);

            if (Equiclasses == null) return NotFound();

            ViewBag.AvailableDates = Enumerable.Range(0, 7)
                .Select(i => DateTime.Today.AddDays(i))
                .ToList();



            var viewModel = new EquinoxViewModel
            {
                EquinoxClasses = Equiclasses,
                Club = ViewBag.Club,
                ClassCategory = ViewBag.ClassCategory,
                ActiveClub = session.GetActiveClubs(),
                ActiveClassCategory = session.GetActiveClassCategory(),
            };

            return View(viewModel); // Now this matches the view's @model declaration
        }

        [HttpPost]
        public IActionResult BookClass(int id)
        {
            var session = new EquinoxSession(HttpContext.Session);
            var cart = HttpContext.Session.GetObject<List<Booking>>("Cart") ?? new List<Booking>();
            var reservation = new Booking
            {
                EquinoxClassId = id,
            };
            cart.Add(reservation);
            HttpContext.Session.SetObject("Cart", cart);


            var cookieCart = new List<Booking>();
            if (Request.Cookies.TryGetValue("PreReservationCart", out var cookieData))
            {
                cookieCart = JsonSerializer.Deserialize<List<Booking>>(cookieData) ?? new List<Booking>();
            }

            cookieCart.Add(new Booking
            {
                EquinoxClassId = id,
            });

            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
                IsEssential = true
            };

            Response.Cookies.Append("PreReservationCart", JsonSerializer.Serialize(cookieCart), options);

            TempData["Message"] = "Booking done successfully!";
            return RedirectToAction("Index", "EquiClass", new
            {
                ActiveClub = session.GetActiveClubs(),
                ActiveClassCategory = session.GetActiveClassCategory(),
            });
        }

        public IActionResult Cart()
        {
            List<Booking> cart = new List<Booking>();

            var session = new EquinoxSession(HttpContext.Session);
            string activeClub = session.GetActiveClubs();
            string activeClassCategory = session.GetActiveClassCategory();

            if (Request.Cookies.TryGetValue("PreReservationCart", out var cookieData))
            {
                var cookieCart = JsonSerializer.Deserialize<List<Booking>>(cookieData) ?? new List<Booking>();

                cart = new List<Booking>();

                foreach (var item in cookieCart)
                {
                    var equiClass = _context.EquinoxClass
                        .Include(m => m.Club)
                        .Include(m => m.ClassCategory)
                        .Include(t => t.User)
                        .FirstOrDefault(x => x.EquinoxClassId == item.EquinoxClassId);

                    if (equiClass != null)
                    {
                        cart.Add(new Booking
                        {
                            EquinoxClassId = item.EquinoxClassId,
                            EquinoxClass = equiClass,
                        });
                    }
                }

                HttpContext.Session.SetObject("Cart", cart);
            }

            var viewModel = new EquinoxViewModel
            {
                Bookings = cart,
                ActiveClub = activeClub,
                ActiveClassCategory = activeClassCategory
            };

            return View(viewModel);
        }

        public IActionResult RemoveFromCart(int id)
        {
            var cart = HttpContext.Session.GetObject<List<Booking>>("Cart") ?? new List<Booking>();
            var itemToRemove = cart.FirstOrDefault(r => r.EquinoxClassId == id);
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                HttpContext.Session.SetObject("Cart", cart);
            }

            // Remove from Cookie
            var cookieCartRaw = Request.Cookies["PreReservationCart"];
            if (!string.IsNullOrEmpty(cookieCartRaw))
            {
                var cookieCart = JsonSerializer.Deserialize<List<Booking>>(cookieCartRaw)
                        ?? new List<Booking>();
                var cookieItem = cookieCart.FirstOrDefault(r => r.EquinoxClassId == id);
                if (cookieItem != null)
                {
                    cookieCart.Remove(cookieItem);
                    // Update cookie
                    Response.Cookies.Append("PreReservationCart", JsonSerializer.Serialize(cookieCart), new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(7),
                        IsEssential = true
                    });
                }
            }

            return RedirectToAction("Cart");
        }

    }
}
