using AerostarLogbook.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Services
{
    public class MessagingService : IMessagingService
    {
        public void ShowMessage(string message, string title = "Error")
        {
            // MainThread ensures that UI changes are made on the main thread
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.DisplayAlert(title, message, "OK");
            });
        }
    }
}
