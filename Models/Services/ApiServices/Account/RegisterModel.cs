using System.ComponentModel.DataAnnotations;

namespace SchedulerDesktop.Models.Services.ApiServices.Account;

public class RegisterModel
{
    [Required] public string? Id { get; set; }
    
    [Required] public string? UserName { get; set; }
    
    [Required] 
    [DataType(DataType.PhoneNumber)] 
    public string? PhoneNumber { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password confirmation mismatch.")]
    public string? ConfirmPassword { get; set; }
}