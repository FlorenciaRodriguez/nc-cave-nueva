namespace Assets.UnityAssets.Editor.Platforms.Hooks.Android
{
  using Assets.UnityAssets.Editor.Platforms.Hooks;

  /// <summary>
  /// All subclasses of AndroidBaseHook will be executed during each Android build.
  /// You must implement all methods in order to prevent "NotImplementedException". 
  /// </summary>
  public class AndroidBaseHook : IBaseHook
  {
    /// <summary>
    /// Executed before Android build 
    /// </summary>
    public virtual void PreBuild()
    {
      throw new System.NotImplementedException();
    }

    /// <summary>
    /// Executed after Android build 
    /// </summary>
    public virtual void PostBuild()
    {
      throw new System.NotImplementedException();
    }
  }
}
