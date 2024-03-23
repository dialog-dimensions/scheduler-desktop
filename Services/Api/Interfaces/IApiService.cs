using System.Net.Http;
using Microsoft.Extensions.Configuration;
using SchedulerDesktop.Models.DTOs.Interfaces;

namespace SchedulerDesktop.Services.Api.Interfaces;

public interface IApiService<T, in TKey, in TDto> where TDto : IDto<T, TDto>
{
    IConfigurationSection Endpoints { get; set; }
    HttpClient HttpClient { get; set; }

    // 'GET' METHODS
    Task<T?> GetRequestAsync(TKey key, string endpointParamName);
    Task<IEnumerable<T>?> GetRequestAsync(string endpointParamName);
    Task<TType?> GetRequestAsync<TType, TTypeDto>(TKey key, string endpointParamName) where TTypeDto : IDto<TType, TTypeDto>;
    Task<IEnumerable<TType>?> GetRequestAsync<TType, TTypeDto>(string endpointParamName) where TTypeDto : IDto<TType, TTypeDto>;

    // 'POST' METHODS
    Task PostRequestAsync(T entity, string endpointParamName);
    Task PostRequestAsync<TAlternativeDto>(T entity, string endpointParamName) where TAlternativeDto : IDto<T, TAlternativeDto>;
    Task<TResponse?> PostRequestAsync<TResponse, TResponseDto>(T entity, string endpointParamName) where TResponseDto : IDto<TResponse, TResponseDto>;
    Task PostRequestAsync(IEnumerable<T> entities, string endpointParamName);
    Task<TResponse?> PostRequestAsync<TResponse, TResponseDto>(IEnumerable<T> entities, string endpointParamName) where TResponseDto : IDto<TResponse, TResponseDto>;
    Task<bool> PostRequestAsync(string endpointParamName);
    Task<bool> PostRequestAsync<TType>(TType entity, string endpointParamName);
    
    // 'PUT' METHODS
    Task PutRequestAsync(TKey key, T entity, string endpointParamName);
    Task<TResponse?> PutRequestAsync<TResponse, TResponseDto>(TKey key, T entity, string endpointParamName) where TResponseDto : IDto<TResponse, TResponseDto>;
    Task PutRequestAsync(IEnumerable<T> entities, string endpointParamName);
    Task<TResponse?> PutRequestAsync<TResponse, TResponseDto>(IEnumerable<T> entities, string endpointParamName) where TResponseDto : IDto<TResponse, TResponseDto>;

    // 'PATCH' METHODS
    Task PatchRequestAsync<TPatchDto>(TKey key, TPatchDto dto, string endpointParamName) where TPatchDto : IDto<T, TPatchDto>;
    Task<TResponse?> PatchRequestAsync<TPatchDto, TResponse, TResponseDto>(TKey key, TPatchDto dto, string endpointParamName) where TPatchDto : IDto<T, TPatchDto> where TResponseDto : IDto<TResponse, TResponseDto>;
    Task PatchRequestAsync<TPatchDto>(IEnumerable<TPatchDto> dtos, string endpointParamName) where TPatchDto : IDto<T, TPatchDto>;
    Task<TResponse?> PatchRequestAsync<TPatchDto, TResponse, TResponseDto>(IEnumerable<TPatchDto> dto, string endpointParamName) where TPatchDto : IDto<T, TPatchDto> where TResponseDto : IDto<TResponse, TResponseDto>;

    // 'DELETE' METHODS
    Task DeleteRequestAsync(TKey key, string endpointParamName);
    Task<TResponse?> DeleteRequestAsync<TResponse, TResponseDto>(TKey key, string endpointParamName) where TResponseDto : IDto<TResponse, TResponseDto>;
}