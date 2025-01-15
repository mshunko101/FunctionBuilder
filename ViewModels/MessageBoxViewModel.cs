using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace FunctionBuilder.ViewModels;

public partial class MessageBoxViewModel : EmbedViewModel
{
    [ObservableProperty]
    private string? title;
    [ObservableProperty]
    private string? message;

    public MessageBoxViewModel(string caller, Action<EmbedViewModel> closeCallBack)
    : base(caller, closeCallBack)
    {

    }

    public void CloseCommand()
    {
        _closeCallBack?.Invoke(this);
    }
}