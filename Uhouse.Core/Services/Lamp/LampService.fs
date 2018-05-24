module Uhouse.Core.LampService

open Uhouse.Hardware.Relay

let getDummyService() = 
    { new ILampService with
          member this.IsEnabled: bool = true
          member this.TurnOff(): unit = ()
          member this.TurnOn(): unit = ()
    }

let getRelayService pinId : ILampService =

    let relayControl = SolidStateRelay.getRelayControl pinId
    { new ILampService with
          member this.IsEnabled: bool = relayControl.IsEnabled
          member this.TurnOff(): unit = relayControl.TurnOff()
          member this.TurnOn(): unit = relayControl.TurnOn()
    }

