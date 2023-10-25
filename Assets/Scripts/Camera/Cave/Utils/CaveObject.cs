namespace Assets.Scripts.Camera.Cave.Utils
{
  using Assets.Scripts.Helpers;

  using UnityEngine;

  public class CaveObject : MonoBehaviour
  {
    protected void Start()
    {
      if (ApplicationHelper.ApplicationCameraMode != ApplicationHelper.ApplicationCameraModeEnum.Cave)
      {
        this.gameObject.SetActive(false);
      }
    }
  }
}
