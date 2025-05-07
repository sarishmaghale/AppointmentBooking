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
using System.Runtime.InteropServices;

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
            int maxRegNo = db.TblOpdregistrations.Any()
     ? db.TblOpdregistrations.Max(item => item.SrNo)
     : 0;
            return (maxRegNo + 1);
        }
        public int GetMaximumReceiptNo()
        {
            int maxRecNo = db.TblCashReceipts.Any()? db.TblCashReceipts.Max(item => item.ReceiptNo):0;
            return (maxRecNo + 1);
        }
        public int GetIPDRegNo()
        {
            int maxRecNo = db.TblIpdregistrations.Any()? db.TblIpdregistrations.Max(item => item.IpdregNo):0;
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
        public int? GetBedCount()
        {
            var beds = db.TblIpdbedStatuses.Count();
            return beds;
        }
        public int? GetDepartmentCount()
        {
            return (db.TblDepartments.Count());
        }
        public int? GetDoctorCount()
        {
            return db.TblDoctorSetups.Count();
        }


        public async Task<double> GetHospitalRating()
        {
            var feedbacks = await db.TblPatientFeedbacks.ToListAsync();
            int positiveCount = 0;

            foreach (var feedback in feedbacks)
            {
                if (feedback != null && IsPositive(feedback.FeedbackText))
                {
                    positiveCount++;
                }
            }

            double totalFeedbacks = feedbacks.Count;
            if (totalFeedbacks == 0)
                return 0; 

            double ratingOutOf5 = (positiveCount / totalFeedbacks) * 5.0; 
            ratingOutOf5 = Math.Round(ratingOutOf5, 2);
            return ratingOutOf5;
        }

        public static bool IsPositive(string feedbackText)
        {
            if (string.IsNullOrWhiteSpace(feedbackText))
                return false;

            var words = feedbackText.ToLower().Split(new[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
            var positiveKeywords = new HashSet<string> {
        "good", "excellent", "great", "satisfactory", "happy", "amazing", "positive", "like",
        "outstanding", "fantastic", "pleased", "delightful", "commendable", "wonderful",
        "exceptional", "superb", "brilliant", "perfect", "impressive", "remarkable"
    };
            var negativeKeywords = new HashSet<string> {
        "bad", "poor", "horrible", "unhappy", "dissatisfied", "negative",
        "terrible", "awful", "disappointing", "unpleasant", "substandard", "unsatisfactory",
        "dreadful", "miserable", "inadequate", "lousy", "inferior", "pathetic", "frustrating"
    };

            bool hasPositive = false;
            bool hasNegative = false;

            for (int i = 0; i < words.Length; i++)
            {
                if (i < words.Length - 1 && (words[i] == "not" || words[i] == "no" || words[i] == "wasn't" || words[i] == "weren't"
                    || words[i] == "isn't" || words[i] == "aren't" || words[i] == "don't" || words[i] == "doesn't" || words[i] == "didn't"))
                {
                    // Check if the word following negation is positive/negative
                    if (positiveKeywords.Contains(words[i + 1]))
                        hasNegative = true;
                    if (negativeKeywords.Contains(words[i + 1]))
                        hasPositive = true;
                    i++; // Skip next word after handling negation
                }
                else
                {
                    if (positiveKeywords.Contains(words[i]))
                        hasPositive = true;
                    if (negativeKeywords.Contains(words[i]))
                        hasNegative = true;
                }
            }

            // Feedback is positive if positive keywords are present and no negative keywords are found
            return hasPositive && !hasNegative;
        }
    }
}
