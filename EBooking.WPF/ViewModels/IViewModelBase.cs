using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.ViewModels
{
    public interface IViewModelBase
    {
        public string GetId() => string.Empty;

        public void Dispose() { }

        public bool CanNavigateFrom() => true;
    }
}
