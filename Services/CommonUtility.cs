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
        private MedicareAppointmentDbContext db;

        public CommonUtility(MedicareAppointmentDbContext _db)
        {
            db = _db;
        }

        public string GetTodayDate()
        {
           return DateTime.Now.ToShortDateString();
        }
        public string GetCurrenTime()
        {
            return DateTime.Now.ToString("HH:mm:ss tt");
        }
        public int CalculateAge(DateOnly Date)
        {
            int today = DateTime.Now.Year;
            return today - Date.Year;
        }

        public SelectList GetDepartments()
        {
            var dept = db.TblDepartments.ToList();
            var data = new SelectList(dept, "DepartmentId", "DepartmentName");
            return data;
        }
        public string GetDepartmentName(int DepartmentId)
        {
            return db.TblDepartments.Where(x => x.DepartmentId == DepartmentId).Select(x => x.DepartmentName).FirstOrDefault();
        }
        public string GetCurrentDateTime()
        {
            return DateTime.Now.ToString();
        }
        public SelectList GetDistricts()
        {
            var district = db.TblDropDownItems.Where(x=>x.ItemName=="District").ToList();
            var data = new SelectList(district, "ItemValue", "ItemValue");
            return data;
        }
        public SelectList GetEthnicity()
        {
            var ethnicity = db.TblDropDownItems.Where(x => x.ItemName == "Ethnicity").ToList();
            var data = new SelectList(ethnicity, "ItemValue", "ItemValue");
            return data;
        }

        public async Task<IEnumerable<PatientViewModel>> GetValuesOnDepartment(int DepartmentId)
        {
            var result = await (from doctors in db.TblDoctorSetups
                         join dept in db.TblDepartments
                         on doctors.DepartmentId equals dept.DepartmentId
                         where doctors.DepartmentId==DepartmentId
                         select new PatientViewModel
                         {
                             DoctorId=doctors.DoctorId,
                             DoctorName=doctors.DoctorName,
                             RoomNo=dept.RoomNo,
                             FloorName=dept.FloorName,
                         }).ToListAsync();

            return result;

        }
        public async Task<OPDViewModel> GetPatientsByUhid(decimal uhid)
        {
            var data = await db.TblPatientRegistrations.Where(x => x.Uhid == uhid).Select(m => new OPDViewModel()
            {
                FirstName = m.FirstName,
                LastName = m.LastName,
                Dob = m.Dob,
                Age = m.Age,
                AgeType = m.AgeType,
                Address = m.Address,
                Contactno = m.Contactno,
                Gender = m.Gender,
                Uhid=m.Uhid,
            }).FirstOrDefaultAsync();
            return data;
        }

        public int GetOPDQueue(string Date)
        {
           int maxQueue=db.TblOpdregistrations.Where(item=> item.CreatedDate==Date).Max(item => item.Opdqueue)??0;
            return (maxQueue + 1);
        }

        public SelectList GetCaseType()
        {
            var cases = db.TblCaseTypes.ToList();
            var data = new SelectList(cases, "CaseTypeId", "CaseTypeName");
            return data;
        }
        public SelectList GetReligions()
        {
            var religions = db.TblDropDownItems.Where(x=> x.ItemName=="Religion").ToList();
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
           return db.TblDoctorSetups.Where(x => x.DoctorId == DoctorId).Select(x => x.DoctorName).FirstOrDefault();
        }
        public string GetFeeTypeName(int? FeeeTypeId)
        {
            return db.TblFeeTypes.Where(x => x.FeeTypeId == FeeeTypeId).Select(x => x.FeeTypeName).FirstOrDefault();
        }

    }
}
