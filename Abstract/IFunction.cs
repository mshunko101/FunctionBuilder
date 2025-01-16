using System.Collections.Generic;
using System.Xml.Serialization;
using FunctionBuilder.ViewModels;

namespace FunctionBuilder.Abstract;

public interface IFunction : IXmlSerializable
{
    string Name { get; }
    IList<PointViewModel> PointsData { get; }
    bool Invert();
}
