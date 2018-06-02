namespace Uhouse.Core.Controllers

open Microsoft.AspNetCore.Mvc
open Uhouse.Hardware.PinControl

[<Route("api/[controller]")>]
type LampController (pinControl : IPinControl) =
    inherit Controller()   
    let LampGpio = 12;
    /// <summary>
    /// Turns the lamp on.
    /// </summary>
    [<HttpPost("on")>]
    member __.On() = 
        pinControl.TurnOn LampGpio

    /// <summary>
    /// Turns the lamp off.
    /// </summary>
    [<HttpPost("off")>]
    member __.Off() =
        pinControl.TurnOff LampGpio


    /// <summary>
    /// Checks if the lamp is turned on.
    /// </summary>
    [<HttpGet("isEnabled")>]
    member __.IsEnabled() =
        pinControl.IsEnabled LampGpio
