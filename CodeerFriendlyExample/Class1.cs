using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeerFriendlyExample
{
    internal class Class1 : Interface1
    {
        public int Value { get; set; }
        public string Text { get; set; }

        public int Add(int a, int b)
        {
            Value = a + b;
            return Value;
        }

        public string Concat(string a, string b)
        {
            Text = a + b;
            return Text;
        }
    }
}
