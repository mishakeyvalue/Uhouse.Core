namespace Uhouse.Core

open Persistence
open System

type ITemperatureService =
    abstract GetAll : unit -> TemperatureRecord seq
    abstract GetCurrent : unit -> TemperatureRecord
    abstract ForPeriod : fromDate : DateTime -> toDate : DateTime option -> TemperatureRecord seq

