module Uhouse.Core.Web.PinSwitchers

open Uhouse.Core.PinScheduler
open Uhouse.Hardware.PinControl
open System

let getDummyPinSwitcher() = 
    { new IPinSwitcher with
          member this.TurnOff(): unit = 
            printfn "The pin is turned OFF at %A" DateTime.Now
          member this.TurnOn(): unit = 
            printfn "The pin is turned ON at %A" DateTime.Now
    }

let getPinSwitcher(pinId) =
    let pinControl = PinControlFactory.getPinControl()
    { new IPinSwitcher with
          member this.TurnOff(): unit = 
              pinControl.TurnOff pinId
          member this.TurnOn(): unit = 
              pinControl.TurnOn pinId   
    }