using System;
using System.Linq;
using System.Threading.Tasks;
using FrontDesk.Api;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.AppointmentTests
{
  public class Appointment_EfRepositoryCreate : BaseEfRepoTestFixture
  {
    private readonly EfRepository _repository;

    public Appointment_EfRepositoryCreate(CustomWebApplicationFactory<Startup> factory) : base(factory)
    {
      _repository = GetRepositoryAsync().Result;
    }

    [Fact]
    public async Task CreatesAppointmentAndSetsId()
    {
      var appointment = await CreateAppointment();

      var newAppointment = (await _repository.ListAsync<FrontDesk.Core.Aggregates.Appointment, Guid>()).LastOrDefault();

      Assert.Equal(appointment, newAppointment);
      Assert.True(newAppointment?.Id != Guid.Empty);
    }

    private async Task<FrontDesk.Core.Aggregates.Appointment> CreateAppointment()
    {
      var appointment = new AppointmentBuilder().WithDefaultValues().Build();

      await _repository.AddAsync<FrontDesk.Core.Aggregates.Appointment, Guid>(appointment);

      return appointment;
    }
  }
}
