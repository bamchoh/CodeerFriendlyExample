using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeerFriendlyExampleInterfaces
{
    public interface ExampleInterface
    {
        string Do1(string text);

        int Do2(int value);
    }

    public interface ExampleInterface2
    {
        int PlusOne(int value);
    }
}
