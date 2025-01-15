using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionBuilder.Abstract
{
    public interface IDataImporter
    {
        IFunctionsStore Import(string fileName);
    }
}