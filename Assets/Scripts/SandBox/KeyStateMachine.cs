#pragma warning disable 649
namespace Assets.Scripts.Sandbox
{
  using Assets.Scripts.Exercise.Helpers;

  using MediaLab.Adic.Extensions.EventCaller;
  using MediaLab.Adic.Framework.Attributes;
  using MediaLab.Adic.Framework.Container;
  using MediaLab.NetworkEntities.Entities;
  using MediaLab.NetworkEntities.Helpers;
  using MediaLab.NetworkEntities.Simpa.Instruction;

  using UnityEngine;

  using Constants = MediaLab.NetworkEntities.Constants;

  public class KeyStateMachine : IUpdatable
  {
    [Inject]
    private IInjectionContainer injectionContainer;

    void IUpdatable.Update()
    {
      if (Input.GetKeyDown(KeyCode.Alpha2))
      {
        var playControl = new ControlActionEntity()
        {
          ControlAction = Constants.ControlActionEnum.Play,
          Status = Constants.ControlActionStatusEnum.Pending
        };

        var executor = playControl.GetExecutor();
        this.injectionContainer.Inject(executor);
        executor.Execute();
      }
      else if (Input.GetKeyDown(KeyCode.Alpha3))
      {
        var pauseControl = new ControlActionEntity()
        {
          ControlAction = Constants.ControlActionEnum.Pause,
          Status = Constants.ControlActionStatusEnum.Pending
        };

        var executor = pauseControl.GetExecutor();
        this.injectionContainer.Inject(executor);
        executor.Execute();
      }
      else if (Input.GetKeyDown(KeyCode.Alpha4))
      {
        var resetControl = new ControlActionEntity()
        {
          ControlAction = Constants.ControlActionEnum.Reset,
          Status = Constants.ControlActionStatusEnum.Pending
        };

        var executor = resetControl.GetExecutor();
        this.injectionContainer.Inject(executor);
        executor.Execute();
      }
      else if (Input.GetKeyDown(KeyCode.Alpha5))
      {
        var stopControl = new ControlActionEntity()
        {
          ControlAction = Constants.ControlActionEnum.Stop,
          Status = Constants.ControlActionStatusEnum.Pending
        };

        var executor = stopControl.GetExecutor();
        this.injectionContainer.Inject(executor);
        executor.Execute();
      }
    }
  }
}