using AppointmentBooking.Data;
using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using AppointmentBooking.Models;
using System.Globalization;

namespace AppointmentBooking.Areas.Staff.Services.Repository
{
    public class OPDRespository:IOPDRepository
    {
        private MedicareAppointmentDbContext db;
        public OPDRespository(MedicareAppointmentDbContext _db)
        {
            db = _db;
        }
        public async Task<int> AddOPDRegistration(OPDViewModel model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    TblOpdregistration data = new TblOpdregistration()
                    {
                        Uhid = model.Uhid,
                        RegNo = model.RegNo,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Opdqueue = model.Opdqueue,
                        Department = model.Department,
                        Address = model.Address,
                        District = model.District,
                        Contactno = model.Contactno,
                        Dob = model.Dob,
                        Age = model.Age,
                        AgeType = model.AgeType,
                        Gender = model.Gender,
                        Religion = model.Religion,
                        ConsultantDr = model.ConsultantDr,
                        RoomNo = model.RoomNo,
                        FloorName = model.FloorName,
                        CaseType = model.CaseType,
                        PayType = model.PayType,
                        FeeType = model.FeeType,
                        Amount = model.Amount,
                        PaidAmt = model.PaidAmt,
                        RefererName = model.RefererName,
                        CreatedDate = model.CreatedDate,
                        CreatedTime = model.CreatedTime,
                        CreatedByUser = model.CreatedByUser,
                    };
                    await db.TblOpdregistrations.AddAsync(data);
                    int SrNo = await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return SrNo;

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
           double amount=(double)( (data != null) ? data.Fee : 0);
            return amount;
        }


        public async Task<IEnumerable<OPDViewModel>> GetFilterOPDReports(string paytype, string user, DateTime? fromDate, DateTime? toDate, List<DataTablesOrder> order)
        {
            

            var query = db.TblOpdregistrations.AsQueryable();
            if (!string.IsNullOrEmpty(paytype) && !paytype.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(t => t.PayType.Contains(paytype));
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
            if (order != null && order.Count > 0)
            {
                foreach (var o in order)
                {
                    switch (o.Column)
                    {
                        case 0:
                            query = o.Dir == DataTablesOrderDir.Asc ?
                                query.OrderBy(t => t.SrNo) :
                                query.OrderByDescending(t => t.SrNo);
                            break;
                        case 1:
                            query = o.Dir == DataTablesOrderDir.Asc ?
                                query.OrderBy(t => t.FirstName + " " + t.LastName) :
                                query.OrderByDescending(t => t.FirstName + " " + t.LastName);
                            break;
                        case 2:
                            query = o.Dir == DataTablesOrderDir.Asc ?
                                query.OrderBy(t => t.PayType) :
                                query.OrderByDescending(t => t.PayType);
                            break;
                        case 3:
                            query = o.Dir == DataTablesOrderDir.Asc ?
                                query.OrderBy(t => t.Amount) :
                                query.OrderByDescending(t => t.Amount);
                            break;
                        case 4:
                            query = o.Dir == DataTablesOrderDir.Asc ?
                                query.OrderBy(t => t.FeeType) :
                                query.OrderByDescending(t => t.FeeType);
                            break;
                        // Add more cases for other columns as needed
                        default:
                            break;
                    }
                }
            }

            var results =await query.Select(t => new OPDViewModel
            {
                SrNo = t.SrNo,
                PatientName = t.FirstName+ " "+t.LastName,
                PayType = t.PayType,
                Amount = t.Amount,
                DoctorName = db.TblDoctorSetups.Where(x=> x.DoctorId==t.ConsultantDr).Select(x=> x.DoctorName).FirstOrDefault(),
                FeeTypeName = db.TblFeeTypes.Where(x=> x.FeeTypeId==t.FeeType).Select(x=> x.FeeTypeName).FirstOrDefault(),
                CreatedByUser = t.CreatedByUser
            }).ToListAsync();
      
            return results;
        }

        public async Task<IEnumerable<OPDViewModel>> GetOPDReports()
        {
            var date =DateTime.Now.Date;
          var data= await db.TblOpdregistrations.Where(x => x.CreatedDate == date).Select(m=> new OPDViewModel()
          {
              SrNo=m.SrNo,
             PatientName= m.FirstName+" "+m.LastName,
              PayType=m.PayType,
              Amount=m.Amount,
              ConsultantDr=m.ConsultantDr,
              FeeType=m.FeeType,
          }).ToListAsync();
            return data;
        }
    }
}
