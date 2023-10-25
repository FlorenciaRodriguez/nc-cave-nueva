namespace Assets.Scripts.Camera.Cave.Serializable
{
  using System.Xml.Serialization;

  [XmlRoot(nameof(CaveDisplaySettings))]
  public class CaveDisplaySettings
  {
    [XmlElement(nameof(CaveDisplayProfile))]
    public CaveDisplayProfile[] CaveDisplayProfile { get; set; }
  }
}
