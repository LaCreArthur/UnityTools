using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;
#endif

namespace AS.Toolbox.Editor
{

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class TagDropdownAttribute : Attribute {}


#if UNITY_EDITOR
    public sealed class TagDropdownAttributeDrawer : OdinAttributeDrawer<TagDropdownAttribute, string>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            ValueEntry.SmartValue = EditorGUILayout.TagField(label ?? new GUIContent(""), ValueEntry.SmartValue);
        }
    }

#endif
}