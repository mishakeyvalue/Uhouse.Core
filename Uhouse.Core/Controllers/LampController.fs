namespace Uhouse.Core.Controllers

open Microsoft.AspNetCore.Mvc
open Uhouse.Core
open Uhouse.Hardware.Relay

[<Route("api/[controller]")>]
type LampController (lampService : ILampService) =
    inherit Controller()   
    [<HttpPost("on")>]
    member __.On() = 
        lampService.TurnOn()

    [<HttpPost("off")>]
    member __.Off() =
        lampService.TurnOff()