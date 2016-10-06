using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler1
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Enter the Euler project number to solve. Enter 0 to exit");
            
            string str;
            

            bool done = false;

            int param = 0;
            while (!done)
            {
                str = Console.ReadLine();
                while (!Int32.TryParse(str, out param))
                {
                    Console.WriteLine("Input is not an integer. Try again.");
                    Console.ReadLine();
                }

                if (param == 0)
                    return;

                switch (param)
                {
                    case 1:
                       CalculateEuler1(1000);
                        
                        break;
                    case 201:
                        CalculateEuler201();
                        break;
                    default:
                        Console.WriteLine("Problem {0} has not been solved.", param);
                        break;
                }
                Console.WriteLine("Want to solve another one?");
            }

           
        }

        static void  CalculateEuler1(int param)
        {
            int sum = 0;
            for (int i = 1; i < param; i++)
            {
                if (i % 3 == 0 || i % 5 == 0)
                    sum += i;
            }
            Console.WriteLine("Result = {0}", sum);
        }
        
        static void CalculateEuler201()
        {
            /* S = {1^2, 2^2, ..., 100^2}
             * U(S,50) - unique!(duplicate not included) sums of 50-element subsets of S
             * Question to find sum(U(S,50))
             * Idea: 1. Consider a 50 branch tree (breadth first construction)
             *   Root at level n, is a sum of n-element nodes in S
             *   If there are the same sums across the level then the whole branch can be chopped off.
             *   If we don't run out of memory :), add up the nodes at the bottom of a 50 node tree
             *   
             *  How to trim: Need to remember combinations that can be duplicated
             * 
            */
            long minSum = 0;
            for (long i = 1; i < 51; ++i)
                minSum += (i*i);
            Console.WriteLine("The min sum is {0}", minSum);

            long maxSum = 0;
            for (long i = 100; i > 49; --i)
                maxSum += (i*i);
            Console.WriteLine("The max sum is {0}", maxSum);

            //Min sum = 42925
            //Max sum = 297925

            var originalSet = new int[100];
            for (int i = 1; i < 101; i++)
            {
                originalSet[i - 1] = i*i;
                Console.Write("a[{0}]={1},", i-1, originalSet[i-1]);
            }

            // brute force: array of 300000 fill up with 0's

            var sums = new long[300000];
            
            // numOfOperand
            int maxSubsetSize = 20;
            int curSubsetSize = 0;
            int curSum;
            
            for (int i = 1; i < 300001; i++)
            {
                if( sums[i-1] > 0 )
                    Console.Write("a[{0}]={1},", i - 1, sums[i - 1]);
            }

        }
    }
}
