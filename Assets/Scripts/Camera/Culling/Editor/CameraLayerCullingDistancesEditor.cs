namespace Assets.Scripts.Camera.Culling.Editor
{
  using UnityEditor;

  using UnityEngine;

  [CustomEditor(typeof(CameraLayerCullingDistances))]
  public class CameraLayerCullingDistancesEditor : Editor
  {
    private CameraLayerCullingDistances cameraLayerCullingDistances;

    public override void OnInspectorGUI()
    {
      var layers = this.cameraLayerCullingDistances.LayerCullingDistances;
      for (var i = 0; i < layers.Length; i++)
      {
        var layerName = LayerMask.LayerToName(i);
        if (string.IsNullOrEmpty(layerName))
        {
          GUI.enabled = false;
          EditorGUILayout.FloatField($"Layer {i}", layers[i]);
          GUI.enabled = true;
        }
        else
        {
          EditorGUI.BeginChangeCheck();
          var distance = EditorGUILayout.FloatField(layerName, layers[i]);

          if (EditorGUI.EndChangeCheck())
          {
            Undo.RegisterCompleteObjectUndo(this.target, "Inspector");
            layers[i] = distance;

            EditorUtility.SetDirty(this.target);
          }
        }
      }
    }

    protected void OnEnable()
    {
      this.cameraLayerCullingDistances = (CameraLayerCullingDistances)this.target;
    }
  }
}
