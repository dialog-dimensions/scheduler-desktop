// using System.Net.Http;
// using System.Net.Http.Headers;
// using Microsoft.Extensions.Configuration;
//
// namespace Wpf.Scheduler.Services.ApiServices;
//
// public abstract class ApiServiceBase
// {
//     protected readonly HttpClient Client;
//     protected readonly IConfigurationSection EndpointsSection;
//     protected abstract IConfigurationSection Endpoints { get; set; }
//
//     protected ApiServiceBase(IConfiguration configuration, HttpClient client)
//     {
//         Client = client;
//         Endpoints = configuration.GetSection("Api:Endpoints");
//     }
//
//     protected bool Configure()
//     {
//         var token = App.Token;
//         if (string.IsNullOrEmpty(token))
//         {
//             return false;
//         }
//         Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
//         return true;
//     }
// }
