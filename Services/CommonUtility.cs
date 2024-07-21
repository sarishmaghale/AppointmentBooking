using AppointmentBooking.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppointmentBooking.Models;
using System;
using AppointmentBooking.Areas.Staff.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace AppointmentBooking.Services
{
    public class CommonUtility
    {
        private HospitalManagementDbContext db;

        public CommonUtility(HospitalManagementDbContext _db)
        {
            db = _db;
        }

        public DateTime GetTodayDate()
        {
            var date = DateTime.Now.Date;
            return date;
        }
        public string GetCurrenTime()
        {
            return DateTime.Now.ToString("hh:mm tt");
        }
        public SelectList GetDepartments()
        {
            var dept = db.TblDepartments.ToList();
            var data = new SelectList(dept, "DepartmentId", "DepartmentName");
            return data;
        }
        public string GetDepartmentName(int DepartmentId)
        {
            return db.TblDepartments.Where(x => x.DepartmentId == DepartmentId).Select(x => x.DepartmentName).FirstOrDefault()??"ddd";
        }
        public string GetCurrentDateTime()
        {
            return DateTime.Now.ToString();
        }
        public SelectList GetDistricts()
        {
            var district = db.TblDropDownItems.Where(x => x.ItemName == "District").ToList();
            var data = new SelectList(district, "ItemValue", "ItemValue");
            return data;
        }
        public SelectList GetEthnicity()
        {
            var ethnicity = db.TblDropDownItems.Where(x => x.ItemName == "Ethnicity").ToList();
            var data = new SelectList(ethnicity, "ItemValue", "ItemValue");
            return data;
        }
        public SelectList GetTestGroup()
        {
            var group = db.TblTestGroupSetups.ToList();
            var data = new SelectList(group, "TestGroupId", "TestGroupName");
            return data;
        }
        public string GetTestGroupName(int TestGroupId)
        {
            var testGroupName = db.TblTestGroupSetups.Where(x => x.TestGroupId == TestGroupId).Select(x => x.TestGroupName).FirstOrDefault();
            return testGroupName??"ttt";
        }
        public string GetTestName(int TestId)
        {
            var testName = db.TblTestSetups.Where(x => x.TestId == TestId).Select(x => x.TestName).FirstOrDefault();
            return testName??"ttt";
        }
        public async Task<IEnumerable<PatientViewModel>> GetValuesOnDepartment(int DepartmentId)
        {
            var result = await (from doctors in db.TblDoctorSetups
                                join dept in db.TblDepartments
                                on doctors.DepartmentId equals dept.DepartmentId
                                where doctors.DepartmentId == DepartmentId
                                select new PatientViewModel
                                {
                                    DoctorId = doctors.DoctorId,
                                    DoctorName = doctors.DoctorName,
                                    RoomNo = dept.RoomNo,
                                    FloorName = dept.FloorName,
                                }).ToListAsync();

            return result;

        }
        public async Task<OPDViewModel> GetPatientsByUhid(decimal uhid)
        {
            var data = await db.TblPatientRegistrations.Where(x => x.Uhid == uhid).Select(m => new OPDViewModel()
            {
                FirstName = m.FirstName,
                LastName = m.LastName,
                PatientName = m.FirstName + " " + m.LastName,
                Dob = m.Dob,
                Age = m.Age,
                AgeType = m.AgeType,
                Address = m.Address,
                Contactno = m.Contactno,
                Gender = m.Gender,
                Uhid = m.Uhid,
            }).FirstOrDefaultAsync();
            return data;
        }

        public int GetOPDQueue(DateTime? Date)
        {
            int maxQueue = db.TblOpdregistrations.Where(item => item.CreatedDate == Date).Max(item => item.Opdqueue) ?? 0;
            return (maxQueue + 1);
        }
        public decimal GetMaximumUhid()
        {
            decimal maxUhid = db.TblOpdregistrations.Max(item => item.Uhid) ?? 0;
            return (maxUhid + 1);
        }
        public decimal GetMaximumOPDSrNo()
        {
            int maxRegNo = db.TblOpdregistrations.Max(item => item.SrNo);
            return (maxRegNo + 1);
        }
        public int GetMaximumReceiptNo()
        {
            int maxRecNo = db.TblCashReceipts.Max(item => item.ReceiptNo);
            return (maxRecNo + 1);
        }
        public int GetIPDRegNo()
        {
            int maxRecNo = db.TblIpdregistrations.Max(item => item.IpdregNo);
            return (maxRecNo + 1);
        }
        public SelectList GetCaseType()
        {
            var cases = db.TblCaseTypes.ToList();
            var data = new SelectList(cases, "CaseTypeId", "CaseTypeName");
            return data;
        }
        public string? GetCaseTypeName(int? CaseTypeId)
        {
            var result = db.TblCaseTypes.Where(x => x.CaseTypeId == CaseTypeId).Select(x => x.CaseTypeName).FirstOrDefault();
            return result;
        }
        public SelectList GetReligions()
        {
            var religions = db.TblDropDownItems.Where(x => x.ItemName == "Religion").ToList();
            var data = new SelectList(religions, "ItemValue", "ItemValue");
            return data;
        }
        public SelectList GetFeeType()
        {
            var feeType = db.TblFeeTypes.ToList();
            var data = new SelectList(feeType, "FeeTypeId", "FeeTypeName");
            return data;
        }
        public SelectList GetDoctors()
        {
            var doctors = db.TblDoctorSetups.ToList();
            var data = new SelectList(doctors, "DoctorId", "DoctorName");
            return data;
        }
        public SelectList GetUsers()
        {
            var users = db.TblUserAccounts.ToList();
            var data = new SelectList(users, "Username", "Username");
            return data;
        }
        public string GetDoctorName(int? DoctorId)
        {
            return db.TblDoctorSetups.Where(x => x.DoctorId == DoctorId).Select(x => x.DoctorName).FirstOrDefault()??" ";
        }
        public string GetFeeTypeName(int? FeeeTypeId)
        {
            return db.TblFeeTypes.Where(x => x.FeeTypeId == FeeeTypeId).Select(x => x.FeeTypeName).FirstOrDefault()??"fff";
        }

        public IEnumerable<DoctorSetupViewModel> GetDoctorList()
        {
            var data = db.TblDoctorSetups.Select(x => new DoctorSetupViewModel()
            {
                DoctorId = x.DoctorId,
                DoctorName = x.DoctorName,
                DepartmentName = db.TblDepartments.Where(d => d.DepartmentId == x.DepartmentId).Select(d => d.DepartmentName).FirstOrDefault(),
                FeeDetails = db.TblDoctorFeeTypeSetups.Where(f => f.DoctorId == x.DoctorId).Select(f => new DoctorFeeViewModel
                {
                    FeeTypeName = db.TblFeeTypes.Where(y => y.FeeTypeId == f.FeeTypeId).Select(y => y.FeeTypeName).FirstOrDefault(),
                    Fee = f.Fee,
                }).ToList()
            }).ToList();
            return data;
        }
        public SelectList GetBedCategory()
        {
            var beds = db.TblIpdbedTypes.ToList();
            var data = new SelectList(beds, "BedTypeId", "BedTypeName");
            return data;
        }
        public string GetBedTypeName(int? BedTypeId)
        {
            return db.TblIpdbedTypes.Where(b => b.BedTypeId == BedTypeId).Select(b => b.BedTypeName).FirstOrDefault() ?? "bb";
        }
        public SelectList GetLabTests()
        {
            var groupData = db.TblTestGroupSetups.Where(x => x.TestGroupName == "Pathology").Select(x => x.TestGroupId).FirstOrDefault();
            var testData = db.TblTestSetups.Where(y => y.TestGroupId == groupData).ToList();
            var result = new SelectList(testData, "TestId", "TestName");
            return result;                         
        }
        public SelectList GetLabParameters()
        {
            var data = db.TblLabParameterSetups.ToList();
            var result = new SelectList(data, "ParameterId", "ParameterName");
            return result;
        }
    }
}
