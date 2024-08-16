using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using AerostarLogbook.Models;
using AerostarLogbook.Models.TeamMembers;
using AerostarLogbook.Resources.Converters;

using AerostarLogbook.Services;
using AerostarLogbook.Services.Helpers;
using AerostarLogbook.Services.DatabaseMain;
using AerostarLogbook.Services.DatabaseMiscellaneous;
using Microsoft.Extensions.Logging;
using AerostarLogbook.Interfaces;


namespace AerostarLogbook.ViewModels
{

    public partial class FlightActivityViewModel : ObservableObject
    {
        private readonly IMessagingService _messagingService;

        private string FlightActivityDB { get; set; }
        private string InspectionsDB { get; set; }



        private string localSelectedUAV;
        private static TimeSpan tempEngTime;

        [ObservableProperty]
        private ObservableCollection<FlightActivityModel> flights;

        [ObservableProperty]
        public ObservableCollection<DailyInspection> inspections;

        [ObservableProperty]
        DateTime flightDate = DateTime.Today;
        [ObservableProperty]
        string flightPurpose = "Training";
        [ObservableProperty]
        int totalRefueling = 35;
        [ObservableProperty]
        string refuelName = "Roni Malmud";
        [ObservableProperty]
        DateTime fuelExpDate = DateTime.Today;
        [ObservableProperty]
        string payload = "DSP";
        [ObservableProperty]
        string weight = "30";
        [ObservableProperty]
        bool openPermitsForUI = true;
        [ObservableProperty]
        int openPermits = 1;
        [ObservableProperty]
        string engineHours = string.Empty;

        [ObservableProperty]
        string engineNextMaintenance = "12:30";
        [ObservableProperty]
        string drainAndSample = "none";
        [ObservableProperty]
        string prefUavClosure = "Tamir Nudelman";
        [ObservableProperty]
        string uavgcs = "61";
        [ObservableProperty]
        string arrestDeployed = "yes";
        [ObservableProperty]
        string arrestDir = "South";
        [ObservableProperty]
        string chiefTechnician = "Tamir Nudelman";
        [ObservableProperty]
        string exPilotUavCheck = "Nir Schneider";
        [ObservableProperty]
        string exPilotSteerCheck ="yes";
        [ObservableProperty]
        int rpmMax = 6300;
        [ObservableProperty]
        int rpmMin = 9;
        [ObservableProperty]
        System.TimeSpan startup = new TimeSpan(DateTime.Now.TimeOfDay.Hours, DateTime.Now.TimeOfDay.Minutes, 0);
        [ObservableProperty]
        System.TimeSpan shutdown = new TimeSpan(
            DateTime.Now.TimeOfDay.Add(new TimeSpan(1, 0, 0)).Hours,
            DateTime.Now.TimeOfDay.Add(new TimeSpan(1, 0, 0)).Minutes,
            0);
        [ObservableProperty]
        System.TimeSpan totalTime;

        [ObservableProperty]
        int fuelAtLanding = 25;
        [ObservableProperty]
        int touchDowns = 3;
        [ObservableProperty]
        string afterLanding = "good";
        [ObservableProperty]
        string uavHours = "500";
        [ObservableProperty]
        string engineHoursAfterFlight;

        [ObservableProperty]
        DateTime minimalIspectionDate = DateTime.Today.AddDays(-3);

        [ObservableProperty]
        DateTime inspectionDate = DateTime.Today;
        [ObservableProperty]
        System.TimeSpan inspectionTime = DateTime.Now.TimeOfDay;
        [ObservableProperty]
        string inspectionTechnician = "Roni Malmud";

        partial void OnStartupChanged(TimeSpan value)
        {
            UpdateTotalTime();
        }

        partial void OnShutdownChanged(TimeSpan value)
        {
            UpdateTotalTime();
        }

        private void UpdateTotalTime()
        {
            
            if (Shutdown > Startup)
            {
                TotalTime = Shutdown - Startup;
            }
            else
            {
                TotalTime = (TimeSpan.FromDays(1) - Startup) + Shutdown;
            }

        }

