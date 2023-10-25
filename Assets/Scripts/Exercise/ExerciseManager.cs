#pragma warning disable 649
namespace Assets.Scripts.Exercise
{
  using System;
  using System.Collections.Generic;
  using System.IO;

  using Assets.Scripts.Apps.Bindings;
  using Assets.Scripts.Exercise.EventArgs;
  using Assets.Scripts.Exercise.Helpers;
  using Assets.Scripts.Exercise.Utils;
  using Assets.Scripts.Helpers;
  using Assets.Scripts.SceneLoadingOnDemand.Task;
  using Assets.UI.Splash.Scripts;

  using MediaLab.Adic.Extensions.EventCaller;
  using MediaLab.Adic.Extensions.InitializableCaller;
  using MediaLab.Adic.Framework.Attributes;
  using MediaLab.Adic.Framework.Container;
  using MediaLab.NetworkEntities.Simpa.Exercises;
  using MediaLab.NetworkEntities.Simpa.Instruction;

  using UnityEngine;
  using UnityEngine.SceneManagement;

  public class ExerciseManager : IInitializable, IUpdatable
  {
    public const string StartupSceneName = "Startup";

    private readonly Dictionary<ApplicationHelper.AppTypeEnum, List<AppBindingConfigurer>> appBindingDictionary = new Dictionary<ApplicationHelper.AppTypeEnum, List<AppBindingConfigurer>>();

    private readonly ExerciseOperation loadScenesOperation = new ExerciseOperation { ProgressFactor = 1.0f };

    private readonly List<ExerciseOperation> exerciseOperations = new List<ExerciseOperation>();

    [Inject]
    private IInjectionContainer injectionContainer;

    private bool resetMode;

    private bool enabled;

    public event EventHandler<ExerciseStateEventArgs> BeforeLoad;

    public event EventHandler<ExerciseStateEventArgs> AfterLoad;

    public event EventHandler<ExerciseStateEventArgs> BeforeUnload;

    public event EventHandler<ExerciseStateEventArgs> AfterUnload;

    public event EventHandler<ExerciseStateEventArgs> UserReady;

    public event EventHandler<ExerciseStateEventArgs> Started;

    public event EventHandler<ExerciseStateEventArgs> Paused;

    public event EventHandler<ExerciseStateEventArgs> UnPaused;

    public event EventHandler<ExerciseFinishedEventArgs> Finished;

    public enum StateEnum
    {
      Loading,
      Loaded,
      Unloading,
      Unloaded,
      UserReady,
      Executing,
      Paused,
      Finished
    }

    public ExerciseEntity ExerciseEntity { get; private set; }

    public bool? IsSuccess { get; private set; }

    public ExerciseFinishedControlActionEntity ExerciseReport { get; private set; }

    public StateEnum State { get; private set; } = StateEnum.Unloaded;

    private bool CanLoadExercise => this.State == StateEnum.Unloaded || this.State == StateEnum.Finished;

    private bool CanResetExecise =>
      this.State == StateEnum.Loaded || this.State == StateEnum.UserReady || this.State == StateEnum.Executing
      || this.State == StateEnum.Paused || this.State == StateEnum.Finished;

    private bool CanUnloadExercise => this.CanResetExecise;

    private bool CanSetAsReady => this.State == StateEnum.Loaded;

    private bool CanStartExercise => this.State == StateEnum.UserReady;

    private bool CanPauseExercise => this.State == StateEnum.Executing;

    private bool CanUnpauseExercise => this.State == StateEnum.Paused;

    private bool CanFinishExercise => this.State == StateEnum.Executing || this.State == StateEnum.Paused;

    public void RegisterAppBinding(ApplicationHelper.AppTypeEnum appType, AppBindingConfigurer appBindingConfigurer)
    {
      List<AppBindingConfigurer> bindings;
      if (this.appBindingDictionary.TryGetValue(appType, out bindings))
      {
        if (!bindings.Contains(appBindingConfigurer))
        {
          bindings.Add(appBindingConfigurer);
        }
      }
      else
      {
        bindings = new List<AppBindingConfigurer> { appBindingConfigurer };

        this.appBindingDictionary.Add(appType, bindings);
      }
    }

    public void UnregisterAppBinding(ApplicationHelper.AppTypeEnum appType, AppBindingConfigurer appBindingConfigurer)
    {
      List<AppBindingConfigurer> bindings;
      if (this.appBindingDictionary.TryGetValue(appType, out bindings))
      {
        bindings.Remove(appBindingConfigurer);
      }
    }

