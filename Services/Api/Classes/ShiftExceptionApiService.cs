using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using SchedulerDesktop.Extensions;
using SchedulerDesktop.JWT.Interfaces;
using SchedulerDesktop.Models.DTOs.Entities;
using SchedulerDesktop.Models.Entities.Other_Objects;
using SchedulerDesktop.Services.Api.BaseClasses;
using SchedulerDesktop.Services.Api.Interfaces;

namespace SchedulerDesktop.Services.Api.Classes;

public class ShiftExceptionApiService : ApiServiceBase<ShiftException, (DateTime, int), ShiftExceptionDto>, IShiftExceptionApiService
{
    public ShiftExceptionApiService(IConfiguration configuration, IJwtTools jwt, HttpClient httpClient) : base(configuration, jwt, httpClient)
    {
    }

    public async Task<IEnumerable<ShiftException>?> GetExceptionsAsync()
    { 
        return await GetRequestAsync("GetExceptions");
    }

    public async Task<IEnumerable<ShiftException>?> GetEmployeeExceptionsAsync(int employeeId)
    {
        var url = $"{Endpoints["GetEmployeeExceptions"]}/{employeeId}";
        var response = await HttpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var responseDtos = await response.Content.ReadFromJsonAsync<IEnumerable<ShiftExceptionDto>>();
            return responseDtos?.Select(dto => dto.ToEntity());
        }

        throw new UnsuccessfulHttpRequestException(response);
    }

    public async Task<IEnumerable<ShiftException>?> GetScheduleExceptionsAsync(DateTime scheduleKey)
    {
        var url = $"{Endpoints["GetEmployeeExceptions"]}/{scheduleKey.ToJsonString()}";
        var response = await HttpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var responseDtos = await response.Content.ReadFromJsonAsync<IEnumerable<ShiftExceptionDto>>();
            return responseDtos?.Select(dto => dto.ToEntity());
        }

        throw new UnsuccessfulHttpRequestException(response);
    }

    public async Task<ShiftException?> GetExceptionAsync((DateTime, int) key)
    {
        return await GetRequestAsync(key,"GetException");
    }

    public async Task PostExceptionAsync(ShiftException exception)
    {
        await PostRequestAsync(exception, "PostException");
    }

    public async Task DeleteExceptionAsync((DateTime, int) key)
    {
        await DeleteRequestAsync(key, "DeleteException");
    }
}