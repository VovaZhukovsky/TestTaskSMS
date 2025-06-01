using System.Net.Http.Headers;
using System.Text;
using System.Net.Http.Json;
using Newtonsoft.Json;
using TestTaskSMS.SMSHttpClient.Model;

namespace TestTaskSMS.SMSHttpClient;

public class SMSHttpClient
{
    HttpClient _httpClient;
    string _url;
    public SMSHttpClient(string url, string username, string password)
    {
        _url = url;
        _httpClient = new HttpClient();
        var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
    }

    protected async Task<TResponse> SendAsync<TResponse>(HttpMethod method, object content = null)
    {
        using var request = new HttpRequestMessage(method, _url);

        if (content != null)
        {
            request.Content = JsonContent.Create(content);
        }

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }
        return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());

    }

    protected Task<TResponse> PostAsync<TRequest, TResponse>(TRequest request)
    {
        return SendAsync<TResponse>(HttpMethod.Post, request);
    }

    public async Task<GetMenuResponse> GetMenuAsync()
    {
        GetMenuRequest request = new()
        {
            Command = "GetMenu",
            CommandParameters = new Price { WithPrice = true }

        };
        return await PostAsync<GetMenuRequest, GetMenuResponse>(request);
    }
    
        public async Task<BaseResponse> SendOrderAsync(OrderParams orderParams)
    {
        SendOrderRequest request = new()
        {
            Command = "SendOrder",
            CommandParameters = orderParams

        };
        return await PostAsync<SendOrderRequest, BaseResponse>(request);
    }
}