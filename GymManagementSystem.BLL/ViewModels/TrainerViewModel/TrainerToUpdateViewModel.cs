using GymManagementSystem.DAL.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.BLL.ViewModels.Trainer
{
    public class TrainerToUpdateViewModel
    {
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Phone Number Is Required")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone number must be a valid Egyptian mobile number")]
        public string Phone { get; set; } = default!;

        [Required(ErrorMessage = "Building Number Is Required")]
        [RegularExpression(@"^\d+[A-Za-z]?$", ErrorMessage = "Building Number must be a valid number (e.g. 12 or 12B)")]
        public string BuildingNumber { get; set; } = default!;

        [Required(ErrorMessage = "City Is Required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "City must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City can only contain letters and spaces")]
        public string City { get; set; } = default!;

        [Required(ErrorMessage = "Street Is Required")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Street must be between 2 and 150 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Street can only contain letters, numbers, and spaces")]
        public string Street { get; set; } = default!;

        [Required(ErrorMessage = "At least one specialty is required")]
        [MinLength(1, ErrorMessage = "At least one specialty must be selected")]
        public List<Specialties> Specialties { get; set; } = new();
    }
}