namespace Assets.Scripts.SceneLoadingOnDemand.Task
{
  using System.Collections.Generic;

  public class ZoneTaskQueue : ZoneTask
  {
    private readonly List<ZoneTask> tasks = new List<ZoneTask>();

    private int currentIndexTask;

    public override float Progress
    {
      get
      {
        var progress = 0.0f;
        var count = this.tasks.Count;
        for (var i = 0; i < count; i++)
        {
          progress += this.tasks[i].Progress / count;
        }

        return progress;
      }
    }

    private bool CanModifyQueue => !(this.IsDone || this.IsAborted || this.IsExecuting);

    public bool EnqueueTask(ZoneTask task)
    {
      if (this.CanModifyQueue && !this.tasks.Contains(task))
      {
        this.tasks.Add(task);
        return true;
      }

      return false;
    }

    public bool ContainsTask(ZoneTask task)
    {
      return this.tasks.Contains(task);
    }

    public bool RemoveTask(ZoneTask task)
    {
      return this.CanModifyQueue && this.RemoveTask(task);
    }

    protected override bool StartAsyncTask()
    {
      if (this.tasks.Count > 0)
      {
        this.currentIndexTask = 0;
        var task = this.tasks[this.currentIndexTask];
        if (task.IsDone || task.IsAborted)
        {
          if (this.tasks.Count > 1)
          {
            this.HandleTaskCompleted(task);
            return true;
          }

          return false;
        }

        task.Completed += this.HandleTaskCompleted;
        if (task.IsExecuting || task.StartTask())
        {
          return true;
        }

        task.Completed -= this.HandleTaskCompleted;
      }

      return false;
    }

    protected override bool AbortAsyncTask()
    {
      foreach (var task in this.tasks)
      {
        task.Completed -= this.HandleTaskCompleted;
        if (!task.AbortTask())
        {
          return false;
        }
      }

      return true;
    }

    private void HandleTaskCompleted(ZoneTask task)
    {
      task.Completed -= this.HandleTaskCompleted;

      if (task.IsDone)
      {
        this.currentIndexTask++;
        if (this.currentIndexTask < this.tasks.Count)
        {
          var nextTask = this.tasks[this.currentIndexTask];
          if (nextTask.IsDone || nextTask.IsAborted)
          {
            this.HandleTaskCompleted(nextTask);
          }
          else
          {
            nextTask.Completed += this.HandleTaskCompleted;
            if (!nextTask.IsExecuting && !nextTask.StartTask())
            {
              nextTask.Completed -= this.HandleTaskCompleted;

              this.OnTaskAborted();
            }
          }
        }
        else
        {
          this.OnTaskCompleted();
        }
      }
      else if (task.IsAborted)
      {
        this.OnTaskAborted();
      }
    }
  }
}
