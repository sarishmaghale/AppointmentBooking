using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using AppointmentBooking.Data;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace AppointmentBooking.Areas.Staff.Services.Repository
{
    public class PatientHistoryService : IPatientHistoryRepository
    {
        private HospitalManagementDbContext db;
        public PatientHistoryService(HospitalManagementDbContext _db)
        {
            db = _db;
        }
		public List<PatientHistoryItem> BinarySearchHistoryByDate(List<PatientHistoryItem> history, DateTime? targetDate)
		{
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
                Department=db.TblDoctorSetups.Where(c=>c.DoctorId==o.ConsultantDr).Select(c=>c.DepartmentId).FirstOrDefault(),
            }).ToListAsync();

            var ipdHistory = await db.TblIpdregistrations.Where(i => i.Uhid == Uhid).Select(i => new PatientHistoryItem
            {
                HistoryDate = i.CreatedDate,
                Type = "IPD Registration",
                TypeId=2,
                RecordNo = i.IpdregNo,
                ConsultantDr = db.TblDoctorSetups.Where(d => d.DoctorId == i.ConsultantDr).Select(d => d.DoctorName).FirstOrDefault(),
				Department = db.TblDoctorSetups.Where(c => c.DoctorId == i.ConsultantDr).Select(c => c.DepartmentId).FirstOrDefault(),
			}).ToListAsync();
            var receiptHistory = await db.TblCashReceipts.Where(c => c.Uhid == Uhid).Select(c => new PatientHistoryItem
            {
                HistoryDate = c.CreatedDate,
                TypeId=3,
                Type = "Billing",
                RecordNo = c.ReceiptNo,
                BillAmount = c.TotalAmount,
				
			}).ToListAsync();
            var labHistory = await db.TblSampleRegistrations.Where(l => l.Uhid == Uhid && l.IsCollected == 2).Select(l => new PatientHistoryItem
            {
                HistoryDate=l.CreatedDate,
                Type="Lab Registration",
                TypeId=4,
                RecordNo=l.LabNo,
	
			}).ToListAsync();
            // Combine all records into one list and sort by date descending
            var patientHistory=opdHistory.Union(ipdHistory).Union(receiptHistory).Union(labHistory).OrderByDescending(h => h.HistoryDate).ToList();
            if (targetDate != null)
            {
                var result = BinarySearchHistoryByDate(patientHistory, targetDate);
                return result;
            }
            return patientHistory;
        }

        
        public async Task<List<DoctorSetupViewModel>> GetRecommendedDoctors(decimal Uhid)
        {
            var patientHistory = await GetPatientHistory(Uhid, null);
            var doctors = await db.TblDoctorSetups.Include(d => d.Department).ToListAsync();
			var doctorViewModels = doctors.Select(d => new DoctorSetupViewModel
			{
				DoctorId = d.DoctorId,
				DoctorName = d.DoctorName,
				DepartmentId = d.DepartmentId,
				DepartmentName = d.Department.DepartmentName,
				Score = 0 // Initialize score
			}).ToList();

			foreach (var doctor in doctorViewModels)
			{
				doctor.Score = CalculateMatchScore(doctor, patientHistory);
			}
			// Sort doctors by score in descending order
			var recommendedDoctors = FilterAndSortDoctors(doctorViewModels);

			return recommendedDoctors;
		}

		private int CalculateMatchScore(DoctorSetupViewModel doctor, List<PatientHistoryItem> history)
		{
			int score = 0;
			foreach (var historyItem in history)
			{
				if (historyItem.Department.HasValue && historyItem.Department == doctor.DepartmentId)
				{
					score++;
				}
				if (historyItem.ConsultantDr != null && historyItem.ConsultantDr.Equals(doctor.DoctorName, StringComparison.OrdinalIgnoreCase))
				{
					score++;
				}
			}
			return score;
		}
        private List<DoctorSetupViewModel> FilterAndSortDoctors(List<DoctorSetupViewModel> doctors)
        {
            // Conditional Filtering: Filter out doctors with a score less than or equal to 0
            var filteredDoctors = new List<DoctorSetupViewModel>();
            foreach (var doctor in doctors)
            {
                if (doctor.Score > 0)
                {
                    filteredDoctors.Add(doctor);
                }
            }

            // Quick Sort: Sort doctors based on their scores in descending order
            return QuickSortDoctors(filteredDoctors);
        }

        // Quick Sort Algorithm
        private List<DoctorSetupViewModel> QuickSortDoctors(List<DoctorSetupViewModel> doctors)
        {
            if (doctors.Count <= 1)
            {
                return doctors; // Base case: a list with 1 or no element is already sorted
            }

            // Selecting a pivot (middle element)
            var pivot = doctors[doctors.Count / 2];

            // Partitioning into three lists
            var lesser = new List<DoctorSetupViewModel>();
            var equal = new List<DoctorSetupViewModel>();
            var greater = new List<DoctorSetupViewModel>();

            foreach (var doctor in doctors)
            {
                if (doctor.Score > pivot.Score)
                {
                    greater.Add(doctor); // Doctors with scores greater than pivot
                }
                else if (doctor.Score < pivot.Score)
                {
                    lesser.Add(doctor);  // Doctors with scores less than pivot
                }
                else
                {
                    equal.Add(doctor);   // Doctors with scores equal to pivot
                }
            }

            // Recursively sort the lesser and greater lists and concatenate the result
            var sortedDoctors = new List<DoctorSetupViewModel>();
            sortedDoctors.AddRange(QuickSortDoctors(greater)); // Sort the greater (descending)
            sortedDoctors.AddRange(equal);                     // Add the doctors with pivot score
            sortedDoctors.AddRange(QuickSortDoctors(lesser));  // Sort the lesser (descending)

            return sortedDoctors;
        }

    }
}
