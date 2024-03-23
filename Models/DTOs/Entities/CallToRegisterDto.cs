using SchedulerDesktop.Models.DTOs.Interfaces;

namespace SchedulerDesktop.Models.DTOs.Entities;

public class CallToRegisterDto : IDto<CallToRegisterDto, CallToRegisterDto>
{
    public int Id { get; set; }
    public string PhoneNumber { get; set; }

    public static CallToRegisterDto FromEntity(CallToRegisterDto entity) => entity;
    public CallToRegisterDto ToEntity() => this;
}