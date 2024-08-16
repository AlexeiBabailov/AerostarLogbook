using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Models
{
    [TableName("Engines")]
    public class EngineModel
    {
        public EngineModel(string name, string initialHours)
        {
            Engine = name;
            Hours = initialHours;
            InitialHours = initialHours;
        }
        public EngineModel() { }
        public string Engine { get; set; }
        public string Hours  { get; set; }
        public string InitialHours { get; set; }

    }
}
