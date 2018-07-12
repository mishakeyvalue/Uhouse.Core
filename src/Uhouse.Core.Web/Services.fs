module Uhouse.Core.Web.Services

//open Uhouse.Core.PinScheduler
open System
open Uhouse.Hardware.PinControl

//type PinSwitcher (control: IPinControl) =
//    interface IPinSwitcher with
//          member this.TurnOff(): unit = 
//              printfn "The pin is turned OFF at %A" DateTime.Now              
//              control.TurnOff 12 // TODO
//          member this.TurnOn(): unit = 
//              printfn "The pin is turned ON at %A" DateTime.Now
//              control.TurnOn 12

type ITemperatureReader =
    abstract GetCurrent: unit -> double

type TemperatureReader() =
    let file = Sensor.getTemperatureFile() |> Option.get
    interface ITemperatureReader with
        member __.GetCurrent() = Sensor.readTemperature file |> Option.get