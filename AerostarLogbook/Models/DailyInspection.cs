using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Models
{
    [TableName("Inspections")]
    public class DailyInspection
    {
        public DailyInspection(string tail, string date, string time, string name) 
        {
            InspectionsSN = inspectionSN_Count;
            Tail = tail;
            InspectionDate = date;
            InspectionTime = time;
            InspectionTechnician = name;

        }

        public DailyInspection() { }
        public static int inspectionSN_Count;
        public int InspectionsSN { get; set; }
        public string Tail { get; set; }
        public string InspectionDate { get; set; }
        public string InspectionTime { get; set; }
        public string InspectionTechnician { get; set; }



        public static void UpdateInspectionSNCount(int sn)
        {
            inspectionSN_Count = sn + 1;
        }
    }

}
