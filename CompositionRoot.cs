using FunctionBuilder.Abstract;
using FunctionBuilder.ViewModels;
using Unity;
using Unity.Injection;

public static class CompositionRoot
{
    public static IUnityContainer Compose()
    {
        IUnityContainer container = new UnityContainer();

        container.RegisterSingleton<MainWindowViewModel>();
        container.RegisterSingleton<ChartViewModel>();
        container.RegisterSingleton<TableFunctionViewModel>();

        return container;
    }
}