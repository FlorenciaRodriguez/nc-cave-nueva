#pragma warning disable 649
namespace Assets.Scripts.Apps
{
  using System;

  using Assets.Scripts.Exercise;
  using Assets.Scripts.Exercise.EventArgs;

  using MediaLab.Adic.Extensions.BindingCaller;
  using MediaLab.Adic.Framework.Attributes;
  using MediaLab.Adic.Framework.Container;
  using MediaLab.NetworkEntities.Simpa.Exercises;

  public class BaseAppManager : IBindContainer, IUnbindContainer
  {
    [Inject]
    private ExerciseManager exerciseManager;

    public ExerciseEntity ExerciseEntity => this.exerciseManager?.ExerciseEntity;

    public ExerciseManager.StateEnum? ExerciseState => this.exerciseManager?.State;

    void IBindContainer.BindOnContainer(IInjectionContainer container)
    {
      if (this.exerciseManager != null)
      {
        this.exerciseManager.AfterLoad += this.HandleExerciseAfterLoad;
        this.exerciseManager.BeforeUnload += this.HandleExerciseBeforeUnload;
        this.exerciseManager.Started += this.HandleExerciseStarted;
        this.exerciseManager.Paused += this.HandleExercisePaused;
        this.exerciseManager.UnPaused += this.HandleExerciseUnPaused;
        this.exerciseManager.Finished += this.HandleExerciseFinished;
      }
      else
      {
        throw new Exception($"The reference '{nameof(ExerciseManager)}' of  the class '{this.GetType().Name}' has not been assigned");
      }

      this.InitApp();
    }

    void IUnbindContainer.UnbindOnContainer(IInjectionContainer container)
    {
      if (this.exerciseManager != null)
      {
        this.exerciseManager.AfterLoad -= this.HandleExerciseAfterLoad;
        this.exerciseManager.BeforeUnload -= this.HandleExerciseBeforeUnload;
        this.exerciseManager.Started -= this.HandleExerciseStarted;
        this.exerciseManager.Paused -= this.HandleExercisePaused;
        this.exerciseManager.UnPaused -= this.HandleExerciseUnPaused;
        this.exerciseManager.Finished -= this.HandleExerciseFinished;
      }

      this.DestroyApp();
    }

    protected virtual void InitApp()
    {
    }

    protected virtual void DestroyApp()
    {
    }

    protected virtual void LoadApp(ExerciseEntity exerciseEntity, bool resetMode)
    {
    }

    protected virtual void UnloadApp(ExerciseEntity exerciseEntity, bool resetMode)
    {
    }

    protected virtual void StartApp()
    {
    }

    protected virtual void PauseApp()
    {
    }

    protected virtual void UnPauseApp()
    {
    }

    protected virtual void FinishApp()
    {
    }

    private void HandleExerciseAfterLoad(object sender, ExerciseStateEventArgs exerciseStateEventArgs)
    {
      this.LoadApp(exerciseStateEventArgs.ExerciseEntity, exerciseStateEventArgs.IsResetMode);
    }

    private void HandleExerciseBeforeUnload(object sender, ExerciseStateEventArgs exerciseStateEventArgs)
    {
      this.UnloadApp(exerciseStateEventArgs.ExerciseEntity, exerciseStateEventArgs.IsResetMode);
    }

    private void HandleExerciseStarted(object sender, ExerciseStateEventArgs exerciseStateEventArgs)
    {
      this.StartApp();
    }

    private void HandleExercisePaused(object sender, ExerciseStateEventArgs exerciseStateEventArgs)
    {
      this.PauseApp();
    }

    private void HandleExerciseUnPaused(object sender, ExerciseStateEventArgs exerciseStateEventArgs)
    {
      this.UnPauseApp();
    }

    private void HandleExerciseFinished(object sender, ExerciseFinishedEventArgs exerciseFinishedEventArgs)
    {
      this.FinishApp();
    }
  }
}
