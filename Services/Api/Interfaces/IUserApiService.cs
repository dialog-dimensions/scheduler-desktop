using SchedulerDesktop.Models.Entities.Other_Objects;

namespace SchedulerDesktop.Services.Api.Interfaces;

public interface IUserApiService
{
    Task RegisterAsync(string id, string userName, string phoneNumber, string password, string confirmPassword);
    Task LoginAsync(string id, string password);
    Task LogoutAsync();
    Task<User?> GetUserAsync(string id);
    Task<IEnumerable<User>?> GetUsersAsync();
    Task<bool> CallToRegisterAsync(int id, string phoneNumber);
}
