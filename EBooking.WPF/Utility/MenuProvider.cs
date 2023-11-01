using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Utility
{
    internal class MenuProvider
    {
        public enum MenuItem
        {
            LOGIN,
            REGISTER,
            SETTINGS,
            ACCOMMODATIONS,
            FLIGHTS,
            CODEBOOK
        }

        private static readonly Dictionary<MenuItem, string> menuItemCodes;

        private MenuProvider() { }

        static MenuProvider()
        {
            menuItemCodes = new Dictionary<MenuItem, string>()
            {
                { MenuItem.LOGIN, "Menu_LoginTitle" },
                { MenuItem.REGISTER, "Menu_RegisterTitle" },
                { MenuItem.ACCOMMODATIONS, "Menu_AccommodationsTitle" },
                { MenuItem.FLIGHTS, "Menu_FlightsTitle" },
                { MenuItem.CODEBOOK, "Menu_CodebookTitle" },
                { MenuItem.SETTINGS, "Menu_SettingsTitle" },
            };
        }

        public static string GetCode(MenuItem item)
        {
            if (!menuItemCodes.ContainsKey(item))
                return string.Empty;

            return menuItemCodes[item];
        }

        public static string GetCodeByIndex(int index)
        {
            if (index < 0 || index >= menuItemCodes.Count)
                return string.Empty;

            return menuItemCodes.ElementAt(index).Value;
        }

        public static int GetIndexOfCode(string code)
        {
            for (int i = 0; i < menuItemCodes.Count; i++)
            {
                if (menuItemCodes.ElementAt(i).Value == code)
                    return i;
            }
            return -1;
        }
    }
}
