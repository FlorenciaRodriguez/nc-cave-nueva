#pragma warning disable 649
namespace Assets.Scripts.Executors
{
  using Assets.Scripts.Apps.Receivers;

  using MediaLab.Adic.Framework.Attributes;
  using MediaLab.NetworkEntities.Simpa.Configuration;

  public class AppViewEntityExecutor : MediaLab.NetworkEntities.Executors.Executor
  {
    [Inject]
    private AppViewEntityReceiver appViewEntityReceiver;

    public override void Execute(params object[] args)
    {
      var appViewEntity = this.Entity as AppViewEntity;
      if (appViewEntity != null)
      {
        this.appViewEntityReceiver?.OnEntityReceived(appViewEntity);
      }
    }
  }
}
