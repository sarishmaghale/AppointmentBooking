using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using AppointmentBooking.Data;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace AppointmentBooking.Areas.Staff.Services.Repository
{
    public class IPDService : IIPDRepository
    {
        private HospitalManagementDbContext db;
        public IPDService(HospitalManagementDbContext _db)
        {
            db = _db;
        }

        public async Task<int> AddIPDRegistration(IPDViewModel model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //Add registration form data to IPDregistration table
                    var data = new TblIpdregistration()
                    {
                        Uhid = model.Uhid,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Address = model.Address,
                        District = model.District,
                        Contactno = model.Contactno,
                        Dob = model.Dob,
                        Age = model.Age,
                        AgeType = model.AgeType,
                        Gender = model.Gender,
                        Gname = model.Gname,
                        Gaddress = model.Gaddress,
                        Gcontact = model.Gcontact,
                        Grelation = model.Grelation,
                        PayType = model.PayType,
                        AdmCharge = model.AdmCharge,
                        ConsultantDr = model.ConsultantDr,
                        BedType = model.IPDBedTypeId,
                        BedNo = model.BedNo,
                        CaseType = model.CaseType,
                        Complain = model.Complain,
                        Diagnosis = model.Diagnosis,
                        CreatedDate = model.CreatedDate,
                        IsDischarged = 0,
                        CreatedByUser = model.CreatedByUser,
                        OpdSrNo = model.OpdSrNo,
                    };
                    await db.TblIpdregistrations.AddAsync(data);
                    await db.SaveChangesAsync();
                    //fetch the bedInfo of selected BedID
                    var bedStatus = await db.TblIpdbedStatuses.Where(x => x.BedId == model.BedNo).FirstOrDefaultAsync();
                    //change the bed status=1 i.e. occupied, add ipdregistrationNo
                    if (bedStatus != null)
                    {
                        bedStatus.Status = 1;
                        bedStatus.IpdregNo = data.IpdregNo;
                        db.Entry(bedStatus).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                    //add the bedCharge entry in the expenseEntry to count the days that will be paid at the time of discharge
                    var expenseEntry = new TblIpdexpenseEntry()
                    {
                        IpdregNo = data.IpdregNo,
                        Uhid = data.Uhid,
                        TestGroup = "Bed Charge",
                        TestName = db.TblIpdbedTypes.Where(b => b.BedTypeId == model.IPDBedTypeId).Select(b => b.BedTypeName).FirstOrDefault(),
                        Price = model.IPDBedPrice,
                        Quantity = 1,
                        Amount = model.IPDBedPrice,
                        CreatedDate = model.CreatedDate,
                    };
                    await db.TblIpdexpenseEntries.AddAsync(expenseEntry);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return data.IpdregNo;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    transaction.Rollback();
                    return 0;
                }
            }
        }



        public async Task<IEnumerable<IPDViewModel>> GetIPDBedValues(int BedTypeId)
        {
            var result = await (from bedCategory in db.TblIpdbedTypes
                                join beds in db.TblIpdbedStatuses on bedCategory.BedTypeId equals beds.BedTypeId
                                where bedCategory.BedTypeId == BedTypeId && beds.Status == 0 || beds.Status == null
                                select new IPDViewModel
                                {
                                    IPDBedTypeId = bedCategory.BedTypeId,
                                    BedNo = beds.BedId,
                                    IPDBedName = beds.BedName,
                                    IPDBedPrice = bedCategory.Price,
                                    IPDStatus=beds.Status,
                                }).ToListAsync();
            return result;
        }

        public async Task<ReceiptViewModel> GetIPDExpenseEntries(int IPDRegNo)
        {
            var model = new ReceiptViewModel();
            //fetch ExpenseDetails from expenseEntry
            model.ReceiptDetails= await db.TblIpdexpenseEntries.Where(x => x.IpdregNo == IPDRegNo).Select(y => new ReceiptDetails()
            {
                IpdRegNo = IPDRegNo,
                TestGroupName = y.TestGroup,
                TestName = y.TestName,
                TestPrice = y.Price,
                Quantity = y.TestGroup == "Bed Charge" ? (y.CreatedDate.HasValue ? (DateTime.Now - y.CreatedDate.Value).Days : 0) : y.Quantity,
                Amount = y.Price * (y.TestGroup == "Bed Charge" ? (y.CreatedDate.HasValue ? (DateTime.Now - y.CreatedDate.Value).Days : 0) : y.Quantity),
            }).ToListAsync();

            //fetch patient data from ipdRegistration
            var patientInfo = await db.TblIpdregistrations.Where(i => i.IpdregNo == IPDRegNo).FirstOrDefaultAsync();
            if (patientInfo != null)
            {
                model.Uhid = patientInfo.Uhid;
                model.PatientName = patientInfo.FirstName + " " + patientInfo.LastName;
                model.DoctorName = db.TblDoctorSetups.Where(d => d.DoctorId == patientInfo.ConsultantDr).Select(d => d.DoctorName).FirstOrDefault();
                model.Age = patientInfo.Age + " " + patientInfo.AgeType;
                model.Gender = patientInfo.Gender;
                model.Address = patientInfo.Address;
                model.CreatedDate = patientInfo.CreatedDate;
            }
           
            return model;
        }

        public async Task<List<IPDViewModel>> GetIPDPatientCount()
        {
            //Display grouping by Month
            /*
             *   var result = await db.TblIpdregistrations.Where(x=> x.CreatedDate.HasValue).GroupBy(x => new { Month = x.CreatedDate.Value.Month, Year = x.CreatedDate.Value.Year }).Select(g => new IPDViewModel
            {
                MonthYear = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(g.Key.Month),
                Count = g.Count()
            }
            */
            var daysLimit = DateTime.Today.AddDays(-15);
            var last30Days = Enumerable.Range(0, (DateTime.Today - daysLimit).Days + 1)
                                       .Select(offset => daysLimit.AddDays(offset))
                                       .ToList();
            var rawData = await db.TblIpdregistrations
                      .Where(x => x.CreatedDate >= daysLimit)
                      .GroupBy(x => x.CreatedDate)
                      .Select(g => new { CreatedDate = g.Key, Count = g.Count() })
                      .ToListAsync();
            var result = last30Days.Select(date => new IPDViewModel
            {
                CreatedDate = date,
                Count = rawData.FirstOrDefault(d => d.CreatedDate == date)?.Count ?? 0
            }).ToList();
            return result;

        }

        public async Task<List<IPDViewModel>> GetIPDPatientList(IPDViewModel model)
        {
            var query = db.TblIpdregistrations.AsQueryable();
            DateTime? fromDate = model.FromDateFilter;
            DateTime? toDate = model.ToDateFilter;
            int? PatientType = model.IsDischarged;
            if (PatientType.HasValue)
            {
                if (PatientType == 0 || PatientType == 1)
                {
                    query = query.Where(t => t.IsDischarged == PatientType);
                }
                else if (PatientType == 2)
                {
                    
                }
            }
            if (fromDate != null && toDate != null)
            {
                // Filter by date range
                query = query.Where(t => t.CreatedDate >= fromDate && t.CreatedDate <= toDate);
            }
            var results = await query.Select(t => new IPDViewModel
            {
                IpdregNo=t.IpdregNo,
                Uhid=t.Uhid,
                IPDBedName=db.TblIpdbedStatuses.Where(b=>b.BedId==t.BedNo).Select(b=>b.BedName).FirstOrDefault(),
                Gender=t.Gender,
                PatientName=t.FirstName+" "+t.LastName,
                Dob=t.Dob,
                Age=t.Age,
                AgeType=t.AgeType,
                Contactno=t.Contactno,
                Address=t.Address,
                DoctorName=db.TblDoctorSetups.Where(d=>d.DoctorId==t.ConsultantDr).Select(d=>d.DoctorName).FirstOrDefault(),
                CreatedDate=t.CreatedDate,
                DischargeDate = db.TblCashReceipts.Where(c => c.Ipdno==t.IpdregNo).Select(c=>c.CreatedDate).FirstOrDefault(),
                Gname=t.Gname,
                Gcontact=t.Gcontact,
                Grelation=t.Grelation,
                IsDischarged=t.IsDischarged,
                Ward=db.TblIpdbedTypes.Where(w=>w.BedTypeId==t.BedType).Select(w=>w.BedTypeName).FirstOrDefault(),
            }).OrderByDescending(t => t.IpdregNo).ToListAsync();

            return results;
        }

        public async Task<IPDViewModel> GetPatientInfoOnBed(IDPBedViewModel model)
        {
            var result = await (from bedInfo in db.TblIpdbedStatuses
                              join patientInfo in db.TblIpdregistrations on bedInfo.IpdregNo equals patientInfo.IpdregNo
                              where bedInfo.BedId == model.BedId
                              select new IPDViewModel
                              {
                                  IpdregNo = Convert.ToInt32(bedInfo.IpdregNo),
                                  PatientName = patientInfo.FirstName + " " + patientInfo.LastName,
                                  AgeType = patientInfo.Age.ToString() + " " + patientInfo.AgeType,
                                  Contactno=patientInfo.Contactno,
                                  CaseTypeName = db.TblCaseTypes.Where(c => c.CaseTypeId == patientInfo.CaseType).Select(c => c.CaseTypeName).FirstOrDefault(),
                                  DoctorName = db.TblDoctorSetups.Where(d => d.DoctorId == patientInfo.ConsultantDr).Select(d => d.DoctorName).FirstOrDefault(),
                                  Complain = patientInfo.Complain,
                                  Diagnosis = patientInfo.Diagnosis,
                                  CreatedDate = patientInfo.CreatedDate,
                                  Address=patientInfo.Address,
                                  Uhid=patientInfo.Uhid,
                                  IPDBedName=bedInfo.BedName,
                                  Gname=patientInfo.Gname,
                                  Gcontact=patientInfo.Gcontact,
                                  Grelation=patientInfo.Grelation,
                              }).FirstOrDefaultAsync();
            return result;                          
        }

        public async Task<IPDViewModel> GetPatientsByIPD(int ipd)
        {
            var data = await db.TblIpdregistrations.Where(x => x.IpdregNo == ipd).Select(m => new IPDViewModel()
            {
                PatientName = m.FirstName + " " + m.LastName,
                FirstName = m.FirstName,
                LastName = m.LastName,
                Address = m.Address,
                District = m.District,
                Dob = m.Dob,
                Age = m.Age,
                AgeType = m.AgeType,
                BedType= m.BedType,
                IpdregNo=m.IpdregNo,
                IPDBedName=db.TblIpdbedStatuses.Where(b=>b.BedId==m.BedNo).Select(b=>b.BedName).FirstOrDefault(),
                Uhid = m.Uhid,
                Gender = m.Gender,
                ConsultantDr = m.ConsultantDr,
                CreatedDate=m.CreatedDate,
                Contactno=m.Contactno,
                Gname=m.Gname,
                Gcontact=m.Gcontact,
                Gaddress=m.Gaddress,
                CaseType=m.CaseType,
                Grelation=m.Grelation,
                DischargeDate=db.TblCashReceipts.Where(d=>d.Ipdno==m.IpdregNo).Select(d=>d.CreatedDate).FirstOrDefault()??null,
                Complain=m.Complain,
                DoctorName = db.TblDoctorSetups.Where(y => y.DoctorId == m.ConsultantDr).Select(y => y.DoctorName).FirstOrDefault(),
            }).FirstOrDefaultAsync();
            return data;
        }

        public async Task<IEnumerable<IDPBedViewModel>> GetWardsInfo(int BedTypeId)
        {
            try
            {
                var result = await (from bedCategory in db.TblIpdbedTypes
                                    join beds in db.TblIpdbedStatuses on bedCategory.BedTypeId equals beds.BedTypeId
                                    where bedCategory.BedTypeId == BedTypeId
                                    select new IDPBedViewModel
                                    {
                                        BedTypeId = bedCategory.BedTypeId,
                                        BedId = beds.BedId,
                                        BedName = beds.BedName,
                                        Price = bedCategory.Price,
                                        Status = beds.Status,
                                    }).ToListAsync();
                return result;
            }
            catch
            {
                return null;
            }
           
        }
    }
}
