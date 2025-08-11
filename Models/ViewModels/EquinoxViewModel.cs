using Equinox.Models.DomainModels;

namespace Equinox.Models.ViewModels
{
    public class EquinoxViewModel
    {
        public string ActiveClub { get; set; } = "all";
        public string ActiveClassCategory { get; set; } = "all";
        public List<Club> Club { get; set; } = new List<Club>();
        public List<ClassCategory> ClassCategory { get; set; } = new List<ClassCategory>();
        public string SelectedClubId { get; set; } = "all";
        public string SelectedCategoryId { get; set; } = "all";
        public List<EquinoxClass> EquinoxClass { get; set; } = new List<EquinoxClass>();
        public List<Booking> Bookings { get; set; } = new List<Booking>();
        public Club Clubs { get; set; } = new Club();
        public ClassCategory ClassCategories { get; set; } = new ClassCategory();
        public EquinoxClass EquinoxClasses { get; set; } = new EquinoxClass();
        public string CheckActiveClub(string d) =>
            d.ToLower() == ActiveClub.ToLower() ? "active" : "";

        public string CheckActiveClassCategory(string d) =>
            d.ToLower() == ActiveClassCategory.ToLower() ? "active" : "";
    }
}
