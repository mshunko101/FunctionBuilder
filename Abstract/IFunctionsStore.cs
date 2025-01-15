using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml.Serialization;

namespace FunctionBuilder.Abstract;

public interface IFunctionsStore : INotifyCollectionChanged, IXmlSerializable, IEnumerable<IFunction>
{
    IFunction AddNew<T>();
    void Add(IFunction func);
    int Count { get; }
    void RemoveAll();
    bool IsModified { get; }
}