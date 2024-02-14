namespace Platform.Infrastructure.FaultTolerance.Http.Contracts
{
    using System.Net.Http;

    /// <summary>
    /// Blueprint for HttpClientProvider.
    /// </summary>
    public interface IHttpClientProvider
    {
        /// <summary>
        /// Blue print for the Method responsible for providing HttpClient from HttpClientFactory.
        /// </summary>
        /// <returns> Returns HttpClient. </returns>
        HttpClient GetHttpClient();

        HttpClient GetHttpClient(HttpRequestMessage requestMessage);
    }
}
