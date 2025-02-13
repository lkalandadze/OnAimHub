﻿using OnAim.Admin.Domain.Entities.Abstract;
using OnAim.Admin.Contracts.Models;

namespace OnAim.Admin.Domain.Entities;

public class AllowedEmailDomain : BaseEntity
{
    public AllowedEmailDomain()
    {
        
    }
    public AllowedEmailDomain(string domain, int? createdBy)
    {
        Domain = domain;
        CreatedBy = createdBy;
        IsDeleted = false;
        DateCreated = SystemDate.Now;
        IsActive = true;
    }

    public string Domain { get; set; }
    public int? CreatedBy { get; set; }
    public bool IsDeleted { get; set; }
}
