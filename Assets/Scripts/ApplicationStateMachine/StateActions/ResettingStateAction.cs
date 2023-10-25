#pragma warning disable 649
namespace Assets.Scripts.ApplicationStateMachine.StateActions
{
  using Assets.Scripts.ApplicationStateMachine.Event.Args;
  using Assets.Scripts.Communication;
  using Assets.Scripts.Exercise;
  using Assets.Scripts.Exercise.EventArgs;

  using MediaLab.Adic.Extensions.InitializableCaller;
  using MediaLab.Adic.Framework.Attributes;
  using MediaLab.NetworkEntities.Entities;

  using UnityEngine;

  using Constants = MediaLab.NetworkEntities.Constants;

  public class ResettingStateAction : IInitializable
  {
    [Inject]
    private CommunicationManager communicationManager;

    [Inject]
    private ExerciseManager exerciseManager;

    [Inject]
    private StateMachineManager stateMachineManager;

    void IInitializable.Init()
    {
      // Resetting
      this.stateMachineManager.ApplicationStateMachine.OnEntryResettingFromReset += this.HandleReset;
    }

    private void HandleReset(ControlActionEntity entity)
    {
      this.stateMachineManager.FireStateChanged(new StateMachineEventArgs(StateMachineManager.StateEventEnum.Resetting));

      this.communicationManager.ClearBuffers();

      this.exerciseManager.UserReady += this.HandleExerciseUserReady;
      this.exerciseManager.AfterLoad += this.HandleExerciseAfterLoad;
      this.exerciseManager.ResetExercise();
    }

    private void HandleExerciseAfterLoad(object sender, ExerciseStateEventArgs exerciseStateEventArgs)
    {
      this.exerciseManager.AfterLoad -= this.HandleExerciseAfterLoad;
      this.exerciseManager.OnUserReady();
    }

    private void HandleExerciseUserReady(object sender, ExerciseStateEventArgs exerciseStateEventArgs)
    {
      this.exerciseManager.UserReady -= this.HandleExerciseUserReady;

      var controlActionEntity = new ControlActionEntity
      {
        ControlAction = Constants.ControlActionEnum.Reset,
        Status = Constants.ControlActionStatusEnum.Done
      };

      this.communicationManager.SendMessage(controlActionEntity);
      this.stateMachineManager.ApplicationStateMachine.FireResetDone();

      Debug.Log($"Simulation restarted and ready, waiting for play. Message ControlActionEntity({controlActionEntity.ControlAction}, {controlActionEntity.Status}) sent.");
    }
  }
}
