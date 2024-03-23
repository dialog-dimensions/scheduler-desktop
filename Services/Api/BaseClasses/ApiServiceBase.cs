using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SchedulerDesktop.Extensions;
using SchedulerDesktop.JWT.Interfaces;
using SchedulerDesktop.Models.DTOs.Interfaces;
using SchedulerDesktop.Services.Api.Interfaces;

namespace SchedulerDesktop.Services.Api.BaseClasses;

public abstract class ApiServiceBase<T, TKey, TDto> : IApiService<T, TKey, TDto> where TDto : IDto<T, TDto>
{
    private readonly IJwtTools _jwt;
    
    public HttpClient HttpClient { get; set; }
    public IConfigurationSection Endpoints { get; set; }
    
    
    protected ApiServiceBase(IConfiguration configuration, IJwtTools jwt, HttpClient httpClient)
    {
        _jwt = jwt;
        HttpClient = httpClient;
        Endpoints = configuration.GetSection($"Api:Endpoints:{typeof(T).Name}");
    }

    
    protected void Configure()
    {
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwt.TryGetToken());
    }
    protected class UnsuccessfulHttpRequestException(HttpResponseMessage response) : HttpRequestException
    {
        public new HttpStatusCode StatusCode { get; } = response.StatusCode;
        public string? ReasonPhrase { get; } = response.ReasonPhrase;
        public override string Message { get; } = $"{response.StatusCode} {response.ReasonPhrase}";
    }
    
    
    // 'GET' METHODS
    public virtual async Task<T?> GetRequestAsync(TKey key, string endpointParamName)
    {
        try
        {

            return await GetRequestAsync<T, TDto>(key, endpointParamName);
        }
        catch
        {
            throw;
        }
    }

    public virtual async Task<IEnumerable<T>?> GetRequestAsync(string endpointParamName)
    {
        try
        {
            return await GetRequestAsync<T, TDto>(endpointParamName);
        }
        catch
        {
            throw;
        }
    }

    public virtual async Task<TType?> GetRequestAsync<TType, TTypeDto>(TKey key, string endpointParamName) where TTypeDto : IDto<TType, TTypeDto>
    {
        Configure();
        var keyToString = key is DateTime dateTime ? dateTime.ToJsonString() : key?.ToString();
        var url = $"{Endpoints[endpointParamName]!}/{keyToString}";
        var response = await HttpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var dto = await response.Content.ReadFromJsonAsync<TTypeDto>();
            return dto is null ? default : dto.ToEntity();
        }
        
        MessageBox.Show(response.ReasonPhrase, response.StatusCode.ToString(), MessageBoxButton.OK,
            MessageBoxImage.Error);
        throw new UnsuccessfulHttpRequestException(response);
    }

    public virtual async Task<IEnumerable<TType>?> GetRequestAsync<TType, TTypeDto>(string endpointParamName) where TTypeDto : IDto<TType, TTypeDto>
    {
        Configure();

        var url = Endpoints[endpointParamName]!;
        var response = await HttpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var dtos = await response.Content.ReadFromJsonAsync<IEnumerable<TTypeDto>?>();
            return dtos?.Select(dto => dto.ToEntity());
        }
        
        MessageBox.Show(response.ReasonPhrase, response.StatusCode.ToString(), MessageBoxButton.OK,
            MessageBoxImage.Error);
        throw new UnsuccessfulHttpRequestException(response);
    }

    
    // 'POST' METHODS
    public virtual async Task PostRequestAsync(T entity, string endpointParamName)
    {
        await PostRequestAsync<TDto>(entity, endpointParamName);
    }

    public virtual async Task PostRequestAsync(TKey key, T entity, string endpointParamName)
    {
        await PostRequestAsync<TDto>(key, entity, endpointParamName);
    }

    public virtual async Task PostRequestAsync<TDto>(T entity, string endpointParamName) where TDto : IDto<T, TDto>
    {
        Configure();

        var url = Endpoints[endpointParamName];
        var dto = TDto.FromEntity(entity);
        var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync(url, content);
        if (response.IsSuccessStatusCode)
        {
            MessageBox.Show($"Created successfully.");
            return;
        }
        
        MessageBox.Show(response.ReasonPhrase, response.StatusCode.ToString(), MessageBoxButton.OK,
            MessageBoxImage.Error);
        throw new UnsuccessfulHttpRequestException(response);
    }

    public virtual async Task PostRequestAsync<TDto>(TKey key, T entity, string endpointParamName)
        where TDto : IDto<T, TDto>
    {
        Configure();

        var url = $"{Endpoints[endpointParamName]}/{(key is DateTime dateTime ? dateTime.ToJsonString() : key.ToString())}";
        var dto = TDto.FromEntity(entity);
        var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync(url, content);
        if (response.IsSuccessStatusCode)
        {
            MessageBox.Show("Success!");
            return;
        }

        MessageBox.Show(response.ReasonPhrase, response.StatusCode.ToString(), MessageBoxButton.OK,
            MessageBoxImage.Error);
        throw new UnsuccessfulHttpRequestException(response);
    }

    public virtual async Task<TResponse?> PostRequestAsync<TResponse, TResponseDto>(T entity, string endpointParamName) where TResponseDto : IDto<TResponse, TResponseDto>
    {
        Configure();

        var url = Endpoints[endpointParamName]!;
        var dto = TDto.FromEntity(entity);
        var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync(url, content);
        if (response.IsSuccessStatusCode)
        {
            var responseDto = await response.Content.ReadFromJsonAsync<TResponseDto>();
            return responseDto is null ? default : responseDto.ToEntity();
        }
        
        MessageBox.Show(response.ReasonPhrase, response.StatusCode.ToString(), MessageBoxButton.OK,
            MessageBoxImage.Error);
        throw new UnsuccessfulHttpRequestException(response);
    }

    public virtual async Task<TResponse?> PostRequestAsync<TResponse, TResponseDto>(TKey key, T entity,
        string endpointParamName) where TResponseDto : IDto<TResponse, TResponseDto>
    {
        Configure();

        var url = $"{Endpoints[endpointParamName]}/{(key is DateTime dateTime ? dateTime.ToJsonString() : key!.ToString())}";
        var dto = TDto.FromEntity(entity);
        var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync(url, content);
        if (response.IsSuccessStatusCode)
        {
            var responseDto = await response.Content.ReadFromJsonAsync<TResponseDto>();
            if (responseDto is not null)
            {
                return responseDto.ToEntity();
            }
        }

        MessageBox.Show(response.ReasonPhrase, response.StatusCode.ToString(), MessageBoxButton.OK,
            MessageBoxImage.Error);
        throw new UnsuccessfulHttpRequestException(response);
    }

    public virtual async Task PostRequestAsync(IEnumerable<T> entities, string endpointParamName)
    {
        Configure();

        var url = Endpoints[endpointParamName]!;
        var dtos = entities.Select(TDto.FromEntity);
        var content = new StringContent(JsonConvert.SerializeObject(dtos), Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync(url, content);
        if (response.IsSuccessStatusCode)
        {
            return;
        }
        
        MessageBox.Show(response.ReasonPhrase, response.StatusCode.ToString(), MessageBoxButton.OK,
            MessageBoxImage.Error);
        throw new UnsuccessfulHttpRequestException(response);
    }

    public virtual async Task<TResponse?> PostRequestAsync<TResponse, TResponseDto>(IEnumerable<T> entities, string endpointParamName) where TResponseDto : IDto<TResponse, TResponseDto>
    {
        Configure();

        var url = Endpoints[endpointParamName]!;
        var dtos = entities.Select(TDto.FromEntity);
        var content = new StringContent(JsonConvert.SerializeObject(dtos), Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync(url, content);
        if (response.IsSuccessStatusCode)
        {
            var responseDto = await response.Content.ReadFromJsonAsync<TResponseDto>();
            return responseDto is null ? default : responseDto.ToEntity();
        }
        
        MessageBox.Show(response.ReasonPhrase, response.StatusCode.ToString(), MessageBoxButton.OK,
            MessageBoxImage.Error);
        throw new UnsuccessfulHttpRequestException(response);
    }

    public virtual async Task<bool> PostRequestAsync(string endpointParamName)
    {
        Configure();

        var url = Endpoints[endpointParamName]!;
        var response = await HttpClient.PostAsync(url, null);
        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }

    public virtual async Task<bool> PostRequestAsync<TType>(TType entity, string endpointParamName)
    {
        Configure();

        var url = Endpoints[endpointParamName]!;
        var content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync(url, content);
        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }

    
    // 'PUT' METHODS
    public virtual async Task PutRequestAsync(TKey key, T entity, string endpointParamName)
    {
        Configure();

        var url = $"{Endpoints[endpointParamName]}/{key!.ToString()}";
        var dto = TDto.FromEntity(entity);
        var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        var response = await HttpClient.PutAsync(url, content);
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        MessageBox.Show(response.ReasonPhrase, response.StatusCode.ToString(), MessageBoxButton.OK,
            MessageBoxImage.Error);
        throw new UnsuccessfulHttpRequestException(response);
    }

    public virtual async Task<TResponse?> PutRequestAsync<TResponse, TResponseDto>(TKey key, T entity, string endpointParamName) where TResponseDto : IDto<TResponse, TResponseDto>
    {
        Configure();

        var url = $"{Endpoints[endpointParamName]}/{key!.ToString()}";
        var dto = TDto.FromEntity(entity);
        var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        var response = await HttpClient.PutAsync(url, content);
        if (response.IsSuccessStatusCode)
        {
            var responseDto = await response.Content.ReadFromJsonAsync<TResponseDto>();
            return responseDto is null ? default : responseDto.ToEntity();
        }

        MessageBox.Show(response.ReasonPhrase, response.StatusCode.ToString(), MessageBoxButton.OK,
            MessageBoxImage.Error);
        throw new UnsuccessfulHttpRequestException(response);
    }

    public virtual async Task PutRequestAsync(IEnumerable<T> entities, string endpointParamName)
    {
        Configure();

        var url = Endpoints[endpointParamName]!;
        var dtos = entities.Select(TDto.FromEntity);
        var content = new StringContent(JsonConvert.SerializeObject(dtos), Encoding.UTF8, "application/json");
        var response = await HttpClient.PutAsync(url, content);
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        MessageBox.Show(response.ReasonPhrase, response.StatusCode.ToString(), MessageBoxButton.OK,
            MessageBoxImage.Error);
        throw new UnsuccessfulHttpRequestException(response);
    }

    public virtual async Task<TResponse?> PutRequestAsync<TResponse, TResponseDto>(IEnumerable<T> entities, string endpointParamName) where TResponseDto : IDto<TResponse, TResponseDto>
    {
        Configure();

        var url = Endpoints[endpointParamName]!;
        var dtos = entities.Select(TDto.FromEntity);
        var content = new StringContent(JsonConvert.SerializeObject(entities), Encoding.UTF8, "application/json");
        var response = await HttpClient.PutAsync(url, content);
        if (response.IsSuccessStatusCode)
        {
            var responseDto = await response.Content.ReadFromJsonAsync<TResponseDto>();
            return responseDto is null ? default : responseDto.ToEntity();
        }

        MessageBox.Show(response.ReasonPhrase, response.StatusCode.ToString(), MessageBoxButton.OK,
            MessageBoxImage.Error);
        throw new UnsuccessfulHttpRequestException(response);
    }

    
    // 'PATCH' METHODS
    public virtual async Task PatchRequestAsync<TPatchDto>(TKey key, TPatchDto dto, string endpointParamName) where TPatchDto : IDto<T, TPatchDto>
    {
        Configure();

        var url = $"{Endpoints[endpointParamName]}/{(key is DateTime dateTime ? dateTime.ToJsonString() : key!.ToString())}";
        var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        var response = await HttpClient.PatchAsync(url, content);
        if (response.IsSuccessStatusCode)
        {
            MessageBox.Show("Success.");
            return;
        }

        MessageBox.Show(response.ReasonPhrase, response.StatusCode.ToString(), MessageBoxButton.OK,
            MessageBoxImage.Error);
        throw new UnsuccessfulHttpRequestException(response);
    }

    public virtual async Task<TResponse?> PatchRequestAsync<TPatchDto, TResponse, TResponseDto>(TKey key, TPatchDto dto, string endpointParamName) where TPatchDto : IDto<T, TPatchDto> where TResponseDto : IDto<TResponse, TResponseDto>
    {
        Configure();

        var url = $"{Endpoints[endpointParamName]}/{key!.ToString()}";
        var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        var response = await HttpClient.PatchAsync(url, content);
        if (response.IsSuccessStatusCode)
        {
            var responseDto = await response.Content.ReadFromJsonAsync<TResponseDto>();
            return responseDto is null ? default : responseDto.ToEntity();
        }

        MessageBox.Show(response.ReasonPhrase, response.StatusCode.ToString(), MessageBoxButton.OK,
            MessageBoxImage.Error);
        throw new UnsuccessfulHttpRequestException(response);
    }

    public virtual async Task PatchRequestAsync<TPatchDto>(IEnumerable<TPatchDto> dtos, string endpointParamName) where TPatchDto : IDto<T, TPatchDto>
    {
        Configure();

        var url = $"{Endpoints[endpointParamName]}";
        var content = new StringContent(JsonConvert.SerializeObject(dtos), Encoding.UTF8, "application/json");
        var response = await HttpClient.PatchAsync(url, content);
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        MessageBox.Show(response.ReasonPhrase, response.StatusCode.ToString(), MessageBoxButton.OK,
            MessageBoxImage.Error);
        throw new UnsuccessfulHttpRequestException(response);
    }

    public virtual async Task<TResponse?> PatchRequestAsync<TPatchDto, TResponse, TResponseDto>(IEnumerable<TPatchDto> dto, string endpointParamName) where TPatchDto : IDto<T, TPatchDto> where TResponseDto : IDto<TResponse, TResponseDto>
    {
        Configure();

        var url = Endpoints[endpointParamName]!;
        var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        var response = await HttpClient.PatchAsync(url, content);
        if (response.IsSuccessStatusCode)
        {
            var responseDto = await response.Content.ReadFromJsonAsync<TResponseDto>();
            return responseDto is null ? default : responseDto.ToEntity();
        }

        MessageBox.Show(response.ReasonPhrase, response.StatusCode.ToString(), MessageBoxButton.OK,
            MessageBoxImage.Error);
        throw new UnsuccessfulHttpRequestException(response);
    }

    
    // 'DELETE' METHODS
    public virtual async Task DeleteRequestAsync(TKey key, string endpointParamName)
    {
        Configure();

        var url = $"{Endpoints[endpointParamName]!}/{key!.ToString()}";
        var response = await HttpClient.DeleteAsync(url);
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        MessageBox.Show(response.ReasonPhrase, response.StatusCode.ToString(), MessageBoxButton.OK,
            MessageBoxImage.Error);
        throw new UnsuccessfulHttpRequestException(response);
    }

    public virtual async Task<TResponse?> DeleteRequestAsync<TResponse, TResponseDto>(TKey key, string endpointParamName) where TResponseDto : IDto<TResponse, TResponseDto>
    {
        Configure();

        var url = $"{Endpoints[endpointParamName]!}/{key!.ToString()}";
        var response = await HttpClient.DeleteAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var responseDto = await response.Content.ReadFromJsonAsync<TResponseDto>();
            return responseDto is null ? default : responseDto.ToEntity();

        }

        MessageBox.Show(response.ReasonPhrase, response.StatusCode.ToString(), MessageBoxButton.OK,
            MessageBoxImage.Error);
        throw new UnsuccessfulHttpRequestException(response);
    }
}