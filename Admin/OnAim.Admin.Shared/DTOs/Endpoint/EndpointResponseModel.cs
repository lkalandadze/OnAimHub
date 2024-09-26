﻿using OnAim.Admin.Shared.DTOs.EndpointGroup;
using OnAim.Admin.Shared.DTOs.User;

namespace OnAim.Admin.Shared.DTOs.Endpoint;

public class EndpointResponseModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public string? Type { get; set; }
    public UserDto? CreatedBy { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateUpdated { get; set; }
    public DateTimeOffset DateDeleted { get; set; }
    public List<EndpointGroupDto> Groups { get; set; }
}
