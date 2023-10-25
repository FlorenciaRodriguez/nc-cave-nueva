namespace Assets.Scripts.SceneLoadingOnDemand.Editor
{
  using System;

  using UnityEditor;

  using UnityEngine;

  [CustomPropertyDrawer(typeof(SceneAttribute))]
  public class SceneAttributeDrawer : PropertyDrawer
  {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      EditorGUI.showMixedValue = property.hasMultipleDifferentValues;

      var scene = EditorGUI.ObjectField(position, "Scene", this.GetSceneObject(property.stringValue), typeof(SceneAsset), false);
      if (scene == null)
      {
        property.stringValue = string.Empty;
        property.serializedObject.ApplyModifiedProperties();
      }
      else
      {
        var scenePath = AssetDatabase.GetAssetOrScenePath(scene);
        if (!string.Equals(scenePath, property.stringValue, StringComparison.Ordinal))
        {
          property.stringValue = scenePath;
          property.serializedObject.ApplyModifiedProperties();
        }
      }

      EditorGUI.showMixedValue = false;
    }

    private SceneAsset GetSceneObject(string scenePath)
    {
      if (string.IsNullOrEmpty(scenePath))
      {
        return null;
      }

      return AssetDatabase.LoadAssetAtPath(scenePath, typeof(SceneAsset)) as SceneAsset;
    }
  }
}
