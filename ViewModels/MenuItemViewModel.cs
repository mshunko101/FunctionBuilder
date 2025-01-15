using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace FunctionBuilder.ViewModels
{
    public partial class MenuItemViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string description;
        [ObservableProperty]
        private int index;
        [ObservableProperty]
        private bool isChecked;
    }
}