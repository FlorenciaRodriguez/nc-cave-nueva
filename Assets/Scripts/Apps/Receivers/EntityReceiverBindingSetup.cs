namespace Assets.Scripts.Apps.Receivers
{
  using MediaLab.Adic.Extensions.BindingsSetup;
  using MediaLab.Adic.Framework.Container;

  public class EntityReceiverBindingSetup : IBindingsSetup
  {
    public void SetupBindings(IInjectionContainer container)
    {
      container.Bind<AppViewEntityReceiver>().ToSingleton();
      container.Bind<LocationEntityReceiver>().ToSingleton();
      container.Bind<LocationGuyEntityReceiver>().ToSingleton();
      container.Bind<MomentOfTheDayEntityReceiver>().ToSingleton();
      container.Bind<ActivityStateControlActionEntityReceiver>().ToSingleton();
    }
  }
}
