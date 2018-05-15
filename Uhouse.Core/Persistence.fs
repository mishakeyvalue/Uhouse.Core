module Uhouse.Core.Persistence

open Dapper
open Microsoft.Data.Sqlite

let private execute' (f : SqliteConnection -> 'T) : 'T =        
    let databaseFilename = "this.sqlite"
    let connectionStringFile = sprintf "Data Source=%s;" databaseFilename  
    let connection = new SqliteConnection(connectionStringFile)
    connection.Open()
    let r = f connection
    connection.Close()
    r
    
let private execute sql data = execute' (fun c -> c.Execute(sql, data))

let private query<'T> (sql:string)= execute' (fun c -> c.Query<'T>(sql))

let private nonQuery' sql c = 
    let c = new SqliteCommand(sql, c);
    c.ExecuteNonQuery()

let private nonQuery sql = execute' (nonQuery' sql)

type TemperatureRecord = {
        Value:float;
        Timestamp:string;
    }   
    
let insertTemperatureRecord (data:TemperatureRecord) =
    let insertQuery = "INSERT INTO TEMPERATURE(value, Timestamp) VALUES (@Value, @Timestamp)"
    execute insertQuery  data |> ignore

let readTemp() = 
    let readTempSql = "SELECT Value, Timestamp FROM TEMPERATURE"
    query<TemperatureRecord> readTempSql

let private initQuery = "CREATE TABLE IF NOT EXISTS TEMPERATURE (Value float, Timestamp DateTime);"
nonQuery initQuery |> ignore



