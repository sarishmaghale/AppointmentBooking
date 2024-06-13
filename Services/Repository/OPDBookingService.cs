using AppointmentBooking.Data;
using AppointmentBooking.Models;
using AppointmentBooking.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.Services.Repository
{
    public class OPDBookingService:IOPDBookingService
    {
        private MedicareAppointmentDbContext db;
        
        public OPDBookingService(MedicareAppointmentDbContext _db)
        {
            db = _db;
     }

        public async Task<int> AddOPDBooking(OPDBookingViewModel model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    string InsuranceId;
                    if (model.InsuranceId == null)
                    {
                        InsuranceId = "";
                    }
                    else
                    {
                        InsuranceId = model.InsuranceId;
                    }

                    TblOpdbooking data = new TblOpdbooking()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        InsuranceId = model.InsuranceId,
                        BookingDate = model.BookingDate,
                        Department = model.Department,
                        Address = model.Address,
                        District = model.District,
                        Contactno = model.Contactno,
                        Dob = model.Dob,
                        Age = model.Age,
                        Panno = model.Panno,
                        Gender = model.Gender,
                        Ethnicity = model.Ethnicity,
                        Email = model.Email,
                        CreatedDateTime = DateTime.Now.ToString(),
                        AgeType = model.AgeType,
                        PayStatus = 0,

                    };

                    await db.TblOpdbookings.AddAsync(data);
                    await db.SaveChangesAsync();
                    transaction.Commit();
                    return data.OpdbookingId;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                    return 0;
                }
            }

        }

        public async Task<int> UpdatePayStatus(OPDBookingViewModel model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var data = db.TblOpdbookings.Where(x => x.OpdbookingId == model.OpdbookingId).FirstOrDefault();
                    if (data != null)
                    {
                        data.PayStatus = 1;
                        db.Entry(data).State = EntityState.Modified;
                        db.SaveChanges();
                        TblTransactionStatus transactionModel = new TblTransactionStatus()
                        {
                            OpdbookingId = model.OpdbookingId,
                            Amount = double.Parse(model.amount),
                            RefId = model.refid,

                        };
                        await db.TblTransactionStatuses.AddAsync(transactionModel);
                        await db.SaveChangesAsync();
                    }
                    transaction.Commit();
                    return data.OpdbookingId;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                    return 0;
                }
            }
        }

        public async Task<OPDBookingViewModel> GetBookingInfo(int BookingId)
        {
            var data = await db.TblOpdbookings.Where(x => x.OpdbookingId == BookingId).Select(m => new OPDBookingViewModel()
            {
                OpdbookingId = m.OpdbookingId,
                FirstName = m.FirstName,
                LastName = m.LastName,
                Contactno = m.Contactno,
                BookingDate = m.BookingDate,
                Department = m.Department,
                PayStatus = m.PayStatus,
                Age = m.Age,
                AgeType = m.AgeType,
                Gender = m.Gender,
                Address = m.Address,
                District = m.District,
            }).FirstOrDefaultAsync();
            if (data.PayStatus == 1)
            {
                var newData = await (from bk in db.TblOpdbookings
                                     join tr in db.TblTransactionStatuses
                                   on bk.OpdbookingId equals tr.OpdbookingId
                                     where bk.OpdbookingId == BookingId
                                     select new OPDBookingViewModel()
                                     {
                                         OpdbookingId = bk.OpdbookingId,
                                         FirstName = bk.FirstName,
                                         LastName = bk.LastName,
                                         Contactno = bk.Contactno,
                                         BookingDate = bk.BookingDate,
                                         Department = bk.Department,
                                         refid = tr.RefId,
                                         Age = bk.Age,
                                         AgeType = bk.AgeType,
                                         PayStatus = bk.PayStatus,
                                         Gender = bk.Gender,
                                         Address = bk.Address,
                                         District = bk.District,
                                         amount = tr.Amount.ToString(),
                                     }).FirstOrDefaultAsync();
                return newData;
            }
            return data;
        }
    }
}
