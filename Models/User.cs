using System.ComponentModel.DataAnnotations;

namespace Equinox.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter a Name.")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Please enter a PhoneNumber.")]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Please enter a Email.")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Please enter a DOB.")]
        public string DOB { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Please enter a IsCoach.")]
        public bool IsCoach { get; set; }
    }
}
