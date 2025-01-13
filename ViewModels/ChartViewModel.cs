using System.Diagnostics;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using FunctionBuilder.Abstract;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace FunctionBuilder.ViewModels
{
    public partial class ChartViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ISeries[] series;
        [ObservableProperty]
        private Axis[] xAxes;
        private IPointsDataSource dataSource;

        public ChartViewModel(TableFunctionViewModel _dataSource)
        {
            dataSource = _dataSource;
            Series = new ISeries[]
            {
                new LineSeries<ObservablePoint>(dataSource.GetPointsDataSource())
                {
                    DataLabelsSize = 20,
                    DataLabelsPaint = new SolidColorPaint(SKColors.Blue),
                    DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top
                    ,
                }
            }; 
            
            XAxes = new[]
            {
                new Axis(),
            };
            var ss = dataSource.GetPointsDataSource();
            ss.Add(new ObservablePoint(0,0.2));
        }
    }
}