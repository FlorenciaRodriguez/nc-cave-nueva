#pragma warning disable 649
namespace Assets.Scripts.Executors
{
  using Assets.Scripts.ApplicationStateMachine;

  using MediaLab.Adic.Framework.Attributes;
  using MediaLab.NetworkEntities;
  using MediaLab.NetworkEntities.Entities;

  using UnityEngine;

  public class ControlActionEntityExecutor : MediaLab.NetworkEntities.Executors.Executor
  {
    [Inject]
    protected StateMachineManager StateMachineManager { get; set; }

    public override void Execute(params object[] args)
    {
      var controlActionEntity = this.Entity as ControlActionEntity;
      if (controlActionEntity != null)
      {
        if (controlActionEntity.ControlAction == Constants.ControlActionEnum.InitializeControl)
        {
          return;
        }

        Debug.Log($"Message ControlActionEntity({controlActionEntity.ControlAction}, {controlActionEntity.Status}) received.");

        this.StateMachineManager.ApplicationStateMachine.FireNetworkApplicationEvent(controlActionEntity);
      }
      else
      {
        Debug.LogError($"Cannot process entity received because is not type of {nameof(ControlActionEntity)}");
      }
    }
  }
}
