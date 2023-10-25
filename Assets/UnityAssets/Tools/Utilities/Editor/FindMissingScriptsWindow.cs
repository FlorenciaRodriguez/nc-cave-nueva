namespace Assets.UnityAssets.Tools.Utilities.Editor
{
  using System.Collections.Generic;

  using UnityEditor;

  using UnityEngine;

  public class FindMissingScriptsWindow : EditorWindow
  {
    [MenuItem("Tools/Find Missing Scripts")]
    public static void FindMissingScripts()
    {
      EditorWindow.GetWindow(typeof(FindMissingScriptsWindow));
    }

    protected void OnEnable()
    {
      this.titleContent.text = " Missing Scripts";
    }

    protected void OnGUI()
    {
      if (GUILayout.Button("Find Missing Scripts in selected prefabs"))
      {
        FindInSelected();
      }
    }

    private static void FindInSelected()
    {
      List<GameObject> objectsWithDeadLinks = new List<GameObject>();
      foreach (var selected in Selection.gameObjects)
      {
        objectsWithDeadLinks.AddRange(FindMissingScripts(selected));
      }

      if (objectsWithDeadLinks.Count > 0)
      {
        Selection.objects = objectsWithDeadLinks.ToArray();
      }
    }

    private static List<GameObject> FindMissingScripts(GameObject gameObject)
    {
      var list = new List<GameObject>();

      Component[] components = gameObject.GetComponents<Component>();
      for (int i = 0; i < components.Length; i++)
      {
        if (components[i] == null)
        {
          list.Add(gameObject);
          break;
        }
      }

      foreach (Transform child in gameObject.transform)
      {
        list.AddRange(FindMissingScripts(child.gameObject));
      }

      return list;
    }
  }
}