module Sensor

open System
open System.IO
open Utils

type Temperature = double

let private rootDeviceDir = "/sys/bus/w1/devices/"

let private getFullFilePath deviceName = Path.Combine (rootDeviceDir, deviceName, "w1_slave")

let private pickTemperatureFile = Seq.tryFind (s_isPrefixOf "10")

let private parseToFloat (cs : char seq) = String.Concat(cs) |> float |> fun x -> x / 1000.0

let getTemperatureFile' getDirectories =
       pickTemperatureFile (getDirectories |> Seq.map (fun x -> Path.GetFileName(x)))
    |> Option.map getFullFilePath

let getTemperatureFile() = getTemperatureFile' (System.IO.Directory.GetDirectories rootDeviceDir)


let mapTemperature (s : string) = 
    words s 
    |> List.map (stripPrefix "t=")
    |> asum
    |> Option.map parseToFloat

let readTemperature f = File.ReadAllLines f |> Array.map mapTemperature |> Array.toList |> asum
