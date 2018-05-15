module Uhouse.Core.TemperatureHelpers

open System

let epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)

let fromUnixTime (unixTime : int64) : DateTime = DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime

let toUnixTime (t : DateTime) : int64 = (new DateTimeOffset(t)).ToUnixTimeSeconds()