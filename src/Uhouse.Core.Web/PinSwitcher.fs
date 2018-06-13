namespace Uhouse.Core.Web

open Uhouse.Core.PinScheduler
open System
open Uhouse.Hardware.PinControl

type PinSwitcher (control: IPinControl) =
    interface IPinSwitcher with
          member this.TurnOff(): unit = 
              printfn "The pin is turned OFF at %A" DateTime.Now              
              control.TurnOff 12 // TODO
          member this.TurnOn(): unit = 
              printfn "The pin is turned ON at %A" DateTime.Now
              control.TurnOn 12