namespace Assets.Scripts.SceneLoadingOnDemand.Task
{
  using UnityEngine.Events;

  public interface IZoneOperation
  {
    event UnityAction<IZoneOperation> Completed;

    bool IsExecuting { get; }

    bool IsDone { get; }

    bool IsAborted { get; }

    float Progress { get; }
  }
}
