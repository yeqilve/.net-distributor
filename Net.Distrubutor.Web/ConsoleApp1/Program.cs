using Autofac;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<Test>().As<ITest1>().InstancePerLifetimeScope();
            builder.RegisterType<Test2>().As<ITest2>().InstancePerLifetimeScope();
            var container = builder.Build();

            //       ITest1 t = container.Resolve<ITest1>();
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            ITest2 t2 = container.Resolve<ITest2>();
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            t2.Test();
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Console.Read();
        }
    }

    public interface ITest1
    {
        void Excute();
    }

    public class Test : ITest1
    {
        private string test;

        public void Excute()
        {
            
        }
    }

    public interface ITest2
    {
        void Test();
    }
    public class Test2 : ITest2
    {
        private ITest1 test1;
        public Test2(ITest1 test)
        {
            test1 = test;
        }
        public void Test() => Console.WriteLine($"{test1.GetType()}resolved{test1.ToString()}");
    }
}
