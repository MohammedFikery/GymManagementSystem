

using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.BLL.ViewModels.Login;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email Is Required")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Password Is Required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;
    [Display(Name = "Remember Me")]
    public bool RememberMe { get; set; }
}
