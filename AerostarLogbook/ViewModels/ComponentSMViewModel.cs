using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AerostarLogbook.Models;
using AerostarLogbook.Models.TeamMembers;
using AerostarLogbook.Services;
using AerostarLogbook.Services.DatabaseMain;
using AerostarLogbook.Services.Helpers;
using AerostarLogbook.Services.DatabaseMiscellaneous;
using Microsoft.Extensions.Logging;
using AerostarLogbook.Interfaces;


namespace AerostarLogbook.ViewModels
{
    public partial class ComponentSMViewModel : ObservableObject
    {
        private readonly IMessagingService _messagingService;

        private string ComponentSMDB { get; set; }
        private string localSelectedUAV;


        [ObservableProperty]
        public ObservableCollection<ComponentSMModel> componentsCurrentlyAssembled;

        [ObservableProperty]
        public ObservableCollection<ComponentSMModel> componentsSMHistory;

        [ObservableProperty]
        public ObservableCollection<string> componentTypes;

        [ObservableProperty]
        public ObservableCollection<ComponentModel> selectedComponentSN;

        [ObservableProperty]
        string componentType;
        [ObservableProperty]
        string uavTail;
        [ObservableProperty]
        string validUavHours;
        [ObservableProperty]
        string doneAtUavHours = string.Empty;
        [ObservableProperty]
        string sN;
        [ObservableProperty]
        DateTime assemblyDate = DateTime.Today;
        [ObservableProperty]
        string assemblyTechnician = "Roni Malmud";
        [ObservableProperty]
        string assemblyChiefTechnician = "Tamir Nudelman";


        [ObservableProperty]
        ComponentSMModel selectedComponent;

        [ObservableProperty]
        ComponentModel pickerSelectedComponent;


        [ObservableProperty]
        public ObservableCollection<string> technicianList;
        [ObservableProperty]
        public ObservableCollection<string> chiefTechnicianList;


        public ComponentSMViewModel(IMessagingService messagingService)
        {
            _messagingService = messagingService;
            ComponentSMDB = ReflectionHelper.GetTableName<ComponentSMModel>();
            ComponentsCurrentlyAssembled = new();
            ComponentsSMHistory = new();
            ComponentTypes = new();
            TechnicianList = new();
            ChiefTechnicianList = new();
            SelectedComponentSN = new();
            UpdateEventSerialNumberAsync(ComponentSMDB);

        }

        private async Task UpdateEventSerialNumberAsync(string dataBase)
        {
            try
            {
                int maxSerialNumber = await SerialNumberHelper.GetMaxSerialNumberAsync(dataBase);
                ComponentSMModel.UpdateComponentSMSNCount(maxSerialNumber);

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
                if (ComponentsCurrentlyAssembled != null)
                {
                    ComponentsCurrentlyAssembled.Clear();
                }
                if (ComponentsSMHistory != null)
                {
                    ComponentsSMHistory.Clear();
                }
                if (ComponentTypes != null)
                {
                    ComponentTypes.Clear();
                }
                if (ComponentTypes != null)
                {
                    ComponentTypes.Clear();
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
                ObservableCollection<ComponentSMModel> temp_AllList = new();
                temp_AllList = await DatabaseService.GetItemsByTailAsync<ComponentSMModel>(localSelectedUAV);
                if (temp_AllList != null)
                {
                    if (ComponentsCurrentlyAssembled != null)
                    {
                        ComponentsCurrentlyAssembled.Clear();
                        foreach (ComponentSMModel c in temp_AllList)
                        {
                            if (c.DoneAtUavHours == string.Empty)
                            {
                                ComponentsCurrentlyAssembled.Insert(0, c);
                            }
                        }
                    }
                    if (ComponentsSMHistory != null)
                    {
                        ComponentsSMHistory.Clear();
                        foreach (ComponentSMModel h in temp_AllList)
                        {
                            if (h.DoneAtUavHours != string.Empty)
                            {
                                ComponentsSMHistory.Insert(0, h);
                            }
                        }
                    }
                }
                ComponentTypes = await ComponentDatabaseService.GetComponentTypesAsync();
                ChiefTechnicianList = await TeamMembersDatabaseService.GetMembersAsync<ChiefTechnicianModel>();
                TechnicianList = await TeamMembersDatabaseService.GetMembersAsync<TechnicianModel>();
                if (ChiefTechnicianList != null && AssemblyChiefTechnician == null)
                {
                    AssemblyChiefTechnician = ChiefTechnicianList.FirstOrDefault();
                }
                if (TechnicianList != null && AssemblyTechnician == null)
                {
                    AssemblyTechnician = TechnicianList.FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Initilazation Failed");
            }
        }


        [RelayCommand]
        async Task AssembleComponent()
        {
            try
            {
                ComponentSMModel c = new(ComponentType, localSelectedUAV,
            ValidUavHours, "", PickerSelectedComponent.SN,
            AssemblyDate.ToString("dd-MM-yyyy"), AssemblyTechnician, AssemblyChiefTechnician);
                await ViewModelHelper.AddModelInstance(c, ComponentsCurrentlyAssembled, (model) => ComponentSMModel.UpdateComponentSMSNCount(model.ComponentSMSN));

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Assemble Component Failed");
            }
        }

        [RelayCommand]
        async Task DisassembleComponent()
        {
            try
            {
                if (SelectedComponent != null)
                {
                    ComponentSMModel temp = SelectedComponent;
                    await ViewModelHelper.DeleteModelInstance(SelectedComponent, ComponentsCurrentlyAssembled);
                    temp.DoneAtUavHours = DoneAtUavHours;
                    await ViewModelHelper.AddModelInstance(temp, ComponentsSMHistory, (model) => ComponentSMModel.UpdateComponentSMSNCount(model.ComponentSMSN));
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Disassemble Component Failed");
            }
        }


        [RelayCommand]
        async Task UpdateComponentSN(string componentType)
        {
            try
            {
                SelectedComponentSN.Clear();
                SelectedComponentSN = await ComponentDatabaseService.GetComponentSNByTypeAsync(componentType);

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Component Serial numbers updating Failed");
            }
        }
    }
}
