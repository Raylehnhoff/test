# Test Submission

## Purpose

The purpose of this system is to show an N-Tier architecture that could be stubbed out to make an N-tier call. This architecture is far more appropriate for larger, more segmented applications that may have different applications -- such as web, services, etc. 

It is sometimes appropriate for use in Microservices, but typically this architecture would be overkill for a microservice. This approach allows extreme segmentation of code layers, but the tradeoff is there's more boilerplate to do simple things (like output Hello World)

To run the application, pull latest and change the target to the desired output -- options are currently `Test.ConsoleApp` and `Test.Web` -- `Test.Web` can also run from a Docker context.

## Design Philosophies

### Terms

|Term|Definition|
|-|-|
Service|A middle-tier system that can wrap one or more adapters -- these may perform business logic or other necessary transforms on the response from an adapter
Adapter|A back-end system that can connecto to an external service (cache, database, file system, whatever)
CommandFactory|A factory that can proxy calls to arbitrary handlers based on some criteria (command name in these cases)
CommandHandler|A class that  wraps the logic needed for a particular command, can have dependencies injected into them and are individually unit-testable

### Scopes

All concrete classes in `Domain` and `Data` should be internal to prevent them from being accidently `new`'d up. Marking them internal guarantees they are only ever exposed through the dependency injection system from inside the classes.

### Common

The `Common` project should contain DTOs and Interfaces that can be used throughout the stack. This project should, itself, have no dependencies. 

If desired, Interfaces could be further segmented into even more projects to reduce and control the project dependencies and references -- a drawback of the "Common" approach is that you could fetch an Adapter instead of going through its corresponding Service

### Data

The `Data` Project would make a connection out to sql, redis, cosmos, or some other backing persistence layer

If executing from the console app, you can talk to yourself. If configured (see below), the sends will be write-appended to a text file.

### Domain

The `Domain` Project in this architecture would be responsible for data-transformations, business logic/validation, or other such duties

### Configuration

This project makes use of a DI-able `ISettings` interface -- in my experience relying solely on `appsettings.json` bindings doesn't play well in enterprise-type applications which may leverage Kubernetes/Docker secrets.

Using this approach allows you to setup the ISettings properties however you want to -- whether it's from `appsettings.json`, using `Environment`, or what have you.

For fun, you can toggle between the output language by changing the `appsettings.json` prop:

```json
{
    "EnvironmentConfiguration": {
    "TargetLanguage": "EN", // EN | DE
    "OutputTarget": "file", // file is the only supported type,
    "FileName":  "talking-to-myself.txt" 
}

```

This will provide a translation from the adapter-layer. In a real world environment, translations would probably be done through a proper i18n service.

## Extras

I built a CQRS-esque command handler system into the console app so you can talk to yourself; COVID times are weird times.

If `OutputTarget` is set to `file` and `FileName` is specified, all of the handled `SendCommandHandler` will be logged to the file -- it will output to your `Documents` folder. This is implemented inside the documents folder. Mostly it was implemented to show a multi-layer command system that were injected between projects.

I didn't write as many tests as I usually would since I wanted to time-box this to a few hours. Would be happy to discuss approach/ideas/practices.