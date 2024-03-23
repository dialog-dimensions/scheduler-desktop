// using System.Net.Http;
// using System.Net.Http.Json;
// using System.Text;
// using System.Windows;
// using Microsoft.Extensions.Configuration;
// using Newtonsoft.Json;
// using Wpf.Scheduler.Extensions;
// using Wpf.Scheduler.Models.DTOs.Entities;
// using Wpf.Scheduler.Models.Entities;
// using Wpf.Scheduler.Models.Entities.Other_Objects;
// using Wpf.Scheduler.Models.ScheduleEngine;
// using Wpf.Scheduler.Services.ApiServices.Interfaces;
//
// namespace Wpf.Scheduler.Services.ApiServices.Classes;
//
// public class ScheduleApiService : ApiServiceBase, IScheduleApiService
// {
//     private readonly IConfigurationSection _endpoints;
//     
//     public ScheduleApiService(IConfiguration configuration, HttpClient client) : base(configuration, client)
//     {
//         _endpoints = Endpoints.GetSection("Schedule");
//     }
//
//     public Task<ScheduleReport> GetScheduleReportAsync(DateTime key, Schedule schedule)
//     {
//         throw new NotImplementedException();
//     }
//
//     public async Task PostScheduleAsync(Schedule schedule)
//     {
//         Configure();
//         
//         var dto = ScheduleDto.FromEntity(schedule);
//         var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
//
//         HttpResponseMessage response;
//         try
//         {
//             response = await Client.PostAsync(_endpoints["PostSchedule"], content);
//         }
//         catch
//         {
//             MessageBox.Show("Failed to post schedule.", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);
//             return;
//         }
//         
//         if (response.IsSuccessStatusCode)
//         {
//             MessageBox.Show("Schedule posted successfully.", "Success", MessageBoxButton.OK,
//                 MessageBoxImage.Information);
//             return;
//         }
//
//         MessageBox.Show("Unknown error.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
//     }
//
//     public async Task<IEnumerable<FlatSchedule>> GetFlatSchedulesAsync()
//     {
//         Configure();
//
//         var response = await Client.GetAsync(_endpoints["GetFlatSchedules"]);
//         if (response.IsSuccessStatusCode)
//         {
//             var result = await response.Content.ReadFromJsonAsync<IEnumerable<FlatSchedule>>();
//             return result ?? new List<FlatSchedule>();
//         }
//
//         MessageBox.Show("Unknown problem.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
//         return new List<FlatSchedule>();
//     }
//
//     public async Task<Schedule> GetScheduleAsync(DateTime scheduleKey)
//     {
//         Configure();
//         
//         var uri = $"{_endpoints["GetSchedule"]}/{scheduleKey.ToJsonString()}";
//         var response = await Client.GetAsync(uri);
//         if (response.IsSuccessStatusCode)
//         {
//             var dto = await response.Content.ReadFromJsonAsync<ScheduleDto>();
//             if (dto is not null) return dto.ToEntity();
//             
//             MessageBox.Show("Failed to retrieve schedule from database.");
//             throw new KeyNotFoundException("Failed to retrieve schedule from database.");
//
//         }
//
//         MessageBox.Show("Bad response from server.");
//         throw new InvalidOperationException("Bad response from server.");
//     }
//
//     public Task<ScheduleResults> RunSchedulerAsync(DateTime key)
//     {
//         Configure();
//         
//         
//     }
//
//     public async Task<Schedule> GetLatestScheduleAsync()
//     {
//         Configure();
//
//         var uri = _endpoints["GetLatestSchedule"];
//         var response = await Client.GetAsync(uri);
//         if (response.IsSuccessStatusCode)
//         {
//             var dto = await response.Content.ReadFromJsonAsync<ScheduleDto>();
//             if (dto is not null) return dto.ToEntity();
//             
//             MessageBox.Show("Failed to retrieve schedule from database.");
//             throw new KeyNotFoundException("Failed to retrieve schedule from database.");
//
//         }
//
//         MessageBox.Show("Bad response from server.");
//         throw new InvalidOperationException("Bad response from server.");
//     }
//
//     public Task<ScheduleData> GetScheduleDataAsync(DateTime key)
//     {
//         throw new NotImplementedException();
//     }
// }
