using System;

namespace ProxyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Proxy p = new Proxy1();
            p?.Proceed();
            Console.WriteLine("Hello World!");
            Console.Read();
        }
    }
    #region Proxy
    public interface IProcesser
    {
        void Proceed();
    }

    public abstract class Proxy : IProcesser
    {
        public virtual void BeforeExecute(object param = null) { }

        private IProcesser _interapter;
        public Proxy() {
            Console.WriteLine("teste");
        }

        public Proxy(IProcesser interapter)
        {
            this._interapter = interapter;
           
        }

        public void Proceed()
        {
            BeforeExecute();
            _interapter.Proceed();
            AfterExecute();
        }

        public virtual void AfterExecute(object param=null) { }
    }

    public class Proxy1 : Proxy
    {
        private IProcesser _interapter;
        public Proxy1(IProcesser interapter) : base(interapter) { }
   
    }
    #endregion

}
