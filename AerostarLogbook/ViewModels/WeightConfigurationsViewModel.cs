using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AerostarLogbook.Models;
using AerostarLogbook.Models.TeamMembers;
using AerostarLogbook.Services;
using AerostarLogbook.Services.DatabaseMain;
using AerostarLogbook.Services.Helpers;
using AerostarLogbook.Interfaces;
using AerostarLogbook.Resources.Converters;



namespace AerostarLogbook.ViewModels
{
    public partial class WeightConfigurationsViewModel : ObservableObject
    {
        private readonly IMessagingService _messagingService;

        private string ConfDB { get; set; }
        private string localSelectedUAV;
        [ObservableProperty]
        public  ObservableCollection<WeightConfigurationsModel> configurations;

        [ObservableProperty]
        string payloadType = "PW";
        [ObservableProperty]
        string description = "ATOl + sdsda";
        [ObservableProperty]
        string noseWeight = "10.5";
        [ObservableProperty]
        string tailWeight = "0";
        [ObservableProperty]
        string uavAngle = "6.1";
        [ObservableProperty]
        DateTime date = DateTime.Today;
        [ObservableProperty]
        string technician = "Roei Cohen";
        [ObservableProperty]
        string chiefTechnician = "Yoav Gershovitz";
        
        [ObservableProperty]
        WeightConfigurationsModel? selectedConf;

        [ObservableProperty]
        public ObservableCollection<string> chiefTechnicianList;

        [ObservableProperty]
        public ObservableCollection<string> technicianList;

        [ObservableProperty]
        public ObservableCollection<string> payloadList = new()
        {
            "DSP-HD",
            "DSP",
            "PW",
            "SAR",
            "Other"
        };

        public WeightConfigurationsViewModel(IMessagingService messagingService)
        {
            _messagingService = messagingService;

            ConfDB = ReflectionHelper.GetTableName<WeightConfigurationsModel>();
            configurations = new();
            UpdateCongSerialNumberAsync(ConfDB);

        }

        public async Task UpdateCongSerialNumberAsync(string dataBase)
        {
            try
            {
                int maxSerialNumber = await SerialNumberHelper.GetMaxSerialNumberAsync(dataBase);
                WeightConfigurationsModel.UpdateConfSNCount(maxSerialNumber);
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Initilazation Failed");
            }
        }

        [RelayCommand]
        async void RefreshData(string selectedUav)
        {
            try
            {
                localSelectedUAV = selectedUav;
                if (Configurations != null)
                {
                    Configurations.Clear();
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
                Configurations = await DatabaseService.GetItemsByTailAsync<WeightConfigurationsModel>(localSelectedUAV);
                TechnicianList = await TeamMembersDatabaseService.GetMembersAsync<TechnicianModel>();
                ChiefTechnicianList = await TeamMembersDatabaseService.GetMembersAsync<ChiefTechnicianModel>();
                if (Technician == null && TechnicianList != null)
                {
                    Technician = TechnicianList.FirstOrDefault();
                }
                if (ChiefTechnician == null && ChiefTechnicianList != null)
                {
                    ChiefTechnician = ChiefTechnicianList.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Initilazation Failed");
            }
        }

        [RelayCommand]
        async Task AddConfiguration()
        {
            try
            {
                WeightConfigurationsModel c = new(localSelectedUAV,
            PayloadType, Description, NoseWeight, TailWeight, UavAngle,
            Date.ToString("dd-MM-yyyy"), Technician, ChiefTechnician);
                await ViewModelHelper.AddModelInstance(c, Configurations, (model) => WeightConfigurationsModel.UpdateConfSNCount(model.WeightConfigurationsSN));

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Adding Configuration Failed");
            }
        }



        [RelayCommand]
        async Task DeleteConfiguration()
        {
            try
            {
                if (SelectedConf != null)
                {
                    await ViewModelHelper.DeleteModelInstance(SelectedConf, Configurations);
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Deleting Configuration Failed");
            }
        }

        [RelayCommand]
        void UpdateConfigurationForm()
        {
            try
            {
                if (SelectedConf != null)
                {
                    PayloadType = SelectedConf.PayloadType;
                    Description = SelectedConf.Description;
                    NoseWeight = SelectedConf.NoseWeight;
                    TailWeight = SelectedConf.TailWeight;
                    UavAngle = SelectedConf.UavAngle;
                    DateTime plDate = DateTimeToStringConverter.ConvertStringToDate(SelectedConf.Date);
                    //(SelectedConf.Date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime plDate);
                    Date = plDate;

                    Technician = SelectedConf.Technician;
                    ChiefTechnician = SelectedConf.ChiefTechnician;
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Updating Configuration Form Failed");
            }
        }

        [RelayCommand]
        async Task UpdateConfigurationDetails()
        {
            try
            {
                if (SelectedConf != null)
                {
                    WeightConfigurationsModel updatedConf = new(
                        localSelectedUAV,
                        PayloadType, Description, NoseWeight, TailWeight, UavAngle,
                        Date.ToString("dd-MM-yyyy"),
                        Technician, ChiefTechnician
                    );
                    updatedConf.WeightConfigurationsSN = SelectedConf.WeightConfigurationsSN;
                    ViewModelHelper.UpdateModelInstance(SelectedConf, updatedConf, Configurations);

                    SelectedConf = null;
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Updating Configuration Failed");
            }
        }

    }
}
