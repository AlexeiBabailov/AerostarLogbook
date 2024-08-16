using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Models
{
    [TableName("Permits")]
    public class PermitsModel
    {
        public static int permitsSN_Count;
        public int PermitsSN { get; set; }
        public string Tail { get; set; }
        public string OpenDate { get; set; }
        public string OpenBPR { get; set; }
        public string Description { get; set; }
        public string ExpiryDateOrMaxHours { get; set; }
        public string ApprovedBy { get; set; }
        public string OpenChiefTechnician { get; set; }
        public string CloseDate { get; set; }
        public string CloseBPR { get; set; }
        public string CloseTechnician { get; set; }

        public PermitsModel() { }
        public PermitsModel(string tail, string openDate, string openBPR,
            string description, string expiryDateOrMaxHours, string approvedBy,
            string openChiefTechnician)
        {
            PermitsSN = permitsSN_Count;
            Tail = tail;
            OpenDate = openDate;
            OpenBPR = openBPR;
            Description = description;
            ExpiryDateOrMaxHours = expiryDateOrMaxHours;
            ApprovedBy = approvedBy;
            OpenChiefTechnician = openChiefTechnician;
            CloseDate = string.Empty;
            CloseBPR = string.Empty;
            CloseTechnician = string.Empty;
        }

        public void ClosePermit(string closeDate,
            string closeBPR,
            string closeTechnician)
        {
            CloseDate = closeDate;
            CloseBPR = closeBPR;
            CloseTechnician = closeTechnician;
        }

        public static void UpdatePermitsSNCount(int last)
        {
            permitsSN_Count = last + 1;
        }
    }
}
