using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Interfaces
{
    public interface IMessagingService
    {
        void ShowMessage(string message, string title = "Error");
    }
}
