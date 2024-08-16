using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Models
{
    [TableName("ServiceBulletin")]

    public class ServiceBulletinModel
    {
        public static int bulletinSN_Count;
        public int ServiceBulletinSN { get; set; }
        public string Tail { get; set; }
        public int ServiceBulletinNumber { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public string Technician { get; set; }
        public string ChiefTechnician { get; set; }

        public ServiceBulletinModel() { }
        public ServiceBulletinModel(string tail, int bulletinNumber, string bulletinDate, string bulletinDescription, string technician, string chiefTechnician)
        {
            ServiceBulletinSN = bulletinSN_Count;
            Tail = tail;
            ServiceBulletinNumber = bulletinNumber;
            Date = bulletinDate;
            Description = bulletinDescription;
            Technician = technician;
            ChiefTechnician = chiefTechnician;
        }

        public static void UpdateBulletinSNCount(int last)
        {
            bulletinSN_Count = last + 1;
        }
    }
}
