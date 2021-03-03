using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FrontDesk.Blazor.Services
{
  public class HttpService
  {
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl;

    public HttpService(HttpClient httpClient)
    {
      _httpClient = httpClient;
      _apiBaseUrl = _httpClient.BaseAddress.ToString();
    }

    public async Task<T> HttpGetAsync<T>(string uri)
        where T : class
    {
      var result = await _httpClient.GetAsync($"{_apiBaseUrl}{uri}");
      if (!result.IsSuccessStatusCode)
      {
        return null;
      }

      return await FromHttpResponseMessageAsync<T>(result);
    }

    public async Task<string> HttpGetAsync(string uri)
    {
      var result = await _httpClient.GetAsync($"{_apiBaseUrl}{uri}");
      if (!result.IsSuccessStatusCode)
      {
        return null;
      }

      return await result.Content.ReadAsStringAsync();
    }

    public Task<T> HttpDeleteAsync<T>(string uri, object id)
        where T : class
    {
      return HttpDeleteAsync<T>($"{uri}/{id}");
    }

    public async Task<T> HttpDeleteAsync<T>(string uri)
        where T : class
    {
      var result = await _httpClient.DeleteAsync($"{_apiBaseUrl}{uri}");
      if (!result.IsSuccessStatusCode)
      {
        return null;
      }

      return await FromHttpResponseMessageAsync<T>(result);
    }

    public async Task<T> HttpPostAsync<T>(string uri, object dataToSend)
        where T : class
    {
      var content = ToJson(dataToSend);

      var result = await _httpClient.PostAsync($"{_apiBaseUrl}{uri}", content);
      if (!result.IsSuccessStatusCode)
      {
        return null;
      }

      return await FromHttpResponseMessageAsync<T>(result);
    }

    public async Task<T> HttpPutAsync<T>(string uri, object dataToSend)
        where T : class
    {
      var content = ToJson(dataToSend);

      var result = await _httpClient.PutAsync($"{_apiBaseUrl}{uri}", content);
      if (!result.IsSuccessStatusCode)
      {
        return null;
      }

      return await FromHttpResponseMessageAsync<T>(result);
    }


    private StringContent ToJson(object obj)
    {
      return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
    }

    private async Task<T> FromHttpResponseMessageAsync<T>(HttpResponseMessage result)
    {
      return JsonSerializer.Deserialize<T>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true
      });
    }
  }
}
