using System.Threading.Tasks;
using FunctionBuilder.Abstract;
using FunctionBuilder.ViewModels;
using Avalonia.Controls;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using System.Collections.Generic;
using System.Linq;

namespace FunctionBuilder.Services;

public class DialogService : IDialogService
{
    private readonly Window? _mainWindow;
    public DialogService()
    {
        _mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;
    }

    public async Task<string> ShowOpenFileDialogAsync(ViewModelBase parent, string title)
    {
        if (_mainWindow == null)
        {
            return string.Empty;
        }
        var options = new FilePickerOpenOptions();
        options.Title = title;
        options.AllowMultiple = false;
        options.FileTypeFilter = new List<FilePickerFileType>()
            {
                new FilePickerFileType("XML")
                {
                    Patterns = new List<string>(){"xml"}
                }
            };

        var result = await _mainWindow.StorageProvider.OpenFilePickerAsync(options);
        if (result.Count() > 0)
        {
            return result.First().Path.ToString();
        }
        return string.Empty;
    }

    public async Task<string> ShowSaveFileDialogAsync(ViewModelBase parent, string title)
    {
        if (_mainWindow == null)
        {
            return string.Empty;
        }
        var options = new FilePickerSaveOptions();
        options.Title = title;
        options.DefaultExtension = "xml";

        var result = await _mainWindow.StorageProvider.SaveFilePickerAsync(options);
        if (result != null)
        {
            return result.Path.ToString();
        }
        return string.Empty;
    }
}
