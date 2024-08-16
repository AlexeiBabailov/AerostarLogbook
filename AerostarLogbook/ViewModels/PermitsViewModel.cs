using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AerostarLogbook.Models;
using AerostarLogbook.Services;
using AerostarLogbook.Services.Helpers;
using AerostarLogbook.Services.DatabaseMain;
using AerostarLogbook.Services.DatabaseMiscellaneous;
using AerostarLogbook.Models.TeamMembers;
using System.Globalization;
using Microsoft.Extensions.Logging;
using AerostarLogbook.Interfaces;
using AerostarLogbook.Resources.Converters;

namespace AerostarLogbook.ViewModels
{

    public partial class PermitsViewModel : ObservableObject
    {
        private readonly IMessagingService _messagingService;

        private string PermitsDB { get; set; }

        private string localSelectedUAV;


        [ObservableProperty]
        public ObservableCollection<PermitsModel> permits;

        [ObservableProperty]
        DateTime openDate = DateTime.Today;
        [ObservableProperty]
        string openBPR = "B";
        [ObservableProperty]
        string description = "Engine Problem";
        [ObservableProperty]
        string expiryDateOrMaxHours = "10 Hours";
        [ObservableProperty]
        string approvedBy = "Daniel";
        [ObservableProperty]
        string openChiefTechnician = "Tamir";
        [ObservableProperty]
        DateTime closeDate = DateTime.Today;
        [ObservableProperty]
        string closeBPR = "Tamir";
        [ObservableProperty]
        string closeTechnician = "Tamir";
        
        [ObservableProperty]
        PermitsModel? selectedPermit;

        [ObservableProperty]
        public ObservableCollection<string> chiefTechnicianList;

        [ObservableProperty]
        public ObservableCollection<string> technicianList;

        public PermitsViewModel(IMessagingService messagingService)
        {
            _messagingService = messagingService;

            PermitsDB = ReflectionHelper.GetTableName<PermitsModel>();
            Permits = new();
            TechnicianList = new();
            ChiefTechnicianList = new();
            UpdateEventSerialNumberAsync(PermitsDB);


        }

        public async Task UpdateEventSerialNumberAsync(string dataBase)
        {
            try
            {
                int maxSerialNumber = await SerialNumberHelper.GetMaxSerialNumberAsync(dataBase);
                PermitsModel.UpdatePermitsSNCount(maxSerialNumber);

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
                if (Permits != null)
                {
                    Permits.Clear();
                }
                if (TechnicianList != null)
                {
                    TechnicianList.Clear();
                }
                if (ChiefTechnicianList != null)
                {
                    ChiefTechnicianList.Clear();
                }
                GetPageLists();

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
                Permits = await DatabaseService.GetItemsByTailAsync<PermitsModel>(localSelectedUAV);
                ChiefTechnicianList = await TeamMembersDatabaseService.GetMembersAsync<ChiefTechnicianModel>();
                TechnicianList = await TeamMembersDatabaseService.GetMembersAsync<TechnicianModel>();
                if (ChiefTechnicianList != null && OpenChiefTechnician == null)
                {
                    OpenChiefTechnician = ChiefTechnicianList.FirstOrDefault();
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

        async Task AddPermit()
        {
            try
            {
                PermitsModel p = new(MainPageViewModel.staticSelectedUAV,
                    OpenDate.ToString("dd-MM-yyyy"), OpenBPR, Description,
                    ExpiryDateOrMaxHours, ApprovedBy, OpenChiefTechnician);
                await ViewModelHelper.AddModelInstance(p, Permits, (model) => PermitsModel.UpdatePermitsSNCount(model.PermitsSN));
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Adding Permit Failed");
            }
        }

        [RelayCommand]
        async Task ClosePermit()
        {
            try
            {
                if (SelectedPermit != null)
                {
                    PermitsModel p = SelectedPermit;
                    p.ClosePermit(CloseDate.ToString("dd-MM-yyyy"), CloseBPR, CloseTechnician);
                    await ViewModelHelper.UpdateModelInstance(SelectedPermit, p, Permits);
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Closing Permit Failed");
            }
        }

        [RelayCommand]
        async Task DeletePermit()
        {
            try
            {
                if (SelectedPermit != null)
                {
                    await ViewModelHelper.DeleteModelInstance(SelectedPermit, Permits);
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Deleting Permit Failed");
            }
        }

        [RelayCommand]
        void UpdatePermitForm()
        {
            try
            {
                if (SelectedPermit != null)
                {
                    DateTime opDate = DateTimeToStringConverter.ConvertStringToDate(SelectedPermit.OpenDate);
                    //(SelectedPermit.OpenDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime opDate);
                    OpenDate = opDate;
                    OpenBPR = SelectedPermit.OpenBPR;
                    Description = SelectedPermit.Description;
                    ExpiryDateOrMaxHours = SelectedPermit.ExpiryDateOrMaxHours;
                    ApprovedBy = SelectedPermit.ApprovedBy;
                    OpenChiefTechnician = SelectedPermit.OpenChiefTechnician;
                    DateTime clDate = DateTimeToStringConverter.ConvertStringToDate(SelectedPermit.CloseDate);
                    //(SelectedPermit.CloseDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime clDate);
                    CloseDate = clDate;
                    CloseBPR = SelectedPermit.CloseBPR;
                    CloseTechnician = SelectedPermit.CloseTechnician;
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Updating Permit Form Failed");
            }
        }

        [RelayCommand]
        async Task UpdatePermitDetails()
        {
            try
            {
                if (SelectedPermit != null)
                {
                    PermitsModel updatedPermit = new(
                        localSelectedUAV,
                        OpenDate.ToString("dd-MM-yyyy"), OpenBPR, Description,
                        ExpiryDateOrMaxHours, ApprovedBy, OpenChiefTechnician
                    );
                    updatedPermit.PermitsSN = SelectedPermit.PermitsSN;
                    await ViewModelHelper.UpdateModelInstance(SelectedPermit, updatedPermit, Permits);

                    SelectedPermit = null;
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Updating Permit Failed");
            }
        }
    }
}
