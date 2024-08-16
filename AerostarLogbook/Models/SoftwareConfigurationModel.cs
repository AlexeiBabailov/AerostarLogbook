using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Models
{
    [TableName("SoftwareConfiguration")]
    public class SoftwareConfigurationModel
    {
        public static int sWConfSN_Count;
        public int SoftwareConfigurationSN { get; set; }
        public string Tail { get; set; }
        public string MainVersion { get; set; }
        public string INS1Version { get; set; }
        public string INS2Version { get; set; }
        public string ADIVersion { get; set; }
        public string ChangeDescription { get; set; }
        public string SWConfDate { get; set; }
        public string SWConfTechnician { get; set; }
        public string SWConfChiefTechnician { get; set; }
        public SoftwareConfigurationModel() { }
        public SoftwareConfigurationModel(string tail, string mainVersion, string iNS1Version,
            string iNS2Version, string aDIVersion, string changeDescription, string date,
            string technician, string chiefTechnician)
        {
            SoftwareConfigurationSN = sWConfSN_Count;
            Tail = tail;
            MainVersion = mainVersion;
            INS1Version = iNS1Version;
            INS2Version = iNS2Version;
            ADIVersion = aDIVersion;
            ChangeDescription = changeDescription;
            SWConfDate = date;
            SWConfTechnician = technician;
            SWConfChiefTechnician = chiefTechnician;
        }

        public static void UpdateSWConfSNCount(int last)
        {
            sWConfSN_Count = last + 1;
        }
    }
}
