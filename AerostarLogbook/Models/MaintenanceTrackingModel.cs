using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Models
{
    [TableName("MaintenanceTracking")]

    public class MaintenanceTrackingModel
    {
        public static int maintenanceSN_Count;
        public int MaintenanceTrackingSN { get; set; }
        public string Tail { get; set; }
        public string OpenDate { get; set; }
        public string OpenTime { get; set; }
        public string OpenByName { get; set; }
        public string FaultDescription { get; set; }
        public string CorrectiveAction { get; set; }
        public string FodCheck { get; set; }
        public string CloseDate { get; set; }
        public string CloseTime { get; set; }
        public string CloseTechnician { get; set; }
        public string CloseChiefTechnician { get; set; }

        //ParameterLess Constructor for Activator only
        public MaintenanceTrackingModel() { }
        public MaintenanceTrackingModel(string maintenanceTail, string openDate, string openTime, string openByName, string faultDescription) 
        {
            MaintenanceTrackingSN = maintenanceSN_Count;
            Tail = maintenanceTail;
            OpenDate = openDate;
            OpenTime = openTime;
            OpenByName = openByName;
            FaultDescription = faultDescription;
            CorrectiveAction = string.Empty;
            FodCheck = string.Empty;
            CloseDate = string.Empty;
            CloseTime = string.Empty;
            CloseTechnician = string.Empty;
            CloseChiefTechnician = string.Empty;
        }

        public void CloseMaintenance (string correctiveAction, string fodCheck, string closeDate, string closeTime,
            string closeTechnician, string closeChiefTechnician)
        {
            CorrectiveAction = correctiveAction;
            FodCheck = fodCheck;
            CloseDate = closeDate;
            CloseTime = closeTime;
            CloseTechnician = closeTechnician;
            CloseChiefTechnician = closeChiefTechnician;
        }

        public static void UpdateMaintenanceTrackingSNCount(int sn)
        {
            maintenanceSN_Count = sn + 1;
        }

    }
}
