using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using FunctionBuilder.Abstract;

namespace FunctionBuilder.ViewModels;
public partial class TableFunctionViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<PointViewModel> points;
    [ObservableProperty]
    private ObservableCollection<PointViewModel> selectedItems;
    private IClipBoardService cs;


    public TableFunctionViewModel(IFunction dataSource, IClipBoardService cs)
    {
        this.cs = cs;
        var ds = dataSource.PointsData;
        SelectedItems = new ObservableCollection<PointViewModel>();
        if (ds is ObservableCollection<PointViewModel> ocDs)
        {
            Points = ocDs;
        }
        else
        {
            throw new ArgumentException($"Неверный тип аргумента {nameof(dataSource)}");
        }
    }

    public async void MenuPasteCommand()
    {
        var items = await cs.Fetch();
        var index = Points.IndexOf(SelectedItems.LastOrDefault()!);
        if (index != -1)
        {
            foreach (var item in items)
            {
                Points.Insert(index++, item);
            }
        }
        else
        {
            foreach (var item in items)
            {
                Points.Add(item);
            }
        }
    }

    public async void MenuCopyCommand()
    {
        await cs.Put(Points);
    }

    public void AddItemCommand()
    {
        var index = Points.IndexOf(SelectedItems.FirstOrDefault()!);
        if (index != -1)
        {   
            if(index == Points.Count - 1)
            {
                Points.Add(new PointViewModel(0, 0));
            }
            else
            {
                Points.Insert(index, new PointViewModel(0, 0));
            }
        }
        else
        {
            Points.Add(new PointViewModel(0, 0));
        }
    }

    public void DelItemCommand()
    {
        var array = SelectedItems.ToArray();
        foreach (var item in array)
        {
            var index = Points.IndexOf(item);
            if (index != -1)
            {
                Points.RemoveAt(index);
            }
        }
    }
}