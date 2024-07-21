
using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using AppointmentBooking.Data;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.Areas.Staff.Services.Repository
{
    public class ComponentSetupService : IComponentSetupRepository
    {
        private HospitalManagementDbContext db;
        public ComponentSetupService(HospitalManagementDbContext _db)
        {
            db = _db;
        }

        public async Task<bool> AddBedCategory(BedSetupViewModel model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var data = new TblIpdbedType()
                    {
                      
                        BedTypeName = model.BedCategoryName,
                        Price = model.BedPrice,
                    };
                    await db.TblIpdbedTypes.AddAsync(data);
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

        public async Task<bool> AddBeds(BedSetupViewModel model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var data = new TblIpdbedStatus()
                    {
                        BedTypeId = model.BedCategoryId,
                        BedName = model.BedName,
                        Status = 0,
                        IpdregNo = 0,
                    };
                    await db.TblIpdbedStatuses.AddAsync(data);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        public async Task<int> AddDoctor(DoctorSetupViewModel model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //add doctor information
                    TblDoctorSetup doctordata = new TblDoctorSetup() { 
                    DoctorName=model.DoctorName,
                    DepartmentId=model.DepartmentId,
                    };
                    await db.TblDoctorSetups.AddAsync(doctordata);
                    await db.SaveChangesAsync();
                    //add multiple fees for each doctor
                    if (model.FeeDetails != null)
                    {
                        foreach (var item in model.FeeDetails)
                        {
                            TblDoctorFeeTypeSetup docfee = new TblDoctorFeeTypeSetup()
                            {
                                FeeTypeId = item.FeeTypeId,
                                Fee = item.Fee,
                                DoctorId = doctordata.DoctorId,
                            };
                            await db.TblDoctorFeeTypeSetups.AddAsync(docfee);
                            await db.SaveChangesAsync();

                        }
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

        public async Task<bool> AddTest(TestSetupViewModel model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var data = new TblTestSetup()
                    {
                        TestGroupId = model.TestGroupId,
                        TestName = model.TestName,
                        TestPrice = model.TestPrice,
                    };
                    await db.TblTestSetups.AddAsync(data);
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

        public async Task<bool> AddTestGroup(TestSetupViewModel model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var data = new TblTestGroupSetup()
                    {
                        TestGroupName = model.TestGroupName,
                    };
                    await db.TblTestGroupSetups.AddAsync(data);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
               
        }

        public async Task<DoctorSetupViewModel> GetDoctorInfo(int DoctorId)
        {
            var doctor = await db.TblDoctorSetups
            .Include(d => d.TblDoctorFeeTypeSetups)
                             .FirstOrDefaultAsync(d => d.DoctorId == DoctorId);
            if (doctor == null)
            {
                return null;
            }

            var model = new DoctorSetupViewModel
            {
                DoctorId = doctor.DoctorId,
                DoctorName = doctor.DoctorName,
                DepartmentId = doctor.DepartmentId,
                ExistingFees = doctor.TblDoctorFeeTypeSetups.Select(f => new DoctorFeeViewModel
                {
                    FeeTypeId = f.FeeTypeId,
                    Fee = f.Fee
                }).ToList()
            };
            return model;
        }

        public async Task<bool> UpdateDoctorInfo(DoctorSetupViewModel model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var doctordata = await db.TblDoctorSetups
                                             .Include(d => d.TblDoctorFeeTypeSetups)
                                             .FirstOrDefaultAsync(d => d.DoctorId == model.DoctorId);
                    if (doctordata != null)
                    {
                        // Update basic doctor information
                        doctordata.DoctorName = model.DoctorName;
                        doctordata.DepartmentId = model.DepartmentId;

                        // Update or add existing fees
                        if (model.ExistingFees != null)
                        {
                            foreach (var item in model.ExistingFees)
                            {
                                var existingFee = doctordata.TblDoctorFeeTypeSetups
                                                            .FirstOrDefault(f => f.FeeTypeId == item.FeeTypeId);

                                if (existingFee != null)
                                {
                                    existingFee.Fee = item.Fee;
                                }
                                else
                                {
                                    TblDoctorFeeTypeSetup docfee = new TblDoctorFeeTypeSetup()
                                    {
                                        FeeTypeId = item.FeeTypeId,
                                        Fee = item.Fee,
                                        DoctorId = doctordata.DoctorId,
                                    };
                                    await db.TblDoctorFeeTypeSetups.AddAsync(docfee);
                                }
                            }
                            // Remove fees that are no longer in the model
                            var feeTypeIds = model.ExistingFees.Select(f => f.FeeTypeId).ToList();
                            var feesToRemove = doctordata.TblDoctorFeeTypeSetups
                                                         .Where(f => !feeTypeIds.Contains(f.FeeTypeId))
                                                         .ToList();
                            db.TblDoctorFeeTypeSetups.RemoveRange(feesToRemove);
                            // Save changes for the updated and removed fees
                            await db.SaveChangesAsync();
                        }
                        
                        // Add new fee details
                        if (model.FeeDetails != null)
                        {
                            foreach (var newItem in model.FeeDetails)
                            {
                                TblDoctorFeeTypeSetup docfee = new TblDoctorFeeTypeSetup()
                                {
                                    FeeTypeId = newItem.FeeTypeId,
                                    Fee = newItem.Fee,
                                    DoctorId = doctordata.DoctorId,
                                };
                                await db.TblDoctorFeeTypeSetups.AddAsync(docfee);
                            }
                            // Save changes for the new fees
                            await db.SaveChangesAsync();
                        }
                 
                        await transaction.CommitAsync();
                        return true;
                    }

                    await transaction.RollbackAsync();
                    return false;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return false;
                    throw new Exception(ex.Message);
                    
                }
                
            }
               

        }
       
       public async Task<IEnumerable<DoctorSetupViewModel>> GetDoctorList()
        {
            var data = await db.TblDoctorSetups.Select(x => new DoctorSetupViewModel()
            {
                DoctorId = x.DoctorId,
                DoctorName = x.DoctorName,
                DepartmentName = db.TblDepartments.Where(d => d.DepartmentId == x.DepartmentId).Select(d => d.DepartmentName).FirstOrDefault(),
                FeeDetails = db.TblDoctorFeeTypeSetups.Where(f => f.DoctorId == x.DoctorId).Select(f => new DoctorFeeViewModel
                {
                    FeeTypeName = db.TblFeeTypes.Where(y => y.FeeTypeId == f.FeeTypeId).Select(y => y.FeeTypeName).FirstOrDefault(),
                    Fee = f.Fee,
                }).ToList()
            }).ToListAsync();
            return data;
        }

        public async Task<bool> AddNewLabParameter(ParameterSetupViewModel model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var data = new TblLabParameterSetup()
                    {
                        ParameterName = model.ParameterName,
                        Range = model.Range,
                        Unit = model.Unit,
                        Method = model.Method,
                    };
                    await db.TblLabParameterSetups.AddAsync(data);
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

        public async Task<bool> AddParameterTestMapping(ParameterSetupViewModel model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var data = new TblTestParameterMapping()
                    {
                        TestId = model.TestId,
                        ParamaterId = model.Parameterid,
                    };
                    await db.TblTestParameterMappings.AddAsync(data);
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

        public async Task<bool> CheckParameterTestMapping(ParameterSetupViewModel model)
        {
            var existingData = await db.TblTestParameterMappings
     .FirstOrDefaultAsync(x => x.TestId == model.TestId && x.ParamaterId == model.Parameterid);
            if (existingData != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
