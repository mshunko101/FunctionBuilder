using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
        private static PointViewModel empty;

        public PointViewModel(double _x, double _y)
        {
            X = _x;
            Y = _y;
        }
        public PointViewModel()
        {
            X = 0;
            Y = 0;
        }

        public static PointViewModel Empty
        {
            get
            {
                if(empty == null)
                {
                    empty = new PointViewModel();
                }
                return empty;
            }
        }
    }
}