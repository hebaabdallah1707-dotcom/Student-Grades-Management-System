module Stat

open Types 
open Grades

// Statistician – class-wide metrics

let calculatePassRate (averages: float list) : float =
    let passedStudents = 
        averages 
            |> List.filter (fun average -> average >= 50.0) 
            |> List.length

    if List.isEmpty averages then 0.0
    else (float passedStudents / float (List.length averages)) * 100.0

let  calculateClassStatistics (students: Student list) : ClassStatistics =
    if List.isEmpty students then 
        { Highest = 0.0; Lowest = 0.0; PassRate = 0.0 }
    else 
        let averages = List.map Grades.calculateAverage students
        let highest = List.max averages 
        let lowest = List.min averages
        let rate = calculatePassRate averages

        { Highest = highest; Lowest = lowest; PassRate = rate }
