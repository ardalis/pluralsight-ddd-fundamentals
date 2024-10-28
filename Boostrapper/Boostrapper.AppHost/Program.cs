var builder = DistributedApplication.CreateBuilder(args);

var emailServer = builder.AddContainer("mailserver", "jijiechen/papercut")
                         .WithEndpoint(port: 37408, targetPort: 37408, scheme: "http")
                         .WithEndpoint(port: 25, targetPort: 25);

var rabbitmqUser = builder.AddParameter("rabbitmq-username", secret: true);
var rabbitmqPassword = builder.AddParameter("rabbitmq-password", secret: true);

var rabbitmq = builder.AddRabbitMQ("rabbitmq", rabbitmqUser, rabbitmqPassword, port: 5672)
                      .WithDataVolume()
                      .WithManagementPlugin();

builder.AddProject<Projects.VetClinicPublic>("vet-clinic-public")
       .WaitFor(rabbitmq)
       .WaitFor(emailServer);

var frontDeskDb = builder.AddSqlServer("frontdesk-sqlserver", port: 1434)
                         .WithImageTag("2019-latest")
                         .AddDatabase("frontdesk-db");

var frontDeskApi = builder.AddProject<Projects.FrontDesk_Api>("frontdesk-api")
                          .WaitFor(rabbitmq)
                          .WaitFor(frontDeskDb)
                          .WithReference(frontDeskDb, "DefaultConnection");

builder.AddProject<Projects.FrontDesk_Blazor_Host>("frontdesk-blazor-host")
       .WaitFor(frontDeskApi);


var clinicManagementDb = builder.AddSqlServer("clinicmanagement-sqlserver", port: 1435)
                                .WithImageTag("2019-latest")
                                .AddDatabase("clinicmanagement-db");

var clinicManagementApi = builder.AddProject<Projects.ClinicManagement_Api>("clinicmanagement-api")
                                 .WaitFor(rabbitmq)
                                 .WaitFor(clinicManagementDb)
                                 .WithReference(clinicManagementDb, "DefaultConnection");

builder.AddProject<Projects.ClinicManagement_Blazor_Host>("clinicmanagement-blazor-host")
       .WaitFor(clinicManagementApi);

builder.Build().Run();
