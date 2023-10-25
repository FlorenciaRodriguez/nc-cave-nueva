namespace Assets.UnityAssets.Editor.Platforms.Hooks.Android
{
  using Assets.UnityAssets.Editor.Platforms;

  public class AndroidSdkPathHook : AndroidBaseHook
  {
    public override void PreBuild()
    {
      // Check Android SDK Root
      if (!BuildHelper.IsAndroidSdkRootSet() || !BuildHelper.IsAndroidSdkRootValid())
      {
        BuildHelper.FixAndroidSdkRoot();
      }
    }

    public override void PostBuild()
    {
    }
  }
}
