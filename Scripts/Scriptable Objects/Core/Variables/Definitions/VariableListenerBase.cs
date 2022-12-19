using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace AS.Toolbox.ScriptableObjects
{
    [ExecuteAlways]
    public abstract class VariableListenerBase<T, TVariable> : MonoBehaviour where TVariable : VariableSOBase<T>
    {
        [SerializeField, Required("A variable must be assigned for this listener!"), InlineButton("New"), HideIf("_isCreating")]
        protected TVariable variable;

        [SerializeField, ShowIf("@variable==null"), InlineButton("Cancel"), InlineButton("Create"), LabelText("Create new SO, enter name:"),
         ShowIf("_isCreating")]
        string soName = typeof(TVariable).ToString()[(typeof(TVariable).ToString().LastIndexOf(".") + 1)..];

        public bool callEventsOnStart;
        public UnityEvent<T> events;

#if UNITY_EDITOR
#pragma warning disable CS0414
        bool _isCreating;
#pragma warning restore CS0414
        protected void New() => _isCreating = true;

        protected void Cancel() => _isCreating = false;

        protected virtual void Create(string soName)
        {
            TVariable newSO = ScriptableObject.CreateInstance<TVariable>();
            Directory.CreateDirectory(Application.dataPath + "/Scriptable Objects");

            // path has to start at "Assets"
            var path = "Assets/Scriptable Objects/" + soName + ".asset";
            AssetDatabase.CreateAsset(newSO, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();

            //Selection.activeObject = newSO;
            variable = newSO;
            _isCreating = false;
        }
#endif

        void Subscribe()
        {
            if (variable != null)
            {
                variable.onChange?.Add(new ReferencedEvent<UnityEvent<T>>(events, this));
            }
        }

        void Unsubscribe()
        {
            if (variable != null)
            {
                variable.onChange?.Remove(new ReferencedEvent<UnityEvent<T>>(events, this));
            }
        }

        protected virtual void OnEnable() => Subscribe();

        protected virtual void OnDisable() => Unsubscribe();

        void Start()
        {
            if (callEventsOnStart)
                events.Invoke(variable.v);
        }
    }
}