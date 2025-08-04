using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Equinox.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter a Name.")]
        [RegularExpression("^[a-zA-Z0-9 ]+$")]
        [StringLength(50, ErrorMessage = "Please limit your Name to 50 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the PhoneNumber.")]
        [Remote("CheckPhoneNumber", "Validation", areaName: "")]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a date of birth.")]
        [AgeRange(8, 80, ErrorMessage = "Age must be between 8 and 80.")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Please enter a IsCoach.")]
        public bool IsCoach { get; set; } = true;
    }
}
