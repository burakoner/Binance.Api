using System.Net;

namespace Binance.Api.Tests;

internal sealed class RecordingHttpMessageHandler(string response) : HttpMessageHandler
{
    public HttpMethod? Method { get; private set; }
    public Uri? RequestUri { get; private set; }
    public string? Body { get; private set; }
    public string? ContentType { get; private set; }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Method = request.Method;
        RequestUri = request.RequestUri;
        Body = request.Content == null ? null : await request.Content.ReadAsStringAsync(cancellationToken);
        ContentType = request.Content?.Headers.ContentType?.MediaType;

        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(response)
        };
    }
}
