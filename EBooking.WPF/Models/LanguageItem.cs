using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace EBooking.WPF.Models
{
    public class LanguageItem
    {
        public string Key { get; init; }
        public string Name { get; set; }

        public LanguageItem(string key, string name)
        {
            Key = key;
            Name = name;
        }
    }
}
