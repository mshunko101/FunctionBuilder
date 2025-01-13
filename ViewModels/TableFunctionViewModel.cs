using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using FunctionBuilder.Abstract;
using LiveChartsCore.Defaults;

namespace FunctionBuilder.ViewModels
{
    public partial class TableFunctionViewModel : ViewModelBase, IPointsDataSource
    {
        [ObservableProperty]
        private ObservableCollection<ObservablePoint> points;

        public TableFunctionViewModel()
        {
            Points = new ObservableCollection<ObservablePoint>(){new ObservablePoint(0,0)};
        }

        public ICollection<ObservablePoint> GetPointsDataSource()
        {
            return Points;
        }

        public void AddItemCommand()
        {
            Points.Add(new ObservablePoint(5,5));
        }

    }
}