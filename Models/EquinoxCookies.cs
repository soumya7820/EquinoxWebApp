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
    }
}
