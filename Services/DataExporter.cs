using System;
using System.Collections.Generic;
using System.Linq;
using FunctionBuilder.Abstract;
using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using LiveChartsCore.Defaults;
using FunctionBuilder.ViewModels;
using System.Runtime.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Text;
using FunctionBuilder.Utils;


namespace FunctionBuilder.Services
{
    public class DataExporter:IDataExporter, IDataImporter
    {
        public DataExporter()
        {

        }

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
            var store = (IFunctionsStore)serializer.Deserialize(reader)!;
            reader.Close();
            return store!;
        }

        protected void ExportToCustom(IEnumerable<IFunction> funcs,string fileName)
        {
             
        }

        public void Export(IFunctionsStore funcs, string fileName, ExportFormat format)
        {
            switch (format)
            {
                case ExportFormat.Xml:
                    ExportToTxt(funcs, fileName);
                break;
                case ExportFormat.Custom:
                    //ExportToCustom(funcs, fileName);
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
}