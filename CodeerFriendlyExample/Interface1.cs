using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeerFriendlyExample
{
    public interface Interface1
    {
        int Value { get; set; }

        string Text { get; set; }

        int Add(int a, int b);

        string Concat(string a, string b);
    }
}
