module Uhouse.Core.TemperatureReaders

open System

let dummyReader() = 
    let random = new Random()
    let getRandom() = random.NextDouble() * 22.
    { new ITemperatureReader with member __.Read() = getRandom() |> Some }

let private fromSome = function
    | Some x -> x
    | _      -> raise (new InvalidOperationException("Argument was None."))
let sensorReader() =
    let temperatureFile = Sensor.getTemperatureFile() |> fromSome
    { new ITemperatureReader with member __.Read() = Sensor.readTemperature temperatureFile}
