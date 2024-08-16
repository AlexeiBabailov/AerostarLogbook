
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Models
{
    [TableName("Flights")]
    public partial class FlightActivityModel
    {
        public FlightActivityModel(string flightTail,
            string flightDate, string flightPurpose, int totalRefueling, string refuelName,
            string fuelExpDate, string payload, string weight, int openPermits,
            string engineSN, string engineHours, string engineNextMaintenance,
            string drainAndSample, string prefUavClosure, string uav_gcs,
            string arrestDeployed, string arrestDir, string chiefTechnician,
            string exPilotUavCheck, string exPilotSteerCheck, int rpmMax, int rpmMin,
            string startup, string shutdown, int fuelAtLanding, int touchDowns,
            string afterLanding, string uavHours, string engineHoursAfterFlight)
        {
            FlightsSN = flightsSN_Count;
            Tail = flightTail;
            FlightDate = flightDate;
            FlightPurpose = flightPurpose;
            TotalRefueling = totalRefueling;
            RefuelName = refuelName ?? "";
            FuelExpDate = fuelExpDate;
            Payload = payload;
            Weight = weight;
            OpenPermits = openPermits;
            EngineSN = engineSN;
            EngineHours = engineHours;
            EngineNextMaintenance = engineNextMaintenance;
            DrainAndSample = drainAndSample;
            PrefUavClosure = prefUavClosure;
            Uavgcs = uav_gcs;
            ArrestDeployed = arrestDeployed;
            ArrestDir = arrestDir;
            ChiefTechnician = chiefTechnician ?? "";
            ExPilotUavCheck = exPilotUavCheck ?? "";
            ExPilotSteerCheck = exPilotSteerCheck;
            RpmMax = rpmMax;
            RpmMin = rpmMin;
            Startup = startup;
            Shutdown = shutdown;
            FuelAtLanding = fuelAtLanding;
            TouchDowns = touchDowns;
            AfterLanding = afterLanding;
            UavHours = uavHours;
            EngineHoursAfterFlight = engineHoursAfterFlight;
        }

        public FlightActivityModel() { }
        public static int flightsSN_Count;
        public int FlightsSN { get; set; }
        public string Tail { get; set; }
        public string FlightDate { get; set; }
        public string FlightPurpose { get; set; }
        public int TotalRefueling { get; set; }
        public string RefuelName { get; set; }
        public string FuelExpDate { get; set; }
        public string Payload { get; set; }
        public string Weight { get; set; }
        public int OpenPermits  { get; set; }
        public string EngineSN  { get; set; }
        public string EngineHours { get; set; }
        public string EngineNextMaintenance { get; set; }
        public string DrainAndSample { get; set; }
        public string PrefUavClosure { get; set; }
        public string Uavgcs { get; set; }
        public string ArrestDeployed { get; set; }
        public string ArrestDir { get; set; }
        public string ChiefTechnician { get; set; }
        public string ExPilotUavCheck { get; set; }
        public string ExPilotSteerCheck { get; set; }
        public int RpmMax { get; set; }
        public int RpmMin { get; set; }
        public string Startup { get; set; }
        public string Shutdown { get; set; }
        public int FuelAtLanding { get; set; }
        public int TouchDowns { get; set; }
        public string AfterLanding { get; set; }
        public string UavHours { get; set; }
        public string EngineHoursAfterFlight { get; set; }

        public static void UpdateFlightsSNCount(int last)
        {
            flightsSN_Count = last + 1;
        }


    }
}
