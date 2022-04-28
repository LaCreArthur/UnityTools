using System.Text;
using Sirenix.OdinInspector;
using Toolbox.ScriptableObjects.Events;
using UltEvents;
using UnityEditor;
using UnityEngine;

namespace Toolbox.ScriptableObjects.Variables
{
    [ExecuteAlways]
    public abstract class VariableListenerBase<T, TVariable> : MonoBehaviour where TVariable : VariableSOBase<T>
    {
        [SerializeField, AssetSelector, Required("A variable must be assigned for this listener!")]
        protected TVariable variable;
        [SerializeField, ShowIf("@variable==null"), InlineButton("Create")]
        string soName;
        [Searchable]
        public UltEvent<T> events;

        protected virtual void Create(string soName)
        {
            CreateSOAsset(soName);
        }

        protected void CreateSOAsset(string path)
        {
            TVariable newSO = ScriptableObject.CreateInstance<TVariable>();
            // path has to start at "Assets"
            path = "Assets/" + path;
            AssetDatabase.CreateAsset(newSO, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newSO;
        }
    
        void Subscribe()
        {
            if (variable != null)
            {
                variable.onChange?.Add(new ReferencedUltEvent<UltEvent<T>>(this, events));
            }
        }

        void Unsubscribe()
        {
            if (variable != null)
            {
                variable.onChange?.Remove(new ReferencedUltEvent<UltEvent<T>>(this, events));
            }
        }

        protected virtual void OnEnable() => Subscribe();
        protected virtual void OnDisable() => Unsubscribe();
    }
}