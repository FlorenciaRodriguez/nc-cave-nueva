namespace Assets.Scripts.Exercise.EventArgs
{
  using MediaLab.NetworkEntities.Simpa.Exercises;
  using MediaLab.NetworkEntities.Simpa.Instruction;

  public struct ExerciseFinishedEventArgs
  {
    public ExerciseFinishedEventArgs(ExerciseEntity exerciseEntity, bool success, ExerciseFinishedControlActionEntity report)
    {
      this.ExerciseEntity = exerciseEntity;
      this.IsSuccess = success;
      this.Report = report;
    }

    public ExerciseEntity ExerciseEntity { get; }

    public bool IsSuccess { get; }

    public ExerciseFinishedControlActionEntity Report { get; }
  }
}
