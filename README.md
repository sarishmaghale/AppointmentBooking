**AppointmentBooking** is a hospital management system built using **ASP.NET Core**, designed to streamline and manage key hospital operations such as **OPD/IPD bookings**, **patient billing**, **lab data entry**, and **comprehensive patient history tracking**. It enables hospital staff to efficiently manage various departments and allows registered patients to book OPD appointments through an online payment gateway.

##  Key Features
- **OPD & IPD Booking** – Book and manage outpatient/inpatient records by hospital staff.
- **Billing Module** – Generate and manage billing records linked to OPD/IPD visits.
- **Lab Module** – Enter and manage lab test results associated with patient records.
- **Integrated Patient History** – View complete patient history by entering a patient ID, including OPD visits, billing, doctor references, and lab results with dates.
- **Online OPD Booking** – Patients can book appointments online with integrated payment support.
- **Modular Structure** – Each department is organized and managed under separate controllers.
- **Admin & Staff Areas** – Project is structured under `Areas/Staff`, isolating staff functionalities.

- ## 🛠 Tech Stack
- **ASP.NET Core Framework**
- **Entity Framework (EF)** with **Database-First Approach**
- **LINQ** for querying data
- **SQL Server Management Studio (SSMS)** for database
- **Dependency Injection** for service and repository abstraction
- **.cshtml (Razor)** for frontend views
- **JavaScript & jQuery (AJAX)** for dynamic interactions

- ## 📁 Project Structure
- 
```plaintext
AppointmentBooking/
├── Areas/
│   └── Staff/
│       ├── Controllers/
│       ├── Views/
│       └── Services/           # Interfaces & Repositories
├── Data/                       # Table Classes / EF Models
├── wwwroot/                   # Static files
├── program.cs                 # Configuration & DI setup
└── ...
