using System.Diagnostics;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using FunctionBuilder.Abstract;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.Drawing;
using System;
using LiveChartsCore.Kernel;
using ExtendedXmlSerializer;

namespace FunctionBuilder.ViewModels
{
    public partial class ChartViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<ISeries> series;
        [ObservableProperty]
        private Axis[] xAxes;
        [ObservableProperty]
        private Axis[] yAxes;
        private IFunctionsStore funcsStore;
        private bool _isPointClicked;
        private PointViewModel? _draggedPoint;

        private SKColor[] seriesColors = new SKColor[]
        {
            new SKColor(255, 0, 0),
            new SKColor(0, 255, 0),
            new SKColor(0, 0, 255),
            new SKColor(255, 255, 0),
            new SKColor(0, 255, 255),
            new SKColor(127, 255, 200),
            new SKColor(255, 127, 0),
            new SKColor(50, 127, 255),
        };

        public ChartViewModel(IFunctionsStore _funcsStore)
        {
            _draggedPoint = PointViewModel.Empty;
            funcsStore = _funcsStore;
            funcsStore.CollectionChanged += OnFunctionsCollectionsChanged;
            Series = new ObservableCollection<ISeries>();
 
            XAxes = new[]
            {
                new Axis()
                {   
                    LabelsPaint = new SolidColorPaint(SKColors.Black),
                    IsInverted = true,
                    MaxLimit = 10,
                    MinLimit = 0,
                    Position = LiveChartsCore.Measure.AxisPosition.End,
                    SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
                    TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
                    ForceStepToMin = true, 
                    MinStep = 1 
                },
            };

            YAxes = new[]
            {
                new Axis()
                {   
                    LabelsPaint = new SolidColorPaint(SKColors.Black),
                    IsInverted = true,
                    MaxLimit = 10000,
                    MinLimit = 0,
                    SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
                    TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
                },
            };
        }

        public void ChartPointPointerDownCommand(ChartPoint args)
        { 
            if(args.Context.DataSource is PointViewModel point)
            {
                _draggedPoint = point;
                _isPointClicked = true;
            }
        }

        public void PointerReleasedCommand(PointerCommandArgs args)
        {
            _isPointClicked = false;
        }

        public void PointerMoveCommand(PointerCommandArgs args)
        { 
            if(_isPointClicked)
            {
                if(_draggedPoint != null)
                {
                    var chart = (ICartesianChartView<SkiaSharpDrawingContext>)args.Chart;
                    var positionInData = chart.ScalePixelsToData(args.PointerPosition);
                    _draggedPoint.X = Math.Round(positionInData.X, 3);
                    _draggedPoint.Y = Math.Round(positionInData.Y, 0);
                }
            }
        }

        private void OnFunctionsCollectionsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach(var item in e.NewItems)
                    {
                        var function = (IFunction)item;
                        Series.Add(new LineSeries<PointViewModel>(function.PointsData)
                        {
                            DataLabelsSize = 20,
                            DataLabelsPaint = new SolidColorPaint(seriesColors[Series.Count]),
                            LineSmoothness = 0,
                            Mapping = (sample, index) => new(sample.X, sample.Y),
                            Tag = function
                        });
                    }
                break;
                case NotifyCollectionChangedAction.Remove:
                    foreach(var item in e.OldItems)
                    {
                        var function = (IFunction)item;
                        var serie = Series.First(i => i.Tag == function);
                        Series.Remove(serie);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Series.Clear();
                    break;
            }
        }
    }
}