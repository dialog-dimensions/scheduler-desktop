using System.Net.Http;
using System.Windows;
using Microsoft.Extensions.Configuration;
using SchedulerDesktop.JWT.Interfaces;
using SchedulerDesktop.Models.DTOs.Entities;
using SchedulerDesktop.Models.Entities.Other_Objects;
using SchedulerDesktop.Services.Api.BaseClasses;
using SchedulerDesktop.Services.Api.Interfaces;

namespace SchedulerDesktop.Services.Api.Classes;

public class ShiftSwapApiService : ApiServiceBase<ShiftSwap, int, ShiftSwapDto>, IShiftSwapApiService
{
    public ShiftSwapApiService(IConfiguration configuration, IJwtTools jwt, HttpClient httpClient) : base(configuration, jwt, httpClient)
    {
    }

    public async Task<IEnumerable<ShiftSwap>?> GetSwapsAsync()
    {
        try
        {
            return await GetRequestAsync("GetShiftSwaps");
        }
        catch (UnsuccessfulHttpRequestException ex)
        {
            MessageBox.Show(ex.ReasonPhrase, ex.StatusCode.ToString(), MessageBoxButton.OK,
                MessageBoxImage.Exclamation);
            return new List<ShiftSwap>();
        }
    }
}
