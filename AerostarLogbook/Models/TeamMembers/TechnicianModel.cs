using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Models.TeamMembers
{
    [TableName("Technician")]
    public class TechnicianModel
    {
        public string Name { get; set; }
        public TechnicianModel() { }
        public TechnicianModel(string name)
        {
            Name = name;
        }
    }
}