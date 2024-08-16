using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Models
{
    [TableName("BatterySM")]

    public partial class BatterySMModel
    {
        public static int batterySN_Count;
        public int BatterySMSN { get; set; }
        public string Tail { get; set; }
        public string BatteryHrs { get; set; }
        public string InitialActivation { get; set; }
        public string BatteryOfficialSN { get; set; }
        public string BattUavHrs1 { get; set; }
        public string BattDoneAt1 { get; set; }
        public string BattDate1 { get; set; }
        public string BattDoneDate1 { get; set; }
        public string BattTechnician1 { get; set; }
        public string BattChiefTechnician1 { get; set; }
        public string BattUavHrs2 { get; set; }
        public string BattDoneAt2 { get; set; }
        public string BattDate2 { get; set; }
        public string BattDoneDate2 { get; set; }
        public string BattTechnician2 { get; set; }
        public string BattChiefTechnician2  { get; set; }
        public string BattUavHrs3{ get; set; }
        public string BattDoneAt3 { get; set; }
        public string BattDate3 { get; set; }
        public string BattDoneDate3 { get; set; }
        public string BattTechnician3 { get; set; }
        public string BattChiefTechnician3 { get; set; }
        public string BattUavHrs4 { get; set; }
        public string BattDoneAt4 { get; set; }
        public string BattDate4 { get; set; }
        public string BattDoneDate4 { get; set; }
        public string BattTechnician4 { get; set; }
        public string BattChiefTechnician4 { get; set; }
        public string BattUavHrs5 { get; set; }
        public string BattDoneAt5 { get; set; }
        public string BattDate5 { get; set; }
        public string BattDoneDate5 { get; set; }
        public string BattTechnician5 { get; set; }
        public string BattChiefTechnician5 { get; set; }
        public string BattUavHrs6 { get; set; }
        public string BattDoneAt6 { get; set; }
        public string BattDate6 { get; set; }
        public string BattDoneDate6 { get; set; }
        public string BattTechnician6 { get; set; }
        public string BattChiefTechnician6 { get; set; }

        public BatterySMModel() { }
        public BatterySMModel(
            string batteryTail, string batteryHrs, string batteryInAct, string batteryOfficialSN,
            string battUavHrs1, string battDoneAt1, string battDate1, string battDoneDate1, string battTech1, string battChiefTech1,
            string battUavHrs2, string battDoneAt2, string battDate2, string battDoneDate2, string battTech2, string battChiefTech2,
            string battUavHrs3, string battDoneAt3, string battDate3, string battDoneDate3, string battTech3, string battChiefTech3,
            string battUavHrs4, string battDoneAt4, string battDate4, string battDoneDate4, string battTech4, string battChiefTech4,
            string battUavHrs5, string battDoneAt5, string battDate5, string battDoneDate5, string battTech5, string battChiefTech5,
            string battUavHrs6, string battDoneAt6, string battDate6, string battDoneDate6, string battTech6, string battChiefTech6
            )
        {
            BatterySMSN = batterySN_Count;
            Tail = batteryTail;
            BatteryHrs = batteryHrs;
            InitialActivation = batteryInAct;
            BatteryOfficialSN = batteryOfficialSN;
            BattUavHrs1 = battUavHrs1;
            BattDoneAt1 = battDoneAt1;
            BattDate1 = battDate1;
            BattDoneDate1 = battDoneDate1;
            BattTechnician1 = battTech1 ?? "";
            BattChiefTechnician1 = battChiefTech1 ?? "";
            BattUavHrs2 = battUavHrs2;
            BattDoneAt2 = battDoneAt2;
            BattDate2 = battDate2;
            BattDoneDate2 = battDoneDate2;
            BattTechnician2 = battTech2 ?? "";
            BattChiefTechnician2 = battChiefTech2 ?? "";
            BattUavHrs3 = battUavHrs3;
            BattDoneAt3 = battDoneAt3;
            BattDate3 = battDate3;
            BattDoneDate3 = battDoneDate3;
            BattTechnician3 = battTech3 ?? "";
            BattChiefTechnician3 = battChiefTech3 ?? "";
            BattUavHrs4 = battUavHrs4;
            BattDoneAt4 = battDoneAt4;
            BattDate4 = battDate4;
            BattDoneDate4 = battDoneDate4;
            BattTechnician4 = battTech4 ?? "";
            BattChiefTechnician4 = battChiefTech4 ?? "";
            BattUavHrs5 = battUavHrs5;
            BattDoneAt5 = battDoneAt5;
            BattDate5 = battDate5;
            BattDoneDate5 = battDoneDate5;
            BattTechnician5 = battTech5 ?? "";
            BattChiefTechnician5 = battChiefTech5 ?? "";
            BattUavHrs6 = battUavHrs6;
            BattDoneAt6 = battDoneAt6;
            BattDate6 = battDate6;
            BattDoneDate6 = battDoneDate6;
            BattTechnician6 = battTech6 ?? "";
            BattChiefTechnician6 = battChiefTech6 ?? "";
        }
        public static void UpdateBattSNCount(int last)
        {
            batterySN_Count = last + 1;
        }
    }
}
