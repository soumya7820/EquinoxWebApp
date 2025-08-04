using Newtonsoft.Json;

namespace Equinox.Models
{
    public class EquinoxCookies
    {
        public static List<Booking> GetReservationItemsFromCookie(HttpRequest request, string cookieName)
        {
            if (request.Cookies.TryGetValue(cookieName, out string? cookieValue))
            {
                return JsonConvert.DeserializeObject<List<Booking>>(cookieValue) ?? new List<Booking>();
            }
            return new List<Booking>();
        }
        public static void SetReservationItemsCookie(HttpResponse response, string cookieName, List<Booking> bookings)
        {
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
                IsEssential = true
            };
            string jsonData = JsonConvert.SerializeObject(bookings);
            response.Cookies.Append(cookieName, jsonData, options);
        }
    }
}
