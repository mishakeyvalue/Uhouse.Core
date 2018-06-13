namespace Uhouse.Core.Web

module HttpHandlers =

    open Microsoft.AspNetCore.Http
    open Giraffe
    open System
    open Uhouse.Core.Web.Models
    open Uhouse.Core.PinScheduler
    open Uhouse.Hardware.PinControl
    type PinCommand = | TurnOn | TurnOff
    let switchHandler pinId pinCommand =
        fun (next : HttpFunc) (ctx : HttpContext) ->
          task {
                let control = ctx.GetService<IPinControl>()
                match pinCommand with
                | TurnOn -> control.TurnOn pinId
                | TurnOff -> control.TurnOff pinId
                return! Successful.OK () next ctx
          }

    let pinStatusHandler model =
        fun (next : HttpFunc) (ctx : HttpContext) ->
           task {
                let control = ctx.GetService<IPinControl>()
                let result = model.pins 
                            |> List.map (fun id -> (id, control.IsEnabled id))
                            |> dict
                return! json result next ctx
           }


    let scheduleHandler = 
        fun (next: HttpFunc) (ctx : HttpContext) ->
            task {
                let! model = ctx.BindFormAsync<ScheduleModel>()
                let service = ctx.GetService<IPinScheduler>()
                let startDate = DateTimeOffset.Now.Add(Option.defaultValue TimeSpan.Zero model.delay)
                let id = service.Schedule(startDate, model.duration)                
                return! text (id.ToString()) next ctx
            }