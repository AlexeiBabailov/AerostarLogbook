using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AerostarLogbook.Models;
using AerostarLogbook.Models.TeamMembers;
using AerostarLogbook.Services;
using AerostarLogbook.Services.DatabaseMain;
using AerostarLogbook.Services.Helpers;
using Microsoft.Extensions.Logging;
using AerostarLogbook.Interfaces;
using AerostarLogbook.Resources.Converters;




namespace AerostarLogbook.ViewModels
{
    public partial class ServiceBulletinViewModel : ObservableObject
    {
        private readonly IMessagingService _messagingService;

        private string BulletinDB { get; set; }

        private string localSelectedUAV;

        [ObservableProperty]
        public ObservableCollection<ServiceBulletinModel> bulletinList;

        [ObservableProperty]
        int serviceBulletinNumber = 0;
        [ObservableProperty]
        DateTime date = DateTime.Today;
        [ObservableProperty]
        string description = "Service Bulletin Description Here...";
        [ObservableProperty]
        string technician = "Roni Malmud";
        [ObservableProperty]
        string chiefTechnician = "Tamir Nudelman";
        [ObservableProperty]
        ServiceBulletinModel? selectedBulletin;

        [ObservableProperty]
        public ObservableCollection<string> chiefTechnicianList;

        [ObservableProperty]
        public ObservableCollection<string> technicianList;
   

        public ServiceBulletinViewModel(IMessagingService messagingService)
        {
            _messagingService = messagingService;

            BulletinDB = ReflectionHelper.GetTableName<ServiceBulletinModel>();
            BulletinList = new();
            TechnicianList = new();
            ChiefTechnicianList = new();
            UpdateEventSerialNumberAsync(BulletinDB);
        }
        public async Task UpdateEventSerialNumberAsync(string dataBase)
        {
            try
            {
                int maxSerialNumber = await SerialNumberHelper.GetMaxSerialNumberAsync(dataBase);
                ServiceBulletinModel.UpdateBulletinSNCount(maxSerialNumber);

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
                if (BulletinList != null)
                {
                    BulletinList.Clear();
                }
                if (TechnicianList != null)
                {
                    TechnicianList.Clear();
                }
                if (ChiefTechnicianList != null)
                {
                    ChiefTechnicianList.Clear();
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
                BulletinList = await DatabaseService.GetItemsByTailAsync<ServiceBulletinModel>(localSelectedUAV);
                ChiefTechnicianList = await TeamMembersDatabaseService.GetMembersAsync<ChiefTechnicianModel>();
                TechnicianList = await TeamMembersDatabaseService.GetMembersAsync<TechnicianModel>();
                if (ChiefTechnicianList != null && ChiefTechnician == null)
                {
                    ChiefTechnician = ChiefTechnicianList.FirstOrDefault();
                }
                if (TechnicianList != null && Technician == null)
                {
                    Technician = TechnicianList.FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Initilazation Failed");
            }
        }

        [RelayCommand]
        async Task AddServiceBulletin()
        {
            try
            {
                ServiceBulletinModel b = new(MainPageViewModel.staticSelectedUAV, ServiceBulletinNumber,
            Date.ToString("dd-MM-yyyy"), Description, Technician, ChiefTechnician);
                await ViewModelHelper.AddModelInstance(b, BulletinList, (model) => ServiceBulletinModel.UpdateBulletinSNCount(model.ServiceBulletinSN));

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Adding Service Bulletin Failed");
            }
        }

        [RelayCommand]
        async Task DeleteServiceBulletin()
        {
            try
            {
                if (SelectedBulletin != null)
                {
                    await ViewModelHelper.DeleteModelInstance(SelectedBulletin, BulletinList);
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Deleting Service Bulletin Failed");
            }
        }

        [RelayCommand]
        void UpdateServiceBulletinForm()
        {
            try
            {
                if (SelectedBulletin != null)
                {
                    ServiceBulletinNumber = SelectedBulletin.ServiceBulletinNumber;
                    DateTime sbDate = DateTimeToStringConverter.ConvertStringToDate(SelectedBulletin.Date);
                    //(SelectedBulletin.Date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime sbDate);
                    Date = sbDate;

                    Description = SelectedBulletin.Description;
                    Technician = SelectedBulletin.Technician;
                    ChiefTechnician = SelectedBulletin.ChiefTechnician;
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Updating Service Bulletin Form Failed");
            }
        }

        [RelayCommand]
        async Task UpdateServiceBulletin()
        {
            try
            {
                if (SelectedBulletin != null)
                {
                    ServiceBulletinModel updatedBulletin = new(
                        localSelectedUAV,
                        ServiceBulletinNumber,
                        Date.ToString("dd-MM-yyyy"),
                        Description,
                        Technician,
                        ChiefTechnician
                    );
                    updatedBulletin.ServiceBulletinSN = SelectedBulletin.ServiceBulletinSN;
                    await ViewModelHelper.UpdateModelInstance(SelectedBulletin, updatedBulletin, BulletinList);

                    SelectedBulletin = null;
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Updating Service Bulletin Failed");
            }
        }



    }
}
