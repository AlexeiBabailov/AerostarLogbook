
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
using AerostarLogbook.Interfaces;
using AerostarLogbook.Services.Helpers;
using AerostarLogbook.Resources.Converters;


namespace AerostarLogbook.ViewModels
{
    public partial class FrequenciesViewModel : ObservableObject
    {
        private readonly IMessagingService _messagingService;

        private string localSelectedUAV;

        private string CbandDB { get; set; }
        private string UhfDB { get; set; }

        [ObservableProperty]
        public ObservableCollection<FrequenciesCbandModel> cbandFrequencies;
        [ObservableProperty]
        public ObservableCollection<FrequenciesUhfModel> uhfFrequencies;

        [ObservableProperty]
        string cband_1 = "";
        [ObservableProperty]
        string cband_2 = "";
        [ObservableProperty]
        string cband_3 = "";
        [ObservableProperty]
        string cband_4 = "";
        [ObservableProperty]
        string cband_5 = "";
        [ObservableProperty]
        string cband_6 = "";
        [ObservableProperty]
        string cband_7 = "";
        [ObservableProperty]
        string cband_8 = "";
        [ObservableProperty]
        string cband_9 = "";
        [ObservableProperty]
        string cband_10 = "";
        [ObservableProperty]
        DateTime frequencieCbandDate = DateTime.Now;
        [ObservableProperty]
        string frequencieCbandTechnician = "Roni Malmud";
        [ObservableProperty]
        string frequencieCbandChiefTechnician = "Tamir Nudelman";

        [ObservableProperty]
        string frequencieUhf = "";
        [ObservableProperty]
        DateTime frequencieUhfDate = DateTime.Now;
        [ObservableProperty]
        string frequencieUhfTechnician = "Roni Malmud";
        [ObservableProperty]
        string frequencieUhfChiefTechnician = "Tamir Nudelman";

        [ObservableProperty]
        FrequenciesCbandModel? selectedCband;
        [ObservableProperty]
        FrequenciesUhfModel? selectedUhf;

        [ObservableProperty]
        public ObservableCollection<string> chiefTechnicianList;

        [ObservableProperty]
        public ObservableCollection<string> technicianList;


        public FrequenciesViewModel(IMessagingService messagingService)
        {
            _messagingService = messagingService;

            CbandDB = ReflectionHelper.GetTableName<FrequenciesCbandModel>();
            UhfDB = ReflectionHelper.GetTableName<FrequenciesUhfModel>();
            CbandFrequencies = new();  
            UhfFrequencies = new();
            UpdateCBandSerialNumberAsync(CbandDB);
            UpdateCBandSerialNumberAsync(UhfDB);
        }

        public async Task UpdateCBandSerialNumberAsync(string dataBase)
        {
            try
            {
                int maxSerialNumber = await SerialNumberHelper.GetMaxSerialNumberAsync(dataBase);
                FrequenciesCbandModel.UpdateFreCbandSNCount(maxSerialNumber);
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Initilazation Failed");
            }
        }

        public async Task UpdateUHFSerialNumberAsync(string dataBase)
        {
            try
            {
                int maxSerialNumber = await SerialNumberHelper.GetMaxSerialNumberAsync(dataBase);
                FrequenciesUhfModel.UpdateFreUhfSNCount(maxSerialNumber);
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Initilazation Failed");
            }
        }

