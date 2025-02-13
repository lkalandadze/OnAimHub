﻿namespace OnAim.Admin.APP.Services.Hub.ClientServices;

public interface ISagaApiClient
{
    Task<T> Get<T>(string uri, CancellationToken ct = default);
    Task<Stream> GetAsStream(string uri, CancellationToken ct = default);
    Task<HttpResponseMessage> PostAsJson(string uri, object obj, CancellationToken ct = default);
    Task<T> PostAsJsonAndSerializeResultTo<T>(string uri, object obj, CancellationToken ct = default);
    Task<HttpResponseMessage> PutAsJson(string uri, object obj, CancellationToken ct = default);
    Task<HttpResponseMessage> Delete(string uri, CancellationToken ct = default);
    Task<HttpResponseMessage> PostMultipartAsync(string uri, MultipartFormDataContent content, CancellationToken ct = default);
}
