#pragma warning disable 649
namespace Assets.Scripts.ApplicationStateMachine.StateActions
{
  using Assets.Scripts.ApplicationStateMachine.Event.Args;
  using Assets.Scripts.Communication;
  using Assets.Scripts.Exercise;
  using Assets.Scripts.Exercise.EventArgs;

  using MediaLab.Adic.Extensions.InitializableCaller;
  using MediaLab.Adic.Framework.Attributes;
  using MediaLab.Adic.Framework.Container;
  using MediaLab.NetworkEntities.Entities;
  using MediaLab.NetworkEntities.Simpa.Instruction;

  using UnityEngine;

  using Constants = MediaLab.NetworkEntities.Constants;

  public class LoadingStateAction : IInitializable
  {
    [Inject]
    private ExerciseManager exerciseManager;

    [Inject]
    private CommunicationManager communicationManager;

    [Inject]
    private IInjectionContainer injectionContainer;

    [Inject]
    private StateMachineManager stateMachineManager;

    void IInitializable.Init()
    {
      this.stateMachineManager.ApplicationStateMachine.OnEntryLoadingFromLoad += this.HandleLoad;
    }

    private void HandleLoad(ControlActionEntity entity)
    {
      this.stateMachineManager.FireStateChanged(new StateMachineEventArgs(StateMachineManager.StateEventEnum.Loading));

      var loadControlActionEntity = (LoadControlActionEntity)entity;

      this.exerciseManager.UserReady += this.HandleExerciseUserReady;
      this.exerciseManager.AfterLoad += this.HandleExerciseAfterLoad;
      this.exerciseManager.LoadExercise(loadControlActionEntity.ExerciseEntity);
    }

    private void HandleExerciseAfterLoad(object sender, ExerciseStateEventArgs exerciseStateEventArgs)
    {
      this.exerciseManager.AfterLoad -= this.HandleExerciseAfterLoad;
      this.exerciseManager.OnUserReady();
    }

    private void HandleExerciseUserReady(object sender, ExerciseStateEventArgs exerciseStateEventArgs)
    {
      this.exerciseManager.UserReady -= this.HandleExerciseUserReady;

      var controlActionEntity = new LoadControlActionEntity
      {
        ControlAction = Constants.ControlActionEnum.Load,
        Status = Constants.ControlActionStatusEnum.Done
      };

      this.communicationManager.SendMessage(controlActionEntity);
      this.stateMachineManager.ApplicationStateMachine.FireLoaded();

      Debug.Log($"Simulation loaded and ready, waiting for play. Message ControlActionEntity({controlActionEntity.ControlAction}, {controlActionEntity.Status}) sent.");
    }
  }
}
