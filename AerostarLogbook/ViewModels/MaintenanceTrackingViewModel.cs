using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AerostarLogbook.Models;
using AerostarLogbook.Models.TeamMembers;
using System.Configuration;
using AerostarLogbook.Services;
using AerostarLogbook.Services.Helpers;
using AerostarLogbook.Services.DatabaseMain;
using AerostarLogbook.Services.DatabaseMiscellaneous;
using System.Globalization;
using AerostarLogbook.Interfaces;
using AerostarLogbook.Resources.Converters;


namespace AerostarLogbook.ViewModels
{
    public partial class MaintenanceTrackingViewModel : ObservableObject
    {
        private readonly IMessagingService _messagingService;

        private string MaintenanceTrackingDB { get; set; }

        private string localSelectedUAV;


        [ObservableProperty]
        public ObservableCollection<MaintenanceTrackingModel> maintenanceTrackings;

        [ObservableProperty]
        DateTime openDate = DateTime.Today;
        [ObservableProperty]
        string openTime = "12:30";
        [ObservableProperty]
        string openByName = "Tamir";
        [ObservableProperty]
        string faultDescription = "All is a mess";
        [ObservableProperty]
        string correctiveAction = "All is set";
        [ObservableProperty]
        string fodCheck = "no";
        [ObservableProperty]
        DateTime closeDate = DateTime.Today;
        [ObservableProperty]
        string closeTime = "13:00";
        [ObservableProperty]
        string closeTechnician = "Roni";
        [ObservableProperty]
        string closeChiefTechnician = "Tamir";


        [ObservableProperty]
        MaintenanceTrackingModel? selectedMaintenance;
        [ObservableProperty]
        public ObservableCollection<string> technicianList;
        [ObservableProperty]
        public ObservableCollection<string> chiefTechnicianList;

        public MaintenanceTrackingViewModel(IMessagingService messagingService)
        {
            _messagingService = messagingService;

            MaintenanceTrackingDB = ReflectionHelper.GetTableName<MaintenanceTrackingModel>();
            MaintenanceTrackings = new();
            TechnicianList = new();
            ChiefTechnicianList = new();
            UpdateEventSerialNumberAsync(MaintenanceTrackingDB);
        }

        private async Task UpdateEventSerialNumberAsync(string dataBase)
        {
            try
            {
                int maxSerialNumber = await SerialNumberHelper.GetMaxSerialNumberAsync(dataBase);
                MaintenanceTrackingModel.UpdateMaintenanceTrackingSNCount(maxSerialNumber);
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
                if (MaintenanceTrackings != null)
                {
                    MaintenanceTrackings.Clear();
                }
                if (ChiefTechnicianList != null)
                {
                    ChiefTechnicianList.Clear();
                }
                if (TechnicianList != null)
                {
                    TechnicianList.Clear();
                }
                await GetPageLists();

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Initilazation Failed");
            }
        }
        private async Task GetPageLists()
        {
            try
            {
                MaintenanceTrackings = await DatabaseService.GetItemsByTailAsync<MaintenanceTrackingModel>(localSelectedUAV);
                ChiefTechnicianList = await TeamMembersDatabaseService.GetMembersAsync<ChiefTechnicianModel>();
                TechnicianList = await TeamMembersDatabaseService.GetMembersAsync<TechnicianModel>();
                if (ChiefTechnicianList != null && CloseChiefTechnician == null)
                {
                    CloseChiefTechnician = ChiefTechnicianList.FirstOrDefault();
                }
                if (TechnicianList != null && CloseTechnician == null)
                {
                    CloseTechnician = TechnicianList.FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Initilazation Failed");
            }
        }

        [RelayCommand]
        async Task AddMaintenance()
        {
            try
            {
                MaintenanceTrackingModel m = new(MainPageViewModel.staticSelectedUAV,
            OpenDate.ToString("dd-MM-yyyy"), OpenTime.ToString(), OpenByName, FaultDescription);
                await ViewModelHelper.AddModelInstance(m, MaintenanceTrackings, (model) => MaintenanceTrackingModel.UpdateMaintenanceTrackingSNCount(model.MaintenanceTrackingSN));
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Adding Maintenance Tracking Failed");
            }
        }

        [RelayCommand]
        async Task CloseMaintenance()
        {
            try
            {
                if (SelectedMaintenance != null)
                {
                    MaintenanceTrackingModel m = SelectedMaintenance;
                    m.CloseMaintenance(CorrectiveAction, FodCheck, CloseDate.ToString("dd-MM-yyyy"), CloseTime.ToString(), CloseTechnician, CloseChiefTechnician);
                    await ViewModelHelper.UpdateModelInstance(SelectedMaintenance, m, MaintenanceTrackings);
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Closing Maintenance Tracking Failed");
            }
        }

        [RelayCommand]
        async Task DeleteMaintenance()
        {
            try
            {
                if (SelectedMaintenance != null)
                {
                    await ViewModelHelper.DeleteModelInstance(SelectedMaintenance, MaintenanceTrackings);
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Deleting Maintenance Tracking Failed");
            }
        }

        [RelayCommand]
        void UpdateMaintenanceForm()
        {
            try
            {
                if (SelectedMaintenance != null)
                {
                    DateTime opDate = DateTimeToStringConverter.ConvertStringToDate(SelectedMaintenance.OpenDate);
                    //(SelectedMaintenance.OpenDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime opDate);
                    OpenDate = opDate;
                    OpenTime = SelectedMaintenance.OpenTime;
                    OpenByName = SelectedMaintenance.OpenByName;
                    FaultDescription = SelectedMaintenance.FaultDescription;
                    CorrectiveAction = SelectedMaintenance.CorrectiveAction;
                    FodCheck = SelectedMaintenance.FodCheck;
                    DateTime clDate = DateTimeToStringConverter.ConvertStringToDate(SelectedMaintenance.CloseDate);
                    //(SelectedMaintenance.CloseDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime clDate);
                    CloseDate = clDate;
                    CloseTime = SelectedMaintenance.CloseTime;
                    CloseTechnician = SelectedMaintenance.CloseTechnician;
                    CloseChiefTechnician = SelectedMaintenance.CloseChiefTechnician;
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Updating Maintenance Tracking Form Failed");
            }
        }

        [RelayCommand]
        async Task UpdateMaintenance()
        {
            try
            {
                if (SelectedMaintenance != null)
                {
                    MaintenanceTrackingModel updatedMaintenance = new(
                        localSelectedUAV,
                        OpenDate.ToString("dd-MM-yyyy"), OpenTime, OpenByName,
                        FaultDescription
                    );
                    updatedMaintenance.MaintenanceTrackingSN = SelectedMaintenance.MaintenanceTrackingSN;
                    await ViewModelHelper.UpdateModelInstance(SelectedMaintenance, updatedMaintenance, MaintenanceTrackings);

                    SelectedMaintenance = null;
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Updating Maintenance Tracking Failed");
            }
        }

    }
}
