using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using AppointmentBooking.Data;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.Areas.Staff.Services.Repository
{
    public class ReceiptRepository : IReceiptRepository

    {
        private MedicareAppointmentDbContext db;
        public ReceiptRepository(MedicareAppointmentDbContext _db)
        {
            db = _db;
        }

        public async Task<double> GetTestPrice(int TestId)
        {
            var price =await db.TblTestSetups.Where(x => x.TestId == TestId).Select(x => x.TestPrice).FirstOrDefaultAsync();
            return (double)price;
        }

        public async Task<IEnumerable<ReceiptViewModel>> GetTestsDataOnTestGroup(int TestGroupId)
        {
            var result = await (from test in db.TblTestSetups
                                                 join testgroup in db.TblTestGroupSetups
                                                 on test.TestGroupId equals testgroup.TestGroupId
                                                 where test.TestGroupId == TestGroupId
                                                 select new ReceiptViewModel
                                                 {
                                                     TestId = test.TestId,
                                                    TestName=test.TestName,
                                                    TestPrice=test.TestPrice,
                                                    TestGroupName=testgroup.TestGroupName,                                                   
                                                 }).ToListAsync();

            return result;

        }
    }
}
