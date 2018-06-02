namespace Uhouse.Core.Controllers

open Microsoft.AspNetCore.Mvc
open Uhouse.Hardware.PinControl

[<Route("api/[controller]")>]
type PinController (pinControl : IPinControl) =
    inherit Controller()   
    /// <summary>
    /// Enables the GPIO pin with the given id.
    /// </summary>
    [<HttpPost("{id}/on")>]
    member __.On id = 
        pinControl.TurnOn id

    /// <summary>
    /// Disables the GPIO pin with the given id.
    /// </summary>
    [<HttpPost("{id}/off")>]
    member __.Off id =
        pinControl.TurnOff id


    /// <summary>
    /// Checks if the pin with the given id is turned on.
    /// </summary>
    [<HttpGet("{id}/isEnabled")>]
    member __.IsEnabled id =
        pinControl.IsEnabled id
