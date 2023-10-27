using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Stores
{
    public class MessageQueueStore
    {
        private ISnackbarMessageQueue _snackbarMessageQueue;
        public ISnackbarMessageQueue SnackbarMessageQueue
        {
            get => _snackbarMessageQueue;
            set
            {
                _snackbarMessageQueue = value;
            }
        }

        public MessageQueueStore()
        {
            _snackbarMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2))
            {
                DiscardDuplicates = true,
            };
        }
    }
}
