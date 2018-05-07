module Sensor

open System
open System.IO
open Utils

type Temperature = double

let rootDeviceDir = "/sys/bus/w1/devices/"

let getFullFilePath deviceName = Path.Combine (rootDeviceDir, deviceName, "w1_slave")

let pickTemperatureFile = Seq.tryFind (s_isPrefixOf "10")

let getTemperatureFile = 
       pickTemperatureFile (System.IO.Directory.GetDirectories rootDeviceDir |> Seq.map (fun x -> Path.GetDirectoryName(x)))
    |> Option.map getFullFilePath

let parseToFloat (cs : char seq) = String.Concat(cs) |> float |> fun x -> x / 1000.0

let mapTemperature (s : string) = 
    words s 
    |> List.map (stripPrefix "t=")
    |> asum
    |> Option.map parseToFloat

let readTemperature f = File.ReadAllLines f |> Seq.map mapTemperature
