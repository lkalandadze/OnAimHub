using Nito.AsyncEx;
using OnAim.Admin.Shared.DTOs;
using System.Net.Http.Json;

namespace OnAim.Admin.Domain.Exceptions;

public class HubAPIRequestFailedException : Exception
{
    public HubAPIRequestFailedException(HttpResponseMessage res) :
    base(AsyncContext.Run(async () => (await res.Content.ReadFromJsonAsync<IEnumerable<HubAPIErrorResponseDTO>>()).FirstOrDefault().Message))
    { }

    public HubAPIRequestFailedException(string msg) : base(msg) { }

}
