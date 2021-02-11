using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClinicManagement.Blazor.Services
{
  public class HttpService
  {
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl;

    public HttpService(HttpClient httpClient)
    {
      _httpClient = httpClient;
      _apiUrl = _httpClient.BaseAddress.ToString();
    }

    public async Task<T> HttpGetAsync<T>(string uri)
        where T : class
    {
      var result = await _httpClient.GetAsync($"{_apiUrl}{uri}");
      if (!result.IsSuccessStatusCode)
      {
        return null;
      }

      return await FromHttpResponseMessageAsync<T>(result);
    }

    public async Task<string> HttpGetAsync(string uri)
    {
      var result = await _httpClient.GetAsync($"{_apiUrl}{uri}");
      if (!result.IsSuccessStatusCode)
      {
        return null;
      }

      return await result.Content.ReadAsStringAsync();
    }

    public async Task<T> HttpDeleteAsync<T>(string uri, object id)
        where T : class
    {
      var result = await _httpClient.DeleteAsync($"{_apiUrl}{uri}/{id}");
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

      var result = await _httpClient.PostAsync($"{_apiUrl}{uri}", content);
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

      var result = await _httpClient.PutAsync($"{_apiUrl}{uri}", content);
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
