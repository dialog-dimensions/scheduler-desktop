using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using SchedulerDesktop.JWT.Interfaces;
using SchedulerDesktop.Models.DTOs.Entities;
using SchedulerDesktop.Models.DTOs.ScheduleEngineModels;
using SchedulerDesktop.Models.Entities;
using SchedulerDesktop.Models.Entities.Other_Objects;
using SchedulerDesktop.Models.ScheduleEngine;
using SchedulerDesktop.Services.Api.BaseClasses;
using SchedulerDesktop.Services.Api.Interfaces;

namespace SchedulerDesktop.Services.Api.Classes;

public class ScheduleApiService : ApiServiceBase<Schedule, DateTime, ScheduleDto>, IScheduleApiService
{
    public ScheduleApiService(IConfiguration configuration, IJwtTools jwt, HttpClient httpClient) : base(configuration, jwt, httpClient)
    {
    }

    public async Task<IEnumerable<FlatSchedule>?> GetFlatSchedulesAsync()
    {
        return await GetRequestAsync<FlatSchedule, FlatScheduleDto>("GetFlatSchedules");
    }

    public async Task<Schedule?> GetScheduleAsync(DateTime key)
    {
        return await GetRequestAsync(key, "GetSchedule");
    }

    public async Task<Schedule?> GetLatestScheduleAsync()
    {
        Configure();
        
        var url = Endpoints["GetLatestSchedule"]!;
        var response = await HttpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }
            var responseDto = await response.Content.ReadFromJsonAsync<ScheduleDto>();
            return responseDto?.ToEntity();
        }

        throw new UnsuccessfulHttpRequestException(response);
    }

    public async Task<ScheduleResults?> RunSchedulerAsync(DateTime key, Schedule schedule)
    {
        var results = await PostRequestAsync<ScheduleResults, ScheduleResultsDto>(key, schedule, "RunScheduler");
        if (results is null)
        {
            return null;
        }

        return results;
    }

    public async Task<ScheduleData?> GetScheduleDataAsync(DateTime key)
    {
        return await GetRequestAsync<ScheduleData, ScheduleDataDto>(key, "GetScheduleData");
    }

    public async Task<ScheduleReport?> GetScheduleReportAsync(DateTime key, Schedule schedule)
    {
        return await PostRequestAsync<ScheduleReport, ScheduleReportDto>(key, schedule, "GetScheduleReport");
    }

    public async Task PostScheduleAsync(Schedule schedule)
    {
        await PostRequestAsync(schedule, "PostSchedule");
    }

    public async Task AssignEmployeesAsync(Schedule schedule)
    {
        await PatchRequestAsync(schedule.StartDateTime, ScheduleDto.FromEntity(schedule), "AssignEmployees");
    }

    public async Task<bool> WakeScannerAsync()
    {
        return await PostRequestAsync("WakeScanner");
    }

    public async Task<bool> TerminateScannerAsync()
    {
        return await PostRequestAsync("TerminateScanner");
    }
}
