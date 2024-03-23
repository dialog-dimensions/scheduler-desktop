using System.Net.Http;
using System.Text;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SchedulerDesktop.JWT.Interfaces;
using SchedulerDesktop.JWT.WinReg;
using SchedulerDesktop.Models.DTOs.Entities;
using SchedulerDesktop.Models.Entities.Other_Objects;
using SchedulerDesktop.Models.Services.ApiServices.Account;
using SchedulerDesktop.Services.Api.BaseClasses;
using SchedulerDesktop.Services.Api.Interfaces;

namespace SchedulerDesktop.Services.Api.Classes;

public class UserApiService : ApiServiceBase<User, string, UserDto>, IUserApiService
{
    public UserApiService(IConfiguration configuration, IJwtTools jwt, HttpClient httpClient) : base(configuration, jwt, httpClient)
    {
    }
    
    public Task RegisterAsync(string id, string userName, string phoneNumber, string password, string confirmPassword)
    {
        throw new NotImplementedException();
    }

    public async Task LoginAsync(string id, string password)
    {
        var model = new LoginModel { Id = id, Password = password };
        var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
        
        HttpResponseMessage result;
        try
        {
            result = await HttpClient.PostAsync(Endpoints["Login"], content);
        }
        catch
        {
            MessageBox.Show("Login failed due to an unknown error.", "Failure", MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }
        
        if (result.IsSuccessStatusCode)
        {
            var token = await result.Content.ReadAsStringAsync();
            JwtRegistry.RegisterToken(token);
            MessageBox.Show("Logged in successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            MessageBox.Show("Unable to login.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    public Task LogoutAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetUserAsync(string id)
    {
        try
        {
            return await GetRequestAsync(id, "GetUser");
        }
        catch (UnsuccessfulHttpRequestException ex)
        {
            MessageBox.Show(ex.Message, ex.StatusCode.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            throw;
        }
    }

    public async Task<IEnumerable<User>?> GetUsersAsync()
    {
        try
        {
            return await GetRequestAsync("GetUsers");
        }
        catch (UnsuccessfulHttpRequestException ex)
        {
            MessageBox.Show(ex.Message, ex.StatusCode.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            throw;
        }
    }

    public async Task<bool> CallToRegisterAsync(int id, string phoneNumber)
    {
        var dto = new CallToRegisterDto { Id = id, PhoneNumber = phoneNumber };
        
        try
        {
            return await PostRequestAsync(dto, "CallToRegister");
        }
        catch (UnsuccessfulHttpRequestException ex)
        {
            MessageBox.Show(ex.Message, ex.StatusCode.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            throw;
        }
    }
}
