using System.Collections.Generic;
using System.Collections.ObjectModel;
using FunctionBuilder.Abstract;
using FunctionBuilder.ViewModels;
using System;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml;
using System.Xml.Serialization;

namespace FunctionBuilder.Services
{
    public class TableFunction : ObservableCollection<PointViewModel>, IFunction
    {
        public TableFunction()
        {
            Name = nameof(TableFunction);
        }
        [DataMember]
        public string Name {get;set;}
        [IgnoreDataMember()]
        public IList<PointViewModel> PointsData => this;
        public void AddPoint(PointViewModel point)
        {
            Add(point);
        }

        public XmlSchema? GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(Name);
            reader.ReadStartElement(nameof(Count));
            var count = reader.ReadContentAsInt();
            reader.ReadEndElement();
            reader.ReadStartElement(nameof(Items));

            for(var i = 0 ; i < count; i++)
            {
                reader.ReadStartElement("Item");
                reader.ReadStartElement("X");
                var x = reader.ReadContentAsDouble();
                reader.ReadEndElement();
                reader.ReadStartElement("Y");
                var y = reader.ReadContentAsDouble();
                reader.ReadEndElement();
                reader.ReadEndElement();
                var item = new PointViewModel(x,y);
                this.Add(item);
            }
            reader.ReadEndElement();
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(Name);

            writer.WriteStartElement(nameof(Count));
            writer.WriteValue(Count);
            writer.WriteEndElement();

            writer.WriteStartElement(nameof(Items));

            foreach(var item in this)
            {
                writer.WriteStartElement("Item");
                writer.WriteStartElement("X");
                writer.WriteValue(item.X);
                writer.WriteEndElement();
                writer.WriteStartElement("Y");
                writer.WriteValue(item.Y);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }

            writer.WriteEndElement();

            writer.WriteEndElement();
        }
 
    }
}