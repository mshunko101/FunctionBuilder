using System;
using Avalonia.Controls;
using FunctionBuilder.ViewModels;

namespace FunctionBuilder.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.Closing += MainWindow_OnClosing;
    }

    private void MainWindow_OnClosing(object? sender, WindowClosingEventArgs args)
    {
        if(this.DataContext is MainWindowViewModel vm)
        {
            vm.ExitCommand();
        }
        args.Cancel = true;
    }
}