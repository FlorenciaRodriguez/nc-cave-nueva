#pragma warning disable 649
namespace Assets.Scripts.Camera
{
  using Assets.Scripts.Helpers;

  using UnityEngine;

  public class CameraModeActivator : MonoBehaviour
  {
    [SerializeField]
    private GameObject defaultMode;

    [SerializeField]
    private GameObject caveMode;

    protected void Awake()
    {
      switch (ApplicationHelper.ApplicationCameraMode)
      {
        case ApplicationHelper.ApplicationCameraModeEnum.Default:
          this.caveMode.SetActive(false);
          this.defaultMode.SetActive(true);
          break;

        case ApplicationHelper.ApplicationCameraModeEnum.Cave:
          this.defaultMode.SetActive(false);
          this.caveMode.SetActive(true);
          break;
      }
    }

    protected void OnValidate()
    {
      if (this.defaultMode == null)
      {
        Debug.LogError(
          $"The variable '{nameof(this.defaultMode)}' of  the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
          this.gameObject);
      }

      if (this.caveMode == null)
      {
        Debug.LogError(
          $"The variable '{nameof(this.caveMode)}' of  the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
          this.gameObject);
      }
    }
  }
}
