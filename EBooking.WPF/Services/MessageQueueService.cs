using EBooking.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.Services
{
    public class MessageQueueService
    {
        private readonly MessageQueueStore _messageQueueStore;

        public MessageQueueService(MessageQueueStore messageQueueStore)
        {
            _messageQueueStore = messageQueueStore;
        }

        public void Enqueue(string message)
        {
            _messageQueueStore.SnackbarMessageQueue.Enqueue(message, true);
        }
    }
}
