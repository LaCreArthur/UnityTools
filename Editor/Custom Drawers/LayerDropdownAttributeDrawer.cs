using AS.Toolbox.Attributes;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(LayerDropdownAttribute))]
public class LayerDropdownAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.Integer)
        {
            EditorGUI.BeginProperty(position, label, property);
            property.intValue = EditorGUI.LayerField(position, label, property.intValue);
            EditorGUI.EndProperty();
        }
        else
            EditorGUI.LabelField(position, label, new GUIContent("Error: Use LayerDropdown with int type only"));
    }
}
