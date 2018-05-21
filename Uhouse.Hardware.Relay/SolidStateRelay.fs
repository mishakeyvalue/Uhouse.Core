module Uhouse.Hardware.Relay.SolidStateRelay

open Unosquare.RaspberryIO
open Unosquare.RaspberryIO.Gpio

let getRelayControl (pinId: int) : IRelayControl =
    let pin = Pi.Gpio.Pins.Item pinId
    pin.PinMode <- GpioPinDriveMode.Output 

    {new IRelayControl with
        member __.GpioPinId = pinId        

        member __.IsEnabled = pin.Read()

        member __.TurnOn(): unit = 
            pin.Write GpioPinValue.High

        member __.TurnOff(): unit = 
            pin.Write GpioPinValue.Low
    }