module Grades

open Types

// Grade Calculation Developer Functions

let calculateTotal (student: Student) : int =
    List.sum student.Grades

let calculateAverage (student: Student) : float =
    if List.isEmpty student.Grades then 0.0
    else List.averageBy float student.Grades 