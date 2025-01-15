using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FunctionBuilder.Abstract;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Xml.Schema;
using System.Xml;
using System.Xml.Serialization;
using FunctionBuilder.Utils;

namespace FunctionBuilder.Services
{
    public class FunctionsStore : IFunctionsStore
    {
        private ObservableCollection<IFunction> _functionsStore;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public FunctionsStore()
        {
            _functionsStore = new ObservableCollection<IFunction>();
            _functionsStore.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object _, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        public IFunction GetAt(int index)
        {
            return _functionsStore[index];
        }


        public void Add(IFunction func)
        {
            _functionsStore.Add(func);
        }

        public IFunction AddNew<T>()
        {
            var func = (IFunction)Activator.CreateInstance(typeof(T));
            _functionsStore.Add(func);
            return func;
        }

        public IEnumerable<IFunction> GetEnumerable()
        {
            return _functionsStore;
        }

        public void RemoveAll()
        {
            _functionsStore.Clear();
        }

        public XmlSchema? GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(nameof(FunctionsStore));
            reader.ReadStartElement(nameof(Count));
            var count = reader.ReadContentAsInt();
            reader.ReadEndElement();

            reader.ReadStartElement("FunctionsStoreItems");
            
            for(var i = 0 ; i < count; i++)
            {
                XmlSerializer serializer = reader.GetApproapriateSerializator<IFunction>();
                var function = (IFunction)serializer.Deserialize(reader)!; 
                Add(function);
            }
            reader.ReadEndElement();
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(nameof(Count));
            writer.WriteValue(Count);
            writer.WriteEndElement();

            writer.WriteStartElement("FunctionsStoreItems");
            foreach(var func in _functionsStore)
            {
                if(func is IXmlSerializable xmlSerializable)
                {
                    xmlSerializable.WriteXml(writer);
                }
            }
            writer.WriteEndElement();
        }

        public int Count => _functionsStore.Count;
    }
}