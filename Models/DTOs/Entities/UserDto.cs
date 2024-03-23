using SchedulerDesktop.Models.DTOs.Interfaces;
using SchedulerDesktop.Models.Entities.Other_Objects;

namespace SchedulerDesktop.Models.DTOs.Entities;

public class UserDto : User, IDto<User, UserDto>
{
    public static UserDto FromEntity(User entity) => new()
    {
        Id = entity.Id,
        UserName = entity.UserName,
        PhoneNumber = entity.PhoneNumber
    };

    public User ToEntity() => this;
}
