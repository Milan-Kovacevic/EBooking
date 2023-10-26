using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.WPF.ViewModels
{
    public partial class LoginViewModel : ObservableObject, IViewModelBase
    {
        public string Key => nameof(LoginViewModel);
    }
}
