namespace Assets.Scripts.SceneLoadingOnDemand.Editor
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Assets.Scripts.SceneLoadingOnDemand.Task;

  using UnityEditor;

  using UnityEngine;

  [CustomEditor(typeof(Zone))]
  [CanEditMultipleObjects]
  public class ZoneEditor : Editor
  {
    private readonly List<Zone> zones = new List<Zone>();

    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      EditorGUILayout.Space();

      if (this.zones.Count == 1)
      {
        var zoneTarget = this.zones.First();
        EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(nameof(Zone.ScenePath)), zoneTarget.ScenePath);
        EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(nameof(Zone.SceneName)), zoneTarget.SceneName);
        EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(nameof(Zone.SceneBuildIndex)), zoneTarget.SceneBuildIndex.ToString());

        if (Application.isPlaying)
        {
          EditorGUILayout.Toggle(ObjectNames.NicifyVariableName(nameof(Zone.IsSubZone)), zoneTarget.IsSubZone);
        }

        var zoneState = zoneTarget.GetZoneState();
        EditorGUILayout.LabelField("State", zoneState.ToString());

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();

        using (new EditorGUI.DisabledScope(
          !zoneTarget.IsValid || zoneState == Zone.ZoneState.Loaded || zoneState == Zone.ZoneState.Loading))
        {
          if (GUILayout.Button("Load"))
          {
            zoneTarget.Load();
          }
        }

        using (new EditorGUI.DisabledScope(
          !zoneTarget.IsValid || zoneState == Zone.ZoneState.Unloaded || zoneState == Zone.ZoneState.Unloading))
        {
          if (GUILayout.Button("Unload"))
          {
            zoneTarget.Unload();
          }
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        if (!zoneTarget.IsValid)
        {
          EditorGUILayout.Separator();

          if (!Application.isPlaying && GUILayout.Button("Add Scene To Build Settings"))
          {
            this.AddScenePathToBuildSettings(zoneTarget.ScenePath);
          }

          EditorGUILayout.HelpBox("The current scene is not valid. Scene not included in build settings", MessageType.Warning);
        }
      }
      else
      {
        EditorGUI.showMixedValue = true;

        EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(nameof(Zone.ScenePath)), "-");
        EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(nameof(Zone.SceneName)), "-");
        EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(nameof(Zone.SceneBuildIndex)), "-");

        if (Application.isPlaying)
        {
          EditorGUILayout.Toggle(ObjectNames.NicifyVariableName(nameof(Zone.IsSubZone)), false);
        }

        EditorGUILayout.LabelField("State", "-");

        EditorGUI.showMixedValue = false;

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Load"))
        {
          foreach (var zone in this.zones)
          {
            zone.Load();
          }
        }

        if (GUILayout.Button("Unload"))
        {
          foreach (var zone in this.zones)
          {
            zone.Unload();
          }
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();

        if (!Application.isPlaying && GUILayout.Button("Add Scene To Build Settings"))
        {
          foreach (var zone in this.zones)
          {
            if (!zone.IsValid)
            {
              this.AddScenePathToBuildSettings(zone.ScenePath);
            }
          }
        }
      }
    }

    protected void OnEnable()
    {
      this.zones.AddRange(this.targets.Cast<Zone>());
      foreach (var zone in this.zones)
      {
        zone.ZoneLoaded += this.ZoneStateChanged;
        zone.ZoneUnloaded += this.ZoneStateChanged;
      }

      EditorBuildSettings.sceneListChanged += this.EditorBuildSettingsOnSceneListChanged;
    }

    protected void OnDisable()
    {
      foreach (var zone in this.zones)
      {
        zone.ZoneLoaded -= this.ZoneStateChanged;
        zone.ZoneUnloaded -= this.ZoneStateChanged;
      }

      this.zones.Clear();
      EditorBuildSettings.sceneListChanged -= this.EditorBuildSettingsOnSceneListChanged;
    }

    private void ZoneStateChanged(Zone zoneArg, IZoneOperation zoneOperation)
    {
      this.Repaint();
    }

    private void EditorBuildSettingsOnSceneListChanged()
    {
      this.Repaint();
    }

    private void AddScenePathToBuildSettings(string scenePath)
    {
      foreach (var editorBuildSettingsScene in EditorBuildSettings.scenes)
      {
        if (string.Equals(editorBuildSettingsScene.path, scenePath, StringComparison.Ordinal))
        {
          return;
        }
      }

      var scenes = EditorBuildSettings.scenes.ToList();
      scenes.Add(new EditorBuildSettingsScene(scenePath, true));
      EditorBuildSettings.scenes = scenes.ToArray();
    }
  }
}
