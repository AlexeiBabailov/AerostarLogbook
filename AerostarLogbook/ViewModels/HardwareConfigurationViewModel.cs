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
using Microsoft.Extensions.Logging;
using AerostarLogbook.Interfaces;
using AerostarLogbook.Resources.Converters;


namespace AerostarLogbook.ViewModels
{
    public partial class HardwareConfigurationViewModel : ObservableObject
    {
        private readonly IMessagingService _messagingService;

        private string HardwareConfDB { get; set; }

        private string localSelectedUAV;


        [ObservableProperty]
        public ObservableCollection<HardwareConfigurationModel> hWConfList;

        [ObservableProperty]
        DateTime assemblyDate = DateTime.Today;
        [ObservableProperty]
        string assemblyDescription = "Write the change description here..";
        [ObservableProperty]
        string assemblyChiefTechnician = "Yoav GerShovitz";
        [ObservableProperty]
        DateTime disassemblyDate = DateTime.Today;
        [ObservableProperty]
        string disassemblyChiefTechnician = "Tamir Nudelman";
        
        [ObservableProperty]
        HardwareConfigurationModel? selectedHWConf;

        [ObservableProperty]
        public ObservableCollection<string> chiefTechnicianList;

        public HardwareConfigurationViewModel(IMessagingService messagingService)
        {
            _messagingService = messagingService;

            HardwareConfDB = ReflectionHelper.GetTableName<HardwareConfigurationModel>();
            hWConfList = new();
            UpdateHWConfSerialNumberAsync(HardwareConfDB);
        }
        public async Task UpdateHWConfSerialNumberAsync(string dataBase)
        {
            try
            {
                int maxSerialNumber = await SerialNumberHelper.GetMaxSerialNumberAsync(dataBase);
                HardwareConfigurationModel.UpdateHWConfSNCount(maxSerialNumber);
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

                if (HWConfList != null)
                {
                    HWConfList.Clear();
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
                HWConfList = await DatabaseService.GetItemsByTailAsync<HardwareConfigurationModel>(localSelectedUAV);
                ChiefTechnicianList = await TeamMembersDatabaseService.GetMembersAsync<ChiefTechnicianModel>();
                if (ChiefTechnicianList != null)
                {
                    if (AssemblyChiefTechnician == null)
                    {
                        AssemblyChiefTechnician = ChiefTechnicianList.FirstOrDefault();
                    }
                    if (DisassemblyChiefTechnician == null)
                    {
                        DisassemblyChiefTechnician = ChiefTechnicianList.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Initilazation Failed");
            }
        }

        [RelayCommand]
        async Task AddHWConf()
        {
            try
            {
                HardwareConfigurationModel h = new(localSelectedUAV,
            AssemblyDate.ToString("dd-MM-yyyy"), AssemblyDescription, AssemblyChiefTechnician);
                await ViewModelHelper.AddModelInstance(h, HWConfList, (model) => HardwareConfigurationModel.UpdateHWConfSNCount(model.HardwareConfigurationSN));
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Adding Hardware Configuration Failed");
            }
        }

        [RelayCommand]
        async Task DisassemblyHWConf()
        {
            try
            {
                if (SelectedHWConf != null)
                {
                    HardwareConfigurationModel h = SelectedHWConf;
                    h.Disassembly(DisassemblyDate.ToString("dd-MM-yyyy"), DisassemblyChiefTechnician);
                    h.HardwareConfigurationSN = SelectedHWConf.HardwareConfigurationSN;
                    await ViewModelHelper.UpdateModelInstance(SelectedHWConf, h, HWConfList);
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Disassemble of Hardware Configuration Failed");
            }
        }

        [RelayCommand]
        async Task DeleteHWConf()
        {
            try
            {
                if (SelectedHWConf != null)
                {
                    await ViewModelHelper.DeleteModelInstance(SelectedHWConf, HWConfList);
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Deleting Hardware Configuration Failed");
            }
        }

        [RelayCommand]
        void UpdateHWConfForm()
        {
            try
            {
                if (SelectedHWConf != null)
                {
                    DateTime assDate = DateTimeToStringConverter.ConvertStringToDate(SelectedHWConf.AssemblyDate);
                    //(SelectedHWConf.AssemblyDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime assDate);
                    AssemblyDate = assDate;
                    AssemblyDescription = SelectedHWConf.AssemblyDescription;
                    AssemblyChiefTechnician = SelectedHWConf.AssemblyChiefTechnician;
                    DateTime disDate = DateTimeToStringConverter.ConvertStringToDate(SelectedHWConf.DisassemblyDate);
                    //(SelectedHWConf.DisassemblyDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime disDate);
                    DisassemblyDate = disDate;
                    DisassemblyChiefTechnician = SelectedHWConf.DisassemblyChiefTechnician;
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Updating Hardware Configuration Form Failed");
            }
        }

        [RelayCommand]
        async Task UpdateHWConfiguration()
        {
            try
            {
                if (SelectedHWConf != null)
                {
                    HardwareConfigurationModel updatedHWConf = new(
                        localSelectedUAV,
                        AssemblyDate.ToString("dd-MM-yyyy"),
                        AssemblyDescription,
                        AssemblyChiefTechnician
                    );
                    updatedHWConf.HardwareConfigurationSN = SelectedHWConf.HardwareConfigurationSN;
                    await ViewModelHelper.UpdateModelInstance(SelectedHWConf, updatedHWConf, HWConfList);

                    SelectedHWConf = null;
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Updating Hardware Configuration Failed");
            }
        }

    }

}
