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

    [<EntryPoint>]
    let main args =        
        BuildWebHost(args).Run()
        exitCode
