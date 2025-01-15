using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using FunctionBuilder.Abstract;
using System.Collections.Generic;
using FunctionBuilder.Services;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using FunctionBuilder.ViewModels;
using Avalonia.Controls;
namespace FunctionBuilder.Abstract
{
    public interface IDialogService
    {
        Task<string> ShowOpenFileDialogAsync(ViewModelBase parent, string title, FileDialogFilter filter);
        Task<string> ShowSaveFileDialogAsync(ViewModelBase parent, string title, FileDialogFilter filter);
    }
}