using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Messages
{
    public record class LanguageChangeMessage
    {
        public string LanguageCode { get; set; }

        public LanguageChangeMessage(string languageCode)
        {
            LanguageCode = languageCode;
        }
    }
}
