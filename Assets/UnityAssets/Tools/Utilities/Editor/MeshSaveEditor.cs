namespace Assets.UnityAssets.Tools.Utilities.Editor
{
  using UnityEditor;
  using UnityEditor.SceneManagement;

  using UnityEngine;

  public class MeshSaveEditor
  {
    [MenuItem("CONTEXT/MeshFilter/Save Mesh As Asset")]
    public static void SaveMeshAsAsset(MenuCommand menuCommand)
    {
      var meshFilter = menuCommand.context as MeshFilter;
      if (meshFilter != null)
      {
        var mesh = meshFilter.sharedMesh;
        if (mesh != null)
        {
          SaveMesh(mesh, mesh.name, true);

          if (!Application.isPlaying && meshFilter.gameObject != null && meshFilter.gameObject.scene.IsValid())
          {
            EditorSceneManager.MarkSceneDirty(meshFilter.gameObject.scene);
          }
        }
      }
    }

    [MenuItem("CONTEXT/MeshFilter/Save Mesh As Asset", true)]
    public static bool ValidateSaveMeshAsAsset(MenuCommand menuCommand)
    {
      var meshFilter = menuCommand.context as MeshFilter;
      if (meshFilter != null)
      {
        var mesh = meshFilter.sharedMesh;
        if (mesh != null)
        {
          return true;
        }
      }

      return false;
    }

    public static void SaveMesh(Mesh mesh, string name, bool optimizeMesh)
    {
      string path = EditorUtility.SaveFilePanel("Save Mesh As Asset", "Assets/", name, "asset");
      if (string.IsNullOrEmpty(path))
      {
        return;
      }

      if (optimizeMesh)
      {
        MeshUtility.Optimize(mesh);
      }

      AssetDatabase.CreateAsset(mesh, FileUtil.GetProjectRelativePath(path));
      AssetDatabase.SaveAssets();
    }
  }
}