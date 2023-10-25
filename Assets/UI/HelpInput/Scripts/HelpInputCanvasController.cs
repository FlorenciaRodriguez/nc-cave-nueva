#pragma warning disable 649
namespace Assets.UI.HelpInput.Scripts
{
  using System.Text;

  using Assets.Scripts.Adic.Utils;
  using Assets.Scripts.Exercise;
  using Assets.Scripts.Exercise.EventArgs;
  using Assets.Scripts.Exercise.Helpers;
  using Assets.Scripts.Helpers;
  using Assets.UI.Common.Scripts.Helpers;

  using MediaLab.Adic.Framework.Attributes;

  using TMPro;

  using UnityEngine;

  public class HelpInputCanvasController : BaseBehaviour
  {
    [SerializeField]
    private TextMeshProUGUI textMeshPro;

    [Inject]
    private ExerciseManager exerciseManager;

    protected override void Start()
    {
      base.Start();

      if (this.exerciseManager != null)
      {
        this.exerciseManager.AfterLoad += this.HandleExerciseAfterLoad;
        this.exerciseManager.BeforeUnload += this.HandleExerciseBeforeUnload;

        if (this.exerciseManager.State == ExerciseManager.StateEnum.Loaded || this.exerciseManager.State == ExerciseManager.StateEnum.Loading)
        {
          this.HandleExerciseAfterLoad(this.exerciseManager, new ExerciseStateEventArgs(this.exerciseManager.ExerciseEntity));
          return;
        }
      }

      this.HandleExerciseBeforeUnload(null, new ExerciseStateEventArgs(null));
    }

    protected void OnDestroy()
    {
      if (this.exerciseManager != null)
      {
        this.exerciseManager.AfterLoad -= this.HandleExerciseAfterLoad;
        this.exerciseManager.BeforeUnload -= this.HandleExerciseBeforeUnload;
      }
    }

    protected void OnValidate()
    {
      if (this.textMeshPro == null)
      {
        Debug.LogError($"The variable '{nameof(this.textMeshPro)}' of '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.", this.gameObject);
      }
    }

    private void HandleExerciseAfterLoad(object sender, ExerciseStateEventArgs exerciseStateEventArgs)
    {
      var stringBuilder = new StringBuilder();

      this.WriteDefaultHelp(stringBuilder);

      if (exerciseStateEventArgs.ExerciseEntity.GetAppType() == ApplicationHelper.AppTypeEnum.LocationTour)
      {
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();

        stringBuilder.AppendLine(
          ResexsHelper.GetResourceStringValue(
            ResexsHelper.UIResourceNameEnum.LabelLocationTourHelpInput.ToString(),
            ResexsHelper.ResexTypeEnum.UI));
      }
      
      this.textMeshPro.text = stringBuilder.ToString();
    }

    private void HandleExerciseBeforeUnload(object sender, ExerciseStateEventArgs exerciseStateEventArgs)
    {
      var stringBuilder = new StringBuilder();
      this.WriteDefaultHelp(stringBuilder);
      this.textMeshPro.text = stringBuilder.ToString();
    }

    private void WriteDefaultHelp(StringBuilder stringBuilder)
    {
      stringBuilder.Clear();

      stringBuilder.AppendLine(
        ResexsHelper.GetResourceStringValue(
          ResexsHelper.UIResourceNameEnum.LabelHelpInput.ToString(),
          ResexsHelper.ResexTypeEnum.UI));

      if (ApplicationHelper.ApplicationMode == ApplicationHelper.ApplicationModeEnum.Sandbox)
      {
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();

        stringBuilder.AppendLine(
          ResexsHelper.GetResourceStringValue(
            ResexsHelper.UIResourceNameEnum.LabelSandboxHelpInput.ToString(),
            ResexsHelper.ResexTypeEnum.UI));
      }
    }
  }
}
