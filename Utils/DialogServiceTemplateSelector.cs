using System;
using System.Collections.Generic;
using Avalonia.Controls.Templates;
using Avalonia.Controls;
using Avalonia.Metadata;

namespace FunctionBuilder.Utils;

public class DialogServiceTemplateSelector : IDataTemplate
{
    [Content]
    public Dictionary<string, IDataTemplate> AvailableTemplates { get; } = new Dictionary<string, IDataTemplate>();
    public Control Build(object? param)
    {
        var key = param?.ToString();
        if (key is null)
        {
            throw new ArgumentNullException(nameof(param));
        }
        if (AvailableTemplates.TryGetValue(key, out var model))
        {
            return model.Build(param) ?? throw new ArgumentNullException(nameof(param)); ;
        }
        throw new ArgumentNullException(nameof(param));
    }

    public bool Match(object? data)
    {
        var key = string.Empty;
        if (object.ReferenceEquals(data, null))
        {
            throw new ArgumentNullException(nameof(data));
        }
        else
        {
            key = data.ToString() ?? throw new ArgumentNullException(nameof(data));
        }
        return AvailableTemplates.ContainsKey(key);
    }
}
