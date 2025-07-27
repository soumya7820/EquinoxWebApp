using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Equinox.Models
{
    public class EquinoxClass
    {
        public int EquinoxClassId { get; set; }

        [Required(ErrorMessage = "Please enter a Name.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a ClassPicture.")]
        public string ClassPicture { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a ClassDay.")]
        public string ClassDay { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a Time.")]
        [StringLength(50, ErrorMessage = "Time must be between 5 and 50 characters.", MinimumLength = 5)]
        public string Time { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a ClassCategory.")]
        public int ClassCategoryId { get; set; }
        [ValidateNever]
        public ClassCategory ClassCategory { get; set; } = null!;
        
        [Required(ErrorMessage = "Please enter a Club.")]
        public int ClubId { get; set; }
        [ValidateNever]
        public Club Club { get; set; } = null!;

        [Required(ErrorMessage = "Please enter a User.")]
        public int UserId { get; set; }
        [ValidateNever]
        public User User { get; set; } = null!;
    }
}
