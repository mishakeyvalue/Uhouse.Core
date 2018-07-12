module Uhouse.Core.Web.App

open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Uhouse.Core.Web.HttpHandlers
open Uhouse.Core.Web
open Uhouse.Hardware.PinControl
open Uhouse.Core.Web.Models
open Uhouse.Core.Web.InMemoryMocks
open Services

// ---------------------------------
// Web app
// ---------------------------------
let parsingErrorHandler err = RequestErrors.BAD_REQUEST err

let webApp =
    choose [
        subRouteCi "/api"
            (choose [
                subRouteCi "/lamp" 
                    (choose [
                        subRouteCi "/schedule" 
                            (choose [
                                POST >=> scheduleHandler
                            ])

                    ])
                subRouteCi "/pin"
                    (choose [
                        route "/status" >=> tryBindQuery<PinStatusRequestModel> parsingErrorHandler None pinStatusHandler
                        POST >=> routeCif "/%i/on" (fun pinId -> switchHandler pinId TurnOn)
                        POST >=> routeCif "/%i/off" (fun pinId -> switchHandler pinId TurnOff)
                    ])
                subRouteCi "/temperature"
                    (choose [
                        GET >=> route "/current" >=> currentTemperatureHandler
                    ])

            ])
        setStatusCode 404 >=> text "Not Found" ]

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(EventId(), ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

// ---------------------------------
// Config and Main
// ---------------------------------

let configureCors (builder : CorsPolicyBuilder) =
    builder.WithOrigins("http://localhost:8080")
           .AllowAnyMethod()
           .AllowAnyHeader()
           |> ignore

let configureApp (app : IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IHostingEnvironment>()
    (match env.IsDevelopment() with
    | true  -> app.UseDeveloperExceptionPage()
    | false -> app.UseGiraffeErrorHandler errorHandler)
        .UseCors(configureCors)
        .UseGiraffe(webApp)

let configureServices (ctx: WebHostBuilderContext) (services : IServiceCollection) =
    let isDevelopment = ctx.HostingEnvironment.IsDevelopment()
    let pinControl _ = if isDevelopment then dummyPinControl else pinControl
    services.AddSingleton<IPinControl>(pinControl) |> ignore

    //services.AddScoped<PinSwitcher>() |> ignore 

    let temperatureReaderType = 
        if isDevelopment 
            then typeof<DummyTemperatureReader> 
            else typeof<TemperatureReader>
    services.AddSingleton(typeof<ITemperatureReader>, temperatureReaderType) |> ignore

    //let pinSchedulerFactory (servcices:IServiceProvider)=
    //    let pinSwitcher = servcices.GetService<PinSwitcher>()
    //    (PinSchedulerFactory.Init pinSwitcher).Result
        
    //services.AddSingleton<IPinScheduler>(pinSchedulerFactory) |> ignore

    services.AddCors()    |> ignore
    services.AddGiraffe() |> ignore

let configureLogging (builder : ILoggingBuilder) =
    let filter (l : LogLevel) = l.Equals LogLevel.Error
    builder.AddFilter(filter).AddConsole().AddDebug() |> ignore

[<EntryPoint>]
let main _ =
    WebHostBuilder()
        .UseKestrel()
        .UseUrls("http://*:8090")
        .Configure(Action<IApplicationBuilder> configureApp)
        .ConfigureServices(configureServices)
        .ConfigureLogging(configureLogging)
        .Build()
        .Run()
    0