    public bool LoadExercise(ExerciseEntity exerciseEntity)
    {
      if (!this.CanLoadExercise)
      {
        Debug.LogWarning($"Cannot load exercise '{exerciseEntity.App.Title}' in current state '{this.State}'");
        return false;
      }
      
      this.resetMode = false;
      this.InternalLoadExercise(exerciseEntity);

      return true;
    }

    public bool ResetExercise()
    {
      if (!this.CanResetExecise)
      {
        Debug.LogWarning($"Cannot reset exercise in current state '{this.State}'");
        return false;
      }

      this.resetMode = true;
      var currentExerciseEntity = this.ExerciseEntity;
      this.InternalUnloadExercise();
      this.InternalLoadExercise(currentExerciseEntity);

      return true;
    }

    public bool UnloadExercise()
    {
      if (!this.CanUnloadExercise)
      {
        Debug.LogWarning($"Cannot unload exercise in current state '{this.State}'");
        return false;
      }

      this.resetMode = false;
      this.InternalUnloadExercise();

      return true;
    }

    public bool OnUserReady()
    {
      if (!this.CanSetAsReady)
      {
        Debug.LogWarning($"Cannot set exercise as ready in current state '{this.State}'");
        return false;
      }

      Time.timeScale = 1;

      this.OnUserReady(new ExerciseStateEventArgs(this.ExerciseEntity));

      return true;
    }

    public bool StartExercise()
    {
      if (!this.CanStartExercise)
      {
        Debug.LogWarning($"Cannot start exercise in current state '{this.State}'");
        return false;
      }

      Time.timeScale = 1;

      this.OnStartExercise(new ExerciseStateEventArgs(this.ExerciseEntity));

      return true;
    }

    public bool PauseExercise()
    {
      if (!this.CanPauseExercise)
      {
        Debug.LogWarning($"Cannot pause exercise in current state '{this.State}'");
        return false;
      }

      Time.timeScale = 0;

      this.OnPauseExercise(new ExerciseStateEventArgs(this.ExerciseEntity));

      return true;
    }

    public bool UnPauseExercise()
    {
      if (!this.CanUnpauseExercise)
      {
        Debug.LogWarning($"Cannot unpause exercise in current state '{this.State}'");
        return false;
      }

      Time.timeScale = 1;

      this.OnUnPauseExercise(new ExerciseStateEventArgs(this.ExerciseEntity));

      return true;
    }

    public bool FinishExercise(bool sucess, ExerciseFinishedControlActionEntity report)
    {
      if (!this.CanFinishExercise)
      {
        Debug.LogWarning($"Cannot finish exercise in current state '{this.State}'");
        return false;
      }

      Time.timeScale = 1;
      Canvas.ForceUpdateCanvases();

      this.IsSuccess = sucess;
      this.ExerciseReport = report;

      this.OnFinishExercise(new ExerciseFinishedEventArgs(this.ExerciseEntity, this.IsSuccess.Value, this.ExerciseReport));

      return true;
    }

    void IInitializable.Init()
    {
      this.exerciseOperations.Add(this.loadScenesOperation);
    }

    void IUpdatable.Update()
    {
      if (SplashController.Singleton != null && SplashController.Singleton.isActiveAndEnabled)
      {
        SplashController.Singleton.Progress = 0.0f;

        foreach (var operation in this.exerciseOperations)
        {
          SplashController.Singleton.Progress += operation.CurrentProgress;
        }
      }
    }

    protected virtual void OnBeforeLoad(ExerciseStateEventArgs exerciseStateEventArgs)
    {
      this.State = StateEnum.Loading;

      Debug.Log(
        $"Loading exercise with ID {exerciseStateEventArgs.ExerciseEntity.Id} ({exerciseStateEventArgs.ExerciseEntity.App.Title}) (Type: {exerciseStateEventArgs.ExerciseEntity.GetAppType()}). Reset mode: {exerciseStateEventArgs.IsResetMode}");

      this.BeforeLoad?.Invoke(this, exerciseStateEventArgs);
    }

    protected virtual void OnAfterLoad(ExerciseStateEventArgs exerciseStateEventArgs)
    {
      this.State = StateEnum.Loaded;

      Debug.Log(
        $"Exercise with ID {exerciseStateEventArgs.ExerciseEntity.Id} ({exerciseStateEventArgs.ExerciseEntity.App.Title}) (Type: {exerciseStateEventArgs.ExerciseEntity.GetAppType()}) loaded. Reset mode: {exerciseStateEventArgs.IsResetMode}");

      this.AfterLoad?.Invoke(this, exerciseStateEventArgs);
    }

