namespace Assets.Scripts.SceneLoadingOnDemand.Task
{
  using System;
  using UnityEngine.Events;

  public abstract class ZoneTask
  {
    private DateTime startedTime;
    private TimeSpan timeElapsed;

    public event UnityAction<ZoneTask> Started;

    public event UnityAction<ZoneTask> Completed;

    public bool IsExecuting { get; private set; }

    public bool IsDone { get; private set; }

    public bool IsAborted { get; private set; }

    public abstract float Progress { get; }

    public int TimeElapsed => this.IsDone ? (int)this.timeElapsed.TotalMilliseconds : -1; 

    public bool StartTask()
    {
      if (this.IsExecuting || this.IsDone || this.IsAborted || !this.StartAsyncTask())
      {
        return false;
      }

      this.IsExecuting = true;

      this.startedTime = DateTime.Now;

      this.Started?.Invoke(this);

      return true;
    }

    public bool AbortTask()
    {
      if (this.IsDone || this.IsAborted || !this.IsExecuting || !this.AbortAsyncTask())
      {
        return false;
      }

      this.OnTaskAborted();

      return true;
    }

    protected virtual void OnTaskCompleted()
    {
      this.IsDone = true;
      this.IsExecuting = false;

      this.timeElapsed = DateTime.Now - this.startedTime;

      this.Completed?.Invoke(this);
    }

    protected virtual void OnTaskAborted()
    {
      this.IsAborted = true;
      this.IsExecuting = false;

      this.Completed?.Invoke(this);
    }

    protected abstract bool StartAsyncTask();

    protected abstract bool AbortAsyncTask();
  }
}
