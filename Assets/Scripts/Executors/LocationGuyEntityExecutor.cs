#pragma warning disable 649
namespace Assets.Scripts.Executors
{
  using Assets.Scripts.Apps.Receivers;

  using MediaLab.Adic.Framework.Attributes;
  using MediaLab.NetworkEntities.Simpa.Configuration;

  public class LocationGuyEntityExecutor : MediaLab.NetworkEntities.Executors.Executor
  {
    [Inject]
    private LocationGuyEntityReceiver locationGuyEntityReceiver;

    public override void Execute(params object[] args)
    {
      var locationGuyEntity = this.Entity as LocationGuyEntity;
      if (locationGuyEntity != null)
      {
        this.locationGuyEntityReceiver?.OnEntityReceived(locationGuyEntity);
      }
    }
  }
}
