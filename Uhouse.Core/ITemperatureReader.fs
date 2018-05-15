namespace Uhouse.Core

type ITemperatureReader =
    abstract Read : unit -> double option