using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Models
{
    [TableName("uavs")]
    public class UavModel
    {
        
        public UavModel(string tailNum, string hours)
        {
            TailNum = tailNum;
            Hours = hours;
        }
        public UavModel() { }
        public string TailNum { get; set; }
        public string Hours { get; set; }
    }

}
