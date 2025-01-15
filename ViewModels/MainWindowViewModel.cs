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
using Avalonia;

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
    [ObservableProperty]
    private ViewModelBase model;
    [ObservableProperty]
    private double modelOpacity;
    [ObservableProperty]
    private bool modelEnabled;
    [ObservableProperty]
    private double mainViewOpacity;
    [ObservableProperty]
    private bool mainViewEnabled;
    private IDataExporter dataExporter;
    private IDataImporter dataImporter;
    IServiceProvider sp;
    private List<TableFunctionViewModel> tableFunctions;
    private int menuItemCounter;

    public MainWindowViewModel(ChartViewModel chartVm, IFunctionsStore funcsStore, IDataExporter dataExporter, IDataImporter dataImporter, IServiceProvider sp)
    {
        title = string.Empty;
        menuItemCounter = 0;
        chartViewModel = chartVm;
        FunctionsMenuSubItems = new ObservableCollection<MenuItemViewModel>();
        this.funcsStore = funcsStore;
        tableFunctions = new List<TableFunctionViewModel>();
        this.dataExporter = dataExporter;
        this.dataImporter = dataImporter;
        this.sp = sp;
        FuncsStore.AddNew<TableFunction>();
        BuildFunctionsSubMenu();
        ActiveTableFunctionViewModel = tableFunctions.First();
        MainViewOpacity = 1.0;
        ModelOpacity = 0;
        MainViewEnabled = true;
        ModelEnabled = false;
        Model = null;
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
        var filename = await dialogService.ShowSaveFileDialogAsync(this, "Сохранить файл как...");
        dataExporter.Export(FuncsStore, filename, ExportFormat.Xml);
        var modState = FuncsStore.IsModified;
    }

    public async void ImportAllCommand()
    { 
        var dialogService = sp.GetService<IDialogService>();
        var filename = await dialogService.ShowOpenFileDialogAsync(this, "Открыть файл..."); 
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

    public void ExitCommand()
    {
        if(FuncsStore.IsModified)
        {
            ShowDialog(new MessageBoxViewModel(nameof(MainWindowViewModel), CloseDialog) { Title = "Внимание!", Message = "Данные были модифицированы, отменить закрытие программы?" });
            return;
        }
        Environment.Exit(0);
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
            ActiveTableFunctionViewModel = tableFunctions[tableFunctions.Count - 1];
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

    protected void ShowDialog(ViewModelBase vm)
    {
        MainViewOpacity = 0.3;
        ModelOpacity = 1.0;
        MainViewEnabled = false;
        ModelEnabled = true;
        Model = vm;
    }

    protected void CloseDialog(ViewModelBase vm)
    {
        MainViewOpacity = 1.0;
        ModelOpacity = 0;
        MainViewEnabled = true;
        ModelEnabled = false;
        Model = null;
        if (vm is MessageBoxViewModel dvm)
        {
            
        }
    }
}
