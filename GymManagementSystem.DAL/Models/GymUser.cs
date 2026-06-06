using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.DAL.Entities
{
    /// <summary>
    /// Base class shared between Member and Trainer
    /// </summary>
    public abstract class GymUser : BaseEntity
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [RegularExpression(ValidationPatterns.Email, ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [MaxLength(11)]
        [RegularExpression(ValidationPatterns.EgyptianPhone,
            ErrorMessage = "Phone number must be a valid Egyptian number (starts with 010, 011, 012, or 015)")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }

        public Address Address { get; set; } = new Address();

        [MaxLength(500)]
        public string? Photo { get; set; }
    }

    public class Address
    {
        [MaxLength(20)]
        public string BuildingNumber { get; set; } = string.Empty;

        [MaxLength(30)]
        public string Street { get; set; } = string.Empty;

        [MaxLength(30)]
        public string City { get; set; } = string.Empty;
    }


}