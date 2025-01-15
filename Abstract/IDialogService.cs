using System.Threading.Tasks;
using FunctionBuilder.ViewModels;
namespace FunctionBuilder.Abstract;

public interface IDialogService
{
    Task<string> ShowOpenFileDialogAsync(ViewModelBase parent, string title);
    Task<string> ShowSaveFileDialogAsync(ViewModelBase parent, string title);
}
