#pragma warning disable 649
namespace Assets.Scripts.Camera.Culling
{
  using UnityEngine;

  [RequireComponent(typeof(Camera))]
  public class CameraLayerCullingConfigurator : MonoBehaviour
  {
    [SerializeField]
    private CameraLayerCullingDistances layerCullingDistances;

    private Camera targetCamera;

    protected void Awake()
    {
      this.targetCamera = this.GetComponent<Camera>();
    }

    protected void Start()
    {
      this.targetCamera.layerCullDistances = this.layerCullingDistances.LayerCullingDistances;
    }

    protected void OnValidate()
    {
      if (this.layerCullingDistances == null)
      {
        Debug.LogError(
          $"The variable '{nameof(this.layerCullingDistances)}' of the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned",
          this.gameObject);
      }
    }
  }
}
