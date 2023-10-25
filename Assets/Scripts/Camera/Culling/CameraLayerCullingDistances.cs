namespace Assets.Scripts.Camera.Culling
{
  using UnityEngine;

  [CreateAssetMenu]
  public class CameraLayerCullingDistances : ScriptableObject
  {
    [SerializeField]
    private float[] layerCullingDistances = new float[32];

    public float[] LayerCullingDistances => this.layerCullingDistances;
  }
}