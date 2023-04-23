using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TagDropdownAttribute))]
public class TagDropdownAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.String)
        {
            EditorGUI.BeginProperty(position, label, property);
            property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
            EditorGUI.EndProperty();
        }
        else
            EditorGUI.LabelField(position, label, new GUIContent("Error: Use TagDropdown with string type only"));
    }
}
