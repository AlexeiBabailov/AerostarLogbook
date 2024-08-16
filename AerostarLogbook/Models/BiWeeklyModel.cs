using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Models
{
    [TableName("BiWeekly")]
    public class BiWeeklyModel
    {
        public static int biWeeklySN_Count;
        public int BiWeeklySN { get; set; }
        public string Tail { get; set; }
        public string ExpiryDate { get; set; }
        public string DoneOnDate { get; set; }
        public string Technician { get; set; }

        public BiWeeklyModel() { }
        public BiWeeklyModel(string tail, string exDate, string doneDate, string technician)
        {
            BiWeeklySN = biWeeklySN_Count;
            Tail = tail;
            ExpiryDate = exDate;
            DoneOnDate = doneDate;
            Technician = technician;

        }

        public static void UpdateBiWeeklySNCount(int last)
        {
            biWeeklySN_Count = last + 1;
        }
    }
}
