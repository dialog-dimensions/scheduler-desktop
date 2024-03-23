using System.Net.Http;
using Microsoft.Extensions.Configuration;
using SchedulerDesktop.JWT.Interfaces;
using SchedulerDesktop.Models.DTOs.Entities;
using SchedulerDesktop.Models.Entities;
using SchedulerDesktop.Services.Api.BaseClasses;
using SchedulerDesktop.Services.Api.Interfaces;

namespace SchedulerDesktop.Services.Api.Classes;

public class ShiftApiService : ApiServiceBase<Shift, DateTime, ShiftDto>, IShiftApiService
{
    public ShiftApiService(IConfiguration configuration, IJwtTools jwt, HttpClient httpClient) : base(configuration, jwt, httpClient)
    {
    }

    public async Task UpdateEmployeeAsync(DateTime key, Shift shift)
    {
        await PatchRequestAsync(key, FlatShiftEmployeeDto.FromEntity(shift), "UpdateEmployee");
    }

    public async Task UpdateEmployeesAsync(IEnumerable<Shift> shifts)
    {
        await PatchRequestAsync(shifts.Select(FlatShiftEmployeeDto.FromEntity), "UpdateEmployees");
    }
}
