using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionBuilder.Abstract
{
    public enum ExportFormat
    {
        Undefined,
        Xml,
        Custom,
    }

    public interface IDataExporter
    {
        void Export(IFunctionsStore funcs, string fileName, ExportFormat format);
    }
}