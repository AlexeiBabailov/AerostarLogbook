using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Models
{
    [TableName("FrequenciesCband")]
    public class FrequenciesCbandModel
    {
        public static int freCbandSN_Count;
        public int FrequenciesCbandSN { get; set; }
        public string Tail { get; set; }
        public string Cband_1 { get; set; }
        public string Cband_2 { get; set; }
        public string Cband_3 { get; set; }
        public string Cband_4 { get; set; }
        public string Cband_5 { get; set; }
        public string Cband_6 { get; set; }
        public string Cband_7 { get; set; }
        public string Cband_8 { get; set; }
        public string Cband_9 { get; set; }
        public string Cband_10 { get; set; }
        public string FrequencieCbandDate { get; set; }
        public string FrequencieCbandTechnician { get; set; }
        public string FrequencieCbandChiefTechnician { get; set; }

        public FrequenciesCbandModel() { }
        public FrequenciesCbandModel(string freCbandTail, string cband_1, string cband_2, string cband_3,
            string cband_4, string cband_5, string cband_6, string cband_7, string cband_8, string cband_9, 
            string cband_10, string freCbandDate, string freCbandTechnician, string freCbandChiefTechnician)
        {
            FrequenciesCbandSN = freCbandSN_Count;
            Tail = freCbandTail;
            Cband_1 = cband_1;
            Cband_2 = cband_2;
            Cband_3 = cband_3;
            Cband_4 = cband_4;
            Cband_5 = cband_5;
            Cband_6 = cband_6;
            Cband_7 = cband_7;
            Cband_8 = cband_8;
            Cband_9 = cband_9;
            Cband_10 = cband_10;
            FrequencieCbandDate = freCbandDate;
            FrequencieCbandTechnician = freCbandTechnician;
            FrequencieCbandChiefTechnician = freCbandChiefTechnician;
        }
  
        public static void UpdateFreCbandSNCount(int last)
        {
            freCbandSN_Count = last + 1;
        }
    }
}
