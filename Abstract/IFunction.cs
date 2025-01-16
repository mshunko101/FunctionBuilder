using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml.Serialization;
using FunctionBuilder.ViewModels;

namespace FunctionBuilder.Abstract;

public interface IFunction : IXmlSerializable, INotifyCollectionChanged, IList<PointViewModel>
{
    string Name { get; }
    IReadOnlyCollection<PointViewModel> PointsData { get; }
    bool Invert();
}
