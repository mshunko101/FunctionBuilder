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
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

namespace FunctionBuilder.Services;

    public class DialogService : IDialogService
    {
        private readonly Window _mainWindow;
        public DialogService()
        {
            _mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;
        }

        

        public async Task<string> ShowOpenFileDialogAsync(ViewModelBase parent, string title, FileDialogFilter filter)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Title = title,
                AllowMultiple = false
            };
            openFileDialog.Filters.Add(filter);
            var result = await openFileDialog.ShowAsync(_mainWindow);
            return result[0];
        }

        public async Task<string> ShowSaveFileDialogAsync(ViewModelBase parent, string title, FileDialogFilter filter)
        {
            var saveFileDialog = new SaveFileDialog()
            {
                Title = title
            };
            saveFileDialog.Filters.Add(filter);
            return await saveFileDialog.ShowAsync(_mainWindow);
        }
    }
