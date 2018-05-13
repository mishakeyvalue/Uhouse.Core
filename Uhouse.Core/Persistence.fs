namespace Uhouse.Core.Persistence

module Persistence =

    open System
    open System.Collections.Generic
    open Dapper
    open Microsoft.Data.Sqlite
    open System.Linq

    type TemperatureRecord = {
            Value:float;
            Timestamp:string;
        }
   
    let databaseFilename = "this.sqlite"
    let connectionStringFile = sprintf "Data Source=%s;" databaseFilename  

    let dbExecute (connection:SqliteConnection) (sql:string) (data:_) = 
        connection.Execute(sql, data)

    let connection = new SqliteConnection(connectionStringFile)
    connection.Open()
    
    let insertTemperatureRecord (data:TemperatureRecord) =
        let insertQuery = "INSERT INTO TEMPERATURE(value, Timestamp) VALUES (@Value, @Timestamp)"
        dbExecute connection insertQuery  data |> ignore
        ()
    let dbQuery<'T> (connection:SqliteConnection) (sql:string) (parameters:IDictionary<string, obj> option) = 
        match parameters with
            | Some(p) -> connection.Query<'T>(sql, p)
            | None    -> connection.Query<'T>(sql)
    let readTempSql = "SELECT Value, Timestamp FROM TEMPERATURE"
    let readTemp = dbQuery<TemperatureRecord> connection readTempSql None

    let initQuery = "CREATE TABLE IF NOT EXISTS TEMPERATURE (Value float, Timestamp DateTime);"
    let structureCommand = new SqliteCommand(initQuery, connection)
    structureCommand.ExecuteNonQuery() 


