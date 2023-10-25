#pragma warning disable 649
namespace Assets.Scripts.SandBox
{
  using MediaLab.NetworkEntities;
  using MediaLab.NetworkEntities.Entities;
  using MediaLab.NetworkEntities.Simpa.Instruction;

  public class MockInstructor
  {
    private readonly StandAloneClientCommunication standAloneClientCommunication;

    private bool playPendingResponseNeeded;

    private bool initializeVisualDoneResponseNeeded;

    private bool startDoneResponseNeeded;

    private bool finishDoneResponseNeeded;

    private bool finishErrorResponseNeeded;

    public MockInstructor(StandAloneClientCommunication standAloneClientCommunication)
    {
      this.standAloneClientCommunication = standAloneClientCommunication;
    }

    public void HandleMessageFromVisual(JsonEntity jsonEntity)
    {
      var activityStateControlActionEntity = jsonEntity as ActivityStateControlActionEntity;
      if (activityStateControlActionEntity != null)
      {
        if (activityStateControlActionEntity.ControlAction == Constants.ControlActionEnum.Start)
        {
          this.startDoneResponseNeeded = true;
        }
        else if (activityStateControlActionEntity.ControlAction == Constants.ControlActionEnum.Finish)
        {
          if (activityStateControlActionEntity.Status == Constants.ControlActionStatusEnum.Error)
          {
            this.finishErrorResponseNeeded = true;
          }
          else
          {
            this.finishDoneResponseNeeded = true;
          }
        }
      }
      else
      {
        var controlActionEntity = jsonEntity as ControlActionEntity;
        if (controlActionEntity != null)
        {
          if (controlActionEntity.ControlAction == Constants.ControlActionEnum.Load
              || controlActionEntity.ControlAction == Constants.ControlActionEnum.Reset)
          {
            this.playPendingResponseNeeded = true;

            this.startDoneResponseNeeded = false;
            this.finishErrorResponseNeeded = false;
            this.finishDoneResponseNeeded = false;
          }
          else if (controlActionEntity.ControlAction == Constants.ControlActionEnum.InitializeVisual)
          {
            this.initializeVisualDoneResponseNeeded =
              controlActionEntity.Status != Constants.ControlActionStatusEnum.Error;
          }
        }
      }
    }

    public void SendMessageToVisual(JsonEntity jsonEntity)
    {
      this.standAloneClientCommunication.HandleMockMessage(jsonEntity);
    }

    public bool SendMessage(JsonEntity jsonEntity)
    {
      var controlActionEntity = jsonEntity as ControlActionEntity;
      if (controlActionEntity != null)
      {
        this.HandleMessageFromVisual(controlActionEntity);
      }

      return true;
    }

    public void Update()
    {
      if (this.playPendingResponseNeeded)
      {
        this.playPendingResponseNeeded = false;

        var controlActionEntity = new ControlActionEntity
        {
          ControlAction = Constants.ControlActionEnum.Play,
          Status = Constants.ControlActionStatusEnum.Pending
        };

        this.SendMessageToVisual(controlActionEntity);
      }

      if (this.initializeVisualDoneResponseNeeded)
      {
        this.initializeVisualDoneResponseNeeded = false;

        var controlActionEntity = new ControlActionEntity
        {
          ControlAction = Constants.ControlActionEnum.InitializeVisual,
          Status = Constants.ControlActionStatusEnum.Done
        };

        this.SendMessageToVisual(controlActionEntity);
      }

      if (this.startDoneResponseNeeded)
      {
        this.startDoneResponseNeeded = false;

        var controlActionEntity = new ActivityStateControlActionEntity
        {
          ControlAction = Constants.ControlActionEnum.Start,
          Status = Constants.ControlActionStatusEnum.Done
        };

        this.SendMessageToVisual(controlActionEntity);
      }

      if (this.finishDoneResponseNeeded)
      {
        this.finishDoneResponseNeeded = false;

        var activityStateControlActionEntity = new ActivityStateControlActionEntity
        {
          ControlAction = Constants.ControlActionEnum.Finish,
          Status = Constants.ControlActionStatusEnum.Done
        };

        this.SendMessageToVisual(activityStateControlActionEntity);

        // TODO: Cuando terminan todas las actividades, se debe mandar este ExerciseFinishedControlActionEntity
        /*  var controlAction = new ExerciseFinishedControlActionEntity
          {
            ControlAction = Constants.ControlActionEnum.ExerciseSuccess,
            Status = Constants.ControlActionStatusEnum.Pending,
            Criteria = MediaLab.Common.Constants.EventTypeEnum.Success
          };

          this.SendMessageToVisual(controlAction);*/
      }

      if (this.finishErrorResponseNeeded)
      {
        this.finishErrorResponseNeeded = false;

        var activityStateControlActionEntity = new ActivityStateControlActionEntity
        {
          ControlAction = Constants.ControlActionEnum.Finish,
          Status = Constants.ControlActionStatusEnum.Done
        };

        this.SendMessageToVisual(activityStateControlActionEntity);

        var controlAction = new ExerciseFinishedControlActionEntity
        {
          ControlAction = Constants.ControlActionEnum.ExerciseError,
          Status = Constants.ControlActionStatusEnum.Pending,
          Criteria = MediaLab.Common.Constants.EventTypeEnum.Error
        };

        this.SendMessageToVisual(controlAction);
      }
    }
  }
}
