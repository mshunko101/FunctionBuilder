using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FunctionBuilder.ViewModels;
using LiveChartsCore.Defaults;

namespace FunctionBuilder.Abstract
{
    public interface IPointsDataSource
    {
        ICollection<ObservablePoint> GetPointsDataSource();
    }
}