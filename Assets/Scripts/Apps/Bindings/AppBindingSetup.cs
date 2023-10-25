namespace Assets.Scripts.Apps.Bindings
{
  using Assets.Scripts.Apps.LocationTour;

  using MediaLab.Adic.Extensions.BindingsSetup;
  using MediaLab.Adic.Framework.Container;

  public class AppBindingSetup : IBindingsSetup
  {
    public void SetupBindings(IInjectionContainer container)
    {
      container.Bind<LocationTourBindingConfigurer>().ToSingleton();
    }
  }
}
