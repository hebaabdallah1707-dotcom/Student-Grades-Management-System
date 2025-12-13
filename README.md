# Student Grades Management System

This project is a **Student Grades Management System** developed using **F# (.NET)**.  
The system is designed to manage student records and their grades in a structured and modular way, supporting core data management operations, statistical analysis, and role-based access control.

## Project Overview

The system allows users to manage students and their grades through a clear modular architecture.  
It supports creating, updating, deleting, and viewing student records, as well as computing grade-related statistics such as averages and pass rates.  
This project is implemented as an academic system to demonstrate clean design principles and separation of concerns.

## System Features

- Manage student records and grades  
- Perform CRUD operations (Create, Read, Update, Delete)  
- Calculate statistical metrics:
  - Average grade
  - Highest average
  - Lowest average
  - Pass rate
- Role-Based Access Control (RBAC)
- Data persistence using JSON files

## Roles and Permissions

- **Admin**
  - Add student records
  - Update student grades
  - Delete student records
  - Save data to JSON files

- **Viewer**
  - View student records and grades only
  - No permission to add, update, delete, or save data

## Documentation

Detailed project documentation is available here:  
[Project Documentation](./docs/DocForPI3.docx)

## Technologies Used

- F# (.NET)
- Windows Forms
- JSON Serialization

## Conclusion

This project demonstrates a well-structured academic system for managing student grades, applying modular design principles and role-based access control to ensure clarity, maintainability, and future extensibility.
