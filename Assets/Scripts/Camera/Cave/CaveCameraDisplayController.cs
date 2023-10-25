#pragma warning disable 649
namespace Assets.Scripts.Camera.Cave
{
  using Assets.Scripts.Helpers;

  using UnityEngine;

  [RequireComponent(typeof(Camera))]
  public class CaveCameraDisplayController : MonoBehaviour
  {
    [SerializeField]
    private CaveCameraEnum targetCaveCamera;

    private Camera targetCamera;

    protected void Awake()
    {
      this.targetCamera = this.GetComponent<Camera>();
    }

    protected void OnEnable()
    {
      if (ApplicationHelper.ApplicationCameraMode == ApplicationHelper.ApplicationCameraModeEnum.Cave)
      {
        CaveDisplayManager.Updated += this.HandleCaveDisplayUpdated;
        this.HandleCaveDisplayUpdated();
      }
    }

    protected void OnDisable()
    {
      CaveDisplayManager.Updated -= this.HandleCaveDisplayUpdated;
    }

    private void HandleCaveDisplayUpdated()
    {
      this.targetCamera.targetDisplay = CaveDisplayManager.GetDisplay(this.targetCaveCamera);
    }
  }
}
