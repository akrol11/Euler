using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler1
{
    class Program
    {
        static void Euler1(*)()
        static void Main(string[] args)
        {
            int sum = 0;
            string str;
            str = Console.ReadLine();
          
            int param = 0;
            param = Int32.Parse(str);
            for (int i = 1; i < param; i++)
            {
                if (i%3 == 0 || i%5 == 0)
                    sum += i;
            }
            Console.WriteLine("Result = {0}", sum);
            Console.ReadLine();
        }
    }
}
