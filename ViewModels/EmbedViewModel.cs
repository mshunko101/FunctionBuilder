using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace FunctionBuilder.ViewModels;

public abstract partial class EmbedViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? _caller;

    protected Action<EmbedViewModel>? _closeCallBack;

    protected EmbedViewModel(string caller, Action<EmbedViewModel> closeCallBack)
    {
        _caller = caller;
        _closeCallBack = closeCallBack;
    }

}
