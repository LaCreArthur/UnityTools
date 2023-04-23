using UnityEditor;
using UnityEngine;

namespace AS.Toolbox.Attributes
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var showIf = attribute as ShowIfAttribute;
            var shouldShow = GetConditionResult(property, showIf.ConditionExpression);

            if (shouldShow)
                EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var showIf = attribute as ShowIfAttribute;
            var shouldShow = GetConditionResult(property, showIf.ConditionExpression);

            return shouldShow ? EditorGUI.GetPropertyHeight(property, label) : 0f;
        }

        static bool GetConditionResult(SerializedProperty property, string conditionExpression)
        {
            try
            {
                // Define the EvaluateFunction that can handle the property condition evaluation
                bool EvalFunction(string propertyName)
                {
                    var serializedObject = new SerializedObject(property.serializedObject.targetObject);
                    var prop = serializedObject.FindProperty(propertyName);
                    if (prop != null)
                        return prop.boolValue;
                    Debug.LogError($"[ShowIf] Property '{propertyName}' not found in target object.",
                        property.serializedObject.targetObject);
                    return false;
                }
                // Use the parser to evaluate the condition
                return ExpressionParser.EvaluateExpression(conditionExpression, EvalFunction);
            }
            catch
            {
                return false;
            }
        }
    }
}
