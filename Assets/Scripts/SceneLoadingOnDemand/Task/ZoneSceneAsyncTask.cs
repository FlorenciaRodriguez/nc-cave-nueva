namespace Assets.Scripts.SceneLoadingOnDemand.Task
{
  using UnityEngine;

  public abstract class ZoneSceneAsyncTask : ZoneTask
  {
    private AsyncOperation asyncOperation;

    public override float Progress
    {
      get
      {
        if (this.asyncOperation != null)
        {
          return this.asyncOperation.progress;
        }

        return 0.0f;
      }
    }

    protected override bool StartAsyncTask()
    {
      this.asyncOperation = this.StartAsyncOperation();
      if (this.asyncOperation != null)
      {
        this.asyncOperation.completed += this.HandleAsyncOperationCompleted;
        return true;
      }

      return false;
    }

    protected override bool AbortAsyncTask()
    {
      return this.asyncOperation == null;
    }

    protected abstract AsyncOperation StartAsyncOperation();

    private void HandleAsyncOperationCompleted(AsyncOperation operation)
    {
      operation.completed -= this.HandleAsyncOperationCompleted;
      this.OnTaskCompleted();
    }
  }
}
