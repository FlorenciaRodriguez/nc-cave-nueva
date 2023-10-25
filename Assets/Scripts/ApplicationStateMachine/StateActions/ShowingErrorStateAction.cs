#pragma warning disable 649
namespace Assets.Scripts.ApplicationStateMachine.StateActions
{
  using Assets.Scripts.ApplicationStateMachine.Event.Args;
  using Assets.Scripts.Communication;
  using Assets.Scripts.Exercise;

  using MediaLab.Adic.Extensions.InitializableCaller;
  using MediaLab.Adic.Framework.Attributes;
  using MediaLab.NetworkEntities;
  using MediaLab.NetworkEntities.Entities;
  using MediaLab.NetworkEntities.Simpa.Instruction;

  using UnityEngine;

  public class ShowingErrorStateAction : IInitializable
  {
    [Inject]
    private CommunicationManager communicationManager;

    [Inject]
    private StateMachineManager stateMachineManager;

    [Inject]
    private ExerciseManager exerciseManager;

    void IInitializable.Init()
    {
      this.stateMachineManager.ApplicationStateMachine.OnEntryShowingErrorFromExerciseError += this.HandleExerciseError;
    }

    private void HandleExerciseError(ControlActionEntity entity)
    {
      this.communicationManager.ClearBuffers();

      var report = entity as ExerciseFinishedControlActionEntity;

      this.exerciseManager.FinishExercise(false, report);
      this.stateMachineManager.FireStateChanged(new StateExerciseFinishedEventArgs(StateMachineManager.StateEventEnum.ShowingError, report));

      var controlActionEntity = new ExerciseFinishedControlActionEntity
      {
        ControlAction = Constants.ControlActionEnum.ExerciseError,
        Status = Constants.ControlActionStatusEnum.Done
      };

      this.communicationManager.SendMessage(controlActionEntity);

      Debug.Log($"Simulation finished with error. Message ControlActionEntity({controlActionEntity.ControlAction}, {controlActionEntity.Status}) sended.");
    }
  }
}