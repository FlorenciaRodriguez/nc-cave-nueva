namespace Assets.Scripts.Property.Editor
{
  using Assets.Scripts.Property;

  using UnityEditor;

  using UnityEngine;

  [CustomPropertyDrawer(typeof(PrimitiveNullable<>))]
  public class PrimitiveNullablePropertyDrawer : PropertyDrawer
  {
    private const string HasValuePropertyName = "HasValue";

    private const string ValuePropertyName = "Value";

    private const float HasValuePropertyWidth = 20;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      EditorGUI.BeginProperty(position, label, property);

      var hasValueProperty = property.FindPropertyRelative(HasValuePropertyName);
      var valueProperty = property.FindPropertyRelative(ValuePropertyName);

      EditorGUI.PropertyField(
        new Rect(position.x, position.y, HasValuePropertyWidth, position.height),
        hasValueProperty,
        GUIContent.none);

      GUI.enabled = hasValueProperty.boolValue;
      EditorGUI.PropertyField(
        new Rect(position.x + HasValuePropertyWidth, position.y, position.width - HasValuePropertyWidth, position.height),
        valueProperty,
        label);

      EditorGUI.EndProperty();
    }
  }
}