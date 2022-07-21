using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeerFriendlyExampleInterfaces;
using Unity;

namespace CodeerFriendlyExampleClasses
{
    public class ExampleClass : ExampleInterface
    {
        UnityContainer Container { get; set; }

        public ExampleClass()
        {
            Container = new UnityContainer();

            Container.RegisterType<ExampleInterface2, ExampleClass2>();
        }

        public string Do1(string text)
        {
            return string.Format("** {0} **", text);
        }

        public int Do2(int value)
        {
            return value * 2;
        }

        public string Do3(string text = null, int value = default)
        {
            return Do3(text, value, 0.3);
        }

        public string Do3(string text, int value, double value2)
        {
            return string.Format("{0}/{1}={2}", text, value, value2);
        }

        public T Do4<T>()
        {
            return Container.Resolve<T>();
        }

        public T Do5<T>(IUnityContainer container, ExampleClass3 klass, int value = default) where T : new()
        {
            return new T();
        }
    }

    internal class ExampleClass2 : ExampleInterface2
    {
        public int PlusOne(int value)
        {
            return value + 1;
        }

        public string PlusOneString(int value)
        {
            return (value + 1).ToString();
        }
    }

    public class ExampleClass3 : ExampleInterface2
    {
        public int PlusOne(int value)
        {
            throw new NotImplementedException();
        }
    }
}

