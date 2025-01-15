using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FunctionBuilder.ViewModels;
using LiveChartsCore.Defaults;

namespace FunctionBuilder.Abstract
{
    public interface IFunction : IXmlSerializable
    {
        string Name { get; }
        IList<PointViewModel> PointsData {get;}
        void AddPoint(PointViewModel point);
    }
}