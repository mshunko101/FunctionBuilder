namespace FunctionBuilder.Abstract;

public enum ExportFormat
{
    Undefined,
    Xml,
}

public interface IDataExporter
{
    void Export(IFunctionsStore funcs, string fileName, ExportFormat format);
}