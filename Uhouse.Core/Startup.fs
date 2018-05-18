namespace Uhouse.Core

open System
open System.Threading.Tasks
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Uhouse.Core.Persistence
open System.Threading
open Swashbuckle.AspNetCore.Swagger
open Swashbuckle.AspNetCore.SwaggerGen
open System.IO


type Startup private () =
    let startLogging (reader : ITemperatureReader) =
        Persistence.init()
        while true do
            match reader.Read() with
                | Some t -> 
                    Persistence.insertTemperatureRecord{Value = t; Timestamp = DateTime.UtcNow.ToString() }
                | _      -> ()
            Thread.Sleep(1000)
        ()

    new (configuration: IConfiguration, env : IHostingEnvironment) as this =
        Startup() then
        this.Configuration <- configuration 
        this.HostingEnvironment <- env

    // This method gets called by the runtime. Use this method to add services to the container.
    member this.ConfigureServices(services: IServiceCollection) =
        let getService _ = 
            if this.HostingEnvironment.IsDevelopment() 
                then TemperatureReaders.getDummyReader()
                else TemperatureReaders.getSensorReader()
        services.AddSingleton<ITemperatureReader>(getService) |> ignore
        services.AddScoped<ITemperatureService, TemperatureService>() |> ignore

        let info = new Info(Title = "uHouse master node API", Version = "v1")
        let swaggerGen (c :SwaggerGenOptions)= 
            c.SwaggerDoc("v1", info)
            let xmlPath = Path.Combine(AppContext.BaseDirectory, "DOC.XML")
            c.IncludeXmlComments xmlPath
            let schemaFactory() : Schema =
                new Schema(Example = { Value = 22.65; Timestamp = DateTime.UtcNow.ToString()})
            c.MapType<TemperatureRecord>(Func<Schema>(schemaFactory))
        services.AddSwaggerGen(Action<SwaggerGenOptions>(swaggerGen)) |> ignore
        // Add framework services.
        services.AddMvc() |> ignore




    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment) =
        
        let startLogging'() = app.ApplicationServices.GetService<ITemperatureReader>() |> startLogging
        Task.Factory.StartNew(startLogging') |> ignore

        app.UseSwagger() |> ignore
        app.UseMvc() |> ignore
        
   

    member val Configuration : IConfiguration = null with get, set
    member val HostingEnvironment : IHostingEnvironment = null with get, set