module Uhouse.Core.Web.InMemoryMocks

open System.Collections.Generic
open Uhouse.Hardware.PinControl
open Uhouse.Core.Web.Services

let dummyPinControl = 
    let pins = new Dictionary<int, bool>()
    for i in 0..42 do
        pins.Add(i, false)
    {new IPinControl with
                            member this.IsEnabled(id: PinId): bool = 
                                pins.[id]
                            member this.TurnOff(id: PinId): unit = 
                                pins.[id] <- false
                            member this.TurnOn(id: PinId): unit = 
                                pins.[id] <- true
    }


type DummyTemperatureReader() =
    let rnd = new System.Random()
    interface ITemperatureReader with
        member this.GetCurrent(): double = rnd.NextDouble() * 15.