using System.Net.Http;
using System.Windows;
using Microsoft.Extensions.Configuration;
using SchedulerDesktop.JWT.Interfaces;
using SchedulerDesktop.Models.DTOs.Entities;
using SchedulerDesktop.Models.Entities;
using SchedulerDesktop.Services.Api.BaseClasses;
using SchedulerDesktop.Services.Api.Interfaces;

namespace SchedulerDesktop.Services.Api.Classes;

public class EmployeeApiService : ApiServiceBase<Employee, int, EmployeeDto>, IEmployeeApiService
{
    public EmployeeApiService(IConfiguration configuration, IJwtTools jwt, HttpClient httpClient) : base(configuration, jwt, httpClient) { }

    
    public async Task<Employee?> GetEmployeeAsync(int key)
    {
        return await GetRequestAsync(key, "GetEmployee");
    }

    public async Task<IEnumerable<Employee>?> GetEmployeesAsync()
    {
        try
        {
            return await GetRequestAsync("GetEmployees");
        }
        catch (UnsuccessfulHttpRequestException ex)
        {
            MessageBox.Show(ex.Message, "Failed Request", MessageBoxButton.OK, MessageBoxImage.Error);
            throw;
        }
    }

    public async Task<IEnumerable<Employee>?> GetActiveEmployeesAsync()
    {
        return await GetRequestAsync("GetActiveEmployees");
    }

    public async Task<IEnumerable<Employee>?> GetAssignedEmployeesAsync()
    {
        return await GetRequestAsync("GetAssignedEmployees");
    }



    public async Task PostEmployeeAsync(Employee employee)
    {
        try
        {
            await PostRequestAsync(employee, "PostEmployee");
        }
        catch (UnsuccessfulHttpRequestException ex)
        {
            MessageBox.Show(ex.Message, ex.StatusCode.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    
    
    public async Task PutEmployeeAsync(int id, Employee employee)
    {
        try
        {
            await PutRequestAsync(id, employee, "PutEmployee");
        }
        catch (UnsuccessfulHttpRequestException ex)
        {
            MessageBox.Show(ex.Message, ex.StatusCode.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }



    public async Task DeleteEmployeeAsync(int id)
    {
        try
        {
            await DeleteRequestAsync(id, "DeleteEmployee");
        }
        catch (UnsuccessfulHttpRequestException ex)
        {
            MessageBox.Show(ex.Message, ex.StatusCode.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}