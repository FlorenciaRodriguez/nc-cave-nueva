namespace Assets.UI.Common.Scripts.Editor
{
  using System;
  using System.Linq;

  using Assets.UI.Common.Scripts.Helpers;

  using UnityEditor;

  using UnityEngine;

  [CustomEditor(typeof(TextResex))]
  public class TextResexEditor : Editor
  {
    private TextResex textResex;

    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      var resexType = (ResexsHelper.ResexTypeEnum)EditorGUILayout.EnumPopup("Resex Type", this.textResex.ResexType);
      var resourceName = this.textResex.ResourceName;

      var enumType = this.GetTypeForResex(resexType);
      if (enumType != null)
      {
        var displayedOptions = Enum.GetNames(enumType);
        if (displayedOptions.Length > 0)
        {
          var selectedIndex = Array.IndexOf(displayedOptions, resourceName);
          if (selectedIndex < 0)
          {
            selectedIndex = Array.IndexOf(displayedOptions, ResexsHelper.DefaultKey);
            if (selectedIndex < 0)
            {
              selectedIndex = 0;
            }
          }

          resourceName = displayedOptions.ElementAt(EditorGUILayout.Popup("Resource Name", selectedIndex, displayedOptions));
        }
        else
        {
          resourceName = EditorGUILayout.TextField("Resource Name", resourceName);
        }
      }
      else
      {
        resourceName = EditorGUILayout.TextField("Resource Name", resourceName);
      }

      if (GUI.changed)
      {
        Undo.RegisterCompleteObjectUndo(this.textResex, this.textResex.GetType().FullName);

        this.textResex.ResexType = resexType;
        this.textResex.ResourceName = resourceName;

        if (!Application.isPlaying)
        {
          EditorUtility.SetDirty(this.textResex);
        }
        else
        {
          this.textResex.UpdateText();
        }
      }
    }

    protected void OnEnable()
    {
      this.textResex = (TextResex)this.target;
    }

    private Type GetTypeForResex(ResexsHelper.ResexTypeEnum resexType)
    {
      switch (resexType)
      {
        case ResexsHelper.ResexTypeEnum.UI:
          return typeof(ResexsHelper.UIResourceNameEnum);
      }

      return null;
    }
  }
}
