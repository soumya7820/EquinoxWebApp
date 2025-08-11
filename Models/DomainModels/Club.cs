using System.ComponentModel.DataAnnotations;

namespace Equinox.Models.DomainModels
{
    public class Club
    {
        public int ClubId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        [RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "Name can only contain letters, numbers, and spaces.")]
        public string Name { get; set; } = string.Empty;
        [Phone]
        [Required(ErrorMessage = "Please enter a PhoneNumber.")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
