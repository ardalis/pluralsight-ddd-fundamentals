using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Ardalis.HttpClientTestExtensions;
using BlazorShared.Models.Doctor;
using ClinicManagement.Api;
using Xunit;
using Xunit.Abstractions;

namespace FunctionalTests.Api
{
  [Collection("Sequential")]
  public class DoctorsList : IClassFixture<CustomWebApplicationFactory<Startup>>
  {
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _outputHelper;

    public DoctorsList(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper outputHelper)
    {
      _client = factory.CreateClient();
      _outputHelper = outputHelper;
    }

    [Fact]
    public async Task Returns3Doctors()
    {
      var result = await _client.GetAndDeserialize<ListDoctorResponse>("/api/doctors", _outputHelper);

      Assert.Equal(3, result.Doctors.Count());
      Assert.Contains(result.Doctors, x => x.Name == "Dr. Smith");
    }
  }
}
