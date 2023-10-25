namespace Assets.Scripts.SceneLoadingOnDemand.Task
{
  using System.Collections.Generic;

  using UnityEngine.Events;

  public class ZoneOperationGroup : IZoneOperation
  {
    private readonly List<IZoneOperation> zoneOperations = new List<IZoneOperation>();

    public event UnityAction<IZoneOperation> Completed;

    public bool IsExecuting
    {
      get
      {
        foreach (var zoneOperation in this.zoneOperations)
        {
          if (zoneOperation.IsExecuting)
          {
            return true;
          }
        }

        return false;
      }
    }

    public bool IsDone
    {
      get
      {
        foreach (var zoneOperation in this.zoneOperations)
        {
          if (!zoneOperation.IsDone)
          {
            return false;
          }
        }

        return true;
      }
    }

    public bool IsAborted
    {
      get
      {
        foreach (var zoneOperation in this.zoneOperations)
        {
          if (!zoneOperation.IsAborted)
          {
            return false;
          }
        }

        return this.zoneOperations.Count > 0;
      }
    }

    public float Progress
    {
      get
      {
        if (this.zoneOperations.Count > 0)
        {
          var progress = 0.0f;

          foreach (var zoneOperation in this.zoneOperations)
          {
            progress += zoneOperation.Progress;
          }

          return progress / this.zoneOperations.Count;
        }

        return 1.0f;
      }
    }

    public void Add(IZoneOperation zoneOperation)
    {
      if (!this.zoneOperations.Contains(zoneOperation))
      {
        this.zoneOperations.Add(zoneOperation);

        if (!zoneOperation.IsDone || !zoneOperation.IsAborted)
        {
          zoneOperation.Completed += this.HandleZoneOperationCompleted;
        }
      }
    }

    public void Remove(IZoneOperation zoneOperation)
    {
      if (this.zoneOperations.Remove(zoneOperation))
      {
        zoneOperation.Completed -= this.HandleZoneOperationCompleted;
      }
    }

    public void Clear()
    {
      foreach (var zoneOperation in this.zoneOperations)
      {
        zoneOperation.Completed -= this.HandleZoneOperationCompleted;
      }

      this.zoneOperations.Clear();
    }

    private void HandleZoneOperationCompleted(IZoneOperation zoneOperation)
    {
      zoneOperation.Completed -= this.HandleZoneOperationCompleted;

      if (this.IsAllOperationCompleted())
      {
        this.Completed?.Invoke(this);
      }
    }

    private bool IsAllOperationCompleted()
    {
      foreach (var zoneOperation in this.zoneOperations)
      {
        if (!(zoneOperation.IsDone || zoneOperation.IsAborted))
        {
          return false;
        }
      }

      return true;
    }
  }
}
