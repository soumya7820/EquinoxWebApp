namespace Equinox.Models
{
    public class EquinoxSession
    {
        private const string ClubsKey = "MetropolisKey";
        private const string ClassCategoryKey = "PriceRangeKey";

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
    }
}