namespace Uhouse.Core.Web.Models

open System

[<CLIMutable>]
type ScheduleModel = 
    {
        delay: TimeSpan option
        duration: TimeSpan
    }

[<CLIMutable>]
type PinStatusRequestModel = 
    {
        pins: int list
    }