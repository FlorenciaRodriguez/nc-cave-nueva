#pragma warning disable 649
namespace Assets.Scripts.ApplicationStateMachine.StateActions
{
  using Assets.Scripts.ApplicationStateMachine.Event.Args;
  using Assets.Scripts.Communication;
  using Assets.Scripts.Exercise;

  using MediaLab.Adic.Extensions.InitializableCaller;

  using MediaLab.Adic.Framework.Attributes;
  using MediaLab.Adic.Framework.Container;
  using MediaLab.NetworkEntities.Entities;
  using MediaLab.NetworkEntities.Simpa.Instruction;

  using UnityEngine;

  using Constants = MediaLab.NetworkEntities.Constants;

  [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1121:UseBuiltInTypeAlias", Justification = "Reviewed.")]
  public class ShowingReportStateAction : IInitializable
  {
    [Inject]
    private CommunicationManager communicationManager;

    [Inject]
    private ExerciseManager exerciseManager;

    [Inject]
    private StateMachineManager stateMachineManager;

    [Inject]
    private IInjectionContainer injectionContainer;

    void IInitializable.Init()
    {
      this.stateMachineManager.ApplicationStateMachine.OnEntryShowingReportFromExerciseSuccess += this.HandleExerciseSuccess;
    }

    private void HandleExerciseSuccess(ControlActionEntity entity)
    {
      this.communicationManager.ClearBuffers();

      var report = entity as ExerciseFinishedControlActionEntity;
      this.exerciseManager.FinishExercise(true, report);
      this.stateMachineManager.FireStateChanged(new StateExerciseFinishedEventArgs(StateMachineManager.StateEventEnum.ShowingReport, report));

      var controlActionEntity = new ExerciseFinishedControlActionEntity
      {
        ControlAction = Constants.ControlActionEnum.ExerciseSuccess,
        Status = Constants.ControlActionStatusEnum.Done
      };

      this.communicationManager.SendMessage(controlActionEntity);

      Debug.Log($"Simulation finished successfully. Message ControlActionEntity({controlActionEntity.ControlAction}, {controlActionEntity.Status}) sended.");
    }
  }
}
