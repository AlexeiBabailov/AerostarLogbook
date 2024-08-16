using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Models.TeamMembers
{
    [TableName("ExternalPilot")]
    public class ExternalPilotModel
    {
        public string Name { get; set; }
        public ExternalPilotModel() { }
        public ExternalPilotModel(string name)
        {
            Name = name;
        }
    }
}
