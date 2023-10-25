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

  public class ResettingFinishedExerciseStateAction : IInitializable
  {
    [Inject]
    private CommunicationManager communicationManager;

    [Inject]
    private ExerciseManager exerciseManager;

    [Inject]
    private StateMachineManager stateMachineManager;

    void IInitializable.Init()
    {
      this.stateMachineManager.ApplicationStateMachine.OnEntryResettingFinishedExerciseFromPlay += this.HandleResettingFinishedExerciseFromPlay;
    }

    private void HandleResettingFinishedExerciseFromPlay(ControlActionEntity entity)
    {
      this.stateMachineManager.FireStateChanged(new StateMachineEventArgs(StateMachineManager.StateEventEnum.ResettingFinishedExercise));

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

      this.stateMachineManager.ApplicationStateMachine.FireResetFinishedExercise();
    }
  }
}
