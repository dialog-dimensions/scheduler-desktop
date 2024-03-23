namespace SchedulerDesktop.JWT.Interfaces;

public interface IJwtTools
{
    string? TryGetToken();
    string? TryGetId();
    bool ValidateToken();
    void RedirectToLogin();
}