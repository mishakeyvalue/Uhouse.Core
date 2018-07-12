module Tests

open System
open Xunit
open Uhouse.Core.PinScheduler
open System.Threading.Tasks

type InMemoryRepository() =
    let _storage = System.Collections.Generic.HashSet<PinSchedule>()
    interface IPinScheduleRepository with
        member this.Add(el: PinSchedule): unit = 
            _storage.Add(el) |> ignore
        member this.GetAll(): PinSchedule list = 
            _storage |> Seq.toList
        member this.Remove(el: PinSchedule): unit = 
            _storage.Remove(el) |> ignore

[<Fact>]
let ``Add any schedule - it has been raised`` () =
    // arrange
    let mutable setMeTrue = false

    let delay = 1.

    let switcher = { new IPinSwitcher with
                         member this.IsEnabled(arg1: int): bool = false
                         member this.TurnOff(arg1: int): unit = setMeTrue <- true
                         member this.TurnOn(arg1: int): unit = setMeTrue <- true 
                   }
    let scheduler = Scheduler(InMemoryRepository(), switcher) :> IPinScheduler
    // act
    
    scheduler.Add({ StartDate = DateTime.Now; EndDate = DateTime.MaxValue })

    // assert
    Task.Factory.StartNew (fun () -> 
          Async.Sleep (int delay * 4) |> Async.RunSynchronously 
          Assert.True setMeTrue             
          ()
        ) 
        |> Async.AwaitTask 
        |> Async.RunSynchronously 

