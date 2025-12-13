module Crud

open Types
open Roles


let isExist (students: Student list) (id: int) : bool =
    students |> List.exists (fun student -> student.Id = id)

// CRUD Developer – add/edit/delete operations
let addStudent (role:Role) (students: Student list) (newStudent: Student) : Result<Student list, string> = 
    if Roles.checkRole role then
        if isExist students newStudent.Id then
            Error "Student with this ID already exists."
        else
            Ok (List.append students [newStudent])
    else
        Error "You don't have permission"

let deleteStudent (role:Role) (students: Student list) (id: int) : Result<Student list, string> =
    if Roles.checkRole role then
        if isExist students id  then
            let student = students |> List.filter (fun student -> student.Id = id)
            Ok (List.except student students)
        else
            Error "Student with this ID does not exist."
    else
        Error "You don't have permission" 

let updateStudentGrades (role:Role) (students: Student list) (id: int) (grades: int list) : Result<Student list, string> = 
    if Roles.checkRole role then
        if isExist students id then
            Ok (students |> List.map (fun student -> 
                if student.Id = id then 
                    { student with Grades = grades } 
                else student
            ))
        else
            Error "Student with this ID does not exist."
    else
        Error "You don't have permission"