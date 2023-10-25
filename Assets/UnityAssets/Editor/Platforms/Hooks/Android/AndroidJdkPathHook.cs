namespace Assets.UnityAssets.Editor.Platforms.Hooks.Android
{
  using Assets.UnityAssets.Editor.Platforms;

  public class AndroidJdkPathHook : AndroidBaseHook
  {
    public override void PreBuild()
    {
      // Check JDK Path
      if (!BuildHelper.IsJdkPathSet() || !BuildHelper.IsJdkPathValid())
      {
        BuildHelper.FixJdkPath();
      }
    }

    public override void PostBuild()
    {
    }
  }
}