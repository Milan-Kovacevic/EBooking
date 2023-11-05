using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Messages
{
    public record class DeleteSelectedDialogResultMessage
    {
        public bool DialogResult { get; set; }

        public DeleteSelectedDialogResultMessage(bool dialogResult)
        {
            DialogResult = dialogResult;
        }
    }
}
