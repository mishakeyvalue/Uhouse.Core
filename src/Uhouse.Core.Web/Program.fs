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

// ---------------------------------
// Web app
// ---------------------------------

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
                        //subRoute "/schedule" 
                        //(choose [
                        //    POST >=> text ""
                        //])

                    ])
                subRouteCi "/pin"
                    (choose [
                        POST >=> routeCif "/%i/on" (fun pinId -> pinControl.TurnOn pinId; Successful.OK ())
                        POST >=> routeCif "/%i/off" (fun pinId -> pinControl.TurnOff pinId; Successful.OK ())
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
    let pinSwitcher = if ctx.HostingEnvironment.IsDevelopment() then PinSwitchers.getDummyPinSwitcher() else PinSwitchers.getPinSwitcher 0
    let pinScheduler = (PinSchedulerFactory.Init pinSwitcher).Result
    services.Add(ServiceDescriptor(typeof<IPinScheduler>, pinScheduler))

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