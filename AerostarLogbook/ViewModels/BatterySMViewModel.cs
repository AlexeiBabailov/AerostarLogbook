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
    public partial class BatterySMViewModel : ObservableObject
    {
        private readonly IMessagingService _messagingService;

        private string BatterySMDB { get; set; }

        private string localSelectedUAV;


        [ObservableProperty]
        public ObservableCollection<BatterySMModel> batterys;

        [ObservableProperty]
        string batteryHrs = string.Empty;
        [ObservableProperty]
        string initialActivation = string.Empty;
        [ObservableProperty]
        string batteryOfficialSN = string.Empty;

        [ObservableProperty]
        string battUavHrs1 = string.Empty;
        [ObservableProperty]
        string battDoneAt1 = string.Empty;
        [ObservableProperty]
        DateTime battDate1 = DateTime.Today;
        [ObservableProperty]
        DateTime battDoneDate1 = DateTime.Today;
        [ObservableProperty]
        string battTechnician1 = string.Empty;
        [ObservableProperty]
        string battChiefTechnician1 = string.Empty;

        [ObservableProperty]
        string battUavHrs2 = string.Empty;
        [ObservableProperty]
        string battDoneAt2 = string.Empty;
        [ObservableProperty]
        DateTime battDate2 = DateTime.Today;
        [ObservableProperty]
        DateTime battDoneDate2 = DateTime.Today;
        [ObservableProperty]
        string battTechnician2 = string.Empty;
        [ObservableProperty]
        string battChiefTechnician2 = string.Empty;

        [ObservableProperty]
        string battUavHrs3 = string.Empty;
        [ObservableProperty]
        string battDoneAt3 = string.Empty;
        [ObservableProperty]
        DateTime battDate3 = DateTime.Today;
        [ObservableProperty]
        DateTime battDoneDate3 = DateTime.Today;
        [ObservableProperty]
        string battTechnician3 = string.Empty;
        [ObservableProperty]
        string battChiefTechnician3 = string.Empty;

        [ObservableProperty]
        string battUavHrs4 = string.Empty;
        [ObservableProperty]
        string battDoneAt4 = string.Empty;
        [ObservableProperty]
        DateTime battDate4 = DateTime.Today;
        [ObservableProperty]
        DateTime battDoneDate4 = DateTime.Today;
        [ObservableProperty]
        string battTechnician4 = string.Empty;
        [ObservableProperty]
        string battChiefTechnician4 = string.Empty;

        [ObservableProperty]
        string battUavHrs5 = string.Empty;
        [ObservableProperty]
        string battDoneAt5 = string.Empty;
        [ObservableProperty]
        DateTime battDate5 = DateTime.Today;
        [ObservableProperty]
        DateTime battDoneDate5 = DateTime.Today;
        [ObservableProperty]
        string battTechnician5 = string.Empty;
        [ObservableProperty]
        string battChiefTechnician5 = string.Empty;

        [ObservableProperty]
        string battUavHrs6 = string.Empty;
        [ObservableProperty]
        string battDoneAt6 = string.Empty;
        [ObservableProperty]
        DateTime battDate6 = DateTime.Today;
        [ObservableProperty]
        DateTime battDoneDate6 = DateTime.Today;
        [ObservableProperty]
        string battTechnician6 = string.Empty;
        [ObservableProperty]
        string battChiefTechnician6 = string.Empty;




        [ObservableProperty]
        public ObservableCollection<string> chiefTechnicianList;

        [ObservableProperty]
        public ObservableCollection<string> technicianList;



        [ObservableProperty]
        BatterySMModel selectedBattery;

        public BatterySMViewModel(IMessagingService messagingService)
        {           
            _messagingService = messagingService;
            BatterySMDB = ReflectionHelper.GetTableName<BatterySMModel>();
            Batterys = new();
            UpdateBatterySMSerialNumberAsync(BatterySMDB);
        }

        public async Task UpdateBatterySMSerialNumberAsync(string dataBase)
        {
            try
            {
                int maxSerialNumber = await SerialNumberHelper.GetMaxSerialNumberAsync(dataBase);
                BatterySMModel.UpdateBattSNCount(maxSerialNumber);
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Initilazation Failed");
            }
        }

        [RelayCommand]
        void RefreshData(string selectedUav)
        {
            try
            {
                localSelectedUAV = selectedUav;
                if (Batterys != null)
                {
                    Batterys.Clear();
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
                Batterys = await DatabaseService.GetItemsByTailAsync<BatterySMModel>(localSelectedUAV);
                TechnicianList = await TeamMembersDatabaseService.GetMembersAsync<TechnicianModel>();
                ChiefTechnicianList = await TeamMembersDatabaseService.GetMembersAsync<ChiefTechnicianModel>();
                List<string> tempTechnicianList = new()
            {
                BattTechnician1, BattTechnician2, BattTechnician3, BattTechnician4, BattTechnician5, BattTechnician6
            };
                List<string> tempChiefTechnicianList = new()
            {
                BattChiefTechnician1, BattChiefTechnician2, BattChiefTechnician3, BattChiefTechnician4, BattChiefTechnician5, BattChiefTechnician6
            };

                if (TechnicianList != null)
                {
                    for (int i = 0; i < tempTechnicianList.Count; i++)
                    {
                        if (tempTechnicianList[i] == null)
                        {
                            tempTechnicianList[i] = TechnicianList.FirstOrDefault();
                        }
                    }
                }
                if (ChiefTechnicianList != null)
                {
                    for (int i = 0; i < tempChiefTechnicianList.Count; i++)
                    {
                        if (tempChiefTechnicianList[i] == null)
                        {
                            tempChiefTechnicianList[i] = ChiefTechnicianList.FirstOrDefault();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Initilazation Failed");
            }
        }

        [RelayCommand]
        async Task AddBattery()
        {
            try
            {
                BatterySMModel b = new(MainPageViewModel.staticSelectedUAV,
                    BatteryHrs, InitialActivation, BatteryOfficialSN,
                    BattUavHrs1, BattDoneAt1, BattDate1.ToString("dd-MM-yyyy"), BattDoneDate1.ToString("dd-MM-yyyy"), BattTechnician1, BattChiefTechnician1,
                    BattUavHrs2, BattDoneAt2, BattDate2.ToString("dd-MM-yyyy"), BattDoneDate2.ToString("dd-MM-yyyy"), BattTechnician2, BattChiefTechnician2,
                    BattUavHrs3, BattDoneAt3, BattDate3.ToString("dd-MM-yyyy"), BattDoneDate3.ToString("dd-MM-yyyy"), BattTechnician3, BattChiefTechnician3,
                    BattUavHrs4, BattDoneAt4, BattDate4.ToString("dd-MM-yyyy"), BattDoneDate4.ToString("dd-MM-yyyy"), BattTechnician4, BattChiefTechnician4,
                    BattUavHrs5, BattDoneAt5, BattDate5.ToString("dd-MM-yyyy"), BattDoneDate5.ToString("dd-MM-yyyy"), BattTechnician5, BattChiefTechnician5,
                    BattUavHrs6, BattDoneAt6, BattDate6.ToString("dd-MM-yyyy"), BattDoneDate6.ToString("dd-MM-yyyy"), BattTechnician6, BattChiefTechnician6);
                await ViewModelHelper.AddModelInstance(b, Batterys, (model) => BatterySMModel.UpdateBattSNCount(model.BatterySMSN));

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Adding Battery Failed");
            }
        }

        [RelayCommand]
        async Task DeleteBattery()
        {
            try
            {
                if (SelectedBattery != null)
                {
                    await ViewModelHelper.DeleteModelInstance(SelectedBattery, Batterys);
                }
                else
                {
                    _messagingService.ShowMessage("No Battery Selected", "Adding Battery Failed");
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Deleting Battery Failed");
            }
        }

        [RelayCommand]
        void UpdateForm()
        {
            try
            {
                if (SelectedBattery != null)
                {
                    BatteryHrs = SelectedBattery.BatteryHrs;
                    InitialActivation = SelectedBattery.InitialActivation;
                    BatteryOfficialSN = SelectedBattery.BatteryOfficialSN;

                    BattUavHrs1 = SelectedBattery.BattUavHrs1;
                    BattDoneAt1 = SelectedBattery.BattUavHrs1;
                    DateTime d11 = DateTimeToStringConverter.ConvertStringToDate(SelectedBattery.BattDate1);
                    BattDate1 = d11;
                    DateTime d12 = DateTimeToStringConverter.ConvertStringToDate(SelectedBattery.BattDoneDate1);
                    BattDoneDate1 = d12;
                    BattTechnician1 = SelectedBattery.BattTechnician1;
                    BattChiefTechnician1 = SelectedBattery.BattChiefTechnician1;

                    BattUavHrs2 = SelectedBattery.BattUavHrs2;
                    BattDoneAt2 = SelectedBattery.BattDoneAt2;
                    DateTime d21 = DateTimeToStringConverter.ConvertStringToDate(SelectedBattery.BattDate2);
                    BattDate2 = d21;
                    DateTime d22 = DateTimeToStringConverter.ConvertStringToDate(SelectedBattery.BattDoneDate2);
                    BattDoneDate2 = d22;
                    BattTechnician2 = SelectedBattery.BattTechnician2;
                    BattChiefTechnician2 = SelectedBattery.BattChiefTechnician2;

                    BattUavHrs3 = SelectedBattery.BattUavHrs3;
                    BattDoneAt3 = SelectedBattery.BattDoneAt3;
                    DateTime d31 = DateTimeToStringConverter.ConvertStringToDate(SelectedBattery.BattDate3);
                    BattDate3 = d31;
                    DateTime d32 = DateTimeToStringConverter.ConvertStringToDate(SelectedBattery.BattDoneDate3);
                    BattDoneDate3 = d32;
                    BattTechnician3 = SelectedBattery.BattTechnician3;
                    BattChiefTechnician3 = SelectedBattery.BattChiefTechnician3;

                    BattUavHrs4 = SelectedBattery.BattUavHrs4;
                    BattDoneAt4 = SelectedBattery.BattDoneAt4;
                    DateTime d41 = DateTimeToStringConverter.ConvertStringToDate(SelectedBattery.BattDate4);
                    BattDate4 = d41;
                    DateTime d42 = DateTimeToStringConverter.ConvertStringToDate(SelectedBattery.BattDoneDate4);
                    BattDoneDate4 = d42;
                    BattTechnician4 = SelectedBattery.BattTechnician4;
                    BattChiefTechnician4 = SelectedBattery.BattChiefTechnician4;

                    BattUavHrs5 = SelectedBattery.BattUavHrs5;
                    BattDoneAt5 = SelectedBattery.BattDoneAt5;
                    DateTime d51 = DateTimeToStringConverter.ConvertStringToDate(SelectedBattery.BattDate5);
                    BattDate5 = d51;
                    DateTime d52 = DateTimeToStringConverter.ConvertStringToDate(SelectedBattery.BattDoneDate5);
                    BattDoneDate5 = d52;
                    BattTechnician5 = SelectedBattery.BattTechnician5;
                    BattChiefTechnician5 = SelectedBattery.BattChiefTechnician5;

                    BattUavHrs6 = SelectedBattery.BattUavHrs6;
                    BattDoneAt6 = SelectedBattery.BattDoneAt6;
                    DateTime d61 = DateTimeToStringConverter.ConvertStringToDate(SelectedBattery.BattDate6);
                    BattDate6 = d61;
                    DateTime d62 = DateTimeToStringConverter.ConvertStringToDate(SelectedBattery.BattDoneDate6);
                    BattDoneDate6 = d62;
                    BattTechnician6 = SelectedBattery.BattTechnician6;
                    BattChiefTechnician6 = SelectedBattery.BattChiefTechnician6;
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Updating Battery Form Failed");
            }
        }

        [RelayCommand]
        async Task UpdateBatteryActivity()
        {
            try
            {
                if (SelectedBattery != null)
                {
                    BatterySMModel b = new(MainPageViewModel.staticSelectedUAV,
                    BatteryHrs, InitialActivation, BatteryOfficialSN,
                    BattUavHrs1, BattDoneAt1, BattDate1.ToString("dd-MM-yyyy"), BattDoneDate1.ToString("dd-MM-yyyy"), BattTechnician1, BattChiefTechnician1,
                    BattUavHrs2, BattDoneAt2, BattDate2.ToString("dd-MM-yyyy"), BattDoneDate2.ToString("dd-MM-yyyy"), BattTechnician2, BattChiefTechnician2,
                    BattUavHrs3, BattDoneAt3, BattDate3.ToString("dd-MM-yyyy"), BattDoneDate3.ToString("dd-MM-yyyy"), BattTechnician3, BattChiefTechnician3,
                    BattUavHrs4, BattDoneAt4, BattDate4.ToString("dd-MM-yyyy"), BattDoneDate4.ToString("dd-MM-yyyy"), BattTechnician4, BattChiefTechnician4,
                    BattUavHrs5, BattDoneAt5, BattDate5.ToString("dd-MM-yyyy"), BattDoneDate5.ToString("dd-MM-yyyy"), BattTechnician5, BattChiefTechnician5,
                    BattUavHrs6, BattDoneAt6, BattDate6.ToString("dd-MM-yyyy"), BattDoneDate6.ToString("dd-MM-yyyy"), BattTechnician6, BattChiefTechnician6);
                    b.BatterySMSN = SelectedBattery.BatterySMSN;
                    await ViewModelHelper.UpdateModelInstance(SelectedBattery, b, Batterys);

                    SelectedBattery = null;
                }
                else
                {
                    _messagingService.ShowMessage("No Battery Selected", "Adding Battery Failed");
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Update Battery Failed");
            }
        }

       
    }
}
