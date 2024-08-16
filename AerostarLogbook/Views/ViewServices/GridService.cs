using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Views.ViewServices
{
    public class GridService
    {
        private Dictionary<string, Grid> _grids = new Dictionary<string, Grid>();

        public void RegisterGrid(string key, Grid grid)
        {
            if (!_grids.ContainsKey(key))
            {
                _grids.Add(key, grid);
            }
        }

        public void ShowOnly(string key)
        {
            foreach (var grid in _grids)
            {
                grid.Value.IsVisible = grid.Key == key;
            }
        }

        public void HideAll()
        {
            foreach (var grid in _grids.Values)
            {
                grid.IsVisible = false;
            }
        }

        public void UnregisterGrid(string key)
        {
            if (_grids.ContainsKey(key))
            {
                _grids.Remove(key);
            }
        }
    }
}
