using System;
using System.Threading.Tasks;
using FrontDesk.Core.ScheduleAggregate;
using FrontDesk.Core.SyncedAggregates;
using FrontDesk.Infrastructure.Data;
using PluralsightDdd.SharedKernel;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.ScheduleTests
{
  public class EfRepositoryGetById : BaseEfRepoTestFixture
  {
    private readonly EfRepository<Schedule> _repository;

    public EfRepositoryGetById()
    {
      _repository = GetRepositoryAsync<Schedule>().Result;
    }

    [Fact]
    public async Task GetsByIdScheduleAfterAddingIt()
    {
      var id = Guid.NewGuid();
      var newSchedule = await AddSchedule(id);

      var scheduleFromRepo = await _repository.GetByIdAsync(id);

      Assert.Equal(newSchedule, scheduleFromRepo);
      Assert.True(scheduleFromRepo.Id == id);
    }

    private async Task<Schedule> AddSchedule(Guid id)
    {
      int clinicId = 2;
      var schedule = new Schedule(id, new DateTimeOffsetRange(DateTimeOffset.Now.Date, TimeSpan.FromDays(1)), clinicId);

      await _repository.AddAsync(schedule);

      return schedule;
    }
  }
}
