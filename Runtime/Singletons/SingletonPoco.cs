namespace AS.Toolbox.Singletons
{
    public class SingletonPoco<T> where T : new()
    {
        static T s_instance;
        protected SingletonPoco() {}

        public static T Instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = new T();
                return s_instance;
            }
        }
    }
}