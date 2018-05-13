namespace Uhouse.Core

open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Logging
open Uhouse.Core.Persistence
open System.Threading.Tasks

module Program =
    open System.Threading.Tasks
    open System.Threading
    open Persistence

    let exitCode = 0

    let BuildWebHost args =
        WebHost
            .CreateDefaultBuilder(args)
            .UseUrls("http://*:8000")
            .UseStartup<Startup>()
            .Build()

    let startLogging() =
        let temperatureFile = Sensor.getTemperatureFile() |> Option.defaultValue ""
        if temperatureFile = "" then nullArg "temperature file"

        while true do
            match Sensor.readTemperature temperatureFile with
                | Some t -> 
                    Persistence.insertTemperatureRecord {Value = t; Timestamp = DateTime.UtcNow.ToString() }
                    printf "%A" {Value = t; Timestamp = DateTime.UtcNow.ToString() }
                | _      -> ()
            Thread.Sleep(1000)
        ()

    [<EntryPoint>]
    let main args =        
        Task.Factory.StartNew(startLogging)
        BuildWebHost(args).Run()

        exitCode
