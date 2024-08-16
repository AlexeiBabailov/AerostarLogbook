using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Models
{
    [TableName("WeightConfigurations")]

    public class WeightConfigurationsModel
    {
        public static int confSN_Count;
        public int WeightConfigurationsSN { get; set; }
        public string Tail { get; set; }
        public string PayloadType { get; set; }
        public string Description { get; set; }
        public string NoseWeight { get; set; }
        public string TailWeight { get; set; }
        public string UavAngle { get; set; }
        public string Date { get; set; }
        public string Technician { get; set; }
        public string ChiefTechnician { get; set; }

        public WeightConfigurationsModel() { }
        public WeightConfigurationsModel(string tail, string payloadType, string confDescription, string noseWeight,
            string tailWeight, string uavAngle, string date, string confTechnician, string confChiefTechnician)
        {
            WeightConfigurationsSN = confSN_Count;
            Tail = tail;
            PayloadType = payloadType;
            Description = confDescription;
            NoseWeight = noseWeight;
            TailWeight = tailWeight;
            UavAngle = uavAngle;
            Date = date;
            Technician = confTechnician;
            ChiefTechnician = confChiefTechnician;
        }

        public static void UpdateConfSNCount(int last)
        {
            confSN_Count = last + 1;
        }
    }
}
