using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace FunctionBuilder.ViewModels
{
    public partial class PointViewModel : ViewModelBase
    {
        [ObservableProperty]
        private double x;
        [ObservableProperty]
        private double y;
        public PointViewModel(double _x, double _y)
        {
            X = _x;
            Y = _y;
        }
    }
}