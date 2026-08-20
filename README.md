# KickBlast Judo Gym - Training Fee Calculation System

![KickBlast Judo Gym](Resources/judo_background.jpg)

## Overview
**KickBlast Judo Gym System** is a Windows Forms (WinForms) application built using **C#** and **.NET Framework 4.7.2** in **Visual Studio 2022**. It was developed for KickBlast Judo Gym to streamline athlete registrations, compute itemized monthly training fees, enforce gym eligibility rules, evaluate weight categories, and manage athlete database records.

This project is submitted as part of **Unit 01: Programming Assignment (Pearson BTEC HND in Computing)**.

---

## Key Features

### 1. User Authentication & Dashboard
- **Login Screen (`Form1`)**: Secure entry for coaches and admins with custom judo artwork background. Default credentials: `admin` / `1234`.
- **Gym Dashboard (`Form3`)**: Overview of system features, instructions, and quick navigation to fee calculation and registration.

### 2. Athlete Registration & Fee Calculation (`Form2`)
Calculates itemized monthly expenses based on student enrolment options:
- **Training Plans**:
  - **Beginner** (2 sessions/week): £25.00 / month
  - **Intermediate** (3 sessions/week): £30.00 / month
  - **Elite** (5 sessions/week): £35.00 / month
- **Private Coaching**:
  - Rate: £9.50 per hour.
  - *Rule Enforced*: Maximum **5 hours per week** (max 20 hours/month calculated over 4 weeks).
- **Competitions**:
  - Entry Fee: £22.00 per competition.
  - *Rule Enforced*: Competitions are restricted to **Intermediate** and **Elite** athletes only. Beginners entering competitions are flagged with a rule validation error.

### 3. Athlete Weight Category Comparison
Automatically evaluates the athlete's **Current Weight (kg)** against their **Competition Weight Category (kg)**:
- **On Weight**: Current weight matches competition category.
- **Over Weight**: Highlights exact excess weight (e.g. *Over Weight by 0.50 kg*).
- **Under Weight**: Highlights required weight gain (e.g. *Under Weight by 1.20 kg*).

### 4. Database CRUD & Data Persistence (`DatabaseHelper.cs`)
Full data management and persistence supporting all CRUD operations:
- **Register / Calculate**: Computes itemized costs and outputs formatted summary report.
- **Save**: Persists athlete record to local JSON database storage (`kickblast_athletes.json`).
- **Update**: Updates training plan, weight, coaching hours, or competition entries for existing athletes.
- **Delete**: Removes athlete records with confirmation prompt.
- **Search**: Auto-populates forms by searching athlete name or ID.
- **Clear**: Resets input textboxes and report display.

---

## Technical Specifications & Architecture

| Component | Description |
| :--- | :--- |
| **Language** | C# (Visual C# 7.3 / .NET Framework 4.7.2) |
| **IDE** | Visual Studio 2022 |
| **UI Framework** | Windows Forms (WinForms) |
| **Storage** | File-backed JSON Database (`kickblast_athletes.json`) |
| **Project Solution** | `KickblastJudoGym.sln` / `KickblastJudoGym.csproj` |

---

## Project Structure

```
Programming Assignment E222561 M.C Lamahewage/
├── App.config                     # Application configuration
├── DatabaseHelper.cs              # Persistence & CRUD storage manager
├── Form1.cs                       # Login Form logic
├── Form1.Designer.cs              # Login Form UI layout
├── Form1.resx                     # Login Form resources
├── Form2.cs                       # Registration & Fee Calculator logic
├── Form2.Designer.cs              # Registration & Fee Calculator UI layout
├── Form2.resx                     # Registration Form resources
├── Form3.cs                       # Dashboard Form logic
├── Form3.Designer.cs              # Dashboard Form UI layout
├── Form3.resx                     # Dashboard Form resources
├── KickblastJudoGym.csproj        # Visual Studio C# Project File
├── KickblastJudoGym.sln           # Visual Studio Solution File
├── Program.cs                     # Main entry point (Main)
├── Properties/                    # Assembly and Resource settings
├── Resources/                     # Background images & assets
└── README.md                      # Project documentation
```

---

## How to Run

### Method 1: Visual Studio 2022 (Recommended)
1. Double-click [KickblastJudoGym.sln](KickblastJudoGym.sln) to open in Visual Studio 2022.
2. Press **F5** (or click **Start**) to build and run the application.

### Method 2: Command Line / Executable
Execute the compiled binary from PowerShell or Command Prompt:
```powershell
Start-Process "bin\Debug\KickblastJudoGym.exe"
```

---

## Student & Course Details
- **Student Name**: Malshi Charindri Lamahewage
- **Student ID**: E222561
- **Course**: Pearson BTEC HND in Computing
- **Module**: Unit 01 – Programming
- **Assignment Title**: Training Fee Calculation System for KickBlast Judo
- **Instructor**: Mr. R. Jeykanth
