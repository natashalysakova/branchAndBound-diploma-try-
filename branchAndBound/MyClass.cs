using System;
using System.Diagnostics;

namespace branchAndBound
{
    class MyClass
    {
        public virtual void Method1()
        {
            Debug.WriteLine(nameof(MyClass) + "Method1");
        }

        protected void Method2()
        {
            
        }
    }

    class MyClass2 : MyClass
    {
        public override void Method1()
        {
            base.Method1();
            Debug.WriteLine(nameof(MyClass2) + "Method1");
        }
    }


    public class Singleton
    {
        private static Singleton _instance;

        private Singleton()
        {
            Id = Guid.NewGuid();
        }

        public static Singleton Instance
        {
            get
            {
              if(_instance == null)
                    _instance = new Singleton();
                return _instance;
            }
        }

        public Guid Id { get; }
    }
}
