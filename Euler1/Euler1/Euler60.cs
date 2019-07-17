using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler1
{
    class Euler60
    {
        private static Dictionary<long, List<long>> _primeNumbers;
        public static long maxNumber = 85000000; 
        public static void solveEuler60()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            _primeNumbers = new Dictionary<long, List<long>>();
            GetPrimeNumbers(maxNumber);
            //for(int i = 1; i < n ; i++ )
            //    if(_primeNumbers.ContainsKey(i))
            //        Console.Write("{0}, ",i);

            FindSpecialPrimes(maxNumber);
            int solveFor = 5;

            //for (int i = 1; i < maxNumber; i++)
            //     if (_primeNumbers.ContainsKey(i))
            //     {
            //         var values = _primeNumbers[i];
            //         if (values.Count > solveFor)
            //         {
            //             Console.Write("{0}: ", i);
            //             for (int j = 0; j < values.Count; ++j)
            //                 Console.Write("{0} ", values[j]);
            //             Console.WriteLine();
            //         }
            //     }

            for(int i = 0; i < maxNumber; i++)
                if(_primeNumbers.ContainsKey(i) && _primeNumbers[i].Count < solveFor - 1)
                    _primeNumbers[i].Clear();
            
            var cycle = FindCycle(solveFor, maxNumber);
            if (cycle.Count == 0)
                return;

            for (int j = 0; j < cycle.Count; j++)
                Console.WriteLine(cycle[j]);

            sw.Stop();

            Console.WriteLine("Elapsed={0}", sw.Elapsed);
        }

        public static void GetPrimeNumbers(long n)
        {
            var primeNumbers = new bool[n];
            
            for(long i = 1; i < n; ++i )
                for (long j = i; i+j+2*i*j < n; ++j)
                {
                    primeNumbers[i+j+2*i*j] = true; 
                }

            for( long i = 1; i < n ; ++i)
                if(!primeNumbers[i] && !_primeNumbers.ContainsKey(i*2+1))
                    _primeNumbers.Add(i*2+1,new List<long>());

        }

        private static void FindSpecialPrimes(long n)
        {
            for(long i=1; i < n; i++)
                if(_primeNumbers.ContainsKey(i))
                    FindCompositePrimes( i );

        }

        private static void FindCompositePrimes(long n)
        {
            var str = n.ToString();
            
            for (int i = 1; i < str.Length; i++)
            {
                var str1 = str.Substring(0, i);
                var str2 = str.Substring(i, str.Length-i);

                var num1 = long.Parse(str1);
                var num2 = long.Parse(str2);
                if( _primeNumbers.ContainsKey(num1) && 
                    _primeNumbers.ContainsKey(num2) &&
                    _primeNumbers.ContainsKey(long.Parse(str2+str1)) &&
                    str2[0] !='0')
                {
                    var values = _primeNumbers[num1];
                    values.Add(num2);
                    _primeNumbers[num1] = values;

              //      Console.WriteLine("Composite of {0} is {1} and {2}", str, str1, str2);
                }
            }
        }

        private static List<string> FindCycle(int cycleLength, long maxNumber)
        {
            var ret = new List<string>();
            var cycle = new List<long>();
            int cycleInd = 0;
            long curNumber = 0;
            for (long i = maxNumber; i > 0; i--)
            {
                if (i == 1 && cycle.Count > 0)
                {
                    //Console.WriteLine(i);
                    --cycleInd;
                    i = cycle[cycleInd] - 1;
                    cycle.RemoveAt(cycleInd);
                }

                //Console.Write("i={0}, cycleInd={1}, cycle = ", i, cycleInd);
                //for (int j = 0; j < cycleInd; j++)
                //    Console.Write("{0} ", cycle[j]);
                //Console.WriteLine();

                if (!_primeNumbers.ContainsKey(i) || _primeNumbers[i].Count < cycleLength - 1)
                    continue;
                
                //Console.WriteLine("Find Next Number after {0}", i);
                curNumber = FindNext(i, maxNumber, cycle);
                
                if (curNumber > 0)
                {
                    cycle.Add(curNumber);
                    ++cycleInd;
                    i = curNumber;

                    if (cycle.Count == cycleLength)
                    {
                    string str = "";
                    long sum = 0;
                        for (int j = 0; j < cycleInd; j++)
                        {
                            sum += cycle[j];
                            str += " " + cycle[j];
                        }
                        str += ("; sum = " + sum);
                    ret.Add(str);
                 }
                }
                else if (cycleInd == 0)
                    return ret;
                else
                {
                    --cycleInd;
                    i = cycle[cycleInd];
                    cycle.RemoveAt(cycleInd);
                }
            }

            return ret;
        }

        private static long FindNext(long start, long maxNumber, List<long> containsValues)
        {
            for (long i = start; i > 0; i--)
            {
                if (!_primeNumbers.ContainsKey(i))
                    continue;
                var commonList = containsValues.Select(a => a).Intersect(_primeNumbers[i]);
                if (commonList.Count() == containsValues.Count)
                    return i;
            }
            return -1;
        }
    }
}
