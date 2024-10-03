﻿using OnAim.Admin.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace OnAim.Admin.Domain.Entities.Abstract;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateUpdated { get; set; }
    public DateTimeOffset DateDeleted { get; set; }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
        IsActive = false;
        DateDeleted = SystemDate.Now;
    }
}