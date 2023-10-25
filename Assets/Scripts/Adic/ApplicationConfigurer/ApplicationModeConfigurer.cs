namespace Assets.Scripts.Adic.ApplicationConfigurer
{
  using MediaLab.Adic.Framework.Container;

  public abstract class ApplicationModeConfigurer
  {
    public abstract void SetupBindings(IInjectionContainer container);
  }
}