module Uhouse.Core.TemperatureReaders

open System

let getDummyReader() = 
    let random = new Random()
    let getRandom() = random.NextDouble() * 22.
    { new ITemperatureReader with member __.Read() = getRandom() |> Some }

let fromSome = function
    | Some x -> x
    | _      -> raise (new InvalidOperationException("Argument was None."))
let getSensorReader() =
    let temperatureFile = Sensor.getTemperatureFile() |> fromSome
    { new ITemperatureReader with member __.Read() = Sensor.readTemperature temperatureFile}
