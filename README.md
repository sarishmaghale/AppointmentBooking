**AppointmentBooking** is a hospital management system built using **ASP.NET Core**, designed to streamline and manage key hospital operations such as **OPD/IPD bookings**, **patient billing**, **lab data entry**, and **comprehensive patient history tracking**. It enables hospital staff to efficiently manage various departments and allows registered patients to book OPD appointments through an online payment gateway.
## ⚙️ Set Up
1. **Clone the Repository** - git clone https://github.com/sarishmaghale/AppointmentBooking.git
2. **Create Database manually**
3.  **Configure Database connection**-

🔹 appsettings.json

"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=YOUR_DB_NAME;Trusted_Connection=True;"
}

🔹 HospitalManagementDbContext.cs

optionsBuilder.UseSqlServer("Server=YOUR_SERVER_NAME;Database=YOUR_DB_NAME;Trusted_Connection=True;");

5.  **Apply Entity Fraamework Migrations**-
   Navigate to Project Directory (Where .csproj exists):

     cd path/to/your/AppointmentBooking/
    
   Open the terminal, then run:
   
dotnet ef migrations add InitialDatabaseCreate

dotnet ef database update

NOTE: if you don't have the EF CLI tools installed, run:

dotnet tool install --global dotnet -ef
7.  **Insert Default Admin Credentials (Manually)**
8.  **Debug and Run the project**
9.  
##  Key Features
- **OPD & IPD Booking** – Book and manage outpatient/inpatient records by hospital staff.
- **Billing Module** – Generate and manage billing records linked to OPD/IPD visits.
- **Lab Module** – Enter and manage lab test results associated with patient records.
- **Integrated Patient History** – View complete patient history by entering a patient ID, including OPD visits, billing, doctor references, and lab results with dates.
- **Online OPD Booking** – Patients can book appointments online with integrated payment support.
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
