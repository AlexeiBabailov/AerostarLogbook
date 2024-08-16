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

    public partial class AdministratorViewModel : ObservableObject
    {
        private readonly IMessagingService _messagingService;

        [ObservableProperty]
        public static ObservableCollection<string> technicians;
        [ObservableProperty]
        public static ObservableCollection<string> chiefTechnicians;
        [ObservableProperty]
        public static ObservableCollection<string> externalPilots;
        [ObservableProperty]
        public static ObservableCollection<UavModel> uavs;
        [ObservableProperty]
        public static ObservableCollection<EngineModel> engines;
        [ObservableProperty]
        public static ObservableCollection<ComponentModel> components;



        [ObservableProperty]
        string chiefTechnician = string.Empty;
        [ObservableProperty]
        string selectedChiefTechnician;

        [ObservableProperty]
        string technician = string.Empty;
        [ObservableProperty]
        string selectedTechnician;

        [ObservableProperty]
        string externalPilot = string.Empty;
        [ObservableProperty]
        string selectedExternalPilot;

        [ObservableProperty]
        string tailNum;
        [ObservableProperty]
        string uavHours;
        [ObservableProperty]
        UavModel selectedUav;

        [ObservableProperty]
        string engineName;
        [ObservableProperty]
        string engineInitialHours;
        [ObservableProperty]
        EngineModel selectedEngine;

        [ObservableProperty]
        string componentType;
        [ObservableProperty]
        string componentSN;
        [ObservableProperty]
        ComponentModel selectedComponent;

        [ObservableProperty]
        string[] componentTypes = ComponentModel.ComponentTypes;


        public AdministratorViewModel(IMessagingService messagingService)
        {
            Technicians = new();
            ChiefTechnicians = new();
            ExternalPilots = new();
            uavs = new();
            engines = new();
            components = new();
            GetPageLists();
            _messagingService = messagingService;
        }

        public async Task GetPageLists()
        {
            try
            {
                Technicians = await TeamMembersDatabaseService.GetMembersAsync<TechnicianModel>();
                ChiefTechnicians = await TeamMembersDatabaseService.GetMembersAsync<ChiefTechnicianModel>();
                ExternalPilots = await TeamMembersDatabaseService.GetMembersAsync<ExternalPilotModel>();
                Uavs = await UavDatabaseService.GetUavsAsync();
                Engines = await EngineDatabaseService.GetEnginesFromDB();
                Components = await ComponentDatabaseService.GetComponentsFromDBAsync();
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Loading Data Failed");
            }
        }

        [RelayCommand]
        async Task AddChiefTechnician()
        {
            try
            {
                if (Valid(ChiefTechnician))
                {
                    ChiefTechnicianModel member = new(ChiefTechnician);
                    await TeamMembersDatabaseService.AddMemberAsync<ChiefTechnicianModel>(member);
                    chiefTechnicians.Add(ChiefTechnician);
                    ChiefTechnician = String.Empty;
                }
                else
                {
                    _messagingService.ShowMessage("Invalid Chief Technician name", "Adding Chief Technician Failed");
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Adding Chief Technician Failed");
            }
        }

        [RelayCommand]
        async Task DeleteChiefTechnician()
        {
            try
            {
                if (SelectedChiefTechnician != null)
                {
                    ChiefTechnicianModel member = new(SelectedChiefTechnician);
                    await TeamMembersDatabaseService.DeleteMemberAsync<ChiefTechnicianModel>(member);
                    chiefTechnicians.Remove(SelectedChiefTechnician);
                }
                else
                {
                    _messagingService.ShowMessage("Chief Technician not selected", "Adding Chief Technician Failed");
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Deleting Chief Technician Failed");
            }
        }

        [RelayCommand]
        async Task AddTechnician()
        {
            try
            {
                if (Valid(Technician))
                {
                    TechnicianModel member = new(Technician);
                    await TeamMembersDatabaseService.AddMemberAsync<TechnicianModel>(member);
                    technicians.Add(Technician);
                    Technician = String.Empty;
                }
                else
                {
                    _messagingService.ShowMessage("Invalid Technician name", "Adding Chief Technician Failed");
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Adding Technician Failed");
            }
        }

        [RelayCommand]
        async Task DeleteTechnician()
        {
            try
            {
                if (SelectedTechnician != null)
                {
                    TechnicianModel member = new(SelectedTechnician);
                    await TeamMembersDatabaseService.DeleteMemberAsync<TechnicianModel>(member);
                    technicians.Remove(SelectedTechnician);
                }
                else
                {
                    _messagingService.ShowMessage("Technician not selected", "Adding Chief Technician Failed");
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Deleting Technician Failed");
            }
        }

        [RelayCommand]
        async Task AddExternalPilot()
        {
            try
            {
                if (Valid(ExternalPilot))
                {
                    ExternalPilotModel member = new(ExternalPilot);
                    await TeamMembersDatabaseService.AddMemberAsync<ExternalPilotModel>(member);
                    externalPilots.Add(ExternalPilot);
                    ExternalPilot = string.Empty;
                }
                else
                {
                    _messagingService.ShowMessage("Invalid External Pilot name", "Adding Chief Technician Failed");
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Adding External Pilot Failed");
            }
        }

        [RelayCommand]
        async Task DeleteExternalPilot()
        {
            try
            {
                if (SelectedExternalPilot != null)
                {
                    ExternalPilotModel member = new(SelectedExternalPilot);
                    await TeamMembersDatabaseService.DeleteMemberAsync<ExternalPilotModel>(member);
                    externalPilots.Remove(SelectedExternalPilot);
                }
                else
                {
                    _messagingService.ShowMessage("External Pilot not selected", "Adding Chief Technician Failed");
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Adding External Pilot Failed");
            }
        }

        [RelayCommand]
        async Task AddUav()
        {
            try
            {
                if (Valid(TailNum) && Valid(UavHours))
                {
                    UavModel uav = new(TailNum, UavHours);
                    await UavDatabaseService.AddUavAsync<UavModel>(uav);
                    uavs.Add(uav);
                    TailNum = string.Empty;
                    UavHours = string.Empty;
                }
                else
                {
                    _messagingService.ShowMessage("Invalid UAV parameters", "Adding Chief Technician Failed");
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Adding UAV Failed");
            }
        }

        [RelayCommand]
        async Task DeleteUav()
        {
            try
            {
                if (SelectedUav != null)
                {                   
                    UavModel uav = new(SelectedUav.TailNum, SelectedUav.Hours);
                    await UavDatabaseService.DeleteUavAsync<UavModel>(uav);
                    uavs.Remove(SelectedUav);
                }
                else
                {
                    _messagingService.ShowMessage("UAV not selected", "Adding Chief Technician Failed");
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Deleting UAV Failed");
            }
        }

        [RelayCommand]
        async Task AddEngine()
        {
            try
            {
                if (Valid(EngineName) && Valid(EngineInitialHours))
                {
                    EngineModel engine = new(EngineName, EngineInitialHours);
                    await EngineDatabaseService.AddEngineAsync<EngineModel>(engine);
                    engines.Add(engine);
                    EngineName = string.Empty;
                    EngineInitialHours = string.Empty;
                }
                else
                {
                    _messagingService.ShowMessage("Invalid Engine parameters", "Adding Chief Technician Failed");
                }

            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Adding Engine Failed");
            }
        }

        [RelayCommand]
        async Task DeleteEngine()
        {
            _messagingService.ShowMessage("It is forbidden to delete engine - might cause problem in flight records hostory", "Forbidden Action");
        }



        [RelayCommand]
        async Task AddComponent()
        {
            try
            {
                if (Valid(ComponentType) && Valid(ComponentSN))
                {
                    ComponentModel component = new(ComponentType, ComponentSN, String.Empty);
                    await ComponentDatabaseService.AddComponentAsync(component);
                    Components.Add(component);
                    ComponentType = string.Empty;
                    ComponentSN = string.Empty;
                }
                else
                {
                    _messagingService.ShowMessage("One or more of typed values is not valid", "Invalid Value");
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Adding Component Failed");
            }
        }

        [RelayCommand]
        async Task DeleteComponent()
        {
            try
            {
                if (SelectedComponent != null)
                {
                    ComponentModel component = new(SelectedComponent.Type, SelectedComponent.SN, SelectedComponent.Tail);
                    await ComponentDatabaseService.DeleteComponentAsync<ComponentModel>(component);
                    Components.Remove(SelectedComponent);
                }
                else
                {
                    _messagingService.ShowMessage("Component not selected", "Adding Chief Technician Failed");
                }
            }
            catch (Exception ex)
            {
                _messagingService.ShowMessage(ex.Message, "Deleting Component Failed");
            }
        }

        private static bool Valid(string property)
        {
            if (property != null && !property.Equals(String.Empty))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
