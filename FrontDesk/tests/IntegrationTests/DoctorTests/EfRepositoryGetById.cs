﻿using System.Threading.Tasks;
using FrontDesk.Core.SyncedAggregates;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.DoctorTests
{
  public class EfRepositoryGetById : BaseEfRepoTestFixture
  {
    private readonly EfRepository<Doctor> _repository;

    public EfRepositoryGetById()
    {
      _repository = GetRepositoryAsync<Doctor>().Result;
    }

    [Fact]
    public async Task GetsByIdDoctorAfterAddingIt()
    {
      var id = 9;
      var doctor = await AddDoctor(id);

      var newDoctor = await _repository.GetByIdAsync(id);

      Assert.Equal(doctor, newDoctor);
      Assert.True(newDoctor?.Id == id);
    }

    private async Task<Doctor> AddDoctor(int id)
    {
      var doctor = new DoctorBuilder().Id(id).Build();

      await _repository.AddAsync(doctor);

      return doctor;
    }
  }
}
