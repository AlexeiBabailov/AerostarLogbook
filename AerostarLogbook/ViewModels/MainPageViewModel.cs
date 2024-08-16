using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AerostarLogbook.Services;
using AerostarLogbook.Models;
using System.Diagnostics;
using AerostarLogbook.Services.DatabaseMiscellaneous;


namespace AerostarLogbook.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {

        [ObservableProperty]
        public static ObservableCollection<UavModel> uavsMP = new();
        
        [ObservableProperty]
        public static ObservableCollection<string> check = new();
        
        [ObservableProperty]
        public string selectedUAV;

        public static string staticSelectedUAV;
        public MainPageViewModel()
        {
            LoadUavsAsync();



        }

        public static async Task LoadUavsAsync()
        {

            uavsMP = await UavDatabaseService.GetUavsAsync();
            foreach (UavModel uav in uavsMP)
            {
                check.Add(uav.TailNum);
            }
        }

        [RelayCommand]
        void SelectionChanged()
        {
            staticSelectedUAV = SelectedUAV;
        }

    }

}
