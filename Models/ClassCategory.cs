using System.ComponentModel.DataAnnotations;

namespace Equinox.Models
{
    public class ClassCategory
    {
        public int ClassCategoryId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        [RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "Name can only contain letters, numbers, and spaces.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a Image.")]
        public string Image { get; set; } = string.Empty;
    }
}
