# Pluralsight DDD Fundamentals Sample

Sample code for the Pluralsight DDD Fundamentals course (coming soon) by Julie Lerman and Steve "ardalis" Smith. If you are looking for the .NET Framework sample from [the original 2014 DDD Fundamentals course](https://app.pluralsight.com/library/courses/domain-driven-design-fundamentals), it's available as the [ddd-vet-clinic sample](https://github.com/ardalis/ddd-vet-sample).

## Give a Star! :star:

If you like or are using this project to learn, please give it a star. Thanks!

## Running the Sample

The easiest way to run the sample is using docker. Download the source and run this command from the root folder:

```powershell
docker-compose build
docker-compose up
```

This will start RabbitMQ (for messaging between apps) and build and run each of the applications involved in the sample:

- FrontDesk.Api
- FrontDesk.Blazor
- ClinicManagement.Api
- ClinicManagement.Blazor
- VetClinicPublic

### Visual Studio

Running the sample from Visual Studio requires some additional setup. You will need to run multiple solutions side by side. You will also need to run RabbitMQ, ideally as a docker image, which you can so using this command:

```powershell
docker run --rm -it --hostname ddd-sample-rabbit -p 15672:15672 -p 5672:5672 rabbitmq:3-management
```

You should be able to open [localhost:15672](http://localhost:15672/#/) to view RabbitMQ management interface.

## Architecture Notes

## Deveoper Notes

## Credits

This sample is from [Julie Lerman](https://www.pluralsight.com/authors/julie-lerman) and [Steve Smith](https://www.pluralsight.com/authors/steve-smith)'s Pluralsight course. The original sample was written for .NET Framework by Steve. The current .NET 5 version was initially ported with the help of [Shady Nagy](https://twitter.com/ShadyNagy_). Progress Software provided the [Blazor Scheduler control](https://www.telerik.com/blazor-ui/scheduler) used to display the clinic's schedule. Additional credits include:

- TBD
