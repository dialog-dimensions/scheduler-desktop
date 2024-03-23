using System.ComponentModel.DataAnnotations;

namespace SchedulerDesktop.Models.Services.ApiServices.Account;

public class LoginModel
{
    [Required] public string? Id { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}