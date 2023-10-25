#pragma warning disable 649
namespace Assets.Scripts.ApplicationStateMachine.StateActions
{
  using Assets.Scripts.ApplicationStateMachine.Event.Args;
  using Assets.Scripts.Communication;

  using MediaLab.Adic.Framework.Attributes;
  using MediaLab.NetworkEntities;
  using MediaLab.NetworkEntities.Entities;

  using UnityEngine;

  public class InitializingStateAction
  {
    [Inject]
    private CommunicationManager communicationManager;

    [Inject]
    private StateMachineManager stateMachineManager;

    public void Start()
    {
      this.stateMachineManager.FireStateChanged(new StateMachineEventArgs(StateMachineManager.StateEventEnum.Initializing));

      var controlActionEntity = new ControlActionEntity
      {
        ControlAction = Constants.ControlActionEnum.InitializeVisual,
        Status = Constants.ControlActionStatusEnum.Pending
      };

      this.communicationManager.SendMessage(controlActionEntity);

      Debug.Log($"Simulation initialized. Message ControlActionEntity({controlActionEntity.ControlAction}, {controlActionEntity.Status}) sent.");
    }
  }
}
