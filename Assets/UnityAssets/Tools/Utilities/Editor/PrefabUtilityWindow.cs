namespace Assets.UnityAssets.Tools.Utilities.Editor
{
  using UnityEditor;

  using UnityEngine;
  using UnityEngine.SceneManagement;

  public class PrefabUtilityWindow : EditorWindow
  {
    [SerializeField] 
    private GameObject prefab;

    [MenuItem("Tools/Utilities/Prefab Utility")]
    protected static void CreateReplaceWithPrefab()
    {
      EditorWindow.GetWindow<PrefabUtilityWindow>();
    }

    protected void OnEnable()
    {
      this.titleContent.text = "Prefab Utility";
    }

    protected void OnGUI()
    {
      EditorGUILayout.LabelField("Replace Game Object With Prefab", EditorStyles.boldLabel);

      this.prefab = EditorGUILayout.ObjectField(ObjectNames.NicifyVariableName(nameof(this.prefab)), this.prefab, typeof(GameObject), false) as GameObject;

      var selectedGameObjects = Selection.gameObjects;

      GUI.enabled = false;
      EditorGUILayout.LabelField("Selection count: " + selectedGameObjects.Length);

      GUI.enabled = this.prefab != null && selectedGameObjects.Length > 0;
      if (GUILayout.Button("Replace"))
      {
        var newObjects = new UnityEngine.Object[selectedGameObjects.Length];

        for (var i = selectedGameObjects.Length - 1; i >= 0; --i)
        {
          var selected = selectedGameObjects[i];
          var newObject = (GameObject)PrefabUtility.InstantiatePrefab(this.prefab);

          Undo.RegisterCreatedObjectUndo(newObject, "Replace With Prefabs");

          SceneManager.MoveGameObjectToScene(newObject, selected.scene);

          newObject.name = selected.name;
          newObject.transform.parent = selected.transform.parent;
          newObject.transform.localPosition = selected.transform.localPosition;
          newObject.transform.localRotation = selected.transform.localRotation;
          newObject.transform.localScale = selected.transform.localScale;
          newObject.transform.SetSiblingIndex(selected.transform.GetSiblingIndex());

          Undo.DestroyObjectImmediate(selected);

          newObjects[i] = newObject;
        }

        Selection.objects = newObjects;
      }

      this.Repaint();
    }
  }
}