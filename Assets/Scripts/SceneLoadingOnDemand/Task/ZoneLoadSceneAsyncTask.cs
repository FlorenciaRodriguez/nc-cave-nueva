namespace Assets.Scripts.SceneLoadingOnDemand.Task
{
  using UnityEngine;
  using UnityEngine.SceneManagement;

  public class ZoneLoadSceneAsyncTask : ZoneSceneAsyncTask
  {
    private readonly int sceneBuildIndex;

    private readonly LoadSceneMode loadSceneMode;

    private readonly string sceneName;

    private readonly bool loadFromBuildIndex;

    public ZoneLoadSceneAsyncTask(int sceneBuildIndex, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
    {
      this.sceneBuildIndex = sceneBuildIndex;
      this.loadSceneMode = loadSceneMode;
      this.loadFromBuildIndex = true;
    }

    public ZoneLoadSceneAsyncTask(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
    {
      this.sceneName = sceneName;
      this.loadSceneMode = loadSceneMode;
      this.loadFromBuildIndex = false;
    }

    protected override AsyncOperation StartAsyncOperation()
    {
      if (this.loadFromBuildIndex)
      {
        return SceneManager.LoadSceneAsync(this.sceneBuildIndex, this.loadSceneMode);
      }

      return SceneManager.LoadSceneAsync(this.sceneName, this.loadSceneMode);
    }
  }
}
