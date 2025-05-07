using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Data;
using AppointmentBooking.Models;
using AppointmentBooking.Services.Interface;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.Services.Repository
{
    public class OPDBookingService:IOPDBookingService
    {
        private HospitalManagementDbContext db;
        
        public OPDBookingService(HospitalManagementDbContext _db)
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
                        Gender = model.Gender,
                        Ethnicity = model.Ethnicity,
                        CreatedDate = model.CreatedDate,
                        CreatedTime=model.CreatedTime,
                        ConsultantDr=model.ConsultantDr,
                        RoomNo=model.RoomNo,
                        FloorName=model.FloorName,
                        CaseType=model.CaseType,
                        Amount=model.Amount,
                        AgeType = model.AgeType,
                        PayStatus = 0,
                        Uhid=model.Uhid,
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
                            Amount =Convert.ToDouble(model.amount),
                            RefId = model.refid,

                        };
                        await db.TblTransactionStatuses.AddAsync(transactionModel);
                        await db.SaveChangesAsync();
                       

                        int OPDQueue = GetOPDQueue(data.CreatedDate, Convert.ToInt32(data.Department));
                        var opdRegister = new TblOpdregistration()
                        {
                            Uhid = data.Uhid,
                            FirstName = data.FirstName,
                            LastName = data.LastName,
                            Opdqueue = OPDQueue,
                            Department = data.Department,
                            Address = data.Address,
                            District = data.District,
                            Contactno = data.Contactno,
                            Dob = data.Dob,
                            Age = data.Age,
                            AgeType = data.AgeType,
                            Gender = data.Gender,
                            Ethnicity = data.Ethnicity,
                            ConsultantDr = data.ConsultantDr,
                            RoomNo = data.RoomNo,
                            FloorName = data.FloorName,
                            CaseType = data.CaseType,
                            PayType = "online",
                            FeeType = 1,
                            Amount = data.Amount,
                            CreatedDate = data.CreatedDate,
                            CreatedTime = data.CreatedTime,
                            CreatedByUser = "OnlineBooking",
                           
                            RefId=model.refid,
                        };
                        await db.TblOpdregistrations.AddAsync(opdRegister);
                        await db.SaveChangesAsync();
                        transaction.Commit();
                        return opdRegister.SrNo;
                    }
                    transaction.Rollback();        
                    return 0;
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
            var newData = await db.TblOpdregistrations.Where(x => x.SrNo == BookingId).Select(m => new OPDBookingViewModel() {

                Uhid = m.Uhid,
                FirstName = m.FirstName,
                LastName = m.LastName,
                Opdqueue = m.Opdqueue,
                Department = m.Department,
                Address = m.Address,
                District = m.District,
                Contactno = m.Contactno,
                Dob = m.Dob,
                Age = m.Age,
                AgeType = m.AgeType,
                Gender = m.Gender,
                Ethnicity = m.Ethnicity,
                ConsultantDr = m.ConsultantDr,
                RoomNo = m.RoomNo,
                FloorName = m.FloorName,
                CaseType = m.CaseType,
                Amount = m.Amount,
                CreatedDate = m.CreatedDate,
                CreatedTime = m.CreatedTime,
                OpdbookingId=m.SrNo,
                refid=m.RefId,
            }).FirstOrDefaultAsync();


            return newData;
        }

        public int GetOPDQueue(DateTime? Date, int Department)
        {
            int maxQueue = db.TblOpdregistrations.Where(item => item.CreatedDate == Date && item.Department == Department).Max(item => item.Opdqueue) ?? 0;
            return (maxQueue + 1);
        }

        public async Task<PatientViewModel> GetPatientsByUhid(decimal uhid)
        {
            var data = await db.TblPatientRegistrations.Where(x => x.Uhid == uhid).Select(m => new PatientViewModel()
            {
                FirstName = m.FirstName,
                LastName = m.LastName,
                Dob = m.Dob,
                Age = m.Age,
                AgeType = m.AgeType,
                District = m.District,
                Address = m.Address,
                Contactno = m.Contactno,
                Gender = m.Gender,
                Ethnicity = m.Ethnicity,
                Uhid = m.Uhid,
            }).FirstOrDefaultAsync();
            return data;
        
        }

        public async Task<double> GetDoctorFees(int DoctorId, int FeeTypeId)
        {
            var data = await db.TblDoctorFeeTypeSetups.Where(x => x.DoctorId == DoctorId && x.FeeTypeId == FeeTypeId).FirstOrDefaultAsync();
            double amount = Convert.ToDouble((data != null) ? data.Fee : 0);
            return amount;
        }

		public Task<PatientViewModel> GetRecommendedDoctors(int Uhid)
		{
			throw new NotImplementedException();
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

		public async Task<bool> AddPatientFeedback(PatientViewModel model)
		{
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var data = new TblPatientFeedback()
                    {
                        PatientName = model.PatientName,
                        FeedbackText = model.Remarks,
                        SubmittedDate = model.SearchDate,
                    };
                    await db.TblPatientFeedbacks.AddAsync(data);
                    await db.SaveChangesAsync();
					await transaction.CommitAsync();
					return true;
                   
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await transaction.RollbackAsync();
                    return false;
                }
            }
		}

		public async Task<IEnumerable<PatientFeedbackViewModel>> GetAllPatientFeedback()
		{
            var data = await db.TblPatientFeedbacks.Select(x => new PatientFeedbackViewModel()
            {
                PatientName = x.PatientName,
                FeedbackText = x.FeedbackText,
                SubmittedDate = x.SubmittedDate,
            }).OrderByDescending(x=>x.SubmittedDate).ToListAsync();
            return data;
		}
	}
}
