using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using FunctionBuilder.Abstract;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView.Drawing;
using System;
using LiveChartsCore.Kernel;
using Avalonia.Input;
using System.Collections.Generic;
using System.Globalization;

namespace FunctionBuilder.ViewModels;
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
                    ShowSeparatorLines = true,
                    MinStep = 3,
                    Name = "Температура [°C]",
                    SubticksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
                    SubseparatorsCount = 10,
                },
            };

        YAxes = new[]
        {
                new Axis()
                {
                    LabelsPaint = new SolidColorPaint(SKColors.Black),
                    MaxLimit = 0,
                    MinLimit = -10000,
                    Position = LiveChartsCore.Measure.AxisPosition.Start,
                    SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
                    TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
                    Name = "Абсолютная отметка [мм]",
                    SubticksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
                    SubseparatorsCount = 10,
                },
        };
    }

    public void ChartPointPointerDownCommand(ChartPoint args)
    {
        if (args != null && args.Context != null && args.Context.DataSource is PointViewModel point)
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
        if (_isPointClicked)
        {
            if (_draggedPoint != null)
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
        if(e == null)
        {
            return;
        }
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems)
                    {
                        var function = (IFunction)item;
                        if(function.PointsData is IList<PointViewModel> list)
                        {
                            Series.Add(new LineSeries<PointViewModel>(list)
                            {
                                GeometrySize = 5,
                                Fill = null,
                                DataLabelsSize = 0,
                                DataLabelsPaint = null,
                                LineSmoothness = 0,
                                Mapping = (sample, index) => new(sample.X, sample.Y),
                                Tag = function,
                                Name = $"{function.Name} {e.NewStartingIndex + 1}",
                                XToolTipLabelFormatter = null, 
                                YToolTipLabelFormatter = point => $"({point.Coordinate.PrimaryValue.ToString(CultureInfo.InvariantCulture)}, {point.Coordinate.SecondaryValue.ToString(CultureInfo.InvariantCulture)})",
                                DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top
                            });
                        }
                    }
                }
                break;
            case NotifyCollectionChangedAction.Remove:
                if (e.OldItems != null)
                {
                    foreach (var item in e.OldItems)
                    {
                        var function = (IFunction)item;
                        var serie = Series.First(i => i.Tag == function);
                        Series.Remove(serie);
                    }
                }
                break;
            case NotifyCollectionChangedAction.Reset:
                Series.Clear();
                break;
        }
    }
}