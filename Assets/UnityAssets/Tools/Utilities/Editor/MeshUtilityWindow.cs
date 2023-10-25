namespace Assets.UnityAssets.Tools.Utilities.Editor
{
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;

  using UnityEditor;

  using UnityEngine;

  using Object = UnityEngine.Object;

  public class MeshUtilityWindow : EditorWindow
  {
    [SerializeField]
    private bool foldoutReadableMeshes;

    private Vector2 scrollPosition;

    [MenuItem("Tools/Mesh Utility")]
    protected static void CreateReplaceWithPrefab()
    {
      EditorWindow.GetWindow<MeshUtilityWindow>();
    }

    protected void OnEnable()
    {
      this.titleContent.text = "Mesh Utility";
    }

    protected void OnGUI()
    {
      EditorGUILayout.LabelField("Non-Readable Models", EditorStyles.boldLabel);

      ICollection<ModelImporter> configurableModels = null;

      this.foldoutReadableMeshes = EditorGUILayout.Foldout(this.foldoutReadableMeshes, "Configurable Models");
      if (this.foldoutReadableMeshes)
      {
        EditorGUILayout.BeginVertical();
        EditorGUI.indentLevel++;

        configurableModels = this.GetConfigurableModelImporters();

        foreach (var modelImporter in configurableModels)
        {
          EditorGUILayout.ObjectField(
            new GUIContent(Path.GetFileNameWithoutExtension(modelImporter.assetPath), modelImporter.assetPath),
            modelImporter,
            typeof(ModelImporter),
            false);
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
      }

      if (GUILayout.Button("Configure Models"))
      {
        if (configurableModels == null)
        {
          configurableModels = this.GetConfigurableModelImporters();
        }

        for (var i = 0; i < configurableModels.Count; i++)
        {
          var modelImporter = configurableModels.ElementAt(i);

          if (EditorUtility.DisplayCancelableProgressBar(
            "Configure Non-Readable Meshes",
            $"Configurating {modelImporter.assetPath}",
            (float)i / configurableModels.Count))
          {
            break;
          }

          modelImporter.isReadable = false;
          modelImporter.meshCompression = ModelImporterMeshCompression.Off;
          modelImporter.SaveAndReimport();
        }

        EditorUtility.ClearProgressBar();
      }
    }

    private ICollection<ModelImporter> GetConfigurableModelImporters()
    {
      var assets = new HashSet<string>();
      var models = new List<ModelImporter>();

      // ReSharper disable once ArrangeStaticMemberQualifier
      foreach (var meshFilter in MeshFilter.FindObjectsOfType<MeshFilter>())
      {
        var assetPath = AssetDatabase.GetAssetPath(meshFilter.sharedMesh);
        if (!assets.Contains(assetPath))
        {
          assets.Add(assetPath);

          var modelImporter = AssetImporter.GetAtPath(assetPath) as ModelImporter;
          if (modelImporter != null && this.IsConfigurable(modelImporter))
          {
            models.Add(modelImporter);
          }
        }
      }

      return models;
    }

    private bool IsConfigurable(ModelImporter modelImporter)
    {
      return modelImporter.isReadable || modelImporter.meshCompression != ModelImporterMeshCompression.Off;
    }
  }
}