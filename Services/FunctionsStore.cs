using System;
using System.Collections.Generic;
using FunctionBuilder.Abstract;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Xml.Schema;
using System.Xml;
using System.Xml.Serialization;
using FunctionBuilder.Utils;
using System.IO;
using System.Collections;
using Avalonia.Threading;

namespace FunctionBuilder.Services;

public class FunctionsStore : IFunctionsStore
{
    private ObservableCollection<IFunction> _functionsStore;
    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    private string lastState;
    DispatcherTimer wasModifiedTimer;

    public FunctionsStore()
    {
        lastState = string.Empty;
        _functionsStore = new ObservableCollection<IFunction>();
        _functionsStore.CollectionChanged += OnCollectionChanged;
        wasModifiedTimer = new DispatcherTimer(DispatcherPriority.ApplicationIdle);
        wasModifiedTimer.Interval = TimeSpan.FromSeconds(1);
        wasModifiedTimer.Tick += OnCheckIsModified;
        wasModifiedTimer.Start();
    }
 
    private void OnCollectionChanged(object? _, NotifyCollectionChangedEventArgs e)
    {
        CollectionChanged?.Invoke(this, e);
    }

    public void Add(IFunction func)
    {
        _functionsStore.Add(func);
    }

    public IFunction AddNew<T>()
    {
        var func = Activator.CreateInstance(typeof(T)) as IFunction ?? throw new ArgumentException("Невозможно создать экземпляр типа");
        _functionsStore.Add(func);
        return func;
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

        for (var i = 0; i < count; i++)
        {
            XmlSerializer serializer = reader.GetApproapriateSerializator<IFunction>();
            var function = serializer.Deserialize(reader) as IFunction ?? throw new ArgumentException("Ошибка импорта функции");
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
        foreach (var func in _functionsStore)
        {
            if (func is IXmlSerializable xmlSerializable)
            {
                xmlSerializable.WriteXml(writer);
            }
        }
        writer.WriteEndElement();
    }

    public int Count => _functionsStore.Count;
    
    private void OnCheckIsModified(object? sender, EventArgs e)
    {
        using (StringWriter textWriter = new StringWriter())
        {
            var writer = XmlWriter.Create(textWriter);
            XmlSerializer serializer = new XmlSerializer(GetType());
            serializer.Serialize(writer, this);
            writer.Close();
            if(string.IsNullOrEmpty(lastState))
            {
                lastState = textWriter.ToString();
                return;
            }
            var result = textWriter.ToString() != lastState;
            lastState =  textWriter.ToString() ;
            if(result)
            {
                OnCollectionChanged(this, default!);
            }
        }
    }

    public IEnumerator<IFunction> GetEnumerator()
    {
        return _functionsStore.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IFunction GetAt(int index)
    {
        return _functionsStore[index];
    }
}
