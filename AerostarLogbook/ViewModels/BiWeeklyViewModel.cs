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
using AerostarLogbook.Interfaces;
using AerostarLogbook.Resources.Converters;
using AerostarLogbook.Services.DatabaseMain;

namespace AerostarLogbook.ViewModels
{
    public partial class BiWeeklyViewModel : ObservableObject
    {
        private readonly IMessagingService _messagingService;

        private string BiWeeklyDB { get; set; }

        private string localSelectedUAV;

        [ObservableProperty]
        public  ObservableCollection<BiWeeklyModel> biWeeklyList;

        [ObservableProperty]
        DateTime expiryDate = DateTime.Today.AddDays(14);
        [ObservableProperty]
        DateTime doneOnDate = DateTime.Today;
        [ObservableProperty]
        string technician;

        [ObservableProperty]
        BiWeeklyModel? selectedBiWeekly;

        [ObservableProperty]
        public ObservableCollection<string> technicianList;


        public BiWeeklyViewModel(IMessagingService messagingService)
        {
            BiWeeklyDB = ReflectionHelper.GetTableName<BiWeeklyModel>();
            BiWeeklyList = new();
            UpdateBiWeeklySerialNumberAsync(BiWeeklyDB);
            _messagingService = messagingService;

        }

        public async Task UpdateBiWeeklySerialNumberAsync(string dataBase)
        {
            try
            {
                int maxSerialNumber = await SerialNumberHelper.GetMaxSerialNumberAsync(dataBase);
                BiWeeklyModel.UpdateBiWeeklySNCount(maxSerialNumber);
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
                if (BiWeeklyList != null)
                {
                    BiWeeklyList.Clear();
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

        public async Task GetPageLists()
        {
            try
            {
                BiWeeklyList = await DatabaseService.GetItemsByTailAsync<BiWeeklyModel>(localSelectedUAV);
                TechnicianList = await TeamMembersDatabaseService.GetMembersAsync<TechnicianModel>();
                if (Technician == null && TechnicianList != null) 
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
        async Task AddBiWeekly()
        {
            try
            {
                BiWeeklyModel b = new(localSelectedUAV,
            ExpiryDate.ToString("dd-MM-yyyy"),
            DoneOnDate.ToString("dd-MM-yyyy"),
            Technician);
                await ViewModelHelper.AddModelInstance(b, BiWeeklyList, (model) => BiWeeklyModel.UpdateBiWeeklySNCount(model.BiWeeklySN));

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Add Operation Failed");
            }        
        }

        [RelayCommand]
        async Task DeleteBiWeekly()
        {
            try
            {
                if (SelectedBiWeekly != null)
                {
                    await ViewModelHelper.DeleteModelInstance(SelectedBiWeekly, BiWeeklyList);
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Delete Operation Failed");
            }
        }

        [RelayCommand]
        void UpdateForm()
        {
            try
            {
                if (SelectedBiWeekly != null)
                {
                    DateTime exdate = DateTimeToStringConverter.ConvertStringToDate(SelectedBiWeekly.ExpiryDate);
                    //(SelectedBiWeekly.ExpiryDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime exdate);
                    ExpiryDate = exdate;
                    DateTime donedate = DateTimeToStringConverter.ConvertStringToDate(SelectedBiWeekly.DoneOnDate);
                    //(SelectedBiWeekly.DoneOnDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime donedate);
                    DoneOnDate = donedate;
                    Technician = SelectedBiWeekly.Technician;
                }
            }
            catch (Exception ex) 
            { 
                _messagingService.ShowMessage(ex.Message, "Updating Form Operation Failed");  
            }
        }

        [RelayCommand]
        async Task UpdateBiWeekly()
        {
            try
            {
                if (SelectedBiWeekly != null)
                {
                    BiWeeklyModel updatedBiWeekly =
                       new(localSelectedUAV,
                            ExpiryDate.ToString("dd-MM-yyyy"),
                            DoneOnDate.ToString("dd-MM-yyyy"),
                            Technician);
                    updatedBiWeekly.BiWeeklySN = SelectedBiWeekly.BiWeeklySN;
                    await ViewModelHelper.UpdateModelInstance(SelectedBiWeekly, updatedBiWeekly, BiWeeklyList);

                    SelectedBiWeekly = null;
                }
                else
                {
                    _messagingService.ShowMessage("No Bi-Weekly Selected", "Updating Bi-Weekly Failed");
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Updating Operation Failed");
            }
        }





    }
}
