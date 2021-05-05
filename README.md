# Pluralsight DDD Fundamentals Sample

Sample code for the Pluralsight DDD Fundamentals course (coming soon) by Julie Lerman and Steve "ardalis" Smith. If you are looking for the .NET Framework sample from [the original 2014 DDD Fundamentals course](https://app.pluralsight.com/library/courses/domain-driven-design-fundamentals), it's available as the [ddd-vet-clinic sample](https://github.com/ardalis/ddd-vet-sample).

## Give a Star! :star:

If you like or are using this project to learn, please give it a star. Thanks!

## Table of Contents

[1. Running the Sample](#1-running-the-sample)

&nbsp;&nbsp;[1.1 Docker](#11-docker)

&nbsp;&nbsp;[1.2 Visual Studio](#12-visual-studio)

[2. Student Recommendations](#2-student-recommendations)

[3. Architecture Notes](#3-architecture-notes)

[4. Developer Notes](#4-developer-notes)

[5. Credits](#5-credits)

## 1. Running the Sample

You can run this sample in Docker or in Visual Studio. Docker is recommended.

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

| Service (in docker)            | Docker Port    | Visual Studio Port |
|--------------------------------|---------------:|-------------------:|
| FrontDesk (main app)           |           5100 |               5150 |
| ClinicManagement               |           6100 |               6150 |
| VetClinicPublic                |           7100 |               7150 |
| FrontDesk API / Swagger        |           5200 |               5250 |
| ClinicManagement API / Swagger |           6200 |               6250 |
| RabbitMQ Management            |          15673 |              15672 |
| RabbitMQ Service               |         (5672) |               5672 |
| Papercut Management            |          37409 |              37408 |
| Papercut SMTP                  |           (25) |                 25 |

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

## 4. Developer Notes

## 5. Credits

This sample is from [Julie Lerman](https://www.pluralsight.com/authors/julie-lerman) and [Steve Smith](https://www.pluralsight.com/authors/steve-smith)'s Pluralsight course. The original sample was written for .NET Framework by Steve. The current .NET 5 version was initially ported with the help of [Shady Nagy](https://twitter.com/ShadyNagy_). Progress Software provided the [Blazor Scheduler control](https://www.telerik.com/blazor-ui/scheduler) used to display the clinic's schedule. Additional credits include:

- Your name could be here...
