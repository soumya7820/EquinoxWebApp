using System.Text.Json;
using Equinox.Models.Utils;
using Equinox.Models.DataLayer;
using Equinox.Models.DomainModels;
using Equinox.Models.Infrastructure;
using Equinox.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Equinox.Models.DataLayer.Repositories;

namespace Equinox.Controllers
{
    public class EquiClassController : Controller
    {
        private readonly Repository<EquinoxClass> classRepo;
        private readonly Repository<Club> clubRepo;
        private readonly Repository<ClassCategory> categoryRepo;
        private readonly Repository<Booking> bookingRepo;
        private readonly EquinoxDbContext _context;

        public EquiClassController(EquinoxDbContext context)
        {
            _context = context;
            classRepo = new Repository<EquinoxClass>(context);
            clubRepo = new Repository<Club>(context);
            categoryRepo = new Repository<ClassCategory>(context);
            bookingRepo = new Repository<Booking>(context);
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
            var cartCount = session.Bookings.Count;

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

            var viewModel = new EquinoxViewModel
            {
                EquinoxClasses = Equiclasses,
                Club = ViewBag.Club,
                ClassCategory = ViewBag.ClassCategory,
                ActiveClub = session.GetActiveClubs(),
                ActiveClassCategory = session.GetActiveClassCategory(),
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult BookClass(int id)
        {
            var session = new EquinoxSession(HttpContext.Session);

            var equinoxClass = classRepo.Get(id);
            if (equinoxClass == null)
            {
                TempData["Error"] = "Class not found.";
                return RedirectToAction("Index");
            }

            var booking = new Booking
            {
                EquinoxClassId = id,
                EquinoxClass = equinoxClass
            };

            bookingRepo.Insert(booking);
            bookingRepo.Save();

            var cart = session.Bookings;
            cart.Add(booking);
            HttpContext.Session.SetObject("Cart", cart);

            var cookieCart = new List<Booking>();
            if (Request.Cookies.TryGetValue("PreReservationCart", out var cookieData))
            {
                cookieCart = JsonSerializer.Deserialize<List<Booking>>(cookieData) ?? new List<Booking>();
            }
            cookieCart.Add(booking);

            Response.Cookies.Append("PreReservationCart", JsonSerializer.Serialize(cookieCart), new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
                IsEssential = true
            });

            TempData["Message"] = "Booking done successfully!";
            return RedirectToAction("Index", new
            {
                ActiveClub = session.GetActiveClubs(),
                ActiveClassCategory = session.GetActiveClassCategory()
            });
        }

        public IActionResult Cart()
        {
            var session = new EquinoxSession(HttpContext.Session);
            string activeClub = session.GetActiveClubs();
            string activeClassCategory = session.GetActiveClassCategory();

            List<Booking> cart = new List<Booking>();

            if (Request.Cookies.TryGetValue("PreReservationCart", out var cookieData))
            {
                var cookieCart = JsonSerializer.Deserialize<List<Booking>>(cookieData) ?? new List<Booking>();

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
                            EquinoxClass = equiClass
                        });
                    }
                }

                session.Bookings = cart;
            }
            else
            {
                cart = session.Bookings;
            }

            return View(new EquinoxViewModel
            {
                Bookings = cart,
                ActiveClub = activeClub,
                ActiveClassCategory = activeClassCategory
            });
        }

        public IActionResult RemoveFromCart(int id)
        {
            var session = new EquinoxSession(HttpContext.Session);
            var cart = session.Bookings;

            var itemToRemove = cart.FirstOrDefault(r => r.EquinoxClassId == id);
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                session.Bookings = cart;
            }

            if (Request.Cookies.TryGetValue("PreReservationCart", out var cookieCartRaw))
            {
                var cookieCart = JsonSerializer.Deserialize<List<Booking>>(cookieCartRaw) ?? new List<Booking>();
                var cookieItem = cookieCart.FirstOrDefault(r => r.EquinoxClassId == id);
                if (cookieItem != null)
                {
                    cookieCart.Remove(cookieItem);
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
