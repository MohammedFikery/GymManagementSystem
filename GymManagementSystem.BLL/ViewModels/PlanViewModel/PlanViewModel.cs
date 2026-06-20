namespace GymManagementSystem.BLL.ViewModels.PlanViewModel;

public class PlanViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DurationDays { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; } 
}


