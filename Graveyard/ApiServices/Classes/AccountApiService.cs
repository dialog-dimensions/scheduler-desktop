// using System.Net.Http;
// using System.Text;
// using System.Windows;
// using Microsoft.Extensions.Configuration;
// using Newtonsoft.Json;
// using Wpf.Scheduler.Models.Services.ApiServices.Account;
// using Wpf.Scheduler.Services.ApiServices.Interfaces;
//
// namespace Wpf.Scheduler.Services.ApiServices.Classes;
//
// public class AccountApiService : ApiServiceBase, IAccountApiService
// {
//     private IConfigurationSection _endpoints;
//     
//     public AccountApiService(IConfiguration configuration, HttpClient client) : base(configuration, client)
//     {
//         _endpoints = Endpoints.GetSection("Account");
//     }
//
//     public async Task RegisterAsync(string id, string userName, string phoneNumber, string password, string confirmPassword)
//     {
//         var model = new RegisterModel
//         {
//             Id = id,
//             UserName = userName, 
//             PhoneNumber = phoneNumber, 
//             Password = password,
//             ConfirmPassword = confirmPassword
//         };
//
//         throw new NotImplementedException();
//     }
//
//     public async Task LoginAsync(string id, string password)
//     {
//         var model = new LoginModel { Id = id, Password = password };
//         var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
//         
//         HttpResponseMessage result;
//         try
//         {
//             result = await Client.PostAsync(_endpoints["Login"], content);
//         }
//         catch
//         {
//             MessageBox.Show("Login failed due to an unknown error.", "Failure", MessageBoxButton.OK,
//                 MessageBoxImage.Error);
//             return;
//         }
//         
//         if (result.IsSuccessStatusCode)
//         {
//             App.Token = await result.Content.ReadAsStringAsync();
//             MessageBox.Show("Logged in successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
//         }
//         else
//         {
//             MessageBox.Show("Unable to login.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
//         }
//     }
//
//     public async Task LogoutAsync()
//     {
//         App.Token = string.Empty;
//         
//         throw new NotImplementedException();
//     }
// }