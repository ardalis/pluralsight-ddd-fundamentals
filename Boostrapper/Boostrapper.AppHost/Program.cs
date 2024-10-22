
var builder = DistributedApplication.CreateBuilder(args);

var emailServer = builder.AddContainer("mailserver", "jijiechen/papercut")
                         .WithEndpoint(port: 37408, targetPort: 37408, scheme: "http")
                         .WithEndpoint(port: 25, targetPort: 25);

var rabbitmqUser = builder.AddParameter("rabbitmq-username", secret: true);
var rabbitmqPassword = builder.AddParameter("rabbitmq-password", secret: true);

var rabbitmq = builder.AddRabbitMQ("rabbitmq", rabbitmqUser, rabbitmqPassword, port: 5672)
                      .WithDataVolume()
                      .WithManagementPlugin();

var vetClinic = builder.AddProject<Projects.VetClinicPublic>("vet-clinic-public")
                       .WaitFor(rabbitmq)
                       .WaitFor(emailServer);

builder.Build().Run();
