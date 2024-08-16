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
    public partial class FuselageScheduledMaintenanceViewModel : ObservableObject
    {
        private readonly IMessagingService _messagingService;

        private string localSelectedUAV;

        private string FsmDB { get; set; }

        [ObservableProperty]
        public  ObservableCollection<FuselageScheduledMaintenanceModel> fsmList;

        [ObservableProperty]
        string validUavHr = "234";
        [ObservableProperty]
        string doneAtUavHr = "234";
        [ObservableProperty]
        DateTime date = DateTime.Now;
        [ObservableProperty]
        string technician = "Roni Malmud";
        [ObservableProperty]
        string chiefTechnician = "Tamir Nudelman";
        [ObservableProperty]
        string type = "500";


        [ObservableProperty]
        FuselageScheduledMaintenanceModel? selectedFsm;

        [ObservableProperty]
        public ObservableCollection<string> technicianList;

        [ObservableProperty]
        public ObservableCollection<string> chiefTechnicianList;

        [ObservableProperty]
        public ObservableCollection<string> typesOfFsm = new() {"500", "1000"};


        public FuselageScheduledMaintenanceViewModel(IMessagingService messagingService)
        {
            _messagingService = messagingService;
            FsmDB = ReflectionHelper.GetTableName<FuselageScheduledMaintenanceModel>();
            FsmList = new();
            UpdateBiWeeklySerialNumberAsync(FsmDB);
        }
        public async Task UpdateBiWeeklySerialNumberAsync(string dataBase)
        {
            try
            {
                int maxSerialNumber = await SerialNumberHelper.GetMaxSerialNumberAsync(dataBase);
                FuselageScheduledMaintenanceModel.UpdateFsmSNCount(maxSerialNumber);
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

                if (FsmList != null)
                {
                    FsmList.Clear();
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
                FsmList = await DatabaseService.GetItemsByTailAsync<FuselageScheduledMaintenanceModel>(localSelectedUAV);
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
        async Task AddFsm()
        {
            try
            {
                FuselageScheduledMaintenanceModel f = new(localSelectedUAV,
            ValidUavHr, DoneAtUavHr, Date.ToString("dd-MM-yyyy"), Technician, ChiefTechnician, Type);
                await ViewModelHelper.AddModelInstance(f, FsmList, (model) => FuselageScheduledMaintenanceModel.UpdateFsmSNCount(model.FuselageScheduledMaintenanceSN));

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Adding Fuseledge Schedule Maintenance Failed");
            }
        }

        [RelayCommand]
        async Task DeleteFsm()
        {
            try
            {
                if (SelectedFsm != null)
                {
                    await ViewModelHelper.DeleteModelInstance(SelectedFsm, FsmList);
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Deleting Fuseledge Schedule Maintenance Failed");
            }
        }

        [RelayCommand]
        void UpdateFsmForm()
        {
            try
            {
                if (SelectedFsm != null)
                {
                    ValidUavHr = SelectedFsm.ValidUavHr;
                    DoneAtUavHr = SelectedFsm.DoneAtUavHr;
                    DateTime recordedDate = DateTimeToStringConverter.ConvertStringToDate(SelectedFsm.Date);
                    //(SelectedFsm.Date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime recordedDate);
                    Date = recordedDate;

                    Technician = SelectedFsm.Technician;
                    ChiefTechnician = SelectedFsm.ChiefTechnician;
                    Type = SelectedFsm.Type;
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Updating Fuseledge Schedule Maintenance Form Failed");
            }
        }

        [RelayCommand]
        async Task UpdateFsmDetails()
        {
            try
            {
                if (SelectedFsm != null)
                {
                    FuselageScheduledMaintenanceModel updatedFsm = new(
                        localSelectedUAV,
                        ValidUavHr, DoneAtUavHr,
                        Date.ToString("dd-MM-yyyy"),
                        Technician, ChiefTechnician, Type
                    );
                    updatedFsm.FuselageScheduledMaintenanceSN = SelectedFsm.FuselageScheduledMaintenanceSN;
                    await ViewModelHelper.UpdateModelInstance(SelectedFsm, updatedFsm, FsmList);

                    SelectedFsm = null;
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Updating Fuseledge Schedule Maintenance Failed");
            }
        }


    }
}
