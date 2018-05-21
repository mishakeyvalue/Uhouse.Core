namespace Uhouse.Core

type ILampService =
    abstract TurnOn : unit -> unit
    abstract TurnOff : unit -> unit