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

  public class WaitingStateAction : IInitializable
  {
    [Inject]
    private ExerciseManager exerciseManager;

    [Inject]
    private CommunicationManager communicationManager;

    [Inject]
    private StateMachineManager stateMachineManager;

    void IInitializable.Init()
    {
      this.stateMachineManager.ApplicationStateMachine.OnEntryWaitingFromLoaded += this.HandleWaiting;
      this.stateMachineManager.ApplicationStateMachine.OnEntryWaitingFromResetDone += this.HandleWaiting;
      this.stateMachineManager.ApplicationStateMachine.OnEntryWaitingFromPause += this.HandlePause;
    }

    private void HandlePause(ControlActionEntity entity)
    {
      this.exerciseManager.PauseExercise();

      this.stateMachineManager.FireStateChanged(new StateMachineEventArgs(StateMachineManager.StateEventEnum.Paused));

      var controlActionEntity = new ControlActionEntity
      {
        ControlAction = Constants.ControlActionEnum.Pause,
        Status = Constants.ControlActionStatusEnum.Done
      };

      this.communicationManager.SendMessage(controlActionEntity);

      Debug.Log($"Simulation paused. Message ControlActionEntity({controlActionEntity.ControlAction}, {controlActionEntity.Status}) sent.");
    }

    private void HandleWaiting()
    {
      this.stateMachineManager.FireStateChanged(new StateMachineEventArgs(StateMachineManager.StateEventEnum.Waiting));
    }
  }
}
