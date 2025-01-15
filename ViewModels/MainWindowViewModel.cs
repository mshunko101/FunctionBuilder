using CommunityToolkit.Mvvm.ComponentModel;
using FunctionBuilder.Abstract;
using System.Collections.Generic;
using FunctionBuilder.Services;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Avalonia.Controls;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace FunctionBuilder.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ChartViewModel chartViewModel;
    [ObservableProperty]
    private TableFunctionViewModel activeTableFunctionViewModel;
    [ObservableProperty]
    private IFunctionsStore funcsStore;
    [ObservableProperty]
    private ObservableCollection<MenuItemViewModel> functionsMenuSubItems;
    [ObservableProperty]
    private string title;
    private IDataExporter dataExporter;
    private IDataImporter dataImporter;
    IServiceProvider sp;
    private List<TableFunctionViewModel> tableFunctions;
    private int selectedFunction;
    private int menuItemCounter;

    public MainWindowViewModel(ChartViewModel chartVm, IFunctionsStore funcsStore, IDataExporter dataExporter, IDataImporter dataImporter, IServiceProvider sp)
    {
        menuItemCounter = 0;
        chartViewModel = chartVm;
        this.funcsStore = funcsStore;
        selectedFunction = 0; 
        tableFunctions = new List<TableFunctionViewModel>();
        this.dataExporter = dataExporter;
        this.dataImporter = dataImporter;
        this.sp = sp;
        FuncsStore.AddNew<TableFunction>();
        BuildFunctionsSubMenu();
        ActiveTableFunctionViewModel = tableFunctions.First();
    }

    private void BuildFunctionsSubMenu()
    {
        menuItemCounter = 0;
        var addFuncMenuItem = new MenuItemViewModel{ Description = "_Добавить функцию", Index = 0, IsChecked = false};
        FunctionsMenuSubItems = new ObservableCollection<MenuItemViewModel>(){addFuncMenuItem};
        foreach(var f in FuncsStore.GetEnumerable())
        {
            AddTableFunctionToView(f);
        }
        FunctionsMenuSubItems[FunctionsMenuSubItems.Count - 1].IsChecked = true;
        Title = $"Построитель функций - Функция {FunctionsMenuSubItems.Count - 1}";
    }

    public async void ExportAllCommand()
    { 
        var dialogService = sp.GetService<IDialogService>();
        var fileExtensions = new List<string>() { $"XML File|*.xml" };
        var extensionFilter = new FileDialogFilter() { Extensions = fileExtensions , Name="name of unknown"};
        var filename = await dialogService.ShowSaveFileDialogAsync(this, "Сохранить файл как...", extensionFilter);
        dataExporter.Export(funcsStore, filename, ExportFormat.Xml);
    }

    public async void ImportAllCommand()
    {
        var dialogService = sp.GetService<IDialogService>();
        var fileExtensions = new List<string>() { $"xml" };
        var extensionFilter = new FileDialogFilter() { Extensions = fileExtensions , Name = "XML File|*.xml"};
        var filename = await dialogService.ShowOpenFileDialogAsync(this, "Открыть файл...", extensionFilter); 
        var funcs = dataImporter.Import(filename);
        FuncsStore.RemoveAll();
        foreach(var f in funcs.GetEnumerable())
        {
            FuncsStore.Add(f);
        }
        tableFunctions.Clear();
        BuildFunctionsSubMenu();
        ActiveTableFunctionViewModel = tableFunctions[tableFunctions.Count - 1];
    }

    public void ExportFuncCommand()
    {

    }

    public void ImportFuncCommand()
    {

    }

    public void FunctionMenuCommand(object param)
    {
        foreach(var item in FunctionsMenuSubItems)
        {
            item.IsChecked = false;
        } 

        int index = (int)param;
        if(index == 0)
        {
            var func = funcsStore.AddNew<TableFunction>();
            AddTableFunctionToView(func);
            FunctionsMenuSubItems[FunctionsMenuSubItems.Count - 1].IsChecked = true;
            Title = $"Построитель функций - Функция {FunctionsMenuSubItems.Count - 1}";
        }
        else
        {
            ActiveTableFunctionViewModel = tableFunctions[index - 1];
            Title = $"Построитель функций - Функция {index}";
        }

    } 

    private TableFunctionViewModel AddTableFunctionToView(IFunction func)
    { 
        var addFuncMenuItem = new MenuItemViewModel{Index = ++menuItemCounter, Description = $"Функция _{menuItemCounter}", };
        FunctionsMenuSubItems.Add(addFuncMenuItem);
        var editTableVm = new TableFunctionViewModel(func, sp.GetService<IClipBoardService>());
        tableFunctions.Add(editTableVm);
        return editTableVm;
    }
}