        [RelayCommand]
        async Task RefreshData(string selectedUav = "111")
        {
            try
            {
                localSelectedUAV = selectedUav;

                if (CbandFrequencies != null)
                {
                    CbandFrequencies.Clear();

                }
                if (UhfFrequencies != null)
                {
                    UhfFrequencies.Clear();
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

        public async Task GetPageLists()
        {
            try
            {
                CbandFrequencies = await DatabaseService.GetItemsByTailAsync<FrequenciesCbandModel>(localSelectedUAV);
                UhfFrequencies = await DatabaseService.GetItemsByTailAsync<FrequenciesUhfModel>(localSelectedUAV);
                ChiefTechnicianList = await TeamMembersDatabaseService.GetMembersAsync<ChiefTechnicianModel>();
                TechnicianList = await TeamMembersDatabaseService.GetMembersAsync<TechnicianModel>();
                if (TechnicianList != null)
                {
                    if (FrequencieCbandTechnician == null)
                    {
                        FrequencieCbandTechnician = TechnicianList.FirstOrDefault();
                    }
                    if (FrequencieUhfTechnician == null)
                    {
                        FrequencieUhfTechnician = TechnicianList.FirstOrDefault();
                    }
                }
                if (ChiefTechnicianList != null)
                {
                    if (FrequencieCbandChiefTechnician == null)
                    {
                        FrequencieCbandChiefTechnician = ChiefTechnicianList.FirstOrDefault();
                    }
                    if (FrequencieUhfChiefTechnician == null)
                    {
                        FrequencieUhfChiefTechnician = ChiefTechnicianList.FirstOrDefault();
                    }
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Initilazation Failed");
            }
        }

        [RelayCommand]
        async Task AddCbandFrec()
        {
            try
            {
                FrequenciesCbandModel f = new(localSelectedUAV, Cband_1, Cband_2, Cband_3, Cband_4, Cband_5, Cband_6,
                    Cband_7, Cband_8, Cband_9, Cband_10, FrequencieCbandDate.ToString("dd-MM-yyyy"), FrequencieCbandTechnician, FrequencieCbandChiefTechnician);
                await ViewModelHelper.AddModelInstance(f, CbandFrequencies, (model) => FrequenciesCbandModel.UpdateFreCbandSNCount(model.FrequenciesCbandSN));

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Adding C-Band Frequencie Failed");
            }
        }

        [RelayCommand]
        async Task AddUhfFrec()
        {
            try
            {
                FrequenciesUhfModel f = new(localSelectedUAV, FrequencieUhf,
            FrequencieUhfDate.ToString("dd-MM-yyyy"), FrequencieUhfTechnician, FrequencieUhfChiefTechnician);
                await ViewModelHelper.AddModelInstance(f, UhfFrequencies, (model) => FrequenciesUhfModel.UpdateFreUhfSNCount(model.FrequenciesUhfSN));

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Adding UHF Frequencie Failed");
            }
        }

        [RelayCommand]
        async Task DeleteCbandFre()
        {
            try
            {
                if (SelectedCband != null)
                {
                    await ViewModelHelper.DeleteModelInstance(SelectedCband, CbandFrequencies);
                }
                else
                {
                    _messagingService.ShowMessage("No C-Band Frequencie selected", "Deleting C-Band Frequencie Failed");
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Deleting C-Band Frequencie Failed");
            }
        }

        [RelayCommand]
        async Task DeleteUhfFre()
        {
            try
            {
                if (SelectedUhf != null)
                {
                    await ViewModelHelper.DeleteModelInstance(SelectedUhf, UhfFrequencies);
                }
                else
                {
                    _messagingService.ShowMessage("No UHF Frequencie selected", "Deleting C-Band Frequencie Failed");
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Deleting UHF Frequencie Failed");
            }
        }

        [RelayCommand]
        void UpdateCbandFrequenciesForm()
        {
            try
            {
                if (SelectedCband != null)
                {
                    Cband_1 = SelectedCband.Cband_1;
                    Cband_2 = SelectedCband.Cband_2;
                    Cband_3 = SelectedCband.Cband_3;
                    Cband_4 = SelectedCband.Cband_4;
                    Cband_5 = SelectedCband.Cband_5;
                    Cband_6 = SelectedCband.Cband_6;
                    Cband_7 = SelectedCband.Cband_7;
                    Cband_8 = SelectedCband.Cband_8;
                    Cband_9 = SelectedCband.Cband_9;
                    Cband_10 = SelectedCband.Cband_10;
                    DateTime date = DateTimeToStringConverter.ConvertStringToDate(SelectedCband.FrequencieCbandDate);
                    //(SelectedCband.FrequencieCbandDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
                    FrequencieCbandDate = date;

                    FrequencieCbandTechnician = SelectedCband.FrequencieCbandTechnician;
                    FrequencieCbandChiefTechnician = SelectedCband.FrequencieCbandChiefTechnician;
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Updating C-Band Frequencie Form Failed");
            }
        }

        [RelayCommand]
        async Task UpdateCbandFrequencie()
        {
            try
            {
                if (SelectedCband != null)
                {
                    FrequenciesCbandModel updatedCband = new(
                        localSelectedUAV,
                        Cband_1, Cband_2, Cband_3, Cband_4, Cband_5,
                        Cband_6, Cband_7, Cband_8, Cband_9, Cband_10,
                        FrequencieCbandDate.ToString("dd-MM-yyyy"),
                        FrequencieCbandTechnician, FrequencieCbandChiefTechnician
                    );
                    updatedCband.FrequenciesCbandSN = SelectedCband.FrequenciesCbandSN;

                    await ViewModelHelper.UpdateModelInstance(SelectedCband, updatedCband, CbandFrequencies);

                    SelectedCband = null;
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Updating C-Band Frequencie Failed");
            }
        }

        [RelayCommand]
        void UpdateUhfFrequenciesForm()
        {
            try
            {
                if (SelectedUhf != null)
                {
                    FrequencieUhf = SelectedUhf.FrequencieUhf;
                    DateTime uhfDate = DateTimeToStringConverter.ConvertStringToDate(SelectedUhf.FrequencieUhfDate);
                    //(SelectedUhf.FrequencieUhfDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime uhfDate);
                    FrequencieUhfDate = uhfDate;

                    FrequencieUhfTechnician = SelectedUhf.FrequencieUhfTechnician;
                    FrequencieUhfChiefTechnician = SelectedUhf.FrequencieUhfChiefTechnician;
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Updating UHF Frequencie Form Failed");
            }
        }

        [RelayCommand]
        async Task UpdateUhfFrequencie()
        {
            try
            {
                if (SelectedUhf != null)
                {
                    FrequenciesUhfModel updatedUhf = new(
                        localSelectedUAV, FrequencieUhf,
                        FrequencieUhfDate.ToString("dd-MM-yyyy"),
                        FrequencieUhfTechnician,
                        FrequencieUhfChiefTechnician
                    );
                    updatedUhf.FrequenciesUhfSN = SelectedUhf.FrequenciesUhfSN;

                    await ViewModelHelper.UpdateModelInstance(SelectedUhf, updatedUhf, UhfFrequencies);

                    SelectedUhf = null;
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Updating UHF Frequencie Failed");
            }
        }
    }
}
