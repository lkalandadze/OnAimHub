using Nito.AsyncEx;
using OnAim.Admin.Contracts.ApplicationInfrastructure.Validation;
using System.Net.Http.Json;

namespace OnAim.Admin.CrossCuttingConcerns.Exceptions;

public class HubAPIRequestFailedException : Exception
{
    public HubAPIRequestFailedException(HttpResponseMessage res) :
    base(AsyncContext.Run(async () => (await res.Content.ReadFromJsonAsync<IEnumerable<HubAPIErrorResponseDTO>>()).FirstOrDefault().Message))
    { }

    public HubAPIRequestFailedException(string msg) : base(msg) { }

}
