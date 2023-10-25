namespace Assets.UnityAssets.Editor.Platforms.Hooks
{
  public interface IBaseHook
  {
    void PreBuild();

    void PostBuild();
  }
}