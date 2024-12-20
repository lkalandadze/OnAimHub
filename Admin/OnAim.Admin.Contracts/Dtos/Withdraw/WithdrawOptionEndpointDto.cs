﻿namespace OnAim.Admin.Contracts.Dtos.Withdraw;

public class WithdrawOptionEndpointDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Endpoint { get; set; }
    public EndpointContentType ContentType { get; set; }
    public string Content { get; set; }
}
