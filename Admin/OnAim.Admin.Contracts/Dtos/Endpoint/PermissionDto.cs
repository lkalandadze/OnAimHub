﻿namespace OnAim.Admin.Contracts.Dtos.Endpoint;

public class PermissionDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
}