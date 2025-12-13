module Types

// Student Model Designer – F# records + data structures

    type Student = {
        Id : int
        Name : string
        Grades : int list
    }

    type ClassStatistics = {
        Highest : float
        Lowest : float
        PassRate : float
    }

    type Role = Admin | Viewer