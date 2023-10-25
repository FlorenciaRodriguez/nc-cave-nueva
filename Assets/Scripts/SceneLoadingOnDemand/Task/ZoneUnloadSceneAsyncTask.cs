namespace Assets.Scripts.SceneLoadingOnDemand.Task
{
  using UnityEngine;
  using UnityEngine.SceneManagement;

  public class ZoneUnloadSceneAsyncTask : ZoneSceneAsyncTask
  {
    private readonly int sceneBuildIndex;

    private readonly string sceneName;

    private readonly bool loadFromBuildIndex;

    public ZoneUnloadSceneAsyncTask(int sceneBuildIndex)
    {
      this.sceneBuildIndex = sceneBuildIndex;
      this.loadFromBuildIndex = true;
    }

    public ZoneUnloadSceneAsyncTask(string sceneName)
    {
      this.sceneName = sceneName;
      this.loadFromBuildIndex = false;
    }

    protected override AsyncOperation StartAsyncOperation()
    {
      if (this.loadFromBuildIndex)
      {
        return SceneManager.UnloadSceneAsync(this.sceneBuildIndex);
      }

      return SceneManager.UnloadSceneAsync(this.sceneName);
    }
  }
}
