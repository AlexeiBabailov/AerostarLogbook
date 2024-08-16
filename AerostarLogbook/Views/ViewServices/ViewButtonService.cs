using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Views.ViewServices
{
    public class ViewButtonService
    {
        private List<Button> _buttons = new List<Button>();
        private Color _defaultColor;

        public ViewButtonService()
        {
            _defaultColor = Color.FromRgb(71, 86, 109);
        }

        public void RegisterButton(Button button)
        {
            _buttons.Add(button);
            button.Clicked += HandleButtonClicked;
        }

        private void HandleButtonClicked(object sender, EventArgs e)
        {
            var clickedButton = sender as Button;

            if (clickedButton != null)
            {
                foreach (var button in _buttons)
                {
                    button.FontAttributes = FontAttributes.None;
                    button.BackgroundColor = _defaultColor;
                }

                clickedButton.FontAttributes = FontAttributes.Bold;
                clickedButton.BackgroundColor = Color.FromRgb(38, 122, 169);
            }
        }

        public void NoneButtonClicked()
        {
            foreach (var button in _buttons)
            {
                button.FontAttributes = FontAttributes.None;
                button.BackgroundColor = _defaultColor;
            }
        }

        public void UnregisterButton(Button button)
        {
            if (_buttons.Contains(button))
            {
                button.Clicked -= HandleButtonClicked;
                _buttons.Remove(button);
            }
        }
    }
}
