# Pluralsight DDD Fundamentals Sample

Sample code for the [Pluralsight DDD Fundamentals course (2nd edition) by Julie Lerman and Steve "ardalis" Smith](https://www.pluralsight.com/courses/fundamentals-domain-driven-design). If you are looking for the .NET Framework sample from [the original 2014 DDD Fundamentals course](https://app.pluralsight.com/library/courses/domain-driven-design-fundamentals), it's available as the [ddd-vet-clinic sample](https://github.com/ardalis/ddd-vet-sample).

[Additional exercises from Steve's DDD workshops](https://ardalis.gumroad.com/l/ZCSml) (separate from Pluralsight course)

## Give a Star! :star:

If you like or are using this project to learn, please give it a star. Thanks!!!

## Table of Contents

[1. Running the Sample](#1-running-the-sample)

&nbsp;&nbsp;[1.1 Docker](#11-docker)

&nbsp;&nbsp;[1.2 Visual Studio and VS Code](#12-visual-studio-and-vs-code)

[2. Student Recommendations](#2-student-recommendations)

[3. Architecture Notes](#3-architecture-notes)

[4. Developer Notes](#4-developer-notes)

[5. Credits](#5-credits)

## 1. Running the Sample

You can run this sample in Docker or in Visual Studio. Docker is recommended.

Watch how to set up and run the sample app using Docker, Visual Studio, or VS Code here:

[![image](https://user-images.githubusercontent.com/782127/194625565-5cef5019-e6fb-4c1d-a287-783b1924b74f.png)](https://www.youtube.com/watch?v=wl7aF6iJDVM)

[Running the DDD Fundamentals Sample](https://www.youtube.com/watch?v=wl7aF6iJDVM) by [Philippe Vaillancourt](https://blog.nimblepros.com/author/snowfrog)

### 1.1 Docker

The easiest way to run the sample is using docker. Download the source and run this command from the root folder:

```powershell
docker-compose build --parallel
docker-compose up
```

The `build` step will take a while. It's way faster if you run it in parallel, assuming you have a fast machine and download speed. The `up` command is much faster but will also take a moment and you may see some errors as apps try to connect to docker or databases before they're responsive. Give it a minute or two and it should succeed. RabbitMQ errors should go away once that service starts up. If you get SQL Server login errors, I've found it's best to just restart everything (`ctrl-c`, then `docker-compose up` again).

This will start RabbitMQ (for messaging between apps) and build and run each of the applications involved in the sample:

- FrontDesk.Api
- FrontDesk.Blazor
- ClinicManagement.Api
- ClinicManagement.Blazor
- VetClinicPublic

It also adds the following supporting containers:

- RabbitMQ with Management
- FrontDesk SQL Server
- ClinicManagement SQL Server
- Test Mailserver (Papercut)

Once running, you should be able to access the various apps using localhost (HTTP not HTTPS because of [Kestrel configuration restrictions](https://docs.microsoft.com/en-us/aspnet/core/security/docker-compose-https)) and the following ports (you can also find these bindings in the docker-compose.yml file):

| Service (in docker)            | Docker Port | Visual Studio Port |
| ------------------------------ | ----------: | -----------------: |
| FrontDesk (main app)           |        5100 |               5150 |
| ClinicManagement               |        6100 |               6150 |
| VetClinicPublic                |        7100 |               7150 |
| FrontDesk API / Swagger        |        5200 |               5250 |
| ClinicManagement API / Swagger |        6200 |               6250 |
| RabbitMQ Management            |       15673 |              15672 |
| RabbitMQ Service               |      (5672) |               5672 |
| Papercut Management            |       37409 |              37408 |
| Papercut SMTP                  |        (25) |                 25 |

The ports in () are only open inside of docker, not exposed otherwise.

Here are the (localhost) links you should use once the apps are running in docker:

- [FrontDesk](http://localhost:5100)
- [Clinic Management](http://localhost:6100/clients)
- [VetClinicPublic](http://localhost:7100/) (there are links here to send test emails and view them, too)

If you want to quickly clean up all of your docker containers (**All** of them not just the ones you created for this sample!) you can run this command:

```powershell
docker kill $(docker ps -q)
```

Note that any data changes you make will not be persisted if you **docker remove** the SQL Server container. The Docker container for SQL Server will be recreated and seeded on the next **docker run**. In other scenarios, you might be using Docker volumes to persist or share the database across container instances but that's overkill for this demo.

### 1.2 Visual Studio and VS Code

You may need to configure a local NuGet server and put this package in it:

```
/FrontDesk/src/FrontDesk.Blazor/deps/Pluralsight.DDD.Deps/Pluralsight.DDD.Deps.1.0.0.nupkg
```

Running the sample from Visual Studio (or VS Code or Rider, etc) requires some additional setup. You will need to run multiple solutions side by side. You will also need to run RabbitMQ and PaperCut, ideally as a docker images. You can run RabbitMQ from Docker using this command:

```powershell
docker run --rm -it --hostname ddd-sample-rabbit -p 15672:15672 -p 5672:5672 rabbitmq:3-management
```

You should be able to open [localhost:15672](http://localhost:15672/#/) to view RabbitMQ management interface (login as guest/guest).

![rabbitmq management app](https://user-images.githubusercontent.com/782127/112649139-8a943b00-8e20-11eb-8bd5-1bfb15a9a90d.png)

When new appointments are created, confirmation emails are sent out to clients. Start a test mailserver using this command ([learn more](https://ardalis.com/configuring-a-local-test-email-server/)):

```powershell
docker run --name=papercut -p 25:25 -p 37408:37408 jijiechen/papercut:latest
```

You should be able to open [localhost:37408](http://localhost:37408) to view Papercut test mailserver management interface, where sent emails will appear.

![Papercut management app](https://user-images.githubusercontent.com/5007120/113427976-fb4cd180-93a3-11eb-826e-9ba2b76466c0.png)

You can run individual solutions independently from one another, but obviously you won't see live sync between them when entities are updated, new appointments created, appointment confirmation emails clicked, etc. To get that, you'll need to run all three of the web applications:

- FrontDesk
- ClinicManagement
- VetClinicPublic

Some of the ports may not be set up in config; you may need to adjust them by hand. They assume you'll run primarily in docker to see everything running. If you're trying to get things working outside of docker, you should try with the ports shown in the table above.

## 2. Student Recommendations

If you're coming here from the Pluralsight Domain-Driven Design Fundamentals course, great! Download this sample and look around. See if you can run it on your machine (docker recommended). Your next assignment is to look at the `TODO` comments in the code, and see if you can implement any of them. You can [view todo comments as tasks in Visual Studio](https://ardalis.com/tracking-tasks-in-visual-studio/), or there are plugins for VS Code.

Don't worry about submitting a pull request for any `TODO` comments you fix. They're left there intentionally to help students learn by providing some ways to extend the solution from the course.

## 3. Architecture Notes

The reference application is built as a small set of related monolithic ASP.NET Core applications. The two Blazor applications are built using a modified version of the [CleanArchitecture solution template](https://github.com/ardalis/CleanArchitecture) and also bears a great deal of similiarity to the [eShopOnWeb application](https://github.com/dotnet-architecture/eShopOnWeb) (also maintained by @ardalis). To learn more about migrating toward a domain-centric architecture from a data-centric one, check out [Steve's two courses on N-Tier architecture on Pluralsight](https://www.pluralsight.com/authors/steve-smith). You'll also find [videos covering Clean Architecture on Steve's YouTube channel](https://www.youtube.com/c/Ardalis/search?query=clean) as well as on the [eShopOnWeb home page](https://www.youtube.com/watch?v=vRZ8ucGac8M&ab_channel=Ardalis).

If you or your team need help with architecting your .NET application following principles of clean architecture, with or without microservices, [Steve and his team and NimblePros](https://nimblepros.com) have a great deal of experience helping clients do just that. Get in touch.

## 4. Developer Notes

If you're new to this kind of application development, and you have a Pluralsight subscription, I strongly advise you to learn about [SOLID principles](https://www.pluralsight.com/courses/csharp-solid-principles) and various [code smells and refactoring techniques](https://www.pluralsight.com/courses/refactoring-csharp-developers). You'll find the background in these principles informs most of the design decisions used in the individual classes and projects used in the sample code provided here. If you're really serious about learning these topics, watch the original versions of these courses, which were able to go into more depth (more recent courses need to be shorter since Pluralsight found too many students didn't complete longer courses. But not _you_ - you have what it takes to finish the whole thing. Right?). They're available from [Steve's author page on Pluralsight](https://www.pluralsight.com/authors/steve-smith).

The sample doesn't include exhaustive test coverage, but does demonstrate some automated tests. If testing is new to you, [Julie has a great course on automated testing](https://www.pluralsight.com/courses/automated-testing-fraidy-cats) that you should check out.

### 4.1 Referenced NuGet Packages

This course uses several NuGet packages that you may find useful.

[Ardalis.ApiEndpoints](https://www.nuget.org/packages/Ardalis.ApiEndpoints/)
A simple base class that lets you keep your API endpoints small and focused on one endpoint at a time. MVC is replaced with Request-EndPoint-Response (REPR).

[Ardalis.GuardClauses](https://www.nuget.org/packages/Ardalis.GuardClauses/)
Contains common guard clauses so you can use them consistently. Also can be easily extended to apply your own guards for custom/domain exception cases.

[Ardalis.Specification](https://www.nuget.org/packages/Ardalis.Specification/)
An implementation of the Specification design pattern that is well-suited to work with ORMs like Entity Framework (Core).

[Ardalis.Specification.EntityFrameworkCore](https://www.nuget.org/packages/Ardalis.Specification.EntityFrameworkCore/)
Adds EF Core-specific functionality, including a default implementation of a generic EF repository that supports Specifications.

[Ardalis.Result](https://www.nuget.org/packages/Ardalis.Result/)
Provides a generic result type that can be returned from application services. Can easily be translated into HTTP status codes or ActionResult types.

[Ardalis.HttpClientTestExtensions](https://www.nuget.org/packages/Ardalis.HttpClientTestExtensions/)
Removes boilerplate code from ASP.NET Core API integration/functional tests.

[Autofac](https://www.nuget.org/packages/Autofac/)
Powerful open source DI/IOC container for .NET that supports more features than built-in ServiceCollection.

[Blazored.LocalStorage](https://www.nuget.org/packages/Blazored.LocalStorage/)
Blazor utility for accessing browser local storage in Blazor WebAssembly apps.

[MediatR](https://www.nuget.org/packages/MediatR/)
Used to implement mediator pattern for commands and events.

[Pluralsight.DDD.Deps](https://github.com/ardalis/pluralsight-ddd-fundamentals/tree/main/FrontDesk/src/FrontDesk.Blazor/deps)
Includes required trial binaries from Telerik. Currently this includes both Kendo UI and Blazor controls; Kendo should be replaced with just Blazor later in 2021. To build locally you may need to place this package in a [local nuget repository](https://docs.microsoft.com/en-us/nuget/hosting-packages/local-feeds). This package and its contents are subject to Telerik's EULA located in the same folder.

[PluralsightDdd.SharedKernel](https://www.nuget.org/packages/PluralsightDdd.SharedKernel/)
An example SharedKernel package used by this sample built just for this course.

[RabbitMQ.Client](https://www.nuget.org/packages/RabbitMQ.Client/)
Client for communicating with RabbitMQ.

## 5. Credits

This sample is from [Julie Lerman](https://www.pluralsight.com/authors/julie-lerman) and [Steve Smith](https://www.pluralsight.com/authors/steve-smith)'s Pluralsight course. The original sample was written for .NET Framework by Steve. The current .NET 5 version was initially ported with the help of [Shady Nagy](https://twitter.com/ShadyNagy_). Progress Software provided the [Blazor Scheduler control](https://www.telerik.com/blazor-ui/scheduler) used to display the clinic's schedule\*.

\* _Initial version is using a Kendo schedule since certain features weren't available at recording time_

Additional credits include:

Matheus Penido:
Fixing a bug (https://github.com/ardalis/pluralsight-ddd-fundamentals/issues/36)
https://github.com/matheuspenido
https://www.linkedin.com/in/matheus-penido-8419a890/

- Your name could be here...
