#pragma warning disable 649
namespace Assets.Scripts.ApplicationStateMachine.StateActions
{
  using Assets.Scripts.ApplicationStateMachine.Event.Args;
  using Assets.Scripts.Communication;
  using Assets.Scripts.Exercise;

  using MediaLab.Adic.Extensions.InitializableCaller;
  using MediaLab.Adic.Framework.Attributes;
  using MediaLab.NetworkEntities.Entities;

  using UnityEngine;

  using Constants = MediaLab.NetworkEntities.Constants;

  public class ExecutingStateAction : IInitializable
  {
    [Inject]
    private CommunicationManager communicationManager;

    [Inject]
    private ExerciseManager exerciseManager;

    [Inject]
    private StateMachineManager stateMachineManager;

    void IInitializable.Init()
    {
      this.stateMachineManager.ApplicationStateMachine.OnEntryExecutingFromPlay += this.HandlePlay;
      this.stateMachineManager.ApplicationStateMachine.OnEntryExecutingFromResetFinishedExerciseDone += this.HandleResetFinishedExerciseDone;
    }

    private void HandlePlay(ControlActionEntity entity)
    {
      var controlActionEntity = new ControlActionEntity
      {
        ControlAction = Constants.ControlActionEnum.Play,
        Status = Constants.ControlActionStatusEnum.Done
      };

      this.communicationManager.SendMessage(controlActionEntity);

      Debug.Log($"Simulation started. Message ControlActionEntity({controlActionEntity.ControlAction}, {controlActionEntity.Status}) sent.");

      if (this.exerciseManager.State == ExerciseManager.StateEnum.Paused)
      {
        this.exerciseManager.UnPauseExercise();
      }
      else
      {
        this.exerciseManager.StartExercise();
      }

      this.stateMachineManager.FireStateChanged(new StateMachineEventArgs(StateMachineManager.StateEventEnum.Playing));
    }

    private void HandleResetFinishedExerciseDone()
    {
      this.HandlePlay(null);
    }
  }
}
