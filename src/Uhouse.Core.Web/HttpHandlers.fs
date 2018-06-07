namespace Uhouse.Core.Web

module HttpHandlers =

    open Microsoft.AspNetCore.Http
    open Giraffe
    open System
    open Uhouse.Core.Web.Models
    open Giraffe.HttpStatusCodeHandlers
    open Uhouse.Core.PinScheduler

    let handleGetHello =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                return! json "Everything is OK!" next ctx
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