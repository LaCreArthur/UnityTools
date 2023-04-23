using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace AS.Toolbox.ScriptableObjects
{
    public class SOListVar<T> : SOVar<List<T>>
    {
        [TitleGroup("Values")]
        public bool clearOnEnable;

        [FoldoutGroup("On Added"), HideLabel, InlineProperty, HideReferenceObjectPicker, OnInspectorGUI("RemoveNullAdded")]
        public ReferencedCallbacks<T> onAdded = new ReferencedCallbacks<T>();
        [FoldoutGroup("On Removed"), HideLabel, InlineProperty, HideReferenceObjectPicker, OnInspectorGUI("RemoveNullRemoved")]
        public ReferencedCallbacks<T> onRemoved = new ReferencedCallbacks<T>();

        public T this[int i]
        {
            get => v[i];
            set => v[i] = value;
        }
        public int Count => v.Count;

        protected override void OnEnable()
        {
            base.OnEnable();
            v ??= new List<T>();
            if (clearOnEnable) Clear();
        }

        void RemoveNullAdded() => onAdded?.RemoveAll(c => c.reference == null);
        void RemoveNullRemoved() => onRemoved?.RemoveAll(c => c.reference == null);

        public void Add(T x)
        {
            v.Add(x);
            OnChange();
            onAdded.Invoke(this, x, false);
        }
        public bool Remove(T x)
        {
            bool remove = v.Remove(x);
            if (remove)
            {
                OnChange();
                onRemoved.Invoke(this, x, false);
            }

            return remove;
        }
        public void Clear()
        {
            v.Clear();
            OnChange();
        }
    }
}
