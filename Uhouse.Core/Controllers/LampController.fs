namespace Uhouse.Core.Controllers

open Microsoft.AspNetCore.Mvc
open Uhouse.Core
open Uhouse.Hardware.Relay

[<Route("api/[controller]")>]
type LampController (lampService : ILampService) =
    inherit Controller()   

    /// <summary>
    /// Turns the lamp on.
    /// </summary>
    [<HttpPost("on")>]
    member __.On() = 
        lampService.TurnOn()

    /// <summary>
    /// Turns the lamp off.
    /// </summary>
    [<HttpPost("off")>]
    member __.Off() =
        lampService.TurnOff()


    /// <summary>
    /// Checks if the lamp is turned on.
    /// </summary>
    [<HttpGet("isEnabled")>]
    member __.IsEnabled() =
        lampService.IsEnabled
