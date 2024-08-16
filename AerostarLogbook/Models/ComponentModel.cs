using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Models
{
    [TableName("Components")]
    public class ComponentModel
    {
        public static readonly string[] ComponentTypes = 
            {
            "IMU",
            "Main Box",
            "INS",
            "Magnetometer",
            "LRF",
            "Humidity & Temp Sensor",
            "Strobe",
            "DPGS",
            "GPS",
            "Pitot",
            "IFF Transponder",
            "IFF BPU",
            "Right Flap",
            "Left Flap",
            "Right Elevator",
            "Left Elevator",
            "Right Rudder",
            "Left Rudder",
            "Right Aileron",
            "Left Aileron",
            "Nose Wheel",
            "Right Brake",
            "Left Brake",
            "Main Landing Gear",
            "Nose Landing Gear",
            "ADT",
            "ADI",
            "ADA",
            "UHF",
            "12V Battery",
            "Regulator"
        };

        public ComponentModel(string type, string sN, string tail)
        {
            Type = type;
            SN = sN;
            Tail = tail;
        }
        public ComponentModel() { }
        public string Type { get; set; }
        public string SN { get; set; }
        public string Tail { get; set; }

        public bool IsFreeForPicker => string.IsNullOrEmpty(Tail);

        public string AssembleStatus
        {
            get
            {
                if (string.IsNullOrEmpty(Tail))
                {
                    return $"{SN}";
                }
                else
                {
                    return $"{SN} assembled on UAV {Tail}";
                }
            }
        }



    }
}
