using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentBooking.Migrations
{
    public partial class InitialDatabaseCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblCaseType",
                columns: table => new
                {
                    CaseTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCaseType", x => x.CaseTypeId);
                });

            migrationBuilder.CreateTable(
                name: "tblDepartments",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RoomNo = table.Column<int>(type: "int", nullable: true),
                    FloorName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDepartments", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "tblDropDownItems",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ItemValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDropDownItems", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tblFeeType",
                columns: table => new
                {
                    FeeTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeeTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFeeType", x => x.FeeTypeId);
                });

            migrationBuilder.CreateTable(
                name: "tblIPDBedType",
                columns: table => new
                {
                    BedTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BedTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBedType", x => x.BedTypeId);
                });

            migrationBuilder.CreateTable(
                name: "tblIPDExpenseEntry",
                columns: table => new
                {
                    ExpenseEntryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IPDRegNo = table.Column<int>(type: "int", nullable: true),
                    Uhid = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    TestGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TestName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblIPDExpenseEntry", x => x.ExpenseEntryId);
                });

            migrationBuilder.CreateTable(
                name: "tblLabParameterSetup",
                columns: table => new
                {
                    ParameterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParameterName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Range = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Method = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblLabParameterSetup", x => x.ParameterId);
                });

            migrationBuilder.CreateTable(
                name: "tblOPDBookings",
                columns: table => new
                {
                    OPDBookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BookingDate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InsuranceId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Department = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    District = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Contactno = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DOB = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Ethnicity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    AgeType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PayStatus = table.Column<int>(type: "int", nullable: true),
                    UHID = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    CreatedTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ConsultantDr = table.Column<int>(type: "int", nullable: true),
                    RoomNo = table.Column<int>(type: "int", nullable: true),
                    FloorName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CaseType = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOPDBookings", x => x.OPDBookingId);
                });

            migrationBuilder.CreateTable(
                name: "tblPatientFeedback",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeedbackText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Uhid = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    PatientName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SubmittedDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPatientFeedback", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblPatientRegistration",
                columns: table => new
                {
                    UHID = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true),
                    DOB = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Contactno = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Gender = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    District = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Ethnicity = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    isDelete = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CreatedTime = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CreatedByUser = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    AgeType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPatientRegistration", x => x.UHID);
                });

            migrationBuilder.CreateTable(
                name: "tblResultEntry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LabNo = table.Column<int>(type: "int", nullable: true),
                    TestId = table.Column<int>(type: "int", nullable: true),
                    TestName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ParameterName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Range = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Result = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblResultEntry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblSampleRegistration",
                columns: table => new
                {
                    SrNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uhid = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    RecordType = table.Column<int>(type: "int", nullable: true),
                    RecordNo = table.Column<int>(type: "int", nullable: true),
                    isCollected = table.Column<int>(type: "int", nullable: true),
                    LabNo = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSampleRegistration", x => x.SrNo);
                });

            migrationBuilder.CreateTable(
                name: "tblTestGroupSetup",
                columns: table => new
                {
                    TestGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestGroupName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTestGroupSetup", x => x.TestGroupId);
                });

            migrationBuilder.CreateTable(
                name: "tblTransactionStatus",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OPDBookingId = table.Column<int>(type: "int", nullable: true),
                    RefId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTransactionStatus", x => x.TransactionId);
                });

            migrationBuilder.CreateTable(
                name: "tblUserAccount",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Shift = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUserAccount", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "tblDoctorSetup",
                columns: table => new
                {
                    DoctorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDoctorSetup", x => x.DoctorId);
                    table.ForeignKey(
                        name: "FK__tblDoctor__Depar__619B8048",
                        column: x => x.DepartmentId,
                        principalTable: "tblDepartments",
                        principalColumn: "DepartmentId");
                });

            migrationBuilder.CreateTable(
                name: "tblIPDBedStatus",
                columns: table => new
                {
                    BedId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BedName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BedTypeId = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<int>(type: "int", nullable: true),
                    IPDRegNo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBedStatus", x => x.BedId);
                    table.ForeignKey(
                        name: "FK__tblBedSta__BedTy__40058253",
                        column: x => x.BedTypeId,
                        principalTable: "tblIPDBedType",
                        principalColumn: "BedTypeId");
                });

            migrationBuilder.CreateTable(
                name: "tblIPDRegistration",
                columns: table => new
                {
                    IPDRegNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UHID = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    FirstName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    District = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Contactno = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DOB = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true),
                    AgeType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Gender = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    GName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    GAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GContact = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    GRelation = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    PayType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    AdmCharge = table.Column<double>(type: "float", nullable: true),
                    ConsultantDr = table.Column<int>(type: "int", nullable: true),
                    BedType = table.Column<int>(type: "int", nullable: true),
                    BedNo = table.Column<int>(type: "int", nullable: true),
                    CaseType = table.Column<int>(type: "int", nullable: true),
                    Complain = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Diagnosis = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsDischarged = table.Column<int>(type: "int", nullable: true),
                    CreatedByUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OpdSrNo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblIPDRegistration", x => x.IPDRegNo);
                    table.ForeignKey(
                        name: "FK__tblIPDRegi__UHID__3B40CD36",
                        column: x => x.UHID,
                        principalTable: "tblPatientRegistration",
                        principalColumn: "UHID");
                });

            migrationBuilder.CreateTable(
                name: "tblTestSetup",
                columns: table => new
                {
                    TestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TestGroupId = table.Column<int>(type: "int", nullable: true),
                    TestPrice = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTestSetup", x => x.TestId);
                    table.ForeignKey(
                        name: "FK__tblTestSe__TestG__2A164134",
                        column: x => x.TestGroupId,
                        principalTable: "tblTestGroupSetup",
                        principalColumn: "TestGroupId");
                });

            migrationBuilder.CreateTable(
                name: "tblDoctorFeeTypeSetup",
                columns: table => new
                {
                    DocFeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(type: "int", nullable: true),
                    FeeTypeId = table.Column<int>(type: "int", nullable: true),
                    Fee = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDoctorFeeTypeSetup", x => x.DocFeeId);
                    table.ForeignKey(
                        name: "FK__tblDoctor__Docto__123EB7A3",
                        column: x => x.DoctorId,
                        principalTable: "tblDoctorSetup",
                        principalColumn: "DoctorId");
                    table.ForeignKey(
                        name: "FK__tblDoctor__FeeTy__1332DBDC",
                        column: x => x.FeeTypeId,
                        principalTable: "tblFeeType",
                        principalColumn: "FeeTypeId");
                });

            migrationBuilder.CreateTable(
                name: "tblOPDRegistration",
                columns: table => new
                {
                    SrNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UHID = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    FirstName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    District = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Contactno = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DOB = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true),
                    AgeType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Gender = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Ethnicity = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    OPDQueue = table.Column<int>(type: "int", nullable: true),
                    Department = table.Column<int>(type: "int", nullable: true),
                    ConsultantDr = table.Column<int>(type: "int", nullable: true),
                    RoomNo = table.Column<int>(type: "int", nullable: true),
                    FloorName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CaseType = table.Column<int>(type: "int", nullable: true),
                    PayType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    FeeType = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedByUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RefId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOPDRegistration", x => x.SrNo);
                    table.ForeignKey(
                        name: "FK__tblOPDReg__CaseT__778AC167",
                        column: x => x.CaseType,
                        principalTable: "tblCaseType",
                        principalColumn: "CaseTypeId");
                    table.ForeignKey(
                        name: "FK__tblOPDReg__Consu__76969D2E",
                        column: x => x.ConsultantDr,
                        principalTable: "tblDoctorSetup",
                        principalColumn: "DoctorId");
                    table.ForeignKey(
                        name: "FK__tblOPDReg__Depar__75A278F5",
                        column: x => x.Department,
                        principalTable: "tblDepartments",
                        principalColumn: "DepartmentId");
                    table.ForeignKey(
                        name: "FK__tblOPDRegi__UHID__74AE54BC",
                        column: x => x.UHID,
                        principalTable: "tblPatientRegistration",
                        principalColumn: "UHID");
                });

            migrationBuilder.CreateTable(
                name: "tblTestParameterMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestId = table.Column<int>(type: "int", nullable: true),
                    ParamaterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTestParameterMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK__tblTestPa__Param__57DD0BE4",
                        column: x => x.ParamaterId,
                        principalTable: "tblLabParameterSetup",
                        principalColumn: "ParameterId");
                    table.ForeignKey(
                        name: "FK__tblTestPa__TestI__56E8E7AB",
                        column: x => x.TestId,
                        principalTable: "tblTestSetup",
                        principalColumn: "TestId");
                });

            migrationBuilder.CreateTable(
                name: "tblCashReceipt",
                columns: table => new
                {
                    ReceiptNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uhid = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    OPDNo = table.Column<int>(type: "int", nullable: true),
                    IPDNo = table.Column<int>(type: "int", nullable: true),
                    PayType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    TotalAmount = table.Column<double>(type: "float", nullable: true),
                    CreatedByUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCashReceipt", x => x.ReceiptNo);
                    table.ForeignKey(
                        name: "FK__tblCashRe__OPDNo__3587F3E0",
                        column: x => x.OPDNo,
                        principalTable: "tblOPDRegistration",
                        principalColumn: "SrNo");
                    table.ForeignKey(
                        name: "FK__tblCashRec__Uhid__3493CFA7",
                        column: x => x.Uhid,
                        principalTable: "tblPatientRegistration",
                        principalColumn: "UHID");
                });

            migrationBuilder.CreateTable(
                name: "tblReceiptDetails",
                columns: table => new
                {
                    DetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceiptNo = table.Column<int>(type: "int", nullable: true),
                    TestGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TestName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TestPrice = table.Column<double>(type: "float", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblReceiptDetails", x => x.DetailsId);
                    table.ForeignKey(
                        name: "FK__tblReceip__Recei__3864608B",
                        column: x => x.ReceiptNo,
                        principalTable: "tblCashReceipt",
                        principalColumn: "ReceiptNo");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblCashReceipt_OPDNo",
                table: "tblCashReceipt",
                column: "OPDNo");

            migrationBuilder.CreateIndex(
                name: "IX_tblCashReceipt_Uhid",
                table: "tblCashReceipt",
                column: "Uhid");

            migrationBuilder.CreateIndex(
                name: "IX_tblDoctorFeeTypeSetup_DoctorId",
                table: "tblDoctorFeeTypeSetup",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_tblDoctorFeeTypeSetup_FeeTypeId",
                table: "tblDoctorFeeTypeSetup",
                column: "FeeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblDoctorSetup_DepartmentId",
                table: "tblDoctorSetup",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_tblIPDBedStatus_BedTypeId",
                table: "tblIPDBedStatus",
                column: "BedTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblIPDRegistration_UHID",
                table: "tblIPDRegistration",
                column: "UHID");

            migrationBuilder.CreateIndex(
                name: "IX_tblOPDRegistration_CaseType",
                table: "tblOPDRegistration",
                column: "CaseType");

            migrationBuilder.CreateIndex(
                name: "IX_tblOPDRegistration_ConsultantDr",
                table: "tblOPDRegistration",
                column: "ConsultantDr");

            migrationBuilder.CreateIndex(
                name: "IX_tblOPDRegistration_Department",
                table: "tblOPDRegistration",
                column: "Department");

            migrationBuilder.CreateIndex(
                name: "IX_tblOPDRegistration_UHID",
                table: "tblOPDRegistration",
                column: "UHID");

            migrationBuilder.CreateIndex(
                name: "IX_tblReceiptDetails_ReceiptNo",
                table: "tblReceiptDetails",
                column: "ReceiptNo");

            migrationBuilder.CreateIndex(
                name: "IX_tblTestParameterMapping_ParamaterId",
                table: "tblTestParameterMapping",
                column: "ParamaterId");

            migrationBuilder.CreateIndex(
                name: "IX_tblTestParameterMapping_TestId",
                table: "tblTestParameterMapping",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_tblTestSetup_TestGroupId",
                table: "tblTestSetup",
                column: "TestGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblDoctorFeeTypeSetup");

            migrationBuilder.DropTable(
                name: "tblDropDownItems");

            migrationBuilder.DropTable(
                name: "tblIPDBedStatus");

            migrationBuilder.DropTable(
                name: "tblIPDExpenseEntry");

            migrationBuilder.DropTable(
                name: "tblIPDRegistration");

            migrationBuilder.DropTable(
                name: "tblOPDBookings");

            migrationBuilder.DropTable(
                name: "tblPatientFeedback");

            migrationBuilder.DropTable(
                name: "tblReceiptDetails");

            migrationBuilder.DropTable(
                name: "tblResultEntry");

            migrationBuilder.DropTable(
                name: "tblSampleRegistration");

            migrationBuilder.DropTable(
                name: "tblTestParameterMapping");

            migrationBuilder.DropTable(
                name: "tblTransactionStatus");

            migrationBuilder.DropTable(
                name: "tblUserAccount");

            migrationBuilder.DropTable(
                name: "tblFeeType");

            migrationBuilder.DropTable(
                name: "tblIPDBedType");

            migrationBuilder.DropTable(
                name: "tblCashReceipt");

            migrationBuilder.DropTable(
                name: "tblLabParameterSetup");

            migrationBuilder.DropTable(
                name: "tblTestSetup");

            migrationBuilder.DropTable(
                name: "tblOPDRegistration");

            migrationBuilder.DropTable(
                name: "tblTestGroupSetup");

            migrationBuilder.DropTable(
                name: "tblCaseType");

            migrationBuilder.DropTable(
                name: "tblDoctorSetup");

            migrationBuilder.DropTable(
                name: "tblPatientRegistration");

            migrationBuilder.DropTable(
                name: "tblDepartments");
        }
    }
}
