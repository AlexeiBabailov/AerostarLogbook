using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SQLite;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AerostarLogbook.Services;
using AerostarLogbook.Services.DatabaseMain;
using AerostarLogbook.Services.Helpers;
using AerostarLogbook.Models;
using AerostarLogbook.Models.TeamMembers;
using AerostarLogbook.Interfaces;
using AerostarLogbook.Resources.Converters;


namespace AerostarLogbook.ViewModels
{
    public partial class UnusualEventViewModel : ObservableObject
    {
        private readonly IMessagingService _messagingService;

        private string EventsDB { get; set; }

        private string localSelectedUAV;

        [ObservableProperty]
        public ObservableCollection<UnusualEventModel> events;

        [ObservableProperty]
        DateTime date = DateTime.Today;
        [ObservableProperty]
        string description = "Event Description Here...";
        [ObservableProperty]
        string chiefTechnician = "Tamir Nudelman";
        
        [ObservableProperty]
        UnusualEventModel? selectedEvent;

        [ObservableProperty]
        public ObservableCollection<string> chiefTechnicianList;

        public  UnusualEventViewModel(IMessagingService messagingService)
        {
            _messagingService = messagingService;

            EventsDB = ReflectionHelper.GetTableName<UnusualEventModel>();
            Events = new();
            ChiefTechnicianList = new();
            UpdateEventSerialNumberAsync(EventsDB);
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
                if (Events != null)
                {
                    Events.Clear();
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
                Events = await DatabaseService.GetItemsByTailAsync<UnusualEventModel>(localSelectedUAV);
                ChiefTechnicianList = await TeamMembersDatabaseService.GetMembersAsync<ChiefTechnicianModel>();
                if (ChiefTechnicianList != null && ChiefTechnician == null)
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
        async Task AddEvent()
        {
            try
            {
                UnusualEventModel e = new(localSelectedUAV, Date.ToString("dd-MM-yyyy"), Description, ChiefTechnician);
                await ViewModelHelper.AddModelInstance(e, Events, (model) => UnusualEventModel.UpdateEventSNCount(model.UnusualEventSN));
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Adding Unusual Event Failed");
            }
        }

        [RelayCommand]
        async Task DeleteEvent()
        {
            try
            {
                if (SelectedEvent != null)
                {
                    await ViewModelHelper.DeleteModelInstance(SelectedEvent, Events);
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Deleting Unusual Event Failed");
            }
        }

        [RelayCommand]
        void UpdateEventForm()
        {
            try
            {
                if (SelectedEvent != null)
                {
                    DateTime evDate = DateTimeToStringConverter.ConvertStringToDate(SelectedEvent.Date);
                    //(SelectedEvent.Date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime evDate);
                    Date = evDate;
                    Description = SelectedEvent.Description;
                    ChiefTechnician = SelectedEvent.ChiefTechnician;
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Updating Unusual Event Form Failed");
            }
        }

        [RelayCommand]
        async Task UpdateEventDetails()
        {
            try
            {
                if (SelectedEvent != null)
                {
                    UnusualEventModel updatedEvent = new(
                        localSelectedUAV,
                        Date.ToString("dd-MM-yyyy"),
                        Description,
                        ChiefTechnician
                    );
                    updatedEvent.UnusualEventSN = SelectedEvent.UnusualEventSN;
                    await ViewModelHelper.UpdateModelInstance(SelectedEvent, updatedEvent, Events);

                    SelectedEvent = null;
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Updating Unusual Event Failed");
            }
        }

    }
}
