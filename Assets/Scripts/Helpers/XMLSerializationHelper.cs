namespace Assets.Scripts.Helpers
{
  using System;

  using System.IO;

  using System.Xml.Serialization;

  public class XMLSerializationHelper
  {
    public static T DeserializeFromXmlFile<T>(string filePath)
    {
      if (!File.Exists(filePath))
      {
        throw new FileNotFoundException($"Cannot deserialize {typeof(T)} from xml file '{filePath}'. File not found");
      }

      T serializableObject;
      StreamReader streamReader = null;

      try
      {
        var xmlSerializer = new XmlSerializer(typeof(T));
        streamReader = new StreamReader(filePath);

        serializableObject = (T)xmlSerializer.Deserialize(streamReader);
      }
      catch (Exception exception)
      {
        throw new InvalidOperationException($"Exception deserializing {typeof(T)}: {exception.Message}", exception);
      }
      finally
      {
        streamReader?.Close();
      }

      return serializableObject;
    }

    public static void SerializeToXmlFile<T>(T serializableObject, string filePath)
    {
      StreamWriter streamWriter = null;

      try
      {
        (new FileInfo(filePath)).Directory?.Create();

        streamWriter = File.CreateText(filePath);

        var xmlSerializer = new XmlSerializer(serializableObject.GetType());
        xmlSerializer.Serialize(streamWriter, serializableObject);
      }
      catch (Exception exception)
      {
        throw new InvalidOperationException($"Exception serializing {typeof(T)}: {exception.Message}", exception);
      }
      finally
      {
        streamWriter?.Close();
      }
    }
  }
}
