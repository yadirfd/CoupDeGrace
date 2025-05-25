using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using lasthope.Entities;
using lasthope.Views;

namespace lasthope.Controllers
{
    public class MenuController
    {
        private readonly MenuView _view;
        private readonly List<string> _items;
        private readonly InputHandler _input;
        private int _selectedIndex;
        
        public MenuController(MenuView view, List<string> items, InputHandler input)
        {
            _view = view;
            _items = items;
            _input = input;
            _selectedIndex = 0;
        }

        // Возвращает true, когда игрок выбрал пункт и нажал Enter
        public bool Update()
        {
            if (_input.IsNewKey(Keys.Up))
                _selectedIndex = (_selectedIndex - 1 + _items.Count) % _items.Count;
            else if (_input.IsNewKey(Keys.Down))
                _selectedIndex = (_selectedIndex + 1) % _items.Count;
            else if (_input.IsNewKey(Keys.Enter))
                return true;
            return false;
        }

        public void Draw()
        {
            _view.Draw(_selectedIndex);
        }

        public bool IsPvC => _selectedIndex == 1;
    }
}