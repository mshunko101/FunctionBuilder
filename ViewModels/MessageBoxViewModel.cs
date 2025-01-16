using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace FunctionBuilder.ViewModels;

public partial class MessageBoxViewModel : EmbedViewModel
{
    [ObservableProperty]
    private string? title;
    [ObservableProperty]
    private string? message;
    [ObservableProperty]
    private bool result;

    public MessageBoxViewModel(string caller, Action<EmbedViewModel> closeCallBack)
    : base(caller, closeCallBack)
    {

    }

    public void OkCommand()
    {
        Result = true;
        _closeCallBack?.Invoke(this);
    }

    public void CancelCommand()
    {
        Result = false;
        _closeCallBack?.Invoke(this);
    }
}