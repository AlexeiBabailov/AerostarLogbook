using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Models
{
    [TableName("ComponentSM")]
    public class ComponentSMModel
    {
        public static int componentSMSN_Count;
        public int ComponentSMSN { get; set; }
        public string ComponentType { get; set; }
        public string Tail { get; set; }
        public string ValidUavHours { get; set; }
        public string DoneAtUavHours { get; set; }
        public string SN { get; set; }
        public string AssemblyDate { get; set; }
        public string AssemblyTechnician { get; set; }
        public string AssemblyChiefTechnician { get; set; }

        public ComponentSMModel() { }
        public ComponentSMModel(string componentType, string tail, string validUavHours, string doneAtUavHours, string sN,
            string assemblyDate, string assemblyTechnician, string assemblyChiefTechnician)
        {
            ComponentSMSN = componentSMSN_Count;
            ComponentType = componentType;
            Tail = tail;
            ValidUavHours = validUavHours;
            DoneAtUavHours = doneAtUavHours;
            SN = sN;
            AssemblyDate = assemblyDate;
            AssemblyTechnician = assemblyTechnician;
            AssemblyChiefTechnician = assemblyChiefTechnician;

        }

        public static void UpdateComponentSMSNCount(int last)
        {
            componentSMSN_Count = last + 1;
        }
    }
}
