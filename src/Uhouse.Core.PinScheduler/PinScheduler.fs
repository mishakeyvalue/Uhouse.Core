module Uhouse.Core.PinScheduler

open System

type PinSchedule = {
    StartDate: DateTime
    EndDate: DateTime
}

type IPinScheduler = 
    abstract GetAll : unit -> PinSchedule list
    abstract Add : PinSchedule -> unit

type IPinScheduleRepository =
    abstract GetAll : unit -> PinSchedule list
    abstract Remove : PinSchedule -> unit
    abstract Add : PinSchedule -> unit

type IPinSwitcher =
    abstract IsEnabled : int -> bool
    abstract TurnOn: int -> unit
    abstract TurnOff: int -> unit

let inline liesBetween a x b = a >= x && x <= b

type Scheduler(repository: IPinScheduleRepository, pinSwitcher: IPinSwitcher) =
    let timerInterval = 1. * 1000.
    let timer = new System.Timers.Timer(timerInterval)
    do timer.AutoReset <- true
    
    // events are automatically IObservable
    let observable = timer.Elapsed

    let pinTick _ = 
        ()
    do observable |> Observable.subscribe pinTick |> ignore
        
    interface IPinScheduler with
        member __.Add(s) = repository.Add s
        member __.GetAll() = repository.GetAll()