        [ObservableProperty]
        EngineModel? selectedEngine;
       
        


        [ObservableProperty]
        public ObservableCollection<string> chiefTechnicianList;

        [ObservableProperty]
        public ObservableCollection<string> techniciansList;

        [ObservableProperty]
        public ObservableCollection<string> externalPilotList;

        [ObservableProperty]
        public ObservableCollection<string> flightPurposeList = new()
        {
            "Training",
            "Experiment",
            "Demonstration",
            "Operational",
            "Other"
        };

        [ObservableProperty]
        public ObservableCollection<string> payloadList = new()
        {
            "DSP-HD",
            "DSP",
            "PW",
            "SAR",
            "Other"
        };

        [ObservableProperty]
        public ObservableCollection<EngineModel> engineList = new(){};

        [ObservableProperty]
        public ObservableCollection<string> cableDirList = new()
        {
            "South",
            "North" 
        };



        [ObservableProperty]
        FlightActivityModel? selectedFlight;

        public FlightActivityViewModel(IMessagingService messagingService)
        {
            _messagingService = messagingService;

            //EngineDatabaseService.RecalculateEnginesHours();
            FlightActivityDB = ReflectionHelper.GetTableName<FlightActivityModel>();
            InspectionsDB = ReflectionHelper.GetTableName<DailyInspection>();
            Flights = new();
            EngineList = new();
            Inspections = new();
            UpdateFlightsSerialNumbersAsync(FlightActivityDB);
            UpdateInspectionsSerialNumbersAsync(InspectionsDB);


        }
        public async Task UpdateFlightsSerialNumbersAsync(string dataBase)
        {
            try
            {
                int maxSerialNumber = await SerialNumberHelper.GetMaxSerialNumberAsync(dataBase);
                FlightActivityModel.UpdateFlightsSNCount(maxSerialNumber);

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Initilazation Failed");
            }
        }
        public async Task UpdateInspectionsSerialNumbersAsync(string dataBase)
        {
            try
            {
                int maxSerialNumber = await SerialNumberHelper.GetMaxSerialNumberAsync(dataBase);
                DailyInspection.UpdateInspectionSNCount(maxSerialNumber);

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Initilazation Failed");
            }
        }


