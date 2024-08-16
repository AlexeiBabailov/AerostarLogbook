using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Models
{
    [TableName("FrequenciesUhf")]

    public class FrequenciesUhfModel
    {
        public static int freUhfSN_Count;
        public int FrequenciesUhfSN { get; set; }
        public string Tail { get; set; }
        public string FrequencieUhf { get; set; }
        public string FrequencieUhfDate { get; set; }
        public string FrequencieUhfTechnician { get; set; }
        public string FrequencieUhfChiefTechnician { get; set; }

        public FrequenciesUhfModel() { }
        public FrequenciesUhfModel(string freUhfTail, string frequencieUhf, string freUhfDate, string freUhfTechnician,
            string freUhfChiefTechnician)
        {
            FrequenciesUhfSN = freUhfSN_Count;
            Tail = freUhfTail;
            FrequencieUhf = frequencieUhf;
            FrequencieUhfDate = freUhfDate;
            FrequencieUhfTechnician = freUhfTechnician;
            FrequencieUhfChiefTechnician = freUhfChiefTechnician;
        }

        public static void UpdateFreUhfSNCount(int last)
        {
            freUhfSN_Count = last + 1;
        }
    }
}
