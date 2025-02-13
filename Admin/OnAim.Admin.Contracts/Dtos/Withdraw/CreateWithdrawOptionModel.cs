﻿namespace OnAim.Admin.Contracts.Dtos.Withdraw;

public class CreateWithdrawOptionModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string Endpoint { get; set; }
    public EndpointContentType ContentType { get; set; }
    public string EndpointContent { get; set; }
    public bool IsTemplate { get; set; }
}
