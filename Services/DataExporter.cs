using System;
using FunctionBuilder.Abstract;
using System.Xml;
using System.Xml.Serialization;
using FunctionBuilder.Utils;

namespace FunctionBuilder.Services;

public class DataExporter : IDataExporter, IDataImporter
{
    protected void ExportToTxt(IFunctionsStore funcs, string fileName)
    {
        XmlSerializer serializer = new XmlSerializer(funcs.GetType());
        XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
        XmlWriter writer = XmlWriter.Create(fileName, settings);
        serializer.Serialize(writer, funcs);
        writer.Close();
    }

    protected IFunctionsStore ImportFormTxt(string fileName)
    {
        XmlReader reader = XmlReader.Create(fileName);
        XmlSerializer serializer = reader.GetApproapriateSerializator<IFunctionsStore>();
        var store =serializer.Deserialize(reader) as IFunctionsStore ?? throw new ArgumentException("Ошибка импорта");
        reader.Close();
        return store;
    }

    public void Export(IFunctionsStore funcs, string fileName, ExportFormat format)
    {
        switch (format)
        {
            case ExportFormat.Xml:
                ExportToTxt(funcs, fileName);
                break;
            default:
                throw new NotImplementedException("Экспорт формата, не реализован");
        }
    }

    public IFunctionsStore Import(string fileName)
    {
        return ImportFormTxt(fileName);
    }
}
