namespace Assets.Scripts.Camera.Cave.Serializable
{
  using System.Xml.Serialization;

  public class CaveDisplayProfile
  {
    [XmlElement(nameof(CaveCamera))]
    public CaveCameraEnum CaveCamera { get; set; }

    [XmlElement(nameof(TargetDisplay))]
    public int TargetDisplay { get; set; }
  }
}
