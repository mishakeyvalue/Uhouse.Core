namespace Uhouse.Core.Controllers

open Microsoft.AspNetCore.Mvc
open Uhouse.Core
open System
open TemperatureHelpers

[<Route("api/[controller]")>]
type TemperatureController (service : ITemperatureService) =
    inherit Controller()   

    let oldDays =  new DateTime(year = 2005, month = 1, day = 1)
    let isStale t = fromUnixTime t < oldDays

    [<HttpGet("period")>]
    member __.Get(fromDate : int64, toDate : int64) = 
        if isStale fromDate then raise (invalidArg "fromDate" "invalid fromDate")

        let fromDate' = fromUnixTime fromDate
        let toDate' = if isStale toDate then None else Some (fromUnixTime toDate)
        
        service.ForPeriod fromDate' toDate'

    [<HttpGet("all")>]
    member __.All() =
        service.GetAll()
        
    [<HttpGet("current")>]
    member __.Current() = service.GetCurrent()

    