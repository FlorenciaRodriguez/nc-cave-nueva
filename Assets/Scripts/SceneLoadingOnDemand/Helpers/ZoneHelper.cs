namespace Assets.Scripts.SceneLoadingOnDemand.Helpers
{
  public static class ZoneHelper
  {
    public static bool IsWorldSubzone(this Zone zone)
    {
      return zone.GetComponent<SubzoneWorld>() != null;
    }
  }
}
