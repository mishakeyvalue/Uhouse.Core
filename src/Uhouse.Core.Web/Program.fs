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
open Uhouse.Core.PinScheduler
open Uhouse.Hardware.PinControl
open Uhouse.Core.Web.Models
open System.Collections.Generic

let dummyPinControl = 
    let pins = new Dictionary<int, bool>()
    for i in 0..42 do
        pins.Add(i, false)
    {new IPinControl with
                            member this.IsEnabled(id: PinId): bool = 
                                pins.[id]
                            member this.TurnOff(id: PinId): unit = 
                                pins.[id] <- false
                            member this.TurnOn(id: PinId): unit = 
                                pins.[id] <- true
    }
// ---------------------------------
// Web app
// ---------------------------------
let parsingErrorHandler err = RequestErrors.BAD_REQUEST err

let webApp =
    choose [
        subRouteCi "/api"
            (choose [
                subRouteCi "/quartz" 
                    (choose [
                        subRouteCi "/schedule" 
                            (choose [
                                POST >=> routeCi "/now" >=> scheduleHandler
                            ])

                    ])
                subRouteCi "/pin"
                    (choose [
                        route "/status" >=> tryBindQuery<PinStatusRequestModel> parsingErrorHandler None pinStatusHandler
                        POST >=> routeCif "/%i/on" (fun pinId -> switchHandler pinId TurnOn)
                        POST >=> routeCif "/%i/off" (fun pinId -> switchHandler pinId TurnOff)
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
    let pinControl _ = if ctx.HostingEnvironment.IsDevelopment() then dummyPinControl else pinControl
    services.AddSingleton<IPinControl>(pinControl) |> ignore

    services.AddScoped<PinSwitcher>() |> ignore 

    let pinSchedulerFactory (servcices:IServiceProvider)=
        let pinSwitcher = servcices.GetService<PinSwitcher>()
        (PinSchedulerFactory.Init pinSwitcher).Result
        
    services.AddSingleton<IPinScheduler>(pinSchedulerFactory) |> ignore

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