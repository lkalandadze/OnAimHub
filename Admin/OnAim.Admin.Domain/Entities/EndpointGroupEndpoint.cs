﻿namespace OnAim.Admin.Domain.Entities;

public class EndpointGroupEndpoint
{
    public EndpointGroupEndpoint()
    {
        
    }
    public EndpointGroupEndpoint(int endpointGroupId, int endpointId)
    {
        EndpointGroupId = endpointGroupId;
        EndpointId = endpointId;
        IsActive = true;
    }

    public int EndpointGroupId { get; set; }
    public EndpointGroup EndpointGroup { get; set; }
    public int EndpointId { get; set; }
    public Endpoint Endpoint { get; set; }
    public bool IsActive { get; set; }

}
