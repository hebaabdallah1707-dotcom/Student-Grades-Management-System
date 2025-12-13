module Program

open System
open System.Windows.Forms
open System.Drawing
open Types
open Grades
open Stat
open Crud
open Json

let mutable students = []
let mutable role = Admin

[<EntryPoint>]
[<STAThread>]
let main argv =
    Application.EnableVisualStyles()
    Application.SetCompatibleTextRenderingDefault false

    let form = new Form(Text = "Student Grades Management System" ,Width = 1000 ,Height = 670,StartPosition = FormStartPosition.CenterScreen)

    let panel = new Panel(Dock = DockStyle.Fill, Visible = false)

    let rolePanel = new Panel(Dock = DockStyle.Fill)

    let label = new Label(Text = "Select Role :", Top = 100 ,Left = 360,AutoSize = true
                                    ,Font = new Font("Segoe UI", 30f, FontStyle.Bold))

    let btnRoleAdmin = new Button(Text = "Admin", Top = 200, Left = 200, Width = 290, Height = 400,
                                    Font = new Font("Segoe UI", 20f, FontStyle.Bold))
    let btnRoleViewer = new Button(Text = "Viewer", Top = 200, Left = 500, Width = 290, Height = 400,
                                            Font = new Font("Segoe UI", 20f, FontStyle.Bold))

    let grid = new DataGridView(Top = 20 ,Left = 20 ,Width = 600 ,Height = 570, Font = new Font("Segoe UI", 9f, FontStyle.Bold))
    grid.ColumnCount <- 4
    grid.Columns.[0].Name <- "ID"
    grid.Columns.[1].Name <- "Name"
    grid.Columns.[2].Name <- "Average"
    grid.Columns.[3].Name <- "Total"

    grid.AutoSizeColumnsMode <- DataGridViewAutoSizeColumnsMode.Fill

    let labelId = new Label(Text = "ID :" ,Top = 20 ,Left = 640,
                                    Font = new Font("Segoe UI", 10f, FontStyle.Bold))

    let textBoxId = new TextBox(Top = 40 ,Left = 640 ,Width = 300, PlaceholderText = "Enter Student ID")
    labelId.Click.Add(fun _ -> textBoxId.Focus() |> ignore )


    let labelName = new Label(Text = "Name :", Top = 80 ,Left = 640,
                                        Font = new Font("Segoe UI", 10f, FontStyle.Bold))

    let textBoxName = new TextBox(Top = 100 ,Left = 640 ,Width = 300, PlaceholderText = "Enter Student Name")
    labelName.Click.Add(fun _ -> textBoxName.Focus() |> ignore )

    let labelGrades = new Label(Text = "Grades :",Top =140 ,Left=640,
                                        Font = new Font("Segoe UI", 10f, FontStyle.Bold))

    let textBoxGrades = new TextBox(Top = 160 ,Left = 640 ,Width = 300, PlaceholderText = "Enter grades separated by commas")
    labelGrades.Click.Add(fun _ -> textBoxGrades.Focus() |> ignore )

    
    let labelFile= new Label(Text = "File Name :" ,Top = 320 ,Left = 640,
                                        Font = new Font("Segoe UI", 10f, FontStyle.Bold))

    let textBoxFile = new TextBox(Top = 340, Left = 640, Width = 300)
    labelFile.Click.Add(fun _ -> textBoxFile.Focus() |> ignore )

    let buttonAdd = new Button(Text = "Add Student" ,Top = 200 ,Left = 640 ,Width = 300 ,Height = 30,
                                        Font = new Font("Segoe UI", 10f, FontStyle.Bold))
    buttonAdd.MouseHover.Add(fun _ -> buttonAdd.BackColor <- Color.LightBlue)
    buttonAdd.MouseLeave.Add(fun _ -> buttonAdd.BackColor <- SystemColors.Control)

    let buttonDelete = new Button(Text = "Delete Student" ,Top = 240 ,Left = 640 ,Width = 300 ,Height = 30,
                                        Font = new Font("Segoe UI", 10f, FontStyle.Bold))
    buttonDelete.MouseHover.Add(fun _ -> buttonDelete.BackColor <- Color.LightBlue)
    buttonDelete.MouseLeave.Add(fun _ -> buttonDelete.BackColor <- SystemColors.Control)

    let buttonUpdate = new Button(Text = "Update Student" ,Top = 280 ,Left = 640 ,Width = 300 ,Height = 30,
                                        Font = new Font("Segoe UI", 10f, FontStyle.Bold))
    buttonUpdate.MouseHover.Add(fun _ -> buttonUpdate.BackColor <- Color.LightBlue)
    buttonUpdate.MouseLeave.Add(fun _ -> buttonUpdate.BackColor <- SystemColors.Control)

    let buttonSave = new Button(Text = "Save Data" ,Top = 380 ,Left = 640 ,Width = 300 ,Height = 30,
                                        Font = new Font("Segoe UI", 10f, FontStyle.Bold))
    
    buttonSave.MouseHover.Add(fun _ -> buttonSave.BackColor <- Color.LightBlue)
    buttonSave.MouseLeave.Add(fun _ -> buttonSave.BackColor <- SystemColors.Control)

    let files = new ComboBox(Top = 420, Left = 640, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList)

    let buttonLoad = new Button(Text = "Load Data" ,Top = 460 ,Left = 640 ,Width = 300 ,Height = 30,
                                        Font = new Font("Segoe UI", 10f, FontStyle.Bold))
    buttonLoad.MouseHover.Add(fun _ -> buttonLoad.BackColor <- Color.LightBlue)
    buttonLoad.MouseLeave.Add(fun _ -> buttonLoad.BackColor <- SystemColors.Control)

    let buttonClass = new Button(Text = "Class Statistics" ,Top = 500 ,Left = 640 ,Width = 300 ,Height = 30,
                                        Font = new Font("Segoe UI", 10f, FontStyle.Bold))
    buttonClass.MouseHover.Add(fun _ -> buttonClass.BackColor <- Color.LightBlue)
    buttonClass.MouseLeave.Add(fun _ -> buttonClass.BackColor <- SystemColors.Control)

    let buttonClear = new Button(Text = "Clear Data" ,Top = 540 ,Left = 640 ,Width = 300 ,Height = 30,
                                        Font = new Font("Segoe UI", 10f, FontStyle.Bold))
    buttonClear.MouseHover.Add(fun _ -> buttonClear.BackColor <- Color.LightBlue)
    buttonClear.MouseLeave.Add(fun _ -> buttonClear.BackColor <- SystemColors.Control)

    let Role () = match role with 
                    | Admin ->
                        buttonAdd.Enabled <- true
                        buttonDelete.Enabled <- true
                        buttonUpdate.Enabled <- true
                        buttonSave.Enabled <- true
                    | Viewer ->
                        buttonAdd.Enabled <- false
                        buttonDelete.Enabled <- false
                        buttonUpdate.Enabled <- false
                        buttonSave.Enabled <- false

    let refreshFiles () =
        files.Items.Clear()
        let Files = System.IO.Directory.GetFiles(Environment.CurrentDirectory + "/data")
        for f in Files do
            files.Items.Add(System.IO.Path.GetFileNameWithoutExtension(f)) |> ignore

    let refreshGrid () =
        grid.Rows.Clear()
        students |> List.sortBy (fun s -> s.Id) |> List.iter (fun s ->
            let avg = Grades.calculateAverage s
            let total = Grades.calculateTotal s
            grid.Rows.Add(s.Id, s.Name, avg, total) |> ignore
        )

    btnRoleAdmin.Click.Add(fun _ ->
        role <- Admin
        Role()
        refreshFiles ()
        rolePanel.Visible <- false
        panel.Visible <- true
    )

    btnRoleViewer.Click.Add(fun _ ->
        role <- Viewer
        Role()
        refreshFiles ()
        rolePanel.Visible <- false
        panel.Visible <- true
    )

    buttonAdd.Click.Add(fun _ ->
        try
            let id = int textBoxId.Text
            let name = textBoxName.Text
            let grades = textBoxGrades.Text.Split(',') |> Array.map int |> Array.toList
            
            let student = {Id = id; Name = name; Grades = grades}
            match Crud.addStudent role students student with
                | Ok updatedStudents ->
                    students <- updatedStudents
                    refreshGrid()
                
                | Error msg ->
                    MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
            
        with
        | _ -> MessageBox.Show("Invalid input", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
    )

    buttonDelete.Click.Add(fun _ ->
        try
            let id = int textBoxId.Text
            match Crud.deleteStudent role students id with
                | Ok updatedStudents ->
                    students <- updatedStudents
                    refreshGrid()
    
                | Error msg ->
                    MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
        with
        | _ -> MessageBox.Show("Invalid input", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
    )

    buttonUpdate.Click.Add(fun _ ->
        try
            let id = int textBoxId.Text
            let grades = textBoxGrades.Text.Split(',') |> Array.map int |> Array.toList
            match Crud.updateStudentGrades role students id grades with
                | Ok updatedStudents ->
                    students <- updatedStudents
                    refreshGrid()
                
                | Error msg ->
                    MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
        with
        | _ -> MessageBox.Show("Invalid input", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
    )

    buttonSave.Click.Add(fun _ ->
        if List.isEmpty students then 
            MessageBox.Show("No data to save", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
        else 
            let fileName = 
                if String.IsNullOrWhiteSpace(textBoxFile.Text) then
                    "students.json"
                else
                    textBoxFile.Text + ".json"
            match Json.save role students fileName with
            | Ok () -> 
                    MessageBox.Show("Data saved successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
                    refreshFiles ()
            | Error msg -> MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
    )

    buttonLoad.Click.Add(fun _ ->
        let fileName = 
            if String.IsNullOrWhiteSpace(textBoxFile.Text) then
                "students.json"
            else
                textBoxFile.Text + ".json"
        if List.isEmpty students  then
            match Json.load fileName with
            | Ok loadedStudents ->
                students <- loadedStudents
                refreshGrid()
                MessageBox.Show("Data loaded successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
            | Error msg ->
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
        else
            MessageBox.Show("Please clear existing data before loading", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning) |> ignore
    )

    buttonClass.Click.Add(fun _ ->
        if List.isEmpty students then
            MessageBox.Show("No students available", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
        else
            let ClassStats = Stat.calculateClassStatistics students
            let msg = sprintf "Number of students: %d\nHighest Average: %.2f\nLowest Average: %.2f\nPass Rate: %.2f%%"
                                (List.length students) ClassStats.Highest ClassStats.Lowest ClassStats.PassRate
            MessageBox.Show(msg, "Class Statistics", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
    )

    buttonClear.Click.Add(fun _ ->
        students <- []
        refreshGrid()
    )


    panel.Controls.AddRange
        [| (labelId :> Control); (textBoxId :> Control);
            (labelName :> Control); (textBoxName :> Control);
            (labelGrades :> Control); (textBoxGrades :> Control);
            (labelFile :> Control); (textBoxFile :> Control);
            (buttonAdd :> Control); (buttonDelete :> Control); (buttonUpdate :> Control);
            (buttonSave :> Control); (buttonLoad :> Control);
            (buttonClass :> Control); (buttonClear :> Control);
            (grid :> Control); (files :> Control);
        |]

    rolePanel.Controls.AddRange [| label :> Control; btnRoleAdmin :> Control; btnRoleViewer :> Control |]

    form.Controls.Add(panel)
    form.Controls.Add(rolePanel)
    // form.Controls.Add(labelId)

    Application.Run(form)
    0 