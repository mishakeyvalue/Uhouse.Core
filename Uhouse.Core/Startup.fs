namespace Uhouse.Core

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Uhouse.Core.Persistence
open System.Threading


type Startup private () =
    let startLogging (reader : ITemperatureReader) =
        while true do
            match reader.Read() with
                | Some t -> 
                    Persistence.insertTemperatureRecord{Value = t; Timestamp = DateTime.UtcNow.ToString() }
                | _      -> ()
            Thread.Sleep(1000)
        ()

    new (configuration: IConfiguration) as this =
        Startup() then
        this.Configuration <- configuration        

    // This method gets called by the runtime. Use this method to add services to the container.
    member this.ConfigureServices(services: IServiceCollection) =
        // Add framework services.
        services.AddMvc() |> ignore        

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment) =
        let getService = if env.IsDevelopment() then TemperatureReaders.dummyReader else TemperatureReaders.sensorReader
        let startLogging'() = getService() |> startLogging
        Task.Factory.StartNew(startLogging') |> ignore
        app.UseMvc() |> ignore
        
   

    member val Configuration : IConfiguration = null with get, set