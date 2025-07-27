using System.ComponentModel.DataAnnotations;

namespace Equinox.Models
{
    public class Club
    {
        public int ClubId { get; set; }

        [Required(ErrorMessage = "Please enter a Name.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a PhoneNumber.")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
