using System.Collections.Generic;

namespace AS.Toolbox.ScriptableObjects
{
    public class SOListVariable<T> : SOVariable<List<T>>
    {
        public bool clearOnEnable;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (v == null) v = new List<T>();
            if (clearOnEnable) Clear();
        }

        public T this[int i]
        {
            get => v[i];
            set => v[i] = value;
        }

        public void Add(T x)
        {
            v.Add(x);
            OnChange();
        }
        public bool Remove(T x)
        {
            bool remove = v.Remove(x);
            if (remove) OnChange();
            return remove;
        }
        public void Clear()
        {
            v.Clear();
            OnChange();
        }
        public int Count => v.Count;
    }
}