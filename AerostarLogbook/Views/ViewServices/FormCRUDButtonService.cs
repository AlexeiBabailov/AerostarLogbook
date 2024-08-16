using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Views.ViewServices
{
    internal class FormCRUDButtonService
    {
        private Button[] _buttons = new Button[3];

        //public ViewButtonService(Color defaultColor)
        //{
        //    _defaultColor = defaultColor;
        //}

        public void RegisterFormButtons(Button addButton, Button updateButton, Button deleteButton)
        {
            _buttons[0] = addButton;
            _buttons[1] = updateButton;
            _buttons[2] = deleteButton;
        }

        public void UnregisterFromButtons()
        {
            Array.Clear(_buttons, 0, _buttons.Length);
        }

        public void Form_LocalAllIsVisible()
        {
            _buttons[0].IsVisible = true;
            _buttons[1].IsVisible = true;
            _buttons[2].IsVisible = true;
        }

        public void Form_LocalAddIsVisible()
        {
            _buttons[0].IsVisible = true;
            _buttons[1].IsVisible = false;
            _buttons[2].IsVisible = false;
        }

        public void Form_LocalUpdateDeleteIsVisible()
        {
            _buttons[0].IsVisible = false;
            _buttons[1].IsVisible = true;
            _buttons[2].IsVisible = true;
        }

        public void Form_LocalNoneIsVisible()
        {
            _buttons[0].IsVisible = false;
            _buttons[1].IsVisible = false;
            _buttons[2].IsVisible = false;
        }


    }
}
