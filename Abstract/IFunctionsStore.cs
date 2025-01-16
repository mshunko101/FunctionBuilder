using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml.Serialization;

namespace FunctionBuilder.Abstract;

public interface IFunctionsStore : INotifyCollectionChanged, IXmlSerializable, IEnumerable<IFunction>
{
    IFunction AddNew<T>();
    void Add(IFunction func);
    IFunction GetAt(int index);
    int Count { get; }
    void RemoveAll();
}