**AppointmentBooking** is a hospital management system built using **ASP.NET Core**, designed to streamline and manage key hospital operations such as **OPD/IPD bookings**, **patient billing**, **lab data entry**, and **comprehensive patient history tracking**. It enables hospital staff to efficiently manage various departments and allows registered patients to book OPD appointments through an online payment gateway.
## ⚙️ Set Up

```bash
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
```

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

## 🖼️ Project Screenshots  

### 🧾 OPD Booking (User Panel with Online Payment Integration)
<p align="center">

  <img src="screenshots/Hospital1.png" alt="OPD Booking Page" width="700">
</p>

---

### 💵 Cash Receipt (Billing)  
Data collected systematically for future reference.
<p align="center">
  <img src="screenshots/Hospital2.png" alt="Cash Receipt Billing" width="700">
</p>

---

### 🏨 IPD Status Management  
<p align="center">
  <img src="screenshots/Hospital3.png" alt="IPD Status Management" width="700">
</p>

---

### 🧪 Lab Result Entry  
<p align="center">
  <img src="screenshots/Hospital4.png" alt="Lab Result Entry" width="700">
</p>

---

### 👨‍⚕️ Patient History View  
When entering patient ID, their up-to-date history is shown with dates and consultant doctor.
<p align="center">
  <img src="screenshots/Hospital5.png" alt="Patient History View 1" width="340">
  <img src="screenshots/Hospital6.png" alt="Patient History View 2" width="340">
</p>

---
