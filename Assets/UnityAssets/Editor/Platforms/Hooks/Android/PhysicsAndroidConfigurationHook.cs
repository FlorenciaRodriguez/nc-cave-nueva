namespace Assets.UnityAssets.Editor.Platforms.Hooks.Android
{
  using UnityEngine;

  public class PhysicsAndroidConfigurationHook : AndroidBaseHook
  {
    private float oldFixedTimestep;

    public override void PreBuild()
    {
      this.oldFixedTimestep = Time.fixedDeltaTime;
      Time.fixedDeltaTime = 0.02f;
    }

    public override void PostBuild()
    {
      Time.fixedDeltaTime = this.oldFixedTimestep;
    }
  }
}
