using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Models
{
    [TableName("UnusualEvent")]
    public class UnusualEventModel
    {
        public UnusualEventModel() { }
        public UnusualEventModel(string uavTailNo, string date, string description, string chiefTechnician)
        {
            UnusualEventSN = eventSN_Count;
            Tail = uavTailNo;
            Date = date;
            Description = description;
            ChiefTechnician = chiefTechnician;

        }
        public static int eventSN_Count;
        public int UnusualEventSN { get; set; }
        public string Tail { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public string ChiefTechnician { get; set; }

        public static void UpdateEventSNCount(int last)
        {
            eventSN_Count = last + 1;
        }
    }
}
