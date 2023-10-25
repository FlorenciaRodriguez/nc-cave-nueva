namespace Assets.Scripts.Exercise.Utils
{
  using Assets.Scripts.SceneLoadingOnDemand.Task;

  public class ExerciseOperation
  {
    public IZoneOperation ZoneOperation { get; set; }

    public float ProgressFactor { get; set; }

    public float CurrentProgress
    {
      get
      {
        if (this.ZoneOperation != null)
        {
          return this.ZoneOperation.Progress * this.ProgressFactor;
        }

        return 0.0f;
      }
    }
  }
}
