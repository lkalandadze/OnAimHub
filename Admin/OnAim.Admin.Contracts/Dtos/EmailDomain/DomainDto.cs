﻿namespace OnAim.Admin.Contracts.Dtos.EmailDomain;

public class DomainDto
{
    public int Id { get; set; }
    public string Domain { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
}
