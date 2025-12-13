module Roles

open Types

// Role Manager – access logic
let checkRole (role : Role) : bool = 
    match role with
    | Admin -> true
    | Viewer -> false
