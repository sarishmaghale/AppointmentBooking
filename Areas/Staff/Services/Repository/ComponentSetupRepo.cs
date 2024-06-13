
using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using AppointmentBooking.Data;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.Areas.Staff.Services.Repository
{
    public class ComponentSetupRepo : IComponentSetupRepo
    {
        private MedicareAppointmentDbContext db;
        public ComponentSetupRepo(MedicareAppointmentDbContext _db)
        {
            db = _db;
        }
        public async Task<int> AddDoctor(DoctorSetupViewModel model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    TblDoctorSetup doctordata = new TblDoctorSetup() { 
                    DoctorName=model.DoctorName,
                    DepartmentId=model.DepartmentId,
                    };
                    await db.TblDoctorSetups.AddAsync(doctordata);
                    await db.SaveChangesAsync();
                    foreach(var item in model.FeeDetails)
                    {
                        TblDoctorFeeTypeSetup docfee = new TblDoctorFeeTypeSetup() { 
                        FeeTypeId=item.FeeTypeId,
                        Fee=item.Fee,
                        DoctorId=doctordata.DoctorId,
                        };
                        await db.TblDoctorFeeTypeSetups.AddAsync(docfee);
                        await db.SaveChangesAsync();

                    }
                    await transaction.CommitAsync();
                    return doctordata.DoctorId;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await transaction.RollbackAsync();
                    return 0;
                }
            }
        }


    }
}
