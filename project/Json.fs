module Json

open System.IO
open System.Text.Json
open Types
open Roles


// Persistence Developer – JSON read/write
// https://compile7.org/serialize-and-deserialize/how-to-serialize-and-deserialize-json-in-f/

let save(role:Role) (students: Student list) fileName : Result<unit, string> =
    if Roles.checkRole role then
        if File.Exists("data/" + fileName) |> not then
            let option = JsonSerializerOptions(WriteIndented = true)
            let json = JsonSerializer.Serialize(students, option)
            File.WriteAllText("data/" + fileName, json)
            Ok ()
        else
            Error "File already exist"
    else
        Error "You don't have permission"

let load (fileName) : Result<Student list, string> = 
    if File.Exists("data/" + fileName) then
        try 
            let json = File.ReadAllText("data/" + fileName)
            let students = JsonSerializer.Deserialize<Student list>(json)
            Ok students
        with
        | _ -> Error "Failed to read the file"
    else
        Error "File does not exist"
