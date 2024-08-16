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
using AerostarLogbook.Services.Helpers;
using AerostarLogbook.Services.DatabaseMain;
using Microsoft.Extensions.Logging;
using AerostarLogbook.Interfaces;
using AerostarLogbook.Resources.Converters;


namespace AerostarLogbook.ViewModels
{
    public partial class SoftwareConfigurationViewModel : ObservableObject
    {
        private readonly IMessagingService _messagingService;

        private string SoftwareConfDB { get; set; }

        [ObservableProperty]
        public ObservableCollection<SoftwareConfigurationModel> softwareConfigurationList;

        private string localSelectedUAV;

        [ObservableProperty]
        string mainVersion = "6.2.1.149";
        [ObservableProperty]
        string iNS1Version = "6.2.1.233";
        [ObservableProperty]
        string iNS2Version = "6.2.1.233";
        [ObservableProperty]
        string aDIVersion = "6.2.1.289 Tiger";
        [ObservableProperty]
        string changeDescription = "Write here the change description...";
        [ObservableProperty]
        DateTime sWConfDate = DateTime.Today;
        [ObservableProperty]
        string sWConfTechnician = "Roni Malmud";
        [ObservableProperty]
        string sWConfChiefTechnician = "Tamir Nudelman";

        [ObservableProperty]
        SoftwareConfigurationModel? selectedSWConf;
        [ObservableProperty]
        public ObservableCollection<string> technicianList;
        [ObservableProperty]
        public ObservableCollection<string> chiefTechnicianList;

        public SoftwareConfigurationViewModel(IMessagingService messagingService)
        {
            _messagingService = messagingService;

            SoftwareConfDB = ReflectionHelper.GetTableName<SoftwareConfigurationModel>();
            SoftwareConfigurationList = new();      
            TechnicianList = new();
            ChiefTechnicianList = new();
            UpdateEventSerialNumberAsync(SoftwareConfDB);
        }

        private async Task UpdateEventSerialNumberAsync(string dataBase)
        {
            try
            {
                int maxSerialNumber = await SerialNumberHelper.GetMaxSerialNumberAsync(dataBase);
                UnusualEventModel.UpdateEventSNCount(maxSerialNumber);
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
                if (SoftwareConfigurationList != null)
                {
                    SoftwareConfigurationList.Clear();
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
                SoftwareConfigurationList = await DatabaseService.GetItemsByTailAsync<SoftwareConfigurationModel>(localSelectedUAV);
                ChiefTechnicianList = await TeamMembersDatabaseService.GetMembersAsync<ChiefTechnicianModel>();
                TechnicianList = await TeamMembersDatabaseService.GetMembersAsync<TechnicianModel>();
                if (ChiefTechnicianList != null && SWConfChiefTechnician == null)
                {
                    SWConfChiefTechnician = ChiefTechnicianList.FirstOrDefault();
                }
                if (TechnicianList != null && SWConfTechnician == null)
                {
                    SWConfTechnician = TechnicianList.FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Initilazation Failed");
            }
        }


        [RelayCommand]
        async Task AddSWConf()
        {
            try
            {
                SoftwareConfigurationModel s = new(MainPageViewModel.staticSelectedUAV,
                    MainVersion, INS1Version, INS2Version, ADIVersion, ChangeDescription,
                    SWConfDate.ToString("dd-MM-yyyy"), SWConfTechnician, SWConfChiefTechnician);
                await ViewModelHelper.AddModelInstance(s, SoftwareConfigurationList, (model) => SoftwareConfigurationModel.UpdateSWConfSNCount(model.SoftwareConfigurationSN));
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Adding Software Configuration Failed");
            }
        }

        [RelayCommand]
        async Task DeleteSWConf()
        {
            try
            {
                if (SelectedSWConf != null)
                {
                    await ViewModelHelper.DeleteModelInstance(SelectedSWConf, SoftwareConfigurationList);
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Deleting Software Configuration Failed");
            }
        }

        [RelayCommand]
        void UpdateSoftwareConfigurationForm()
        {
            try
            {
                if (SelectedSWConf != null)
                {
                    MainVersion = SelectedSWConf.MainVersion;
                    INS1Version = SelectedSWConf.INS1Version;
                    INS2Version = SelectedSWConf.INS2Version;
                    ADIVersion = SelectedSWConf.ADIVersion;
                    ChangeDescription = SelectedSWConf.ChangeDescription;
                    DateTime confDate = DateTimeToStringConverter.ConvertStringToDate(SelectedSWConf.SWConfDate);
                    SWConfDate = confDate;

                    SWConfTechnician = SelectedSWConf.SWConfTechnician;
                    SWConfChiefTechnician = SelectedSWConf.SWConfChiefTechnician;
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Updating Software Configuration Form Failed");
            }
        }

        [RelayCommand]
        async Task UpdateSoftwareConfiguration()
        {
            try
            {
                if (SelectedSWConf != null)
                {
                    SoftwareConfigurationModel updatedSWConf = new(
                        localSelectedUAV,
                        MainVersion, INS1Version, INS2Version, ADIVersion,
                        ChangeDescription, SWConfDate.ToString("dd-MM-yyyy"),
                        SWConfTechnician, SWConfChiefTechnician
                    );
                    updatedSWConf.SoftwareConfigurationSN = SelectedSWConf.SoftwareConfigurationSN;
                    await ViewModelHelper.UpdateModelInstance(SelectedSWConf, updatedSWConf, SoftwareConfigurationList);

                    SelectedSWConf = null;
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Updating Software Configuration Failed");
            }
        }

    }
}
