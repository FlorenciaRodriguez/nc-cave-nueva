namespace Assets.Scripts.Exercise.EventArgs
{
  using MediaLab.NetworkEntities.Simpa.Exercises;

  public struct ExerciseStateEventArgs
  {
    public ExerciseStateEventArgs(ExerciseEntity exerciseEntity, bool resetMode = false)
    {
      this.ExerciseEntity = exerciseEntity;
      this.IsResetMode = resetMode;
    }

    public ExerciseEntity ExerciseEntity { get; }

    public bool IsResetMode { get; }
  }
}
