using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using lasthope.Entities;
using lasthope.Views;

namespace lasthope.Controllers
{
    public class MenuController(MenuView view, List<string> items, InputHandler input)
    {
        private int _selectedIndex = 0;

        // Возвращает true, когда игрок выбрал пункт и нажал Enter
        public bool Update()
        {
            input.Update();
            if (input.IsNewKey(Keys.Up))
                _selectedIndex = (_selectedIndex - 1 + items.Count) % items.Count;
            else if (input.IsNewKey(Keys.Down))
                _selectedIndex = (_selectedIndex + 1) % items.Count;
            else if (input.IsNewKey(Keys.Enter))
                return true;
            return false;
        }

        public void Draw()
        {
            view.Draw(_selectedIndex);
        }

        public bool IsPvC => _selectedIndex == 1;
    }
}