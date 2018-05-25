namespace Uhouse.Core.Controllers

open Microsoft.AspNetCore.Mvc
open Uhouse.Core
open System
open TemperatureHelpers
open Microsoft.AspNetCore.Cors

[<Route("api/[controller]")>]
[<EnableCors("SiteCorsPolicy")>]
type TemperatureController (service : ITemperatureService) =
    inherit Controller()   

    let oldDays =  new DateTime(year = 2005, month = 1, day = 1)
    let isStale t = fromUnixTime t < oldDays

    /// <summary>
    /// Get the temperature information for the specified time period.
    /// </summary>
    /// <param name="fromDate">The beginning of the interval (in the UNIX time</param>  
    /// <param name="toDate">The end of the interval (in the UNIX time)</param>  

    [<HttpGet("period")>]    
    member __.Period(fromDate : int64, toDate : int64) = 
        if isStale fromDate then raise (invalidArg "fromDate" "invalid fromDate")

        let fromDate' = fromUnixTime fromDate
        let toDate' = if isStale toDate then None else Some (fromUnixTime toDate)
        
        service.ForPeriod fromDate' toDate'

    /// <summary>
    /// Returns all stored temperature records
    /// </summary>
    /// <remarks>
    /// Caution! Amount of data could be pretty large!
    /// </remarks>
    [<HttpGet("all")>]
    member __.All() =
        service.GetAll()

    /// <summary>
    /// Returns current temperature
    /// </summary>
    [<HttpGet("current")>]
    member __.Current() = service.GetCurrent()

    