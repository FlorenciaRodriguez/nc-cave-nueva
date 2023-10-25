namespace Assets.Scripts.SceneLoadingOnDemand
{
  using System.Collections.Generic;

  using System.Collections.ObjectModel;
  using System.Linq;

  using Assets.Scripts.SceneLoadingOnDemand.Task;
#if UNITY_EDITOR
  using UnityEditor;

  using UnityEditor.SceneManagement;
#endif

  using UnityEngine;
  using UnityEngine.Events;
  using UnityEngine.SceneManagement;

  [ExecuteInEditMode]
  public class Zone : MonoBehaviour
  {
    private readonly List<Zone> parentZones = new List<Zone>();

    [SerializeField]
    [Scene]
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private string scenePath = string.Empty;

    [SerializeField]
    private bool defaultLoaded;

    [SerializeField]
    private ZoneSelectionMode subzonesLoadMode = ZoneSelectionMode.All;

    [SerializeField]
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    // ReSharper disable once CollectionNeverUpdated.Local
    private List<Zone> subzones = new List<Zone>();

    private ZoneTask zoneLoadTask;

    private ZoneTask zoneUnloadTask;

    private ZoneTaskQueue zoneLoadTaskQueue;

    private ZoneTaskQueue zoneUnloadTaskQueue;

    public event UnityAction<Zone, IZoneOperation> StartLoadZone;

    public event UnityAction<Zone, IZoneOperation> ZoneLoaded;

    public event UnityAction<Zone, IZoneOperation> StartUnloadZone;

    public event UnityAction<Zone, IZoneOperation> ZoneUnloaded;

    public enum ZoneState
    {
      Loading,

      Loaded,

      PartialLoaded,

      Unloading,

      Unloaded
    }

    public enum ZoneSelectionMode
    {
      None,

      All
    }

    public bool IsValid => string.IsNullOrEmpty(this.scenePath) || this.SceneBuildIndex != -1;

    public string ScenePath => this.scenePath;

    public string SceneName => this.scenePath.Split('/').Last().Split('.').First();

    public int SceneBuildIndex => SceneUtility.GetBuildIndexByScenePath(this.scenePath);

    public bool DefaultLoaded
    {
      get
      {
        return this.defaultLoaded;
      }

      set
      {
        this.defaultLoaded = value;
      }
    }

    public ZoneSelectionMode SubzonesLoadMode => this.subzonesLoadMode;

    public ReadOnlyCollection<Zone> Subzones => this.subzones.AsReadOnly();

    public ReadOnlyCollection<Zone> ParentZones => this.parentZones.AsReadOnly();

    public bool IsSubZone { get; private set; }

    public IZoneOperation Load()
    {
      var zoneState = this.GetZoneState();
      if (zoneState != ZoneState.Loaded)
      {
        if (zoneState == ZoneState.Loading)
        {
          return new ZoneOperation(this.zoneLoadTaskQueue);
        }

        return new ZoneOperation(this.LoadAsyncZone(true));
      }

      return new ZoneOperation(null);
    }

    public IZoneOperation Unload()
    {
      var zoneState = this.GetZoneState();
      if (zoneState != ZoneState.Unloaded)
      {
        if (zoneState == ZoneState.Unloading)
        {
          return new ZoneOperation(this.zoneUnloadTaskQueue);
        }

        return new ZoneOperation(this.UnloadAsyncZone(true));
      }

      return new ZoneOperation(null);
    }

    public ZoneState GetZoneState()
    {
      if (this.zoneLoadTaskQueue != null && this.zoneLoadTaskQueue.IsExecuting)
      {
        return ZoneState.Loading;
      }

      if (this.zoneUnloadTaskQueue != null && this.zoneUnloadTaskQueue.IsExecuting)
      {
        return ZoneState.Unloading;
      }

      var scene = SceneManager.GetSceneByPath(this.scenePath);

      if (this.subzonesLoadMode == ZoneSelectionMode.None)
      {
        return scene.isLoaded ? ZoneState.Loaded : ZoneState.Unloaded;
      }

      var balance = 0;
      foreach (var subzone in this.subzones)
      {
        if (subzone == null)
        {
          balance++;
        }
        else
        {
          var subZoneState = subzone.GetZoneState();
          if (subZoneState == ZoneState.Unloaded)
          {
            balance--;
          }
          else if (subZoneState == ZoneState.Loaded)
          {
            balance++;
          }
        }
      }

      if (balance == this.subzones.Count && (string.IsNullOrEmpty(this.scenePath) || scene.isLoaded))
      {
        return ZoneState.Loaded;
      }

      if (balance == -this.subzones.Count && (string.IsNullOrEmpty(this.scenePath) || !scene.isLoaded))
      {
        return ZoneState.Unloaded;
      }

      return ZoneState.PartialLoaded;
    }

    protected virtual void OnStartLoadZone(ZoneOperation zoneOperation)
    {
      this.StartLoadZone?.Invoke(this, zoneOperation);
    }

    protected virtual void OnZoneLoaded(ZoneOperation zoneOperation)
    {
      this.ZoneLoaded?.Invoke(this, zoneOperation);
    }

    protected virtual void OnStartUnloadZone(ZoneOperation zoneOperation)
    {
      this.StartUnloadZone?.Invoke(this, zoneOperation);
    }

    protected virtual void OnZoneUnloaded(ZoneOperation zoneOperation)
    {
      this.ZoneUnloaded?.Invoke(this, zoneOperation);
    }

    protected void OnValidate()
    {
      if (string.IsNullOrEmpty(this.scenePath))
      {
        Debug.LogError($"The variable '{nameof(this.scenePath)}' of '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.", this.gameObject);
      }
      else if (!this.IsValid)
      {
        Debug.LogError($"Invalid scene '{this.scenePath}'. Scene not included in build settings", this.gameObject);
      }
    }

    protected void Awake()
    {
      foreach (var subzone in this.subzones)
      {
        subzone.IsSubZone = true;
        subzone.parentZones.Add(this);
      }
    }

    protected void Start()
    {
      if (Application.isPlaying && !this.IsSubZone && this.defaultLoaded)
      {
        this.Load();
      }
    }

    private ZoneTask LoadAsyncZone(bool handleTask)
    {
      this.zoneLoadTaskQueue = new ZoneTaskQueue();

      if (this.zoneUnloadTaskQueue != null)
      {
        this.zoneUnloadTaskQueue.Started -= this.HandleZoneUnloadTaskStarted;
        this.zoneUnloadTaskQueue.Completed -= this.HandleZoneUnloadTaskCompleted;
        this.zoneUnloadTaskQueue.AbortTask();
        this.zoneUnloadTaskQueue = null;
      }

      var forceLoad = false;

      if (this.zoneUnloadTask != null)
      {
        if (this.zoneUnloadTask.IsExecuting)
        {
          if (!this.zoneUnloadTask.IsAborted && this.zoneUnloadTask.AbortTask())
          {
            Debug.Log($"Unload of zone '{this.SceneName}' aborted");
          }
          else
          {
            Debug.Log(
              $"Unload of zone '{this.SceneName}' is expected to finish to load the zone (scene async operation cannot be aborted)");

            this.zoneLoadTaskQueue.EnqueueTask(this.zoneUnloadTask);

            forceLoad = true;
          }
        }

        this.zoneUnloadTask = null;
      }

      if (!string.IsNullOrEmpty(this.scenePath))
      {
        var scene = SceneManager.GetSceneByPath(this.scenePath);
        if (!scene.isLoaded || forceLoad)
        {
          if (Application.isPlaying)
          {
            this.zoneLoadTask = new ZoneLoadSceneAsyncTask(this.SceneBuildIndex);
            this.zoneLoadTaskQueue.EnqueueTask(this.zoneLoadTask);
          }
#if UNITY_EDITOR
          else
          {
            EditorSceneManager.OpenScene(this.scenePath, OpenSceneMode.Additive);
            SceneView.RepaintAll();

            Debug.Log($"Zone '{this.SceneName}' loaded");
          }
#endif
        }
      }

      if (this.subzonesLoadMode == ZoneSelectionMode.All)
      {
        foreach (var subzone in this.subzones)
        {
          if (subzone != null)
          {
            var subZoneState = subzone.GetZoneState();
            if (subZoneState != ZoneState.Loaded)
            {
              if (subZoneState == ZoneState.Loading)
              {
                this.zoneLoadTaskQueue.EnqueueTask(subzone.zoneLoadTaskQueue);
              }
              else
              {
                this.zoneLoadTaskQueue.EnqueueTask(subzone.LoadAsyncZone(false));
              }
            }
          }
        }
      }

      this.zoneLoadTaskQueue.Started += this.HandleZoneLoadTaskStarted;
      this.zoneLoadTaskQueue.Completed += this.HandleZoneLoadTaskCompleted;

      if (handleTask)
      {
        if (!this.zoneLoadTaskQueue.StartTask())
        {
          this.zoneLoadTaskQueue.Started -= this.HandleZoneLoadTaskStarted;
          this.zoneLoadTaskQueue.Completed -= this.HandleZoneLoadTaskCompleted;
        }
      }

      return this.zoneLoadTaskQueue;
    }

    private void HandleZoneLoadTaskStarted(ZoneTask zoneTask)
    {
      zoneTask.Started -= this.HandleZoneLoadTaskStarted;

      if (this.subzones.Count > 0 && this.subzonesLoadMode == ZoneSelectionMode.All)
      {
        Debug.Log($"Loading zone '{this.SceneName}' and subzones ({this.subzones.Count})");
      }
      else
      {
        Debug.Log($"Loading zone '{this.SceneName}'");
      }

      this.OnStartLoadZone(new ZoneOperation(zoneTask));
    }

    private void HandleZoneLoadTaskCompleted(ZoneTask zoneTask)
    {
      zoneTask.Completed -= this.HandleZoneLoadTaskCompleted;

      if (!zoneTask.IsAborted)
      {
        if (this.subzones.Count > 0 && this.subzonesLoadMode == ZoneSelectionMode.All)
        {
          Debug.Log($"Zone '{this.SceneName}' and subzones ({this.subzones.Count}) loaded");
        }
        else
        {
          Debug.Log($"Zone '{this.SceneName}' loaded");
        }

        this.OnZoneLoaded(new ZoneOperation(zoneTask));
      }

      this.zoneLoadTask = null;
      this.zoneLoadTaskQueue = null;
    }

    private ZoneTask UnloadAsyncZone(bool handleTask)
    {
      this.zoneUnloadTaskQueue = new ZoneTaskQueue();

      if (this.zoneLoadTaskQueue != null)
      {
        this.zoneLoadTaskQueue.Started -= this.HandleZoneUnloadTaskStarted;
        this.zoneLoadTaskQueue.Completed -= this.HandleZoneUnloadTaskCompleted;
        this.zoneLoadTaskQueue.AbortTask();
        this.zoneLoadTaskQueue = null;
      }

      var forceUnload = false;

      if (this.zoneLoadTask != null)
      {
        if (this.zoneLoadTask.IsExecuting)
        {
          if (!this.zoneLoadTask.IsAborted && this.zoneLoadTask.AbortTask())
          {
            Debug.Log($"Load of zone '{this.SceneName}' aborted");
          }
          else
          {
            Debug.Log(
              $"Load of zone '{this.SceneName}' is expected to finish to unload the zone (scene async operation cannot be aborted)");

            this.zoneUnloadTaskQueue.EnqueueTask(this.zoneLoadTask);
            forceUnload = true;
          }
        }

        this.zoneLoadTask = null;
      }

      if (!string.IsNullOrEmpty(this.scenePath))
      {
        var scene = SceneManager.GetSceneByPath(this.scenePath);
        if (scene.isLoaded || forceUnload)
        {
          if (Application.isPlaying)
          {
            this.zoneUnloadTask = new ZoneUnloadSceneAsyncTask(this.SceneBuildIndex);
            this.zoneUnloadTaskQueue.EnqueueTask(this.zoneUnloadTask);
          }
#if UNITY_EDITOR
          else
          {
            EditorSceneManager.CloseScene(scene, true);
            SceneView.RepaintAll();

            Debug.Log($"Zone '{this.SceneName}' unloaded");
          }
#endif
        }
      }

      if (this.subzonesLoadMode == ZoneSelectionMode.All)
      {
        foreach (var subzone in this.subzones)
        {
          if (subzone != null)
          {
            var subZoneState = subzone.GetZoneState();
            if (subZoneState != ZoneState.Unloaded)
            {
              if (subZoneState == ZoneState.Unloading)
              {
                this.zoneUnloadTaskQueue.EnqueueTask(subzone.zoneUnloadTaskQueue);
              }
              else
              {
                this.zoneUnloadTaskQueue.EnqueueTask(subzone.UnloadAsyncZone(false));
              }
            }
          }
        }
      }

      this.zoneUnloadTaskQueue.Started += this.HandleZoneUnloadTaskStarted;
      this.zoneUnloadTaskQueue.Completed += this.HandleZoneUnloadTaskCompleted;

      if (handleTask)
      {
        if (!this.zoneUnloadTaskQueue.StartTask())
        {
          this.zoneUnloadTaskQueue.Started -= this.HandleZoneUnloadTaskStarted;
          this.zoneUnloadTaskQueue.Completed -= this.HandleZoneUnloadTaskCompleted;
        }
      }

      return this.zoneUnloadTaskQueue;
    }

    private void HandleZoneUnloadTaskStarted(ZoneTask zoneTask)
    {
      zoneTask.Started -= this.HandleZoneUnloadTaskStarted;

      if (this.subzones.Count > 0 && this.subzonesLoadMode == ZoneSelectionMode.All)
      {
        Debug.Log($"Unloading zone '{this.SceneName}' and subzones ({this.subzones.Count})");
      }
      else
      {
        Debug.Log($"Unloading zone '{this.SceneName}'");
      }

      this.OnStartUnloadZone(new ZoneOperation(zoneTask));
    }

    private void HandleZoneUnloadTaskCompleted(ZoneTask zoneTask)
    {
      zoneTask.Completed -= this.HandleZoneUnloadTaskCompleted;

      if (!zoneTask.IsAborted)
      {
        if (this.subzones.Count > 0 && this.subzonesLoadMode == ZoneSelectionMode.All)
        {
          Debug.Log($"Zone '{this.SceneName}' and subzones ({this.subzones.Count}) unloaded");
        }
        else
        {
          Debug.Log($"Zone '{this.SceneName}' unloaded");
        }

        this.OnZoneUnloaded(new ZoneOperation(zoneTask));
      }

      this.zoneUnloadTask = null;
      this.zoneUnloadTaskQueue = null;
    }
  }
}