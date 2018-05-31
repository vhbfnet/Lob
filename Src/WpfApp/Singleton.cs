using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public class Singleton<T> where T : class, new()
    {
        private Singleton() { }
        private static readonly Lazy<T> _instance = new Lazy<T>(() => new T());
        public static T Instance { get { return _instance.Value; } }
    }
}
