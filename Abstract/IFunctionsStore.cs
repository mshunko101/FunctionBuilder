using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Xml.Serialization;

namespace FunctionBuilder.Abstract
{
    public interface IFunctionsStore : INotifyCollectionChanged, IXmlSerializable
    {
        IFunction GetAt(int index);
        IFunction AddNew<T>();
        void Add(IFunction func);
        int Count {get;}
        IEnumerable<IFunction> GetEnumerable();
        void RemoveAll();
    }
}