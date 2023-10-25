#pragma warning disable 649
namespace Assets.UI.CommunicationStatistics.Scripts
{
  using System;

  using Assets.Scripts.Adic.Utils;

  using Assets.Scripts.Communication;

  using MediaLab.Adic.Framework.Attributes;

  using UnityEngine;

  using UnityEngine.UI;

  [Serializable]
  [RequireComponent(typeof(Canvas))]
  public class CommunicationStatisticsController : BaseBehaviour
  {
    private const string ValueFormat = "{0:0}";

    [SerializeField]
    private Text textMaximumMessageSentRate;

    [SerializeField]
    private Text textCurrentMessageSentRate;

    [SerializeField]
    private Text textMaximumMessageReceivedRate;

    [SerializeField]
    private Text textCurrentMessageReceivedRate;

    [Inject]
    private CommunicationManager communicationManager;

    private Canvas canvas;

    protected void Awake()
    {
      this.canvas = this.GetComponent<Canvas>();
    }

    protected override void Start()
    {
      base.Start();

      if (this.communicationManager?.CommunicationStatistics != null)
      {
        this.communicationManager.CommunicationStatistics.Enabled = true;
      }
    }

    protected void Update()
    {
      if (this.communicationManager?.CommunicationStatistics != null && this.canvas.enabled)
      {
        this.textMaximumMessageSentRate.text = string.Format(ValueFormat, this.communicationManager.CommunicationStatistics.MaximumMessageSentRate);
        this.textCurrentMessageSentRate.text = string.Format(ValueFormat, this.communicationManager.CommunicationStatistics.CurrentMessageSentRate);
        this.textMaximumMessageReceivedRate.text = string.Format(ValueFormat, this.communicationManager.CommunicationStatistics.MaximumMessageReceivedRate);
        this.textCurrentMessageReceivedRate.text = string.Format(ValueFormat, this.communicationManager.CommunicationStatistics.CurrentMessageReceivedRate);
      }
    }

    protected void OnDestroy()
    {
      if (this.communicationManager?.CommunicationStatistics != null)
      {
        this.communicationManager.CommunicationStatistics.Enabled = false;
      }
    }

    protected void OnValidate()
    {
      if (this.textMaximumMessageSentRate == null)
      {
        Debug.LogError($"The variable '{nameof(this.textMaximumMessageSentRate)}' of '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.", this.gameObject);
      }

      if (this.textCurrentMessageSentRate == null)
      {
        Debug.LogError($"The variable '{nameof(this.textCurrentMessageSentRate)}' of '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.", this.gameObject);
      }

      if (this.textMaximumMessageReceivedRate == null)
      {
        Debug.LogError($"The variable '{nameof(this.textMaximumMessageReceivedRate)}' of '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.", this.gameObject);
      }

      if (this.textCurrentMessageReceivedRate == null)
      {
        Debug.LogError($"The variable '{nameof(this.textCurrentMessageReceivedRate)}' of '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.", this.gameObject);
      }
    }
  }
}
