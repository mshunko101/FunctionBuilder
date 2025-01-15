using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using FunctionBuilder.Abstract;
using FunctionBuilder.ViewModels;

namespace FunctionBuilder.Services;

public class ClipBoardService : IClipBoardService
{
    private string SupportedFormat = "Text";
    public async Task<IEnumerable<PointViewModel>> Fetch()
    {
        var win = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;

        if (win != null)
        {
            var cp = win.Clipboard;
            if(cp == null)
            {
                return Enumerable.Empty<PointViewModel>();
            }
            var cpFormats = await cp.GetFormatsAsync();
            var supported = cpFormats.Contains(SupportedFormat);
            if (supported)
            {
                var data = await cp.GetDataAsync(SupportedFormat);
                if(data != null)
                {
                    return ParseText(data.ToString());
                }
            }
            return Enumerable.Empty<PointViewModel>();
        }
        return Enumerable.Empty<PointViewModel>();
    }

    public async Task<bool> Put(IEnumerable<PointViewModel> data)
    {
        StringBuilder result = new StringBuilder();
        foreach (var point in data)
        {
            var line = $"{point.X}\t{point.Y}";
            result.AppendLine(line);
        }

        var win = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;
        if (win != null)
        {
            var cp = win.Clipboard;
            if (cp != null)
            {
                await cp.ClearAsync();
                await cp.SetTextAsync(result.ToString());
                return true;
            }
            return false;
        }
        return false;
    }

    private IEnumerable<PointViewModel> ParseText(string? data)
    {
        if(string.IsNullOrEmpty(data))
        {
            yield break;
        }
        var lines = data.Split('\n');
        foreach (var line in lines)
        {
            var parts = line.Split('\t');
            if (parts.Count() > 1)
            {
                var xs = parts[0].Replace(",", ".");
                var ys = parts[1].Replace(",", ".");
                var x = double.Parse(xs, CultureInfo.InvariantCulture);
                var y = double.Parse(ys, CultureInfo.InvariantCulture);
                yield return new PointViewModel(x, y);
            }
        }
    }
}
