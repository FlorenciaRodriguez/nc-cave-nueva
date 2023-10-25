namespace Assets.Scripts.SceneLoadingOnDemand.Task
{
  using UnityEngine.Events;

  public class ZoneOperation : IZoneOperation
  {
    private readonly ZoneTask zoneTask;

    public ZoneOperation(ZoneTask zoneTask)
    {
      this.zoneTask = zoneTask;

      if (this.zoneTask != null)
      {
        if (!this.zoneTask.IsDone || !this.zoneTask.IsAborted)
        {
          this.zoneTask.Completed += this.HandleZoneTaskCompleted;
        }
      }

      this.Completed = null;
    }

    public event UnityAction<IZoneOperation> Completed;

    public bool IsExecuting
    {
      get
      {
        if (this.zoneTask != null)
        {
          return this.zoneTask.IsExecuting;
        }

        return false;
      }
    }

    public bool IsDone
    {
      get
      {
        if (this.zoneTask != null)
        {
          return this.zoneTask.IsDone;
        }

        return true;
      }
    }

    public bool IsAborted
    {
      get
      {
        if (this.zoneTask != null)
        {
          return this.zoneTask.IsAborted;
        }

        return false;
      }
    }

    public float Progress
    {
      get
      {
        if (this.zoneTask != null)
        {
          return this.zoneTask.Progress;
        }

        return 1.0f;
      }
    }

    private void HandleZoneTaskCompleted(ZoneTask task)
    {
      task.Completed -= this.HandleZoneTaskCompleted;

      this.Completed?.Invoke(this);
    }
  }
}
