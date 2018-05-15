namespace Uhouse.Core.Controllers

open Microsoft.AspNetCore.Mvc
open Uhouse.Core

[<Route("api/[controller]")>]
type ValuesController () =
    inherit Controller()
    
    [<HttpGet>]
    member this.Get() =
        Persistence.readTemp()
        

    [<HttpGet("{id}")>]
    member this.Get(id:int) =
        "value"

    [<HttpPost>]
    member this.Post([<FromBody>]value:string) =
        ()

    [<HttpPut("{id}")>]
    member this.Put(id:int, [<FromBody>]value:string ) =
        ()

    [<HttpDelete("{id}")>]
    member this.Delete(id:int) =
        ()
