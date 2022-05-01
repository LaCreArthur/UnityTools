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
        [SerializeField, AssetSelector, Required("A variable must be assigned for this listener!"), InlineButton("New"), HideIf("_isCreating")]
        protected TVariable variable;
        [SerializeField, ShowIf("@variable==null"), InlineButton("Cancel"), InlineButton("Create"), LabelText("Create new SO, enter name:"), ShowIf("_isCreating")]
        string soName = typeof(TVariable).ToString()[(typeof(TVariable).ToString().LastIndexOf(".")+1)..];
        [Searchable]
        public UltEvent<T> events;

        bool _isCreating = false;
        protected void New() => _isCreating = true;
        protected void Cancel() => _isCreating = false;

        protected virtual void Create(string soName)
        {
            TVariable newSO = ScriptableObject.CreateInstance<TVariable>();
            // path has to start at "Assets"
            var path = "Assets/_CARFT/Scriptable Objects/" + soName + ".asset";
            AssetDatabase.CreateAsset(newSO, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            //Selection.activeObject = newSO;
            variable = newSO;
            _isCreating = false;
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