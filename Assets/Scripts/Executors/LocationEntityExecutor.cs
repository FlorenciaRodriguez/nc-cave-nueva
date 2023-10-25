#pragma warning disable 649
namespace Assets.Scripts.Executors
{
  using Assets.Scripts.Apps.Receivers;

  using MediaLab.Adic.Framework.Attributes;
  using MediaLab.NetworkEntities.Simpa.Assets;

  public class LocationEntityExecutor : MediaLab.NetworkEntities.Executors.Executor
  {
    [Inject]
    private LocationEntityReceiver locationEntityReceiver;

    public override void Execute(params object[] args)
    {
      var locationEntity = this.Entity as LocationEntity;
      if (locationEntity != null)
      {
        this.locationEntityReceiver?.OnEntityReceived(locationEntity);
      }
    }
  }
}
