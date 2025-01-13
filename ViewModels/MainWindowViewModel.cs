using CommunityToolkit.Mvvm.ComponentModel;

namespace FunctionBuilder.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ChartViewModel chartViewModel;
    [ObservableProperty]
    private TableFunctionViewModel tableFunctionViewModel;

    public MainWindowViewModel(ChartViewModel chartVm, TableFunctionViewModel tableVm)
    {
        chartViewModel = chartVm;
        tableFunctionViewModel = tableVm;
    }
}
