namespace Assets.UI.Common.Scripts.Editor
{
  using Assets.UI.Common.Scripts;

  using UnityEditor;

  using UnityEngine;

  [CustomEditor(typeof(ProgressBarController))]
  public class ProgressBarControllerEditor : Editor
  {
    private ProgressBarController progressBarController;

    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      var currentPercent = EditorGUILayout.Slider("Current Percent", this.progressBarController.CurrentPercent, 0.0f, 1.0f);

      if (GUI.changed)
      {
        Undo.RegisterCompleteObjectUndo(this.progressBarController, this.progressBarController.GetType().FullName);

        this.progressBarController.CurrentPercent = currentPercent;

        if (!Application.isPlaying)
        {
          EditorUtility.SetDirty(this.progressBarController);
        }
      }
    }

    protected void OnEnable()
    {
      this.progressBarController = (ProgressBarController)this.target;
    }
  }
}
