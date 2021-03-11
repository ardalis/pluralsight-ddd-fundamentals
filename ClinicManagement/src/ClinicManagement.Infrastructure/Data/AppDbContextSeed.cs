using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorShared.Models.AppointmentType;
using BlazorShared.Models.Room;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using PluralsightDdd.SharedKernel;

namespace ClinicManagement.Infrastructure.Data
{
  public class AppDbContextSeed
  {
    private Doctor DrSmith => new Doctor(1, "Dr. Smith");
    private Doctor DrWho => new Doctor(2, "Dr. Who");
    private Doctor DrMcDreamy => new Doctor(3, "Dr. McDreamy");
    private Guid _scheduleId = Guid.NewGuid();
    private DateTime _testDate = DateTime.Now;
    public const string MALE_SEX = "Male";
    public const string FEMALE_SEX = "Female";
    private readonly AppDbContext _context;
    private readonly ILogger<AppDbContextSeed> _logger;
    private Client _steve;
    private Client _julie;
    private Patient _darwin;
    private Patient _sampson;

    public AppDbContextSeed(AppDbContext context,
      ILogger<AppDbContextSeed> logger)
    {
      _context = context;
      _logger = logger;
    }

    public async Task SeedAsync(DateTime testDate, int? retry = 0)
    {
      _logger.LogInformation($"Seeding data.");
      _logger.LogInformation($"DbContext Type: {_context.Database.ProviderName}");

      _testDate = testDate;
      int retryForAvailability = retry.Value;
      try
      {
        if(_context.IsRealDatabase())
        {
          // apply migrations if connecting to a SQL database
          _context.Database.Migrate();
        }

        if (!await _context.AppointmentTypes.AnyAsync())
        {
          var apptTypes = await CreateAppointmentTypes();
          await _context.AppointmentTypes.AddRangeAsync(apptTypes);
          await _context.SaveChangesWithIdentityInsert<AppointmentType>();
        }

        if (!await _context.Doctors.AnyAsync())
        {
          var doctors = CreateDoctors();
          await _context.Doctors.AddRangeAsync(doctors);
          await _context.SaveChangesWithIdentityInsert<Doctor>();
        }

        if (!await _context.Clients.AnyAsync())
        {
          await _context.Clients.AddRangeAsync(
              CreateListOfClientsWithPatients(DrSmith, DrWho, DrMcDreamy));

          await _context.SaveChangesAsync();
        }

        if (!await _context.Rooms.AnyAsync())
        {
          var rooms = await CreateRooms();
          await _context.Rooms.AddRangeAsync(rooms);
          await _context.SaveChangesWithIdentityInsert<Room>();
        }
      }
      catch (Exception ex)
      {
        if (retryForAvailability < 1)
        {
          retryForAvailability++;
          _logger.LogError(ex.Message);
          await SeedAsync(_testDate, retryForAvailability);
        }
        throw;
      }

      await _context.SaveChangesAsync();
    }

    private async Task<List<Room>> CreateRooms()
    {
      string fileName = "rooms.json";
      if (!File.Exists(fileName))
      {
        _logger.LogInformation($"Creating {fileName}");
        using Stream writer = new FileStream(fileName, FileMode.OpenOrCreate);
        await JsonSerializer.SerializeAsync(writer, GetDefaultRooms());
      }

      _logger.LogInformation($"Reading rooms from file {fileName}");
      using Stream reader = new FileStream(fileName, FileMode.Open);
      var rooms = await JsonSerializer.DeserializeAsync<List<RoomDto>>(reader);

      return rooms.Select(dto => new Room(dto.RoomId, dto.Name)).ToList();
    }

    private List<RoomDto> GetDefaultRooms()
    {
      List<RoomDto> rooms = new List<RoomDto>();
      for (int i = 1; i < 6; i++)
      {
        rooms.Add(new RoomDto { RoomId = i, Name = $"Exam Room {i}" });
      }
      return rooms;
    }

    private async Task<List<AppointmentType>> CreateAppointmentTypes()
    {
      string fileName = "appointmentTypes.json";
      if (!File.Exists(fileName))
      {
        _logger.LogInformation($"Creating {fileName}");
        using Stream writer = new FileStream(fileName, FileMode.OpenOrCreate);
        await JsonSerializer.SerializeAsync(writer, GetDefaultAppointmentTypes());
      }

      _logger.LogInformation($"Reading appointment types from file {fileName}");
      using Stream reader = new FileStream(fileName, FileMode.Open);
      var apptTypes = await JsonSerializer.DeserializeAsync<List<AppointmentTypeDto>>(reader);

      return apptTypes.Select(dto => new AppointmentType(dto.AppointmentTypeId, dto.Name, dto.Code, dto.Duration)).ToList();
    }

    private List<AppointmentTypeDto> GetDefaultAppointmentTypes()
    {
      var result = new List<AppointmentTypeDto>
            {
                new AppointmentTypeDto {
                  AppointmentTypeId=1,
                  Name="Wellness Exam",
                  Code="WE",
                  Duration=30
                },
                new AppointmentTypeDto {
                  AppointmentTypeId=2,
                  Name="Diagnostic Exam",
                  Code="DE",
                  Duration=60
                },
                new AppointmentTypeDto{
                  AppointmentTypeId=3,
                  Name="Nail Trim",
                  Code="NT",
                  Duration=30
                }
            };

      return result;
    }

    private List<Doctor> CreateDoctors()
    {
      var result = new List<Doctor>
            {
                DrSmith,
                DrWho,
                DrMcDreamy
            };

      return result;
    }

    private IEnumerable<Client> CreateListOfClientsWithPatients(Doctor drSmith, Doctor drWho, Doctor drMcDreamy)
    {
      var clientGraphs = new List<Client>();

      var clientSmith = (CreateClientWithPatient("Steve Smith", "Steve", "Mr.", drSmith.Id, MALE_SEX, "Darwin", "Dog",
        "Poodle"));
      var rumor = new Patient(1, "Rumor", FEMALE_SEX, drWho.Id);
        rumor.AnimalType = new AnimalType("Cat", "Alley");
      clientSmith.Patients.Add(rumor);

      clientGraphs.Add(clientSmith);

      clientGraphs.Add(CreateClientWithPatient("Julia Lerman", "Julie", "Mrs.", drMcDreamy.Id, MALE_SEX, "Sampson", "Dog", "Newfoundland"));

      return clientGraphs;
    }

    private static Client CreateClientWithPatient(string fullName,
        string preferredName,
        string salutation,
        int doctorId,
        string patient1Sex,
        string patient1Name,
        string animalType,
        string breed)
    {
      var client = new Client(fullName, preferredName, salutation, doctorId, "client@example.com");

      var patient = new Patient(1, patient1Name, patient1Sex, doctorId);
      patient.AnimalType = new AnimalType(animalType, breed);
      client.Patients.Add(patient);

      return client;
    }
  }
}
