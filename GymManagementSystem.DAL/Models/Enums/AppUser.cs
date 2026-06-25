
using Microsoft.AspNetCore.Identity;

namespace GymManagementSystem.DAL.Models.Enums;

public class AppUser:IdentityUser
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}
