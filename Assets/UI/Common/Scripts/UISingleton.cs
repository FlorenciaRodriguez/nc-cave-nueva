namespace Assets.UI.Common.Scripts
{
  using UnityEngine;

  // ReSharper disable once InconsistentNaming
  public class UISingleton : MonoBehaviour
  {
    private static UISingleton instance;

    protected void Awake()
    {
      if (instance != null)
      {
        // ReSharper disable once ArrangeStaticMemberQualifier
        UnityEngine.Object.DestroyImmediate(this.gameObject);
        return;
      }

      // ReSharper disable once ArrangeStaticMemberQualifier
      UnityEngine.Object.DontDestroyOnLoad(this.gameObject);

      instance = this;
    }

    protected void OnDestroy()
    {
      if (instance == this)
      {
        instance = null;
      }
    }
  }
}
