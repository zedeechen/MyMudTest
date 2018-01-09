using System;

namespace ZDMMO
{
    public static class SingletonFactory<T> where T : class
    {
        /*	Instance	*/
        private static T _instance;

        /* Static constructor	*/
        static SingletonFactory()
        {
            return;
        }

        public static void Create()
        {
            _instance = (T)Activator.CreateInstance(typeof(T), true);

            return;
        }

        /* Serve the single instance to callers	*/
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    Create();

                return _instance;
            }
        }

        /*	Destroy	*/
        public static void Destroy()
        {

            _instance = null;

            return;
        }

        public static void ResetInstance(T inst)
        {
            _instance = inst;
        }
    }
}