    protected virtual void OnBeforeUnload(ExerciseStateEventArgs exerciseStateEventArgs)
    {
      this.State = StateEnum.Unloading;

      Debug.Log(
        $"Unloading exercise with ID {exerciseStateEventArgs.ExerciseEntity.Id} ({exerciseStateEventArgs.ExerciseEntity.App.Title}) (Type: {exerciseStateEventArgs.ExerciseEntity.GetAppType()}). Reset mode: {exerciseStateEventArgs.IsResetMode}");

      this.BeforeUnload?.Invoke(this, exerciseStateEventArgs);
    }

    protected virtual void OnAfterUnload(ExerciseStateEventArgs exerciseStateEventArgs)
    {
      this.State = StateEnum.Unloaded;

      Debug.Log(
        $"Exercise with ID {exerciseStateEventArgs.ExerciseEntity.Id} ({exerciseStateEventArgs.ExerciseEntity.App.Title}) (Type: {exerciseStateEventArgs.ExerciseEntity.GetAppType()}) unloaded. Reset mode: {exerciseStateEventArgs.IsResetMode}");

      this.AfterUnload?.Invoke(this, exerciseStateEventArgs);
    }

    protected virtual void OnUserReady(ExerciseStateEventArgs exerciseStateEventArgs)
    {
      this.State = StateEnum.UserReady;

      Debug.Log(
        $"Exercise with ID {exerciseStateEventArgs.ExerciseEntity.Id} ({exerciseStateEventArgs.ExerciseEntity.App.Title}) (Type: {exerciseStateEventArgs.ExerciseEntity.GetAppType()}) ready");

      this.UserReady?.Invoke(this, exerciseStateEventArgs);
    }

    protected virtual void OnStartExercise(ExerciseStateEventArgs exerciseStateEventArgs)
    {
      this.State = StateEnum.Executing;

      Debug.Log(
        $"Executing exercise with ID {exerciseStateEventArgs.ExerciseEntity.Id} ({exerciseStateEventArgs.ExerciseEntity.App.Title}) (Type: {exerciseStateEventArgs.ExerciseEntity.GetAppType()})");

      this.Started?.Invoke(this, exerciseStateEventArgs);
    }

    protected virtual void OnPauseExercise(ExerciseStateEventArgs exerciseStateEventArgs)
    {
      this.State = StateEnum.Paused;

      Debug.Log(
        $"Exercise with ID {exerciseStateEventArgs.ExerciseEntity.Id} ({exerciseStateEventArgs.ExerciseEntity.App.Title}) (Type: {exerciseStateEventArgs.ExerciseEntity.GetAppType()}) paused");

      this.Paused?.Invoke(this, exerciseStateEventArgs);
    }

    protected virtual void OnUnPauseExercise(ExerciseStateEventArgs exerciseStateEventArgs)
    {
      this.State = StateEnum.Executing;

      Debug.Log(
        $"Exercise with ID {exerciseStateEventArgs.ExerciseEntity.Id} ({exerciseStateEventArgs.ExerciseEntity.App.Title}) (Type: {exerciseStateEventArgs.ExerciseEntity.GetAppType()}) unpaused and executing");

      this.UnPaused?.Invoke(this, exerciseStateEventArgs);
    }

    protected virtual void OnFinishExercise(ExerciseFinishedEventArgs exerciseFinishedEventArgs)
    {
      this.State = StateEnum.Finished;

      Debug.Log(
        $"Exercise with ID {exerciseFinishedEventArgs.ExerciseEntity.Id} ({exerciseFinishedEventArgs.ExerciseEntity.App.Title}) (Type: {exerciseFinishedEventArgs.ExerciseEntity.GetAppType()}) finished");

      this.Finished?.Invoke(this, exerciseFinishedEventArgs);
    }

    private void InternalLoadExercise(ExerciseEntity exerciseEntity)
    {
      Time.timeScale = 1;

      this.ExerciseEntity = exerciseEntity;
      this.IsSuccess = null;
      this.ExerciseReport = null;

      this.loadScenesOperation.ZoneOperation = null;

      var exerciseStateEventArgs = new ExerciseStateEventArgs(this.ExerciseEntity, this.resetMode);
      this.OnBeforeLoad(exerciseStateEventArgs);

      if (SplashController.Singleton != null)
      {
        SplashController.Singleton.SetActive(true);
      }

      this.HandleAppIntroVideoFinished();
    }

