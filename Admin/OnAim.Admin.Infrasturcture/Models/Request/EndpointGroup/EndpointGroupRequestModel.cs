﻿namespace OnAim.Admin.Infrasturcture.Models.Request.EndpointGroup
{
    public class EndpointGroupRequestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> EndpointIds { get; set; }
    }
}