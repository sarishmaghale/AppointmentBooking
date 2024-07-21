using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using AppointmentBooking.Data;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.Areas.Staff.Services.Repository
{
    public class PatientHistoryService : IPatientHistoryRepository
    {
        private HospitalManagementDbContext db;
        public PatientHistoryService(HospitalManagementDbContext _db)
        {
            db = _db;
        }
        public async Task<List<PatientHistoryItem>> GetPatientHistory(decimal Uhid,DateTime? targetDate)
        {
            //1=Opd, 2=IPD, 3=Billing 4=Lab
            var opdHistory = await db.TblOpdregistrations.Where(o => o.Uhid == Uhid).Select(o => new PatientHistoryItem
            {
                HistoryDate= o.CreatedDate,
                Type="OPD Registration",
                TypeId=1,
                RecordNo= o.SrNo,
                ConsultantDr= db.TblDoctorSetups.Where(d=>d.DoctorId==o.ConsultantDr).Select(d=>d.DoctorName).FirstOrDefault(),
            }).ToListAsync();

            var ipdHistory = await db.TblIpdregistrations.Where(i => i.Uhid == Uhid).Select(i => new PatientHistoryItem
            {
                HistoryDate = i.CreatedDate,
                Type = "IPD Registration",
                TypeId=2,
                RecordNo = i.IpdregNo,
                ConsultantDr = db.TblDoctorSetups.Where(d => d.DoctorId == i.ConsultantDr).Select(d => d.DoctorName).FirstOrDefault(),
            }).ToListAsync();
            var receiptHistory = await db.TblCashReceipts.Where(c => c.Uhid == Uhid).Select(c => new PatientHistoryItem
            {
                HistoryDate = c.CreatedDate,
                TypeId=3,
                Type = "Billing",
                RecordNo = c.ReceiptNo,
                BillAmount = c.TotalAmount,
            }).ToListAsync();
            var labHistory = await db.TblSampleRegistrations.Where(l => l.Uhid == Uhid && l.IsCollected != 0).Select(l => new PatientHistoryItem
            {
                HistoryDate=l.CreatedDate,
                Type="Lab Registration",
                TypeId=4,
                RecordNo=l.LabNo,
            }).ToListAsync();
            // Combine all records into one list and sort by date descending
            var patientHistory =opdHistory.Union(ipdHistory).Union(receiptHistory).OrderByDescending(h => h.HistoryDate).ToList();
            if (targetDate != null)
            {
                var result = BinarySearchHistoryByDate(patientHistory, targetDate);
                return result;
            }
            return patientHistory;
        }

        public List<PatientHistoryItem>BinarySearchHistoryByDate(List<PatientHistoryItem>history, DateTime? targetDate) {
            List<PatientHistoryItem> result = new List<PatientHistoryItem>();
            int left = 0;
            int right = history.Count - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;

                if (history[mid].HistoryDate == targetDate)
                {
                    // Found a match, now collect all matches
                    int index = mid;
                    // Collect all matching records to the left
                    while (index >= 0 && history[index].HistoryDate == targetDate)
                    {
                        result.Add(history[index]);
                        index--;
                    }
                    // Collect all matching records to the right
                    index = mid + 1;
                    while (index < history.Count && history[index].HistoryDate == targetDate)
                    {
                        result.Add(history[index]);
                        index++;
                    }
                    break;
                }
                else if (history[mid].HistoryDate < targetDate)
                {
                    right = mid - 1;
                }
                else
                {
                    left = mid + 1;
                }
            }

            return result;
        }
      
    }
}
