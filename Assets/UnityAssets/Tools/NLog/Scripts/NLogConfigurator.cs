namespace Assets.UnityAssets.Tools.NLog.Scripts
{
  using MediaLab.LogManager;

  using UnityEngine;

  [DisallowMultipleComponent]
  public class NLogConfigurator : MonoBehaviour
  {
    protected void Awake()
    {
#if UNITY_EDITOR
      Log.SetBaseDirectory(System.IO.Directory.GetCurrentDirectory());
#endif

      Log.SetUnityLog();
    }
  }
}
