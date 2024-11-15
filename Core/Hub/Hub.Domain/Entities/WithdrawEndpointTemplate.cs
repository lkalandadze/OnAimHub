﻿#nullable disable

using Hub.Domain.Enum;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class WithdrawEndpointTemplate : BaseEntity<int>
{
    public WithdrawEndpointTemplate()
    {
        
    }

    public WithdrawEndpointTemplate(string name, string endpoint, string endpointContent, EndpointContentType contentType)
    {
        Name = name;
        Endpoint = endpoint;
        EndpointContent = endpointContent;
        ContentType = contentType;
    }

    public string Name { get; set; }
    public string Endpoint { get; private set; }
    public string EndpointContent { get; private set; }
    public EndpointContentType ContentType { get; private set; }

    public void Update(string name, string endpoint, string endpointContent, EndpointContentType contentType)
    {
        Name = name;
        Endpoint = endpoint;
        EndpointContent = endpointContent;
        ContentType = contentType;
    }
}