    private void HandleAppIntroVideoFinished()
    {
      Application.backgroundLoadingPriority = ThreadPriority.High;

      var taskQueue = new ZoneTaskQueue();

      var appScene = SceneManager.GetSceneByName(this.ExerciseEntity.VisualScenario.BaseSceneName);
      if (!appScene.isLoaded)
      {
        taskQueue.EnqueueTask(new ZoneLoadSceneAsyncTask(this.ExerciseEntity.VisualScenario.BaseSceneName, LoadSceneMode.Single));
      }

      if (this.ExerciseEntity.VisualScenario.AdditionalScenes != null)
      {
        foreach (var sceneName in this.ExerciseEntity.VisualScenario.AdditionalScenes)
        {
          var oilfieldScene = SceneManager.GetSceneByName(sceneName);
          if (!oilfieldScene.isLoaded)
          {
            taskQueue.EnqueueTask(new ZoneLoadSceneAsyncTask(sceneName));
          }
        }
      }

      if (taskQueue.StartTask())
      {
        this.loadScenesOperation.ZoneOperation = new ZoneOperation(taskQueue);
        this.loadScenesOperation.ZoneOperation.Completed += this.HandleTaskLoadScenesCompleted;
      }
      else
      {
        this.loadScenesOperation.ZoneOperation = new ZoneOperation(null);
        this.ConfigureExercise();
      }
    }

    private void InternalUnloadExercise()
    {
      Time.timeScale = 1;

      if (SplashController.Singleton != null)
      {
        SplashController.Singleton.SetActive(true);
        SplashController.Singleton.Progress = 0.0f;
      }

      Application.backgroundLoadingPriority = ThreadPriority.High;

      this.loadScenesOperation.ZoneOperation = null;
      this.IsSuccess = null;
      this.ExerciseReport = null;

      var exerciseStateEventArgs = new ExerciseStateEventArgs(this.ExerciseEntity, this.resetMode);
      this.OnBeforeUnload(exerciseStateEventArgs);

      if (this.resetMode)
      {
        this.OnAfterUnload(exerciseStateEventArgs);
      }
      else
      {
        this.RemoveAppBindings(this.ExerciseEntity.GetAppType());

        SceneManager.sceneLoaded += this.HandleStartupSceneLoaded;
        SceneManager.LoadSceneAsync(StartupSceneName, LoadSceneMode.Single);
      }
    }

    private void HandleTaskLoadScenesCompleted(IZoneOperation operation)
    {
      operation.Completed -= this.HandleTaskLoadScenesCompleted;

      this.ConfigureExercise();
    }

    private void ConfigureExercise()
    {
      SceneManager.SetActiveScene(SceneManager.GetSceneByName(this.ExerciseEntity.VisualScenario.BaseSceneName));

      if (!this.resetMode)
      {
        this.AddAppBindings(this.ExerciseEntity.GetAppType());
      }

      this.FinishLoadExercise();
    }

    private void HandleStartupSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
      SceneManager.sceneLoaded -= this.HandleStartupSceneLoaded;
      var exercise = this.ExerciseEntity;

      this.ExerciseEntity = null;
      GC.Collect();

      this.OnAfterUnload(new ExerciseStateEventArgs(exercise, this.resetMode));
    }

    private void FinishLoadExercise()
    {
      if (SplashController.Singleton != null)
      {
        SplashController.Singleton.Progress = 1.0f;
        SplashController.Singleton.SetActive(false);
      }

      this.OnAfterLoad(new ExerciseStateEventArgs(this.ExerciseEntity, this.resetMode));
      GC.Collect();
    }

    private void AddAppBindings(ApplicationHelper.AppTypeEnum appType)
    {
      List<AppBindingConfigurer> bindings;
      if (this.appBindingDictionary.TryGetValue(appType, out bindings))
      {
        if (bindings != null)
        {
          foreach (var appBindingConfigurer in bindings)
          {
            appBindingConfigurer.AddBindings(this.injectionContainer);
          }
        }
      }
    }

    private void RemoveAppBindings(ApplicationHelper.AppTypeEnum appType)
    {
      List<AppBindingConfigurer> bindings;
      if (this.appBindingDictionary.TryGetValue(appType, out bindings))
      {
        if (bindings != null)
        {
          foreach (var appBindingConfigurer in bindings)
          {
            appBindingConfigurer.RemoveBindings(this.injectionContainer);
          }
        }
      }
    }
  }
}