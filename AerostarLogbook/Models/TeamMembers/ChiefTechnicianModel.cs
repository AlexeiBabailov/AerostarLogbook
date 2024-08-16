using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Models.TeamMembers
{
    [TableName("ChiefTechnician")]
    public class ChiefTechnicianModel
    {
        public string Name { get; set; }
        public ChiefTechnicianModel() { }
        public ChiefTechnicianModel(string name)
        {
            Name = name;
        }
    }
}
