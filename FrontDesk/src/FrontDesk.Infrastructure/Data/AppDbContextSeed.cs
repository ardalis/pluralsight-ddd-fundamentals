using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorShared.Models.Room;
using FrontDesk.Core.Aggregates;
using FrontDesk.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PluralsightDdd.SharedKernel;

namespace FrontDesk.Infrastructure.Data
{
  public class AppDbContextSeed
  {
    private static Doctor DrSmith => new Doctor("Dr. Smith");
    private static Doctor DrWho => new Doctor("Dr. Who");
    private static Doctor DrMcDreamy => new Doctor("Dr. McDreamy");
    private static Guid ScheduleId;
    private static DateTime TestDate;
    public const string MALE_SEX = "Male";
    public const string FEMALE_SEX = "Female";
    private readonly AppDbContext _context;
    private readonly ILogger<AppDbContextSeed> _logger;

    public AppDbContextSeed(AppDbContext context,
      ILogger<AppDbContextSeed> logger)
    {
      _context = context;
      _logger = logger;
    }

    public async Task SeedAsync(DateTime TestDate, int? retry = 0)
    {
      _logger.LogInformation($"Seeding data.");
      _logger.LogInformation($"DbContext Type: {_context.Database.ProviderName}");

      AppDbContextSeed.TestDate = TestDate;
      int retryForAvailability = retry.Value;
      try
      {
        if (!_context.Database.ProviderName.Contains("InMemory"))
        {
          // apply migrations if connecting to a SQL database
          _context.Database.Migrate();
        }
        if (!await _context.Schedules.AnyAsync())
        {
          await _context.Schedules.AddAsync(
              CreateSchedule());

          await _context.SaveChangesAsync();
        }

        if (!await _context.AppointmentTypes.AnyAsync())
        {
          await _context.AppointmentTypes.AddRangeAsync(
              CreateAppointmentTypes());

          await _context.SaveChangesAsync();
        }

        if (!await _context.Doctors.AnyAsync())
        {
          await _context.Doctors.AddRangeAsync(
              CreateDoctors());

          await _context.SaveChangesAsync();
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

        if (!await _context.Appointments.AnyAsync())
        {
          await _context.Appointments.AddRangeAsync(
              CreateAppointments(ScheduleId));

          await _context.SaveChangesAsync();
        }
      }
      catch (Exception ex)
      {
        if (retryForAvailability < 1)
        {
          retryForAvailability++;
          _logger.LogError(ex.Message);
          await SeedAsync(TestDate, retryForAvailability);
        }
        throw;
      }

      _context.SaveChanges();
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

    private static Schedule CreateSchedule()
    {
      ScheduleId = Guid.NewGuid();
      return new Schedule(ScheduleId, new DateTimeRange(DateTime.Now, DateTime.Now), 1, null);
    }

    private static List<AppointmentType> CreateAppointmentTypes()
    {
      var result = new List<AppointmentType>
            {
                new AppointmentType("Wellness Exam", "WE", 30),
                new AppointmentType("Diagnostic Exam", "DE", 60),
                new AppointmentType("Nail Trim", "NT", 30)
            };

      return result;
    }

    private static List<Doctor> CreateDoctors()
    {
      var result = new List<Doctor>
            {
                DrSmith,
                DrWho,
                DrMcDreamy
            };

      return result;
    }

    private static IEnumerable<Client> CreateListOfClientsWithPatients(Doctor drSmith, Doctor drWho, Doctor drMcDreamy)
    {
      var clientGraphs = new List<Client>();

      var clientSmith = (CreateClientWithPatient("Steve Smith", "Steve", "Mr.", drSmith.Id, MALE_SEX, "Darwin", "Dog",
        "Poodle"));
      clientSmith.Patients.Add(new Patient(1, "Rumor", FEMALE_SEX, new AnimalType("Cat", "Alley"), drWho.Id));

      clientGraphs.Add(clientSmith);

      clientGraphs.Add(CreateClientWithPatient("Julia Lerman", "Julie", "Mrs.", drMcDreamy.Id, MALE_SEX, "Sampson", "Dog", "Newfoundland"));

      clientGraphs.Add(CreateClientWithPatient("Wes McClure", "Wes", "Mr", drMcDreamy.Id, FEMALE_SEX, "Pax", "Dog", "Jack Russell"));
      clientGraphs.Add(CreateClientWithPatient("Andrew Mallett", "Andrew", "Mr.", drSmith.Id, MALE_SEX, "Barney",
        "Dog", "Corgi"));
      clientGraphs.Add(CreateClientWithPatient("Brian Lagunas", "Brian", "Mr.", drWho.Id, MALE_SEX, "Rocky", "Dog",
        "Italian Greyhound"));
      clientGraphs.Add(CreateClientWithPatient("Corey Haines", "Corey", "Mr.", drMcDreamy.Id, FEMALE_SEX, "Zak",
        "Cat", "Mixed"));
      clientGraphs.Add(CreateClientWithPatient("Reindert-Jan Ekker", "Reindert", "Mr.", drSmith.Id, FEMALE_SEX,
        "Tinkelbel", "Cat", "Mixed"));
      clientGraphs.Add(CreateClientWithPatient("Patrick Hynds", "Patrick", "Mr.", drMcDreamy.Id, MALE_SEX, "Anubis", "Dog",
          "Doberman"));
      clientGraphs.Add(CreateClientWithPatient("Lars Klint", "Lars", "Mr.", drMcDreamy.Id, MALE_SEX, "Boots", "Cat",
        "Tabby"));
      clientGraphs.Add(CreateClientWithPatient("Joe Eames", "Joe", "Mr.", drMcDreamy.Id, MALE_SEX, "Corde", "Dog",
        "Mastiff"));
      clientGraphs.Add(CreateClientWithPatient("Joe Kunk", "Joe", "Mr.", drSmith.Id, MALE_SEX, "Wedgie", "Reptile",
        "Python"));
      clientGraphs.Add(CreateClientWithPatient("Ross Bagurdes", "Ross", "Mr.", drWho.Id, MALE_SEX, "Indiana Jones",
        "Cat", "Tabby"));
      clientGraphs.Add(CreateClientWithPatient("Patrick Neborg", "Patrick", "Mr.", drWho.Id, FEMALE_SEX, "Sugar",
        "Dog", "Maltese"));
      clientGraphs.Add(CreateClientWithPatient("David Mann", "David", "Mr.", drWho.Id, FEMALE_SEX, "Piper",
     "Dog", "Mix"));
      clientGraphs.Add(CreateClientWithPatient("Peter Mourfield", "Peter", "Mr.", drWho.Id, FEMALE_SEX, "Finley",
   "Dog", "Dachshund"));
      clientGraphs.Add(CreateClientWithPatient("Keith Harvey", "Keith", "Mr.", drSmith.Id, FEMALE_SEX, "Bella",
 "Dog", "Terrier"));
      var clientMcConnell = CreateClientWithPatient("Andrew Connell", "Andrew", "Mr.", drWho.Id, FEMALE_SEX, "Luabelle", "Dog", "Labrador");
      clientMcConnell.Patients.Add(new Patient(1, "Sadie", FEMALE_SEX, new AnimalType("Dog", "Mix"), drWho.Id));
      clientGraphs.Add(clientMcConnell);
      var clientYack = (CreateClientWithPatient("Julie Yack", "Julie", "Ms.", drSmith.Id, MALE_SEX, "Ruske", "Dog",
       "Siberian Husky"));
      clientYack.Patients.Add(new Patient(1, "Ginger", FEMALE_SEX, new AnimalType("Dog", "Shih Tzu"), drSmith.Id));
      clientYack.Patients.Add(new Patient(1, "Lizzie", MALE_SEX, new AnimalType("Lizzie", "Green"), drSmith.Id));
      clientGraphs.Add(clientYack);

      var clientLibery =
        CreateClientWithPatient("Jesse Liberty", "Jesse", "Mr.", drMcDreamy.Id, MALE_SEX, "Charlie", "Dog", "Mixed");
      clientLibery.Patients.Add(new Patient(1, "Allegra", FEMALE_SEX, new AnimalType("Cat", "Calico"), drSmith.Id));
      clientLibery.Patients.Add(new Patient(1, "Misty", FEMALE_SEX, new AnimalType("Cat", "Tortoiseshell"), drSmith.Id));
      clientGraphs.Add(clientLibery);
      var clientYoung = CreateClientWithPatient("Tyler Young", "Tyler", "Mr.", drMcDreamy.Id, MALE_SEX, "Willie", "Dog", "Daschaund");
      clientLibery.Patients.Add(new Patient(1, "JoeFish", MALE_SEX, new AnimalType("Fish", "Beta"), drSmith.Id));
      clientLibery.Patients.Add(new Patient(1, "Fabian", MALE_SEX, new AnimalType("Cat", "Mixed"), drMcDreamy.Id));
      clientGraphs.Add(clientYoung);
      var clientPerry =
        (CreateClientWithPatient("Michael Perry", "Michael", "Mr.", drMcDreamy.Id, FEMALE_SEX, "Callie", "Cat",
          "Calico"));

      clientLibery.Patients.Add(new Patient(1, "Radar", MALE_SEX, new AnimalType("Dog", "Pug"), drMcDreamy.Id));
      clientLibery.Patients.Add(new Patient(1, "Tinkerbell", FEMALE_SEX, new AnimalType("Dog", "Chihuahua"), drMcDreamy.Id));

      clientGraphs.Add(clientPerry);

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
      client.Patients.Add(new Patient(1, patient1Name, patient1Sex, new AnimalType(animalType, breed), doctorId));

      return client;
    }

    private static IEnumerable<Appointment> CreateAppointments(Guid scheduleId)
    {
      var appointmentList = new List<Appointment>
              {
                new Appointment(
                    2,
                    scheduleId,
                    1,
                    1,
                    1,
                    1,
                    new DateTimeRange(
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 10, 0, 0),
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 11, 0, 0)),
                    "(WE) Darwin - Steve Smith"),
                new Appointment(
                    1,
                    scheduleId,
                    2,
                    2,
                    3,
                    2,
                    new DateTimeRange(
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 11, 0, 0),
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 11, 30, 0)),
                    "(DE) Sampson - Julie Lerman"),
                new Appointment(
                    1,
                    scheduleId,
                    3,
                    2,
                    4,
                    2,
                    new DateTimeRange(
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 12, 0, 0),
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 12, 30, 0)),
                    "(DE) Pax - Wes McClure"),
                new Appointment(
                    1,
                    scheduleId,
                    19,
                    2,
                    23,
                    3,
                    new DateTimeRange(
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 9, 0, 0),
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 9, 30, 0)),
                    "(DE) Charlie - Jesse Liberty"),
                new Appointment(
                    2,
                    scheduleId,
                    19,
                    6,
                    24,
                    3,
                    new DateTimeRange(
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 9, 30, 0),
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 10, 30, 0)),
                    "(DE) Allegra - Jesse Liberty"),
                new Appointment(
                    1,
                    scheduleId,
                    19,
                    2,
                    25,
                    3,
                    new DateTimeRange(
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 10, 30, 0),
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 11, 00, 0)),
                    "(DE) Misty - Jesse Liberty"),
                new Appointment(
                    3,
                    scheduleId,
                    4,
                    2,
                    5,
                    4,
                    new DateTimeRange(
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 8, 0, 0),
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 8, 30, 0)),
                    "(DE) Barney - Andrew Mallett"),
                new Appointment(
                    2,
                    scheduleId,
                    5,
                    2,
                    6,
                    3,
                    new DateTimeRange(
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 8, 0, 0),
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 9, 0, 0)),
                    "(DE) Rocky - Brian Lagunas",
                    new DateTime(2014,6,8,8,0,0)),
                new Appointment(
                    3,
                    scheduleId,
                    20,
                    2,
                    26,
                    2,
                    new DateTimeRange(
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 9, 0, 0),
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 9, 30, 0)),
                    "(DE) Willie - Tyler Young"),
                new Appointment(
                    3,
                    scheduleId,
                    20,
                    2,
                    27,
                    2,
                    new DateTimeRange(
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 9, 30, 0),
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 10, 00, 0)),
                    "(DE) JoeFish - Tyler Young"),
                new Appointment(
                    1,
                    scheduleId,
                    20,
                    2,
                    27,
                    2,
                    new DateTimeRange(
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 9, 30, 0),
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 10, 00, 0)),
                    "(DE) JoeFish - Tyler Young"),
                new Appointment(
                    1,
                    scheduleId,
                    20,
                    2,
                    28,
                    2,
                    new DateTimeRange(
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 10, 0, 0),
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 10, 30, 0)),
                    "(DE) Fabian - Tyler Young"),
                new Appointment(
                    1,
                    scheduleId,
                    6,
                    2,
                    7,
                    4,
                    new DateTimeRange(
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 11, 0, 0),
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 11, 30, 0)),
                    "(DE) Zak - Corey Haines"),
                new Appointment(
                    3,
                    scheduleId,
                    7,
                    2,
                    8,
                    4,
                    new DateTimeRange(
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 9, 0, 0),
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 9, 30, 0)),
                    "(DE) Tinkelbel - Reindert Ekkert"),
                new Appointment(
                    1,
                    scheduleId,
                    18,
                    2,
                    20,
                    4,
                    new DateTimeRange(
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 9, 0, 0),
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 9, 30, 0)),
                    "(DE) Ruske - Julie Yack"),
                new Appointment(
                    3,
                    scheduleId,
                    18,
                    2,
                    22,
                    4,
                    new DateTimeRange(
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 9, 30, 0),
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 10, 00, 0)),
                    "(DE) Lizzie - Julie Yack"),
                new Appointment(
                    3,
                    scheduleId,
                    18,
                    2,
                    21,
                    4,
                    new DateTimeRange(
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 10, 0, 0),
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 10, 30, 0)),
                    "(DE) Ginger - Julie Yack"),
                new Appointment(
                    2,
                    scheduleId,
                    8,
                    2,
                    9,
                    5,
                    new DateTimeRange(
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 8, 0, 0),
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 9, 00, 0)),
                    "(DE) Anubis - Patrick Hynds"),
                new Appointment(
                    1,
                    scheduleId,
                    21,
                    2,
                    30,
                    1,
                    new DateTimeRange(
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 9, 0, 0),
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 9, 30, 0)),
                    "(DE) Radar - Michael Perry"),
                new Appointment(
                    1,
                    scheduleId,
                    21,
                    2,
                    31,
                    1,
                    new DateTimeRange(
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 9, 0, 0),
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 9, 30, 0)),
                    "(DE) Tinkerbell - Michael Perry"),
                new Appointment(
                    2,
                    scheduleId,
                    10,
                    2,
                    11,
                    1,
                    new DateTimeRange(
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 8, 0, 0),
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 9, 00, 0)),
                    "(DE) Corde - Joe Eames"),
                new Appointment(
                    3,
                    scheduleId,
                    21,
                    2,
                    29,
                    5,
                    new DateTimeRange(
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 9, 0, 0),
                        new DateTime(TestDate.Year, TestDate.Month, TestDate.Day, 9, 30, 0)),
                    "(DE) Callie - Michael Perry"),
              };

      return appointmentList;
    }
  }
}
