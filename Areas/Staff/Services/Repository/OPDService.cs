using AppointmentBooking.Data;
using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using AppointmentBooking.Models;
using System.Globalization;

namespace AppointmentBooking.Areas.Staff.Services.Repository
{
    public class OPDService : IOPDRepository
    {
        private HospitalManagementDbContext db;
        public OPDService(HospitalManagementDbContext _db)
        {
            db = _db;
        }
        public async Task<int> AddOPDRegistration(OPDViewModel model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    int OPDQueue = GetOPDQueue(model.CreatedDate,Convert.ToInt32(model.Department));
                    TblOpdregistration data = new TblOpdregistration()
                    {
                        Uhid = model.Uhid,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Opdqueue = OPDQueue,
                        Department = model.Department,
                        Address = model.Address,
                        District = model.District,
                        Contactno = model.Contactno,
                        Dob = model.Dob,
                        Age = model.Age,
                        AgeType = model.AgeType,
                        Gender = model.Gender,
                        Ethnicity = model.Ethnicity,
                        ConsultantDr = model.ConsultantDr,
                        RoomNo = model.RoomNo,
                        FloorName = model.FloorName,
                        CaseType = model.CaseType,
                        PayType = model.PayType,
                        FeeType = model.FeeType,
                        Amount = model.Amount,
                        CreatedDate = model.CreatedDate,
                        CreatedTime = model.CreatedTime,
                        CreatedByUser = model.CreatedByUser,
                    };
                    await db.TblOpdregistrations.AddAsync(data);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return data.SrNo;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                    return 0;
                }
            }
        }

        public async Task<double> GetDoctorFees(int DoctorId, int FeeTypeId)
        {

           var data = await db.TblDoctorFeeTypeSetups.Where(x => x.DoctorId == DoctorId && x.FeeTypeId == FeeTypeId).FirstOrDefaultAsync();
           double amount=Convert.ToDouble( (data != null) ? data.Fee : 0);
            return amount;
        }

        public int GetOPDQueue(DateTime? Date, int Department)
        {
            int maxQueue = db.TblOpdregistrations.Where(item => item.CreatedDate == Date&& item.Department==Department).Max(item => item.Opdqueue) ?? 0;
            return (maxQueue + 1);
        }
        //public async Task<IEnumerable<OPDViewModel>> GetFilterOPDReports(string paytype, string user, DateTime? fromDate, DateTime? toDate, List<DataTablesOrder> order)
        //{
            

        //    var query = db.TblOpdregistrations.AsQueryable();
        //    if (!string.IsNullOrEmpty(paytype) && !paytype.Equals("all", StringComparison.OrdinalIgnoreCase))
        //    {
        //        query = query.Where(t => t.PayType.Contains(paytype));
        //    }

        //    if (!string.IsNullOrEmpty(user) && !user.Equals("all", StringComparison.OrdinalIgnoreCase))
        //    {
        //        query = query.Where(t => t.CreatedByUser.Contains(user));
        //    }
        //    if (fromDate != null && toDate != null)
        //    {
        //        // Filter by date range
        //        query = query.Where(t => t.CreatedDate >= fromDate && t.CreatedDate <= toDate);
        //    }
        //    if (order != null && order.Count > 0)
        //    {
        //        foreach (var o in order)
        //        {
        //            switch (o.Column)
        //            {
        //                case 0:
        //                    query = o.Dir == DataTablesOrderDir.Desc ?                             
        //                    query.OrderBy(t => t.SrNo) :
        //                        query.OrderByDescending(t => t.SrNo);

        //                    break;
        //                case 1:
        //                    query = o.Dir == DataTablesOrderDir.Asc ?
        //                        query.OrderBy(t => t.FirstName + " " + t.LastName) :
        //                        query.OrderByDescending(t => t.FirstName + " " + t.LastName);
        //                    break;
        //                case 2:
        //                    query = o.Dir == DataTablesOrderDir.Asc ?
        //                        query.OrderBy(t => t.PayType) :
        //                        query.OrderByDescending(t => t.PayType);
        //                    break;
        //                case 3:
        //                    query = o.Dir == DataTablesOrderDir.Asc ?
        //                        query.OrderBy(t => t.Amount) :
        //                        query.OrderByDescending(t => t.Amount);
        //                    break;
        //                case 4:
        //                    query = o.Dir == DataTablesOrderDir.Asc ?
        //                        query.OrderBy(t => t.FeeType) :
        //                        query.OrderByDescending(t => t.FeeType);
        //                    break;
        //                // Add more cases for other columns as needed
        //                default:
        //                    break;
        //            }
        //        }
        //    }

        //    var results =await query.Select(t => new OPDViewModel
        //    {
        //        SrNo = t.SrNo,
        //        PatientName = t.FirstName+ " "+t.LastName,
        //        PayType = t.PayType,
        //        Amount = t.Amount,
        //        DoctorName = db.TblDoctorSetups.Where(x=> x.DoctorId==t.ConsultantDr).Select(x=> x.DoctorName).FirstOrDefault(),
        //        FeeTypeName = db.TblFeeTypes.Where(x=> x.FeeTypeId==t.FeeType).Select(x=> x.FeeTypeName).FirstOrDefault(),
        //        CreatedByUser = t.CreatedByUser
        //    }).ToListAsync();
      
        //    return results;
        //}

        public async Task<List<OPDViewModel>> GetFilterOPDReports(OPDViewModel model)
        {
           
            var query = db.TblOpdregistrations.AsQueryable();
            string? user = model.CreatedByUser;
            string? payType = model.PayType;
            DateTime? fromDate = model.FromDateFilter;
            DateTime? toDate = model.ToDateFilter;

            if (!string.IsNullOrEmpty(payType) && !payType.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(t => t.PayType.Contains(payType));
            }
            if (!string.IsNullOrEmpty(user) && !user.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(t => t.CreatedByUser.Contains(user));
            }
            if (fromDate != null && toDate != null)
            {
                // Filter by date range
                query = query.Where(t => t.CreatedDate >= fromDate && t.CreatedDate <= toDate);
            }
            var results = await query.Select(t => new OPDViewModel
            {
                SrNo = t.SrNo,
                PayType=t.PayType,
                PatientName=t.FirstName+" "+t.LastName,
                Amount=t.Amount,
                FeeTypeName=db.TblFeeTypes.Where(f=> f.FeeTypeId==t.FeeType).Select(f=>f.FeeTypeName).FirstOrDefault(),
                DoctorName=db.TblDoctorSetups.Where(d=> d.DoctorId==t.ConsultantDr).Select(d=> d.DoctorName).FirstOrDefault(),
            }).OrderByDescending(t=>t.SrNo).ToListAsync();
           
            return results;
        }

        public async Task<List<OPDViewModel>> GetOPDReports()
        {
            var date =DateTime.Now.Date;
          var data= await db.TblOpdregistrations.Where(x => x.CreatedDate == date).Select(m=> new OPDViewModel()
          {
              SrNo=m.SrNo,
             PatientName= m.FirstName+" "+m.LastName,
              PayType=m.PayType,
              Amount=m.Amount,
              DoctorName=db.TblDoctorSetups.Where(d=> d.DoctorId==m.ConsultantDr).Select(d=>d.DoctorName).FirstOrDefault(),
              FeeTypeName=db.TblFeeTypes.Where(f=> f.FeeTypeId==m.FeeType).Select(f=>f.FeeTypeName).FirstOrDefault(),
          }).ToListAsync();
            return data;
        }

        public async Task<OPDViewModel> GetPatientsByOPD(int OPDNo)
        {
          var data=  await db.TblOpdregistrations.Where(x => x.SrNo == OPDNo).Select(m => new OPDViewModel()
            {
                PatientName = m.FirstName + " " + m.LastName,
                FirstName=m.FirstName,
                LastName=m.LastName,
                Address = m.Address,
                District=m.District,
                Dob=m.Dob,
                Age = m.Age,
                AgeType=m.AgeType,
                Uhid=m.Uhid,
                Gender = m.Gender,
                ConsultantDr=m.ConsultantDr,
                Department=m.Department,
                Contactno=m.Contactno,
                Opdqueue= m.Opdqueue,
                CreatedDate= m.CreatedDate,
                CreatedByUser=m.CreatedByUser,
                RoomNo=m.RoomNo,
                FloorName=m.FloorName,
                SrNo=m.SrNo,
                DoctorName = db.TblDoctorSetups.Where(y => y.DoctorId == m.ConsultantDr).Select(y => y.DoctorName).FirstOrDefault(),
            }).FirstOrDefaultAsync();
            return data;
        }

        public async Task<List<OPDViewModel>> GetOpdPatientCount()
        {
            //Display grouping by Month
            /*
             *   var result = await db.TblIpdregistrations.Where(x=> x.CreatedDate.HasValue).GroupBy(x => new { Month = x.CreatedDate.Value.Month, Year = x.CreatedDate.Value.Year }).Select(g => new IPDViewModel
            {
                MonthYear = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(g.Key.Month),
                Count = g.Count()
            }
            */
            var startOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var endOfMonth = DateTime.Today;

            var daysInMonth = Enumerable.Range(0, (endOfMonth - startOfMonth).Days + 1)
                                        .Select(offset => startOfMonth.AddDays(offset))
                                        .ToList();
            var rawData = await db.TblOpdregistrations
                .Where(x => x.CreatedDate >= startOfMonth && x.CreatedDate <= endOfMonth)
                .GroupBy(x => x.CreatedDate) // Group by date only, not time
                .Select(g => new { CreatedDate = g.Key, Count = g.Count() })
                .ToListAsync();
            var result = daysInMonth.Select(date => new OPDViewModel
            {
                CreatedDate = date,
                Count = rawData.FirstOrDefault(d => d.CreatedDate == date)?.Count ?? 0
            }).ToList();
            return result;
        }
    }
}
