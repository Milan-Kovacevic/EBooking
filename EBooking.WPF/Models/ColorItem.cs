using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EBooking.WPF.Models
{
    public class ColorItem
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public Color Value { get; set; }

        public ColorItem(string key, Color value, string name)
        {
            Key = key;
            Value = value;
            Name = name;
        }
    }
}
