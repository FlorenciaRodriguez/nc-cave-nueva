#pragma warning disable 649
namespace Assets.Scripts.Camera.Cave.UI
{
  using Assets.Scripts.Helpers;

  using UnityEngine;

  [RequireComponent(typeof(Canvas))]
  public class CanvasCaveDisplayController : MonoBehaviour
  {
    [SerializeField]
    private CaveCameraEnum targetCaveCamera;

    private Canvas targetCanvas;

    public CaveCameraEnum TargetCaveCamera
    {
      get => this.targetCaveCamera;
      set
      {
        this.targetCaveCamera = value;
        if (ApplicationHelper.ApplicationCameraMode == ApplicationHelper.ApplicationCameraModeEnum.Cave && this.targetCanvas != null)
        {
          this.HandleCaveDisplayUpdated();
        }
      }
    }

    protected void Awake()
    {
      this.targetCanvas = this.GetComponent<Canvas>();
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

    protected void OnValidate()
    {
      if (Application.isPlaying && ApplicationHelper.ApplicationCameraMode == ApplicationHelper.ApplicationCameraModeEnum.Cave && this.targetCanvas != null)
      {
        this.HandleCaveDisplayUpdated();
      }
    }

    private void HandleCaveDisplayUpdated()
    {
      this.targetCanvas.targetDisplay = CaveDisplayManager.GetDisplay(this.targetCaveCamera);
    }
  }
}
