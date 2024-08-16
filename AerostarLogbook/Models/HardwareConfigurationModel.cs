using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Models
{
    [TableName("HardwareConfiguration")]

    public class HardwareConfigurationModel
    {
        public static int hWConfSN_Count;
        public int HardwareConfigurationSN { get; set; }
        public string Tail { get; set; }
        public string AssemblyDate { get; set; }
        public string AssemblyDescription { get; set; }
        public string AssemblyChiefTechnician { get; set; }
        public string DisassemblyDate { get; set; }
        public string DisassemblyChiefTechnician { get; set; }

        public HardwareConfigurationModel() { }
        public HardwareConfigurationModel(string tail, string hWConfDate, string hWConfDescription, string assemblchiefTechnician)
        {
            HardwareConfigurationSN = hWConfSN_Count;
            Tail = tail;
            AssemblyDate = hWConfDate;
            AssemblyDescription = hWConfDescription;
            AssemblyChiefTechnician = assemblchiefTechnician;
            DisassemblyDate = string.Empty;
            DisassemblyChiefTechnician = string.Empty;
        }

        public void Disassembly(string disassemlyDate, string disassemlyChiefTechnician)
        {
            DisassemblyDate = disassemlyDate;
            DisassemblyChiefTechnician = disassemlyChiefTechnician;
        }

        public static void UpdateHWConfSNCount(int last)
        {
            hWConfSN_Count = last + 1;
        }
    }
}
