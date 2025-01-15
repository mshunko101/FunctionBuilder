using System.Collections;
using Avalonia;
using Avalonia.Controls;

namespace FunctionBuilder.Utils
{
    public class DataGridAdditionalProperties : AvaloniaObject
    {
        public static readonly AttachedProperty<object> SelectedItemsProperty = 
        AvaloniaProperty.RegisterAttached<DataGrid, Control, object>("SelectedItems");
        static object currValue;

        static DataGridAdditionalProperties()
        {
            SelectedItemsProperty.Changed.AddClassHandler<DataGrid>(HandleSelectedItemsPropertyChanged);
        }

        /// <summary>
        /// <see cref="CommandProperty"/> changed event handler.
        /// </summary>
        private static void HandleSelectedItemsPropertyChanged(DataGrid sender, AvaloniaPropertyChangedEventArgs args)
        { 
            if (currValue != args.NewValue)
            {
                sender.SelectionChanged += OnSelectionChanged;
            }
            else
            {
                 sender.SelectionChanged -= OnSelectionChanged;
            }
            currValue = args.NewValue;
        }

        private static void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if(sender is DataGrid dataGrid)
            {
                IList list = dataGrid.GetValue(SelectedItemsProperty) as IList;
                if(list != null)
                {
                    list.Clear();
                    foreach(var item in dataGrid.SelectedItems)
                    {
                        list.Add(item);
                    }
                }
            }
        }

        public static object GetSelectedItems(DataGrid element)
        {
            return element.GetValue(SelectedItemsProperty);
        }

        public static void SetSelectedItems(DataGrid element, object value)
        {
            element.SetValue(SelectedItemsProperty, value);
        }
        
    }
  
}