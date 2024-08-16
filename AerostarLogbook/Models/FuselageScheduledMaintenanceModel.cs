using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Models
{
    [TableName("FuselageScheduledMaintenance")]
    public class FuselageScheduledMaintenanceModel
    {
        public static int fsmSN_Count;
        public int FuselageScheduledMaintenanceSN { get; set; }
        public string Tail { get; set; }
        public string ValidUavHr { get; set; }
        public string DoneAtUavHr { get; set; }
        public string Date { get; set; }
        public string Technician { get; set; }
        public string ChiefTechnician { get; set; }
        public string Type { get; set; }

        public FuselageScheduledMaintenanceModel() { }
        public FuselageScheduledMaintenanceModel(string tail, string validUavHr, string doneAtUavHr,
            string date, string technician, string chiefTechnician, string type)
        {
            FuselageScheduledMaintenanceSN = fsmSN_Count;
            Tail = tail;
            ValidUavHr = validUavHr;
            DoneAtUavHr = doneAtUavHr;
            Date = date;
            Technician = technician ?? "";
            ChiefTechnician = chiefTechnician ?? "";
            Type = type;
        }


        public static void UpdateFsmSNCount(int last)
        {
            fsmSN_Count = last + 1;
        }
    }
}
