namespace Assets.UnityAssets.Tools.NLog.Editor
{
  using Assets.UnityAssets.Tools.NLog.Scripts;

  using UnityEditor;

  using UnityEngine;

  [CustomEditor(typeof(NLogConfigurator))]
  public class ContextRootEditor : Editor
  {
    protected const int DefaultExecutionOrder = -500;

    private NLogConfigurator nlogConfigurator;

    public override void OnInspectorGUI()
    {
      this.DrawDefaultInspector();

      EditorGUILayout.HelpBox("Use the button below to ensure will be executed before any other MonoBehaviour", MessageType.Info);

      if (GUILayout.Button("Set execution order"))
      {
        var contextRootType = this.nlogConfigurator.GetType();
        var contextRootOrder = ExecutionOrderUtils.SetScriptExecutionOrder(contextRootType, DefaultExecutionOrder);

        EditorUtility.DisplayDialog("Script execution order", string.Format("{0} execution order set to {1}.", contextRootType.Name, contextRootOrder), "Ok");
      }
    }

    protected void OnEnable()
    {
      this.nlogConfigurator = (NLogConfigurator)this.target;
    }
  }
}