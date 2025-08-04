using System.Text.Json;

namespace Equinox.Models
{
    public class EquinoxSession
    {
        private const string ClubsKey = "ClubsKey";
        private const string ClassCategoryKey = "ClassCategoryKey";
        private const string CartKey = "Cart";

        private ISession session { get; set; }
        public EquinoxSession(ISession session) => this.session = session;
        public void SetActiveClubs(string activeClubs) =>
            session.SetString(ClubsKey, activeClubs);
        public string GetActiveClubs() =>
            session.GetString(ClubsKey) ?? string.Empty;
        public void SetActiveClassCategory(string activeClassCategory) =>
            session.SetString(ClassCategoryKey, activeClassCategory);
        public string GetActiveClassCategory() =>
            session.GetString(ClassCategoryKey) ?? string.Empty;

        public List<Booking> Bookings
        {
            get
            {
                var data = session.GetString(CartKey);
                return string.IsNullOrEmpty(data)
                    ? new List<Booking>()
                    : JsonSerializer.Deserialize<List<Booking>>(data) ?? new List<Booking>();
            }
            set
            {
                session.SetString(CartKey, JsonSerializer.Serialize(value));
            }
        }
    }
}