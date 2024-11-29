﻿#nullable disable

using Hub.Domain.Entities.Coins;
using Hub.Domain.Entities.Templates;
using Hub.Domain.Enum;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class WithdrawOption : BaseEntity<int>
{
    public WithdrawOption()
    {
    }

    public WithdrawOption(
        string title, 
        string description, 
        string imageUrl,
        EndpointContentType contentType,
        string endpoint = null, 
        string endpointContent = null, 
        int? endpointTemplate = null,
        IEnumerable<OutCoin> outCoins = null)
    {
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        ContentType = contentType;
        Endpoint = endpoint;
        EndpointContent = endpointContent;
        WithdrawEndpointTemplateId = endpointTemplate;
        OutCoins = outCoins?.ToList() ?? [];
    }

    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string Endpoint { get; set; }
    public EndpointContentType ContentType { get; set; }
    public string EndpointContent { get; set; }

    public int? WithdrawEndpointTemplateId { get; private set; }
    public WithdrawOptionEndpoint WithdrawEndpointTemplate { get; set; }

    public ICollection<WithdrawOptionGroup> WithdrawOptionGroups { get; set; }
    public ICollection<OutCoin> OutCoins { get; set; }

    public void Update(
        string title, 
        string description, 
        string imageUrl, 
        string endpoint = null, 
        string endpointContent = null, 
        int? endpointTemplate = null, 
        IEnumerable<OutCoin> outCoins = null)
    {
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        Endpoint = endpoint;
        EndpointContent = endpointContent;
        WithdrawEndpointTemplateId = endpointTemplate;
        OutCoins = outCoins?.ToList() ?? [];
    }
}