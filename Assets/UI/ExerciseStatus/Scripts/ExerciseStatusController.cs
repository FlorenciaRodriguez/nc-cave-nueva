#pragma warning disable 649
namespace Assets.UI.ExerciseStatus.Scripts
{
  using System.Collections.Generic;

  using Assets.Scripts.Adic.Utils;
  using Assets.Scripts.Exercise;
  using Assets.Scripts.Exercise.EventArgs;
  using Assets.UI.Common.Scripts.Helpers;

  using MediaLab.Adic.Framework.Attributes;

  using TMPro;

  using UnityEngine;
  using UnityEngine.UI;

  public class ExerciseStatusController : BaseBehaviour
  {
    private readonly List<Canvas> canvases = new List<Canvas>();

    [Inject]
    private ExerciseManager exerciseManager;

    [SerializeField]
    private Text textIcon;

    [SerializeField]
    private TextMeshProUGUI textLabel;

    protected void Awake()
    {
      this.canvases.AddRange(this.GetComponentsInChildren<Canvas>());
    }

    protected override void Start()
    {
      base.Start();

      if (this.exerciseManager != null)
      {
        this.exerciseManager.BeforeUnload += this.HandleExerciseInactive;
        this.exerciseManager.BeforeLoad += this.HandleExerciseInactive;
        this.exerciseManager.Started += this.HandleExerciseInactive;
        this.exerciseManager.Finished += this.HandleExerciseInactive;
        this.exerciseManager.UnPaused += this.HandleExerciseInactive;
        this.exerciseManager.Paused += this.HandleExercisePaused;
        this.exerciseManager.UserReady += this.HandleExerciseUserReady;
      }

      this.SetVisible(false);
    }

    protected void OnDestroy()
    {
      if (this.exerciseManager != null)
      {
        this.exerciseManager.BeforeUnload -= this.HandleExerciseInactive;
        this.exerciseManager.BeforeLoad -= this.HandleExerciseInactive;
        this.exerciseManager.Started -= this.HandleExerciseInactive;
        this.exerciseManager.Finished -= this.HandleExerciseInactive;
        this.exerciseManager.UnPaused -= this.HandleExerciseInactive;
        this.exerciseManager.Paused -= this.HandleExercisePaused;
        this.exerciseManager.UserReady -= this.HandleExerciseUserReady;
      }
    }

    protected void OnValidate()
    {
      if (this.textIcon == null)
      {
        Debug.LogError($"The variable '{nameof(this.textIcon)}' of the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.", this.gameObject);
      }

      if (this.textLabel == null)
      {
        Debug.LogError($"The variable '{nameof(this.textLabel)}' of the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.", this.gameObject);
      }
    }

    private void SetVisible(bool value)
    {
      foreach (var canvas in this.canvases)
      {
        canvas.enabled = value;
      }
    }

    private void HandleExerciseInactive(object sender, ExerciseStateEventArgs exerciseStateEventArgs)
    {
      this.SetVisible(false);
    }

    private void HandleExerciseInactive(object sender, ExerciseFinishedEventArgs exerciseFinishedEventArgs)
    {
      this.SetVisible(false);
    }

    private void HandleExercisePaused(object sender, ExerciseStateEventArgs exerciseStateEventArgs)
    {
      this.textIcon.text = ResexsHelper.GetResourceStringValue(ResexsHelper.UIResourceNameEnum.IconExerciseStatePaused.ToString(), ResexsHelper.ResexTypeEnum.UI);
      this.textLabel.text = ResexsHelper.GetResourceStringValue(ResexsHelper.UIResourceNameEnum.LabelExerciseStatePaused.ToString(), ResexsHelper.ResexTypeEnum.UI);
      this.SetVisible(true);
    }

    private void HandleExerciseUserReady(object sender, ExerciseStateEventArgs exerciseStateEventArgs)
    {
      this.textIcon.text = ResexsHelper.GetResourceStringValue(ResexsHelper.UIResourceNameEnum.IconExerciseStateWaiting.ToString(), ResexsHelper.ResexTypeEnum.UI);
      this.textLabel.text = ResexsHelper.GetResourceStringValue(ResexsHelper.UIResourceNameEnum.LabelExerciseStateWaiting.ToString(), ResexsHelper.ResexTypeEnum.UI);
      this.SetVisible(true);
    }
  }
}
