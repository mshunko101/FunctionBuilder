using FunctionBuilder.Abstract;
using FunctionBuilder.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using FunctionBuilder.Services;
using System;
 

public static class CompositionRoot
{
    public static IServiceProvider Compose()
    {
        var services = new ServiceCollection();
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<ChartViewModel>();
        services.AddSingleton<TableFunctionViewModel>();
        services.AddScoped<IDialogService, DialogService>();
        services.AddScoped<IDataExporter, DataExporter>();
        services.AddScoped<IDataImporter, DataExporter>();
        services.AddScoped<IFunctionsStore,FunctionsStore>();
        services.AddScoped<IClipBoardService, ClipBoardService>();

        return services.BuildServiceProvider();
    }

}