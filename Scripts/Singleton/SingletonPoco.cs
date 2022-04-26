namespace UnityReusables.Managers
{
    public class SingletonPoco<T> where T : new()
    {
        static T _instance;
        protected SingletonPoco() {}

        public static T instance
        {
            get { if (_instance == null)
                    _instance = new T();
                return _instance; 
            }
        }
    }
}