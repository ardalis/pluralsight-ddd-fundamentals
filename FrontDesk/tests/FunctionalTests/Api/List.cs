using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorShared.Models.Room;
using FrontDesk.Api;
using Xunit;
using Xunit.Abstractions;

namespace FunctionalTests.Api
{
  public class List : IClassFixture<CustomWebApplicationFactory<Startup>>
  {
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _outputHelper;

    public List(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper outputHelper)
    {
      _client = factory.CreateClient();
      _outputHelper = outputHelper;
    }

    [Fact]
    public async Task ReturnsTwoItems()
    {
      var options = new JsonSerializerOptions
      {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
      var response = await _client.GetAsync("/api/rooms");
      response.EnsureSuccessStatusCode();
      var stringResponse = await response.Content.ReadAsStringAsync();
      _outputHelper.WriteLine(stringResponse);
      var result = JsonSerializer.Deserialize<ListRoomResponse>(stringResponse, options);

      Assert.Equal(5, result.Rooms.Count());
      Assert.Contains(result.Rooms, room => room.Name == "Exam Room 1");
    }
  }
}
