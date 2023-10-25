#pragma warning disable 649
namespace Assets.Scripts.Executors
{
  using Assets.Scripts.Apps.Receivers;

  using MediaLab.Adic.Framework.Attributes;
  using MediaLab.NetworkEntities.Simpa.Instruction;

  public class ActivityStateControlActionEntityExecutor : MediaLab.NetworkEntities.Executors.Executor
  {
    [Inject]
    private ActivityStateControlActionEntityReceiver activityStateControlActionEntityReceiver;

    public override void Execute(params object[] args)
    {
      var activityStateControlActionEntity = this.Entity as ActivityStateControlActionEntity;
      if (activityStateControlActionEntity != null)
      {
        this.activityStateControlActionEntityReceiver?.OnEntityReceived(activityStateControlActionEntity);
      }
    }
  }
}