        [RelayCommand]
        async Task RefreshData(string selectedUav)
        {
            try
            {
                localSelectedUAV = selectedUav;
                if (Flights != null)
                {
                    Flights.Clear();

                }
                if (Inspections != null)
                {
                    Inspections.Clear();
                    //CheckIfDIIsValid(Inspections);
                }
                await GetPageLists();

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Initilazation Failed");
            }
        }
        public async Task GetPageLists()
        {
            try
            {
                Flights = await DatabaseService.GetItemsByTailAsync<FlightActivityModel>(localSelectedUAV);
                Inspections = await DatabaseService.GetItemsByTailAsync<DailyInspection>(localSelectedUAV);
                TechniciansList = await TeamMembersDatabaseService.GetMembersAsync<TechnicianModel>();
                ChiefTechnicianList = await TeamMembersDatabaseService.GetMembersAsync<ChiefTechnicianModel>();
                ExternalPilotList = await TeamMembersDatabaseService.GetMembersAsync<ExternalPilotModel>();
                EngineList = await EngineDatabaseService.GetEnginesFromDB();
                if (EngineList != null)
                {
                    SelectedEngine = EngineList.FirstOrDefault();
                    EngineHoursAfterFlight = TimeSpanToStringConverter.ConvertTimeSpanToString(
                        TimeSpanToStringConverter.ConvertStringToTimeSpan(SelectedEngine.Hours) + TotalTime);
                }
                if (ChiefTechnicianList != null && ChiefTechnician == null)
                {
                    ChiefTechnician = ChiefTechnicianList.FirstOrDefault();
                }
                if (TechniciansList != null && RefuelName == null)
                {
                    RefuelName = TechniciansList.FirstOrDefault();
                }
                if (ExternalPilotList != null && ExPilotUavCheck == null)
                {
                    ExPilotUavCheck = ExternalPilotList.FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Initilazation Failed");
            }
        }

        [RelayCommand]
        async Task AddFlight()
        {
            try
            {
                OpenPermits = IntToBoolConverter.ConvertBoolToInt(OpenPermitsForUI);
                FlightActivityModel f = new(localSelectedUAV, FlightDate.ToString("dd-MM-yyyy"), FlightPurpose, TotalRefueling, RefuelName, FuelExpDate.ToString("dd-MM-yyyy"), Payload, Weight,
                       OpenPermits,SelectedEngine.Engine, SelectedEngine.Hours, EngineNextMaintenance, DrainAndSample, PrefUavClosure,
                       Uavgcs, ArrestDeployed, ArrestDir, ChiefTechnician, ExPilotUavCheck, ExPilotSteerCheck, RpmMax, RpmMin, Startup.ToString(),
                       Shutdown.ToString(), FuelAtLanding, TouchDowns, AfterLanding, UavHours, EngineHoursAfterFlight);

                await ViewModelHelper.AddModelInstance(f, Flights, (model) => FlightActivityModel.UpdateFlightsSNCount(model.FlightsSN));

                await EngineDatabaseService.RecalculateEnginesHours();

                Flights = await DatabaseService.GetItemsByTailAsync<FlightActivityModel>(localSelectedUAV);
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Adding Flight Activity Failed");
            }

        }

        [RelayCommand]
        async Task DeleteFlight()
        {
            try
            {
                if (SelectedFlight != null)
                {
                    await ViewModelHelper.DeleteModelInstance(SelectedFlight, Flights);
                    await EngineDatabaseService.RecalculateEnginesHours();
                }
                else
                {
                    _messagingService.ShowMessage("No flight has been selected", "Adding Flight Activity Failed");
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Deleting Flight Activity Failed");
            }
        }

        [RelayCommand]
        void UpdateForm()
        {
            try
            {
                if (SelectedFlight != null)
                {
                    DateTime d = DateTimeToStringConverter.ConvertStringToDate(SelectedFlight.FlightDate);
                    //(SelectedFlight.FlightDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
                    FlightDate = d;
                    FlightPurpose = SelectedFlight.FlightPurpose;
                    TotalRefueling = SelectedFlight.TotalRefueling;
                    RefuelName = SelectedFlight.RefuelName;
                    DateTime f = DateTimeToStringConverter.ConvertStringToDate(SelectedFlight.FuelExpDate);
                    //(SelectedFlight.FuelExpDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out f);
                    FuelExpDate = f;
                    Payload = SelectedFlight.Payload;
                    Weight = SelectedFlight.Weight;
                    OpenPermits = SelectedFlight.OpenPermits;
                    OpenPermitsForUI = IntToBoolConverter.ConvertIntToBool(OpenPermits);
                    SelectedEngine = EngineList.Where(x => x.Engine == SelectedFlight.EngineSN).FirstOrDefault();
                    EngineHours = SelectedFlight.EngineHours;
                    EngineNextMaintenance = SelectedFlight.EngineNextMaintenance;
                    DrainAndSample = SelectedFlight.DrainAndSample;
                    PrefUavClosure = SelectedFlight.PrefUavClosure;
                    Uavgcs = SelectedFlight.Uavgcs;
                    ArrestDeployed = SelectedFlight.ArrestDeployed;
                    ArrestDir = SelectedFlight.ArrestDir;
                    ChiefTechnician = SelectedFlight.ChiefTechnician;
                    ExPilotUavCheck = SelectedFlight.ExPilotUavCheck;
                    ExPilotSteerCheck = SelectedFlight.ExPilotSteerCheck;
                    RpmMax = SelectedFlight.RpmMax;
                    RpmMin = SelectedFlight.RpmMin;
                    Startup = TimeSpanToStringConverter.ConvertStringToTimeSpan(SelectedFlight.Startup);
                    Shutdown = TimeSpanToStringConverter.ConvertStringToTimeSpan(SelectedFlight.Shutdown);
                    FuelAtLanding = SelectedFlight.FuelAtLanding;
                    TouchDowns = SelectedFlight.TouchDowns;
                    AfterLanding = SelectedFlight.AfterLanding;
                    UavHours = SelectedFlight.UavHours;
                    EngineHoursAfterFlight = SelectedFlight.EngineHoursAfterFlight;

                    tempEngTime = TimeSpan.Parse(SelectedFlight.Shutdown) - TimeSpan.Parse(SelectedFlight.Startup);
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Updating Flight Activity Form Failed");
            }
        }

        [RelayCommand]
        async Task UpdateFlightActivity()
        {
            try
            {
                if (SelectedFlight != null)
                {
                    OpenPermits = IntToBoolConverter.ConvertBoolToInt(OpenPermitsForUI);
                    FlightActivityModel updatedFlight =
                        new(SelectedFlight.Tail, FlightDate.ToString("dd-MM-yyyy"), FlightPurpose,
                        TotalRefueling, RefuelName, FuelExpDate.ToString("dd-MM-yyyy"), Payload, Weight,
                        OpenPermits, SelectedEngine.Engine, SelectedEngine.Hours, EngineNextMaintenance,
                        DrainAndSample, PrefUavClosure, Uavgcs, ArrestDeployed, ArrestDir, ChiefTechnician,
                        ExPilotUavCheck, ExPilotSteerCheck, RpmMax, RpmMin, Startup.ToString(), Shutdown.ToString(),
                        FuelAtLanding, TouchDowns, AfterLanding, UavHours, EngineHoursAfterFlight);
                    updatedFlight.FlightsSN = SelectedFlight.FlightsSN;

                    await ViewModelHelper.UpdateModelInstance(SelectedFlight, updatedFlight, Flights);

                    await EngineDatabaseService.RecalculateEnginesHours();
                    SelectedFlight = null;
                }
                else
                {
                    _messagingService.ShowMessage("please select flight to update", "Updating Flight Activity Form Failed");
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Updating Flight Activity Failed");
            }
        }
        


        [RelayCommand]
        async Task UpdateInspection()
        {
            try
            {
                DailyInspection i = new(localSelectedUAV, InspectionDate.ToString("dd-MM-yyyy"), InspectionTime.ToString(), InspectionTechnician); ;
                await ViewModelHelper.AddModelInstance(i, Inspections, (model) => DailyInspection.UpdateInspectionSNCount(model.InspectionsSN));
                //DatabaseService.InsertService(i);
                ////InspectionServer.ServerAddInspection(i);
                //Inspections.Insert(0,i);
                CheckIfDIIsValid(Inspections);
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Updating Inspection Failed");
            }
        }



        [ObservableProperty]
        public static Microsoft.Maui.Graphics.Color isDIValidColor;
        [ObservableProperty]
        public static string isDIValidString;

        [ObservableProperty]
        public static bool isNotValid = true;

        [ObservableProperty]
        public static bool isValid = false;

        public void CheckIfDIIsValid(ObservableCollection<DailyInspection> list)
        {
            try
            {
                isDIValidColor = new Microsoft.Maui.Graphics.Color(1.0f, 0.0f, 0.0f);
                isDIValidString = "DI is not valid";
                foreach (DailyInspection d in list)
                {
                    DateTime date = DateTimeToStringConverter.ConvertStringToDate(d.InspectionDate);
                    TimeSpan time = TimeSpanToStringConverter.ConvertStringToTimeSpan(d.InspectionTime);
                    {
                        DateTime inspectionDateTime = new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, 0);
                        if ((DateTime.Now - inspectionDateTime).TotalDays <= 3)
                        {
                            isDIValidColor = new Microsoft.Maui.Graphics.Color(0.0f, 1.0f, 0.0f);
                            isDIValidString = "DI is Valid";
                            isNotValid = false;
                            isValid = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                _messagingService.ShowMessage(ex.Message, "Checking DI Validation Failed");
            }
        }

    }
}
