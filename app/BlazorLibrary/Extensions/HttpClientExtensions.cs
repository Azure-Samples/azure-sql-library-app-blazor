using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BookManagementApp.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, string requestUri, T value)
        {
            var content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
            var method = new HttpMethod("PATCH");

            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };

            return await client.SendAsync(request);
        }
    }
}