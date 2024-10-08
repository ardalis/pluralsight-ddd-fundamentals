﻿using System.Net.Http;
using System.Threading.Tasks;
using Ardalis.HttpClientTestExtensions;
using BlazorShared.Models.Room;
using Xunit;
using Xunit.Abstractions;

namespace FunctionalTests.RoomEndpoints
{
  [Collection("Sequential")]
  public class List : IClassFixture<CustomWebApplicationFactory<Program>>
  {
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _outputHelper;

    public List(CustomWebApplicationFactory<Program> factory, ITestOutputHelper outputHelper)
    {
      _client = factory.CreateClient();
      _outputHelper = outputHelper;
    }

    [Fact]
    public async Task Returns5Rooms()
    {
      var result = await _client.GetAndDeserializeAsync<ListRoomResponse>(ListRoomRequest.Route, _outputHelper);

      Assert.Equal(5, result.Rooms.Count);
      Assert.Contains(result.Rooms, room => room.Name == "Exam Room 1");
    }
  }
}
