using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeerFriendlyExampleInterfaces;

namespace CodeerFriendlyExampleClasses
{
    public class ExampleClass : ExampleInterface
    {
        public string Do1(string text)
        {
            return string.Format("** {0} **", text);
        }

        public int Do2(int value)
        {
            return value * 2;
        }
    }
}

