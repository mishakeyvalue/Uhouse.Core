namespace Uhouse.Core.Web

module HttpHandlers =

    open Microsoft.AspNetCore.Http
    open Giraffe
    open Uhouse.Core.Web.Models
    open Giraffe.HttpStatusCodeHandlers

    let handleGetHello =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                return! json "Everything is OK!" next ctx
            }

    let scheduleHandler = 
        fun (next: HttpFunc) (ctx : HttpContext) ->
            task {
                let! model = ctx.BindFormAsync<ScheduleModel>()
                let res = sprintf "You have scheduled quartz lamp for %A from now for %A" model.delay model.duration
                return! json res next ctx
            }