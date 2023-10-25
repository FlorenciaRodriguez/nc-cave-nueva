namespace Assets.UnityAssets.Editor.Platforms.Hooks.Windows64
{
  using Assets.UnityAssets.Editor.Platforms.Hooks;

  /// <summary>
  /// All subclasses of Windows64BaseHook will be executed during each Windows64 build.
  /// You must implement all methods in order to prevent "NotImplementedException". 
  /// </summary>
  public class Windows64BaseHook : IBaseHook
  {
    /// <summary>
    /// Executed before Windows64 build 
    /// </summary>
    public virtual void PreBuild()
    {
      throw new System.NotImplementedException();
    }

    /// <summary>
    /// Executed after Windows64 build 
    /// </summary>
    public virtual void PostBuild()
    {
      throw new System.NotImplementedException();
    }
  }
}
