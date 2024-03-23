// using System.Net.Http;
// using System.Net.Http.Json;
// using System.Text;
// using Microsoft.Extensions.Configuration;
// using Newtonsoft.Json;
// using Wpf.Scheduler.Models.DTOs.Entities;
// using Wpf.Scheduler.Services.ApiServices.Interfaces;
//
// namespace Wpf.Scheduler.Services.ApiServices.Classes;
//
// public class EmployeeApiService : ApiServiceBase, IEmployeeApiService
// {
//     private readonly IConfigurationSection _endpoints;
//     
//     public EmployeeApiService(IConfiguration configuration, HttpClient client) : base(configuration, client)
//     {
//         _endpoints = Endpoints.GetSection("Employee");
//     }
//
//     public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync()
//     {
//         Configure();
//
//         var response = await Client.GetAsync(_endpoints["GetEmployees"]);
//         if (!response.IsSuccessStatusCode) return new List<EmployeeDto>();
//         
//         var result = await response.Content.ReadFromJsonAsync<IEnumerable<EmployeeDto>>();
//         return result ?? new List<EmployeeDto>();
//     }
//
//     public async Task<IEnumerable<EmployeeDto>> GetActiveEmployeesAsync()
//     {
//         Configure();
//
//         var response = await Client.GetAsync(_endpoints["GetActiveEmployees"]);
//         if (!response.IsSuccessStatusCode) return new List<EmployeeDto>();
//         
//         var result = await response.Content.ReadFromJsonAsync<IEnumerable<EmployeeDto>>();
//         return result ?? new List<EmployeeDto>();
//     }
//
//     public async Task<IEnumerable<EmployeeDto>> GetAssignedEmployeesAsync()
//     {
//         Configure();
//
//         var response = await Client.GetAsync(_endpoints["GetAssignedEmployees"]);
//         if (!response.IsSuccessStatusCode) return new List<EmployeeDto>();
//         
//         var result = await response.Content.ReadFromJsonAsync<IEnumerable<EmployeeDto>>();
//         return result ?? new List<EmployeeDto>();
//     }
//
//     public async Task<EmployeeDto?> GetEmployeeAsync(int id)
//     {
//         Configure();
//         var uri = $"{_endpoints["GetEmployee"]}/{id}";
//         
//         var response = await Client.GetAsync(uri);
//         if (!response.IsSuccessStatusCode) return null;
//         
//         var result = await response.Content.ReadFromJsonAsync<EmployeeDto>();
//         return result;
//     }
//
//     public async Task<bool> PostEmployeeAsync(EmployeeDto employee)
//     {
//         Configure();
//         
//         var content = new StringContent(
//             JsonConvert.SerializeObject(employee), 
//             Encoding.UTF8, 
//             "application/json"
//             );
//         
//         var response = await Client.PostAsync(_endpoints["PostEmployee"], content);
//         return response.IsSuccessStatusCode;
//     }
//
//     public async Task<bool> PutEmployeeAsync(int id, EmployeeDto employee)
//     {
//         Configure();
//         
//         var content = new StringContent(
//             JsonConvert.SerializeObject(employee), 
//             Encoding.UTF8, 
//             "application/json"
//         );
//
//         var uri = $"{_endpoints["PutEmployee"]}/{id}";
//         var response = await Client.PutAsync(uri, content);
//         return response.IsSuccessStatusCode;
//     }
//
//     public async Task<bool> DeleteEmployeeAsync(int id)
//     {
//         Configure();
//         var uri = $"{_endpoints["DeleteEmployee"]}/{id}";
//
//         var response = await Client.DeleteAsync(uri);
//         return response.IsSuccessStatusCode;
//     }
// }
