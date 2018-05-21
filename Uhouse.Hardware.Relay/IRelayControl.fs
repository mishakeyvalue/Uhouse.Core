namespace Uhouse.Hardware.Relay

type IRelayControl =
    /// General-purpose input/output
    abstract GpioPinId : int
    abstract IsEnabled : bool

    abstract TurnOn: unit -> unit
    abstract TurnOff: unit -> unit