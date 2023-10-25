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

  public class StartedStateAction : IInitializable
  {
    [Inject]
    private CommunicationManager communicationManager;

    [Inject]
    private ExerciseManager exerciseManager;

    [Inject]
    private StateMachineManager stateMachineManager;

    void IInitializable.Init()
    {
      this.stateMachineManager.ApplicationStateMachine.OnEntryStartedFromInitializeVisual += this.HandleStart;
      this.stateMachineManager.ApplicationStateMachine.OnEntryStartedFromStop += this.HandleStop;
    }

    private void HandleStart()
    {
      this.stateMachineManager.FireStateChanged(new StateMachineEventArgs(StateMachineManager.StateEventEnum.Started));
    }

    private void HandleStart(ControlActionEntity entity)
    {
      this.HandleStart();
    }

    private void HandleStop(ControlActionEntity entity)
    {
      this.communicationManager.ClearBuffers();

      this.exerciseManager.AfterUnload += this.HandleExerciseAfterUnload;
      this.exerciseManager.UnloadExercise();
    }

    private void HandleExerciseAfterUnload(object sender, ExerciseStateEventArgs exerciseStateEventArgs)
    {
      this.exerciseManager.AfterUnload -= this.HandleExerciseAfterUnload;  

      var controlActionEntity = new ControlActionEntity
      {
        ControlAction = Constants.ControlActionEnum.Stop,
        Status = Constants.ControlActionStatusEnum.Done
      };

      this.communicationManager.SendMessage(controlActionEntity);

      Debug.Log($"Simulation stopped. Message ControlActionEntity({controlActionEntity.ControlAction}, {controlActionEntity.Status}) sent.");

      this.stateMachineManager.FireStateChanged(new StateMachineEventArgs(StateMachineManager.StateEventEnum.Started));
    }
  }
}
