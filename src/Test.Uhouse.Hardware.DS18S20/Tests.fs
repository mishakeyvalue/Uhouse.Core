module Tests

open System
open Xunit
open System.Linq
open System.IO

[<Fact>]
let ``mapTemperature on valid input`` () =
    
    let validTemperatureFile = "38 00 4b 46 ff ff 0c 10 c7 : crc=c7 YES\n38 00 4b 46 ff ff 0c 10 c7 t=28000"

    Assert.Equal(Some(28.0), Sensor.mapTemperature validTemperatureFile)

[<Fact>]
let `` get temperature file when it is present `` () =
    let filepaths = [|"/sys/bus/w1/devices/10-00080346da3e"|]

    let result = Sensor.getTemperatureFile' filepaths

    let expected = Path.Combine (filepaths.Single(), "w1_slave")

    Assert.Equal(Some expected, result)