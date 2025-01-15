using System;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace FunctionBuilder.Utils;
public static class SerializationHelper
{
    public static XmlSerializer GetApproapriateSerializator<T>(this XmlReader reader)
    {
        var type = typeof(T);
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => p.IsAssignableTo(type))
            .Where(p => !p.IsInterface).ToArray();

        foreach (var t in types)
        {
            XmlSerializer serializer = new XmlSerializer(t);
            var canDeser = serializer.CanDeserialize(reader);
            if (canDeser)
            {
                return serializer;
            }
        }
        throw new ArgumentException($"Не найден подходящий XmlSerializer для типа {typeof(T).Name}");
    }
}