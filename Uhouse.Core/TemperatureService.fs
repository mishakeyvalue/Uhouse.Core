namespace Uhouse.Core 

open System

type TemperatureService(reader : ITemperatureReader) =
    interface ITemperatureService with
        member __.GetAll() = Persistence.readTemperature()
        member __.GetCurrent() = 
               reader.Read() 
            |> TemperatureReaders.fromSome 
            |> fun t -> { Value = t; Timestamp = DateTime.UtcNow.ToString() }
        
        member __.ForPeriod fromDate toDate = Persistence.forPeriod fromDate (Option.defaultValue DateTime.UtcNow toDate)
        

