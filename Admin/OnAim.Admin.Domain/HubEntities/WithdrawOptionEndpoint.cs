﻿using OnAim.Admin.Domain.HubEntities.Enum;

namespace OnAim.Admin.Domain.HubEntities;

public class WithdrawOptionEndpoint
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Endpoint { get; set; }
    public EndpointContentType ContentType { get; set; }
    public string Content { get; set; }
    public int? CreatedByUserId { get; set; }
}