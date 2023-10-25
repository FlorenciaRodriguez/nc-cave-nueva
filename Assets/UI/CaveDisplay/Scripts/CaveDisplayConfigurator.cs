namespace Assets.UI.CaveDisplay.Scripts
{
  using Assets.Scripts.Helpers;

  using UnityEngine;

  public class CaveDisplayConfigurator : MonoBehaviour
  {
    private bool visible;

    public static CaveDisplayConfigurator Singleton { get; private set; }

    protected void Awake()
    {
      if (Singleton != null)
      {
        // ReSharper disable once ArrangeStaticMemberQualifier
        GameObject.DestroyImmediate(this.gameObject);
        return;
      }

      // ReSharper disable once ArrangeStaticMemberQualifier
      GameObject.DontDestroyOnLoad(this.gameObject);

      Singleton = this;
      this.SetVisible(false);
    }

    protected void OnDestroy()
    {
      if (Singleton == this)
      {
        Singleton = null;
      }
    }

    protected void Update()
    {
      if (ApplicationHelper.ApplicationCameraMode == ApplicationHelper.ApplicationCameraModeEnum.Cave)
      {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
          if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
          {
            if (Input.GetKeyUp(KeyCode.Return))
            {
              this.SetVisible(!this.visible);
            }
          }
        }
      }
    }

    private void SetVisible(bool value)
    {
      foreach (Transform child in this.transform)
      {
        child.gameObject.SetActive(value);
      }

      this.visible = value;
#if !UNITY_EDITOR
      Cursor.visible = value || Assets.Scripts.Helpers.ApplicationHelper.ApplicationMode == Assets.Scripts.Helpers.ApplicationHelper.ApplicationModeEnum.Sandbox;
#endif
    }
  }
}
