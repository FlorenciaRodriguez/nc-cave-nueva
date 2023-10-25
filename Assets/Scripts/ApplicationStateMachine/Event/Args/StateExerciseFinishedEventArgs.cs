namespace Assets.Scripts.ApplicationStateMachine.Event.Args
{
  using MediaLab.NetworkEntities.Simpa.Instruction;

  public class StateExerciseFinishedEventArgs : StateMachineEventArgs
  {
    public StateExerciseFinishedEventArgs(StateMachineManager.StateEventEnum stateEventEnum, ExerciseFinishedControlActionEntity exerciseFinishedControlActionEntity)
      : base(stateEventEnum)
    {
      this.ExerciseFinishedControlActionEntity = exerciseFinishedControlActionEntity;
    }

    public ExerciseFinishedControlActionEntity ExerciseFinishedControlActionEntity { get; }
  }
}
