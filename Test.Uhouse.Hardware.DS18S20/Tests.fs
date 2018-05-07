module Tests

open System
open Xunit

[<Fact>]
let ``mapTemperature on valid input`` () =
    
    let validTemperatureFile = "38 00 4b 46 ff ff 0c 10 c7 : crc=c7 YES\n38 00 4b 46 ff ff 0c 10 c7 t=28000"

    Assert.Equal(Some(28.0), Sensor.mapTemperature validTemperatureFile)