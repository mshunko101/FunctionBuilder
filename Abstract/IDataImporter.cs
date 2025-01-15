namespace FunctionBuilder.Abstract;
public interface IDataImporter
{
    IFunctionsStore Import(string fileName);
}