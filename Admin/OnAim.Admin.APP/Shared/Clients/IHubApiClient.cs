namespace OnAim.Admin.APP.Shared.Clients
{
    public interface IHubApiClient
    {
        Task<T> Get<T>(
       string uri,
       CancellationToken ct = default
        );
        Task<Stream> GetAsStream(
            string uri,
            CancellationToken ct = default
        );
        Task<HttpResponseMessage> PostAsJson(
            string uri,
            object obj,
            CancellationToken ct = default
        );
        Task<HttpResponseMessage> PutAsJson(
            string uri,
            object obj,
            CancellationToken ct = default
        );
        Task<T> PostAsJsonAndSerializeResultTo<T>(
            string uri,
            object obj,
            CancellationToken ct = default
        );

        Task<HttpResponseMessage> Delete(
            string uri,
            CancellationToken ct = default
        );

    }